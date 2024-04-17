using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext dbContext)
        {
            if (!dbContext.Database.GetService<IRelationalDatabaseCreator>().Exists())
            {
                dbContext.Database.Migrate();
                SeedData(dbContext);
            }
        }

        public static void SeedData(ApplicationDbContext dbContext)
        {
            #region Settings
            dbContext.Add(new Setting
            {
                CommunityEmail = "community@makta.org",
                CommunityName = "Makta",
                Domain = "makta.org",
                GitHub = "https://github.com/makta-org",
                Linkedin = "https://www.linkedin.com/in/makta",
                Telegram = "https://t.me/+pCbsfL72sxowNjgx",
                Twitter = "",
                Explorer = "https://explorer.makta.org",
                Forum = "https://talk.makta.org",
                WhitePaper = "https://makta.org/whitepaper.pdf"
            });
            #endregion

            #region Countries
            var can = new Country { PhoneCode = "1", PhoneFormat = "xxx-xxx-xxxx", ShortTitle = "CAN", Title = "Canada" };
            dbContext.Add(can);
            #endregion

            #region Provinces
            var CanadaProvinces = new List<Province>()
            {
                new Province() { Name = "Ontario", CountryId=1},
                new Province() { Name = "Quebec", CountryId=1},
                new Province() { Name = "Nova Scotia", CountryId=1},
                new Province() { Name = "New Brunswick", CountryId=1},
                new Province() { Name = "Manitoba", CountryId=1},
                new Province() { Name = "British Columbia", CountryId=1},
                new Province() { Name = "Prince Edward Island", CountryId=1},
                new Province() { Name = "Saskatchewan", CountryId=1},
                new Province() { Name = "Alberta", CountryId=1},
                new Province() { Name = "New foundland/Labrador", CountryId=1},
                new Province() { Name = "Northwest Territories", CountryId=1},
                new Province() { Name = "Nunavut", CountryId=1},
                new Province() { Name = "Yukon", CountryId=1},
            };
            dbContext.AddRange(CanadaProvinces);
            #endregion

            #region Currencies
            var currencies = new List<Currency>()
            {new Currency { Name = "AED", FullName = "UAE Dirham" }
                , new Currency { Name = "AFN", FullName = "Afghani" }
                , new Currency { Name = "ALL", FullName = "Lek" }
                , new Currency { Name = "AMD", FullName = "Armenian Dram" }
                , new Currency { Name = "ANG", FullName = "Netherlands Antillian Guilder" }
                , new Currency { Name = "AOA", FullName = "Kwanza" }
                , new Currency { Name = "ARS", FullName = "Argentine Peso" }
                , new Currency { Name = "AUD", FullName = "Australian Dollar" }
                , new Currency { Name = "AWG", FullName = "Aruban Guilder" }
                , new Currency { Name = "AZN", FullName = "Azerbaijanian Manat" }
                , new Currency { Name = "BAM", FullName = "Convertible Marks" }
                , new Currency { Name = "BBD", FullName = "Barbados Dollar" }
                , new Currency { Name = "BDT", FullName = "Taka" }
                , new Currency { Name = "BGN", FullName = "Bulgarian Lev" }
                , new Currency { Name = "BHD", FullName = "Bahraini Dinar" }
                , new Currency { Name = "BIF", FullName = "Burundi Franc" }
                , new Currency { Name = "BMD", FullName = "Bermudian Dollar" }
                , new Currency { Name = "BND", FullName = "Brunei Dollar" }
                , new Currency { Name = "BOB", FullName = "Boliviano" }
                , new Currency { Name = "BRL", FullName = "Brazilian Real" }
                , new Currency { Name = "BSD", FullName = "Bahamian Dollar" }
                , new Currency { Name = "BTN", FullName = "Ngultrum" }
                , new Currency { Name = "BWP", FullName = "Pula" }
                , new Currency { Name = "BYR", FullName = "Belarussian Ruble" }
                , new Currency { Name = "BZD", FullName = "Belize Dollar" }
                , new Currency { Name = "CAD", FullName = "Canadian Dollar" }
                , new Currency { Name = "CDF", FullName = "Congolese Franc" }
                , new Currency { Name = "CHF", FullName = "Swiss Franc" }
                , new Currency { Name = "CLP", FullName = "Chilean Peso" }
                , new Currency { Name = "CNY", FullName = "Yuan Renminbi" }
                , new Currency { Name = "COP", FullName = "Colombian Peso" }
                , new Currency { Name = "CRC", FullName = "Costa Rican Colon" }
                , new Currency { Name = "CUP", FullName = "Cuban Peso" }
                , new Currency { Name = "CVE", FullName = "Cape Verde Escudo" }
                , new Currency { Name = "CZK", FullName = "Czech Koruna" }
                , new Currency { Name = "DJF", FullName = "Djibouti Franc" }
                , new Currency { Name = "DKK", FullName = "Danish Krone" }
                , new Currency { Name = "DOP", FullName = "Dominican Peso" }
                , new Currency { Name = "DZD", FullName = "Algerian Dinar" }
                , new Currency { Name = "EEK", FullName = "Kroon" }
                , new Currency { Name = "EGP", FullName = "Egyptian Pound" }
                , new Currency { Name = "ERN", FullName = "Nakfa" }
                , new Currency { Name = "ETB", FullName = "Ethiopian Birr" }
                , new Currency { Name = "EUR", FullName = "Euro" }
                , new Currency { Name = "FJD", FullName = "Fiji Dollar" }
                , new Currency { Name = "FKP", FullName = "Falkland Islands Pound" }
                , new Currency { Name = "GBP", FullName = "Pound Sterling" }
                , new Currency { Name = "GEL", FullName = "Lari" }
                , new Currency { Name = "GHS", FullName = "Cedi" }
                , new Currency { Name = "GIP", FullName = "Gibraltar Pound" }
                , new Currency { Name = "GMD", FullName = "Dalasi" }
                , new Currency { Name = "GNF", FullName = "Guinea Franc" }
                , new Currency { Name = "GTQ", FullName = "Quetzal" }
                , new Currency { Name = "GYD", FullName = "Guyana Dollar" }
                , new Currency { Name = "HKD", FullName = "Hong Kong Dollar" }
                , new Currency { Name = "HNL", FullName = "Lempira" }
                , new Currency { Name = "HRK", FullName = "Croatian Kuna" }
                , new Currency { Name = "HTG", FullName = "Gourde" }
                , new Currency { Name = "HUF", FullName = "Forint" }
                , new Currency { Name = "IDR", FullName = "Rupiah" }
                , new Currency { Name = "ILS", FullName = "New Israeli Sheqel" }
                , new Currency { Name = "INR", FullName = "Indian Rupee" }
                , new Currency { Name = "IQD", FullName = "Iraqi Dinar" }
                , new Currency { Name = "IRR", FullName = "Iranian Rial" }
                , new Currency { Name = "ISK", FullName = "Iceland Krona" }
                , new Currency { Name = "JMD", FullName = "Jamaican Dollar" }
                , new Currency { Name = "JOD", FullName = "Jordanian Dinar" }
                , new Currency { Name = "JPY", FullName = "Yen" }
                , new Currency { Name = "KES", FullName = "Kenyan Shilling" }
                , new Currency { Name = "KGS", FullName = "Som" }
                , new Currency { Name = "KHR", FullName = "Riel" }
                , new Currency { Name = "KMF", FullName = "Comoro Franc" }
                , new Currency { Name = "KPW", FullName = "North Korean Won" }
                , new Currency { Name = "KRW", FullName = "Won" }
                , new Currency { Name = "KWD", FullName = "Kuwaiti Dinar" }
                , new Currency { Name = "KYD", FullName = "Cayman Islands Dollar" }
                , new Currency { Name = "KZT", FullName = "Tenge" }
                , new Currency { Name = "LAK", FullName = "Kip" }
                , new Currency { Name = "LBP", FullName = "Lebanese Pound" }
                , new Currency { Name = "LKR", FullName = "Sri Lanka Rupee" }
                , new Currency { Name = "LRD", FullName = "Liberian Dollar" }
                , new Currency { Name = "LSL", FullName = "Loti" }
                , new Currency { Name = "LTL", FullName = "Lithuanian Litas" }
                , new Currency { Name = "LVL", FullName = "Latvian Lats" }
                , new Currency { Name = "LYD", FullName = "Libyan Dinar" }
                , new Currency { Name = "MAD", FullName = "Moroccan Dirham" }
                , new Currency { Name = "MDL", FullName = "Moldovan Leu" }
                , new Currency { Name = "MGA", FullName = "Malagasy Ariary" }
                , new Currency { Name = "MKD", FullName = "Denar" }
                , new Currency { Name = "MMK", FullName = "Kyat" }
                , new Currency { Name = "MNT", FullName = "Tugrik" }
                , new Currency { Name = "MOP", FullName = "Pataca" }
                , new Currency { Name = "MRO", FullName = "Ouguiya" }
                , new Currency { Name = "MUR", FullName = "Mauritius Rupee" }
                , new Currency { Name = "MVR", FullName = "Rufiyaa" }
                , new Currency { Name = "MWK", FullName = "Kwacha" }
                , new Currency { Name = "MXN", FullName = "Mexican Peso" }
                , new Currency { Name = "MYR", FullName = "Malaysian Ringgit" }
                , new Currency { Name = "MZN", FullName = "Metical" }
                , new Currency { Name = "NAD", FullName = "Namibia Dollar" }
                , new Currency { Name = "NGN", FullName = "Naira" }
                , new Currency { Name = "NIO", FullName = "Cordoba Oro" }
                , new Currency { Name = "NOK", FullName = "Norwegian Krone" }
                , new Currency { Name = "NPR", FullName = "Nepalese Rupee" }
                , new Currency { Name = "NZD", FullName = "New Zealand Dollar" }
                , new Currency { Name = "OMR", FullName = "Rial Omani" }
                , new Currency { Name = "PAB", FullName = "Balboa" }
                , new Currency { Name = "PEN", FullName = "Nuevo Sol" }
                , new Currency { Name = "PGK", FullName = "Kina" }
                , new Currency { Name = "PHP", FullName = "Philippine Peso" }
                , new Currency { Name = "PKR", FullName = "Pakistan Rupee" }
                , new Currency { Name = "PLN", FullName = "Zloty" }
                , new Currency { Name = "PYG", FullName = "Guarani" }
                , new Currency { Name = "QAR", FullName = "Qatari Rial" }
                , new Currency { Name = "RON", FullName = "New Leu" }
                , new Currency { Name = "RSD", FullName = "Serbian Dinar" }
                , new Currency { Name = "RUB", FullName = "Russian Ruble" }
                , new Currency { Name = "RWF", FullName = "Rwanda Franc" }
                , new Currency { Name = "SAR", FullName = "Saudi Riyal" }
                , new Currency { Name = "SBD", FullName = "Solomon Islands Dollar" }
                , new Currency { Name = "SCR", FullName = "Seychelles Rupee" }
                , new Currency { Name = "SDG", FullName = "Sudanese Pound" }
                , new Currency { Name = "SEK", FullName = "Swedish Krona" }
                , new Currency { Name = "SGD", FullName = "Singapore Dollar" }
                , new Currency { Name = "SHP", FullName = "Saint Helena Pound" }
                , new Currency { Name = "SLL", FullName = "Leone" }
                , new Currency { Name = "SOS", FullName = "Somali Shilling" }
                , new Currency { Name = "SRD", FullName = "Surinam Dollar" }
                , new Currency { Name = "STD", FullName = "Dobra" }
                , new Currency { Name = "SVC", FullName = "El Salvador Colon" }
                , new Currency { Name = "SYP", FullName = "Syrian Pound" }
                , new Currency { Name = "SZL", FullName = "Lilangeni" }
                , new Currency { Name = "THB", FullName = "Baht" }
                , new Currency { Name = "TJS", FullName = "Somoni" }
                , new Currency { Name = "TND", FullName = "Tunisian Dinar" }
                , new Currency { Name = "TOP", FullName = "Pa'anga" }
                , new Currency { Name = "TRY", FullName = "Turkish Lira" }
                , new Currency { Name = "TTD", FullName = "Trinidad and Tobago Dollar" }
                , new Currency { Name = "TWD", FullName = "New Taiwan Dollar" }
                , new Currency { Name = "TZS", FullName = "Tanzanian Shilling" }
                , new Currency { Name = "UAH", FullName = "Hryvnia" }
                , new Currency { Name = "UGX", FullName = "Uganda Shilling" }
                , new Currency { Name = "USD", FullName = "US Dollar" }
                , new Currency { Name = "UYU", FullName = "Peso Uruguayo" }
                , new Currency { Name = "UZS", FullName = "Uzbekistan Sum" }
                , new Currency { Name = "VED", FullName = "Venezuelan digital bolivar" }
                , new Currency { Name = "VEF", FullName = "Bolivar Fuerte" }
                , new Currency { Name = "VES", FullName = "Bolivar Soberano" }
                , new Currency { Name = "VND", FullName = "Dong" }
                , new Currency { Name = "VUV", FullName = "Vatu" }
                , new Currency { Name = "WST", FullName = "Tala" }
                , new Currency { Name = "XAF", FullName = "CFA Franc BEAC" }
                , new Currency { Name = "XCD", FullName = "East Caribbean Dollar" }
                , new Currency { Name = "XOF", FullName = "CFA Franc BCEAO" }
                , new Currency { Name = "XPF", FullName = "CFP Franc" }
                , new Currency { Name = "YER", FullName = "Yemeni Rial" }
                , new Currency { Name = "ZAR", FullName = "Rand" }
                , new Currency { Name = "ZMK", FullName = "Zambian Kwacha" }
                , new Currency { Name = "ZWD", FullName = "Zimbabwe Dollar" } };
            currencies = currencies.OrderBy(p => p.Name).ToList(); //to make sure CAD is always getting Id of 26 for seeding the first Rate
            dbContext.AddRange(currencies);
            #endregion

            #region IdentityRoles
            var adminRole = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" };
            var partnerRole = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Partner", NormalizedName = "PARTNER" };
            var storeOwnerRole = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "StoreOwner", NormalizedName = "STOREOWNER" };
            var clientRole = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Client", NormalizedName = "CLIENT" };

            dbContext.Roles.Add(adminRole);
            dbContext.Roles.Add(partnerRole);
            dbContext.Roles.Add(storeOwnerRole);
            dbContext.Roles.Add(clientRole);
            #endregion

            #region ApplicationUsers
            var adminUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@makta.org",
                NormalizedEmail = "ADMIN@Makta.Org",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "admin"),
                SecurityStamp = string.Empty,
                ChangedPassword = true,
                CountryId = 1,
                PhoneNumber = "111"
            };

            dbContext.Users.Add(adminUser);
            #endregion

            #region IdentityUserRoles
            var userRoleAdmin = new ApplicationUserRole { UserId = adminUser.Id, RoleId = adminRole.Id };

            dbContext.UserRoles.Add(userRoleAdmin);
            #endregion

            #region Rate
            var cadPoints = new Rate
            {
                CurrencyId = 26, //CAD
                Points = 30,
                SpentAmount = 1
            };

            dbContext.Add(cadPoints);
            #endregion

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
