using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrintProxy.Hub.Data;
using PrintProxy.Hub.Generators;

namespace PrintProxy.Hub.Extensions
{
    public static class AdminUserExtenions
    {

        public static async Task SetupDefaultAdminUserAsync(this WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();

            var UserService = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var RoleService = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var role = await RoleService.FindByNameAsync("admin");

            if (role == null)
            {
                role = new IdentityRole()
                {
                    Name = "admin",
                    NormalizedName = "ADMIN"
                };

                await RoleService.CreateAsync(role);
            }

            if (!UserService.Users.Any(u => u.UserName == "admin"))
            {
                

                ApplicationUser user = new()
                {
                    UserName = "admin",
                    Email = "admin@printproxy.local",
                    EmailConfirmed = true
                };

                string password = string.Empty;
                IdentityResult result = null!;

                do
                {
                    password = RandomPasswordGenerator.GeneratePassword(8);
                    result = await UserService.CreateAsync(user, password);
                } while (!result.Succeeded);

                var adminuser = await UserService.Users.Where(u => u.UserName == "admin").FirstOrDefaultAsync();

                if (adminuser != null)
                {
                    await UserService.AddToRoleAsync(adminuser, "admin");
                }

                app.Logger.LogInformation("######## DEFAULT ADMIN USER PASSWORD: " + password);
            }


        }
        
    }
}
