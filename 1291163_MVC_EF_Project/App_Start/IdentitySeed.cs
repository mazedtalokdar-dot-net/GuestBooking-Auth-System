using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using _1291163_MVC_EF_Project.Models;

namespace _1291163_MVC_EF_Project.App_Start
{
    public static class IdentitySeed
    {
        public static void SeedRolesAndAdmin()
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // Roles
                if (!roleManager.RoleExists("Admin")) roleManager.Create(new IdentityRole("Admin"));
                if (!roleManager.RoleExists("Staff")) roleManager.Create(new IdentityRole("Staff"));

                // Create Admin user (আপনি চাইলে email/password change করতে পারেন)
                var adminEmail = "admin@hotel.com";
                var adminPass = "Admin@123";

                var adminUser = userManager.FindByEmail(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
                    userManager.Create(adminUser, adminPass);
                }

                // Add to Admin role
                if (!userManager.IsInRole(adminUser.Id, "Admin"))
                {
                    userManager.AddToRole(adminUser.Id, "Admin");
                }
            }
        }
    }
}
