using Common;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository, IScopedDependency
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        { }

        public async Task<ApplicationUser> GetByEmailOrPhoneNumber(string input, CancellationToken cancellationToken)
        {
            return await TableNoTracking.Where(p => p.Email == input || p.PhoneNumber == input || p.UserName == input).SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> Exists(string id, string email, string phoneNumber, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                return await TableNoTracking.AnyAsync(p => p.Email.ToLower() == email.ToLower() || p.PhoneNumber == phoneNumber || p.UserName.ToLower() == email.ToLower() || p.UserName == phoneNumber, cancellationToken);
            else
                return await TableNoTracking.AnyAsync(p => p.Id != id && (p.Email == email || p.PhoneNumber == phoneNumber || p.UserName.ToLower() == email.ToLower() || p.UserName == phoneNumber), cancellationToken);
        }

        public async Task<List<ApplicationRole>> GetUserRoles(string id, CancellationToken cancellationToken)
        {
            var userRoles = await DbContext.UserRoles.Where(q => q.UserId == id).Select(p => p.RoleId).ToListAsync(cancellationToken);
            return await DbContext.Roles.Where(p => userRoles.Contains(p.Id)).ToListAsync(cancellationToken);
        }

        public async Task<List<ApplicationRole>> GetApplicationRoles(CancellationToken cancellationToken)
        {
            return await DbContext.Roles.ToListAsync(cancellationToken);
        }
    }
}
