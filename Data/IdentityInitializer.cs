
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data;

namespace BlogApp.Data
{
    public static class IdentityInitializer
    {
        public static async Task SeedRolesAndAdminAsync(
            IServiceProvider serviceProvider,
            string adminEmail,
            string adminPassword)
        {

            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
            var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<ApplicationUser>>();


            string adminRole = "Admin";
            if (await roleManager.FindByNameAsync(adminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            bool roleAssigned = false;

           
            if (adminUser == null)
            {
                adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {

                    await userManager.AddToRoleAsync(adminUser, adminRole);
                    roleAssigned = true;
                }
            }
           
            else if (!await userManager.IsInRoleAsync(adminUser, adminRole))
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
                roleAssigned = true;
            }

           
            if (roleAssigned)
            {
               
                await signInManager.RefreshSignInAsync(adminUser);
            }
        }
    }
}