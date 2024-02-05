using Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<bool> Exists(string id, string email, string phoneNumber, CancellationToken cancellationToken);
        Task<ApplicationUser> GetByEmailOrPhoneNumber(string input, CancellationToken cancellationToken);
        Task<List<ApplicationRole>> GetUserRoles(string id, CancellationToken cancellationToken);
        Task<List<ApplicationRole>> GetApplicationRoles(CancellationToken cancellationToken);
    }
}