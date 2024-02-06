using Common;
using Common.Utilities;
using Data.Repositories;
using Entities;
using Makta.api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Email;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Makta.api
{
    [ApiController]
    [Route("api/v1.0/[action]")]
    public class ApiController : ControllerBase
    {
        private readonly IApiService _apiService;
        private readonly IRepository<Store> _storeRepository;
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IRepository<PointRate> _pointRateRepository;
        private readonly IRepository<Points> _pointsRepository;
        private readonly IEmailSender _emailSender;

        public ApiController(IApiService apiService, IRepository<Store> storeRepository, IRepository<ApplicationUser> userRepository, IRepository<Country> countryRepository, UserManager<ApplicationUser> userManager, IRepository<Currency> currencyRepository, IRepository<PointRate> pointRateRepository, IRepository<Points> pointsRepository, IEmailSender emailSender)
        {
            _apiService = apiService;
            _storeRepository = storeRepository;
            _userRepository = userRepository;
            _countryRepository = countryRepository;
            _userManager = userManager;
            _currencyRepository = currencyRepository;
            _pointRateRepository = pointRateRepository;
            _pointsRepository = pointsRepository;
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> AddPoints([FromBody] AddPointsDto data, CancellationToken cancellationToken)
        {
            try
            {
                if (data == null)
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid data"
                    });

                data.PhoneNumber = data.PhoneNumber.RemoveMasks();

                if (string.IsNullOrEmpty(data.StoreKey))
                    return Unauthorized(new ApiResult
                    {
                        ResultMessage = "Invalid store key"
                    });

                if (string.IsNullOrEmpty(data.CountryCode))
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid country code"
                    });

                if (string.IsNullOrEmpty(data.PhoneNumber))
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid Phone number"
                    });

                if (data.AmountSpent == 0)
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid amount spent"
                    });

                var store = await _storeRepository.TableNoTracking.FirstOrDefaultAsync(p => p.StoreKey.ToLower() == data.StoreKey.ToLower(), cancellationToken);

                if (store == null)
                    return Unauthorized(new ApiResult
                    {
                        ResultMessage = "Invalid store key"
                    });

                if (!store.IsActive)
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Inactive store"
                    });

                var country = await _countryRepository.TableNoTracking.FirstOrDefaultAsync(p =>
                p.Title.ToLower() == data.CountryCode.ToLower() ||
                p.ShortTitle.ToLower() == data.CountryCode.ToLower() ||
                p.Id.ToString() == data.CountryCode.ToLower() ||
                p.PhoneCode.ToLower() == data.CountryCode.ToLower(),
                cancellationToken);

                if (country == null)
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid country code"
                    });

                var currency = await _currencyRepository.TableNoTracking.FirstOrDefaultAsync(p =>
               p.Name.ToLower() == data.Currency.ToLower() ||
               p.FullName.ToLower() == data.Currency.ToLower(),
               cancellationToken);

                if (currency == null)
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid currency"
                    });

                var pointRate = await _pointRateRepository.TableNoTracking.FirstOrDefaultAsync(p => p.CurrencyId == currency.Id, cancellationToken);
                if (pointRate == null)
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid points rate"
                    });

                var client = await _userRepository.TableNoTracking.FirstOrDefaultAsync(p => p.CountryId == country.Id && p.PhoneNumber == data.PhoneNumber, cancellationToken);

                if (client == null) //create a new client
                {
                    var email = StringHelper.GenerateRandomEmail();
                    var password = StringHelper.GenerateRandomPassword();

                    client = new ApplicationUser
                    {
                        CountryId = country.Id,
                        PhoneNumber = data.PhoneNumber,
                        Email = email,
                        EmailConfirmed = true,
                        IsActive = true,
                        UserName = data.PhoneNumber,
                        PhoneNumberConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(client, password);
                    if (result != IdentityResult.Success)
                    {
                        if (result.Errors.Any(p => p.Code == "DuplicateEmail"))
                        {
                            //retry with a new random email to make sure 2 clients won't get the same randomly generated emails
                            email = StringHelper.GenerateRandomEmail();
                            client.Email = email;
                            result = await _userManager.CreateAsync(client, password);
                            if (result != IdentityResult.Success)
                            {
                                var errors = string.Join(", ", result.Errors.Select(p => p.Description));
                                await _emailSender.LogSystem(
                                    $"create client failed for client with countryId:{country.Id}, phone: {data.PhoneNumber}, email: {email}, errors: {errors}");
                                return BadRequest(new ApiResult
                                {
                                    ResultMessage = errors
                                });
                            }
                        }
                    }

                    await _userManager.AddToRoleAsync(client, SystemRoles.Client.ToString());
                }

                if (!client.IsActive)
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Inactive client"
                    });

                //adding points

                var earnedPoints = Math.Round((double)data.AmountSpent / (double)pointRate.SpentAmount * pointRate.Points);

                var points = new Points
                {
                    ClientId = client.Id,
                    Comments = data.Comments,
                    CurrencyId = currency.Id,
                    SpentAmount = data.AmountSpent,
                    StoreId = store.Id,
                    PointRateId = pointRate.Id,
                    EarnedPoints = earnedPoints
                };

                await _pointsRepository.AddAsync(points, cancellationToken);

                var totalPoints = await _pointsRepository.TableNoTracking.Where(p => p.ClientId == client.Id).SumAsync(p => p.EarnedPoints, cancellationToken);

                totalPoints = Math.Max(0, totalPoints); //to return balance as zero at minimum

                return Ok(new ApiResult
                {
                    ResultMessage = "Points collected successfully!",
                    ResultBody = new
                    {
                        EarnedPoints = earnedPoints,
                        TotalPoints = totalPoints,
                        PointsId = points.Id // to be saved on the partner side for returning time
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResult
                {
                    ResultMessage = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPointsBalance([FromBody] GetPointsDto data, CancellationToken cancellationToken)
        {
            try
            {
                if (data == null)
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid data"
                    });

                data.PhoneNumber = data.PhoneNumber.RemoveMasks();

                if (string.IsNullOrEmpty(data.StoreKey))
                    return Unauthorized(new ApiResult
                    {
                        ResultMessage = "Invalid store key"
                    });

                if (string.IsNullOrEmpty(data.CountryCode))
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid country code"
                    });

                if (string.IsNullOrEmpty(data.PhoneNumber))
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid phone number"
                    });

                var store = await _storeRepository.TableNoTracking.FirstOrDefaultAsync(p => p.StoreKey.ToLower() == data.StoreKey.ToLower(), cancellationToken);

                if (store == null)
                    return Unauthorized(new ApiResult
                    {
                        ResultMessage = "Invalid store key"
                    });

                if (!store.IsActive)
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Inactive store"
                    });

                var country = await _countryRepository.TableNoTracking.FirstOrDefaultAsync(p =>
                p.Title.ToLower() == data.CountryCode.ToLower() ||
                p.ShortTitle.ToLower() == data.CountryCode.ToLower() ||
                p.Id.ToString() == data.CountryCode.ToLower() ||
                p.PhoneCode.ToLower() == data.CountryCode.ToLower(),
                cancellationToken);

                if (country == null)
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid country code"
                    });

                var client = await _userRepository.TableNoTracking.FirstOrDefaultAsync(p => p.CountryId == country.Id && p.PhoneNumber == data.PhoneNumber, cancellationToken);

                if (client == null)
                    return NotFound(new ApiResult
                    {
                        ResultMessage = "Client not found"
                    });

                var totalPoints = await _pointsRepository.TableNoTracking.Where(p => p.ClientId == client.Id).SumAsync(p => p.EarnedPoints, cancellationToken);

                return Ok(new ApiResult
                {
                    ResultBody = totalPoints.ToString()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResult
                {
                    ResultMessage = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPointsHistory([FromBody] GetPointsDto data, CancellationToken cancellationToken)
        {
            try
            {
                if (data == null)
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid data"
                    });

                data.PhoneNumber = data.PhoneNumber.RemoveMasks();

                if (string.IsNullOrEmpty(data.StoreKey))
                    return Unauthorized(new ApiResult
                    {
                        ResultMessage = "Invalid store key"
                    });

                if (string.IsNullOrEmpty(data.CountryCode))
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid country code"
                    });

                if (string.IsNullOrEmpty(data.PhoneNumber))
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid phone number"
                    });

                var store = await _storeRepository.TableNoTracking.FirstOrDefaultAsync(p => p.StoreKey.ToLower() == data.StoreKey.ToLower(), cancellationToken);

                if (store == null)
                    return Unauthorized(new ApiResult
                    {
                        ResultMessage = "Invalid store key"
                    });

                if (!store.IsActive)
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Inactive store"
                    });

                var country = await _countryRepository.TableNoTracking.FirstOrDefaultAsync(p =>
                p.Title.ToLower() == data.CountryCode.ToLower() ||
                p.ShortTitle.ToLower() == data.CountryCode.ToLower() ||
                p.Id.ToString() == data.CountryCode.ToLower() ||
                p.PhoneCode.ToLower() == data.CountryCode.ToLower(),
                cancellationToken);

                if (country == null)
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid country code"
                    });

                var client = await _userRepository.TableNoTracking.FirstOrDefaultAsync(p => p.CountryId == country.Id && p.PhoneNumber == data.PhoneNumber, cancellationToken);

                if (client == null)
                    return NotFound(new ApiResult
                    {
                        ResultMessage = "Client not found"
                    });

                var totalPoints = await _pointsRepository.TableNoTracking.Where(p => p.ClientId == client.Id).SumAsync(p => p.EarnedPoints, cancellationToken);

                var pointsList = await _pointsRepository.TableNoTracking
                    .Include(p => p.Currency)
                    .OrderByDescending(p => p.InsertDateTime)
                    .Where(p => p.ClientId == client.Id && p.StoreId == store.Id)
                    .Skip(data.ItemsPerPage * (data.PageNumber - 1))
                    .Take(data.ItemsPerPage)
                    .Select(p => new
                    {
                        DateTime = p.InsertDateTime.ToString("yyyy-MM-dd"),
                        SpentAmount = $"{p.SpentAmount} {p.Currency.Name}",
                        p.EarnedPoints,
                        p.Comments
                    })
                    .ToListAsync(cancellationToken);

                return Ok(new ApiResult
                {
                    ResultBody = new
                    {
                        TotalPoints = totalPoints.ToString(),
                        PointsList = pointsList
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResult
                {
                    ResultMessage = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCountryCodes([FromBody] StoreKeyDto data, CancellationToken cancellationToken)
        {
            try
            {
                if (data == null)
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Invalid data"
                    });

                if (string.IsNullOrEmpty(data.StoreKey))
                    return Unauthorized(new ApiResult
                    {
                        ResultMessage = "Invalid store key"
                    });

                var store = await _storeRepository.TableNoTracking.FirstOrDefaultAsync(p => p.StoreKey.ToLower() == data.StoreKey.ToLower(), cancellationToken);

                if (store == null)
                    return Unauthorized(new ApiResult
                    {
                        ResultMessage = "Invalid store key"
                    });

                if (!store.IsActive)
                    return BadRequest(new ApiResult
                    {
                        ResultMessage = "Inactive store"
                    });

                var countryList = await _countryRepository.TableNoTracking.ToListAsync(cancellationToken);

                return Ok(new ApiResult
                {
                    ResultBody = countryList
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResult
                {
                    ResultMessage = ex.Message
                });
            }
        }
    }
}