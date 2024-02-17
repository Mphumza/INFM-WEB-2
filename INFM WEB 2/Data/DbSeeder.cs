using Microsoft.AspNetCore.Identity;
using System.Data;
using INFM_WEB_2.Constants;

namespace INFM_WEB_2.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();

            // Adding some roles to Db
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // Create admin user
            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@techserve.com",
                EmailConfirmed = true,
            };

            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb == null)
            {
                await userMgr.CreateAsync(admin, "Admin$$9901");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }
    }
}
