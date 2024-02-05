using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Services.DataInitializer
{
    public class UserDataInitializer : IDataInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserDataInitializer(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public void InitializeData()
        {
            if (!_userManager.Users.AsNoTracking().Any(p => p.UserName == "Admin"))
            {
                var user = new ApplicationUser
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserName = "admin",
                    Email = "admin@site.com"
                };
            }
        }
    }
}