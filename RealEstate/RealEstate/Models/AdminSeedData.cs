using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace RealEstate.Models
{
    public class AdminSeedData
    {
        private const string AdminName="Admin";
        private const string AdminPassword="Admin_123";
        private const string AdminEmail="admin@istanbulemlak.com";
        private const string AdminRole="Admin";
        public static async void AdminUser(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<RealEstateContext>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();


                if(!await roleManager.RoleExistsAsync(AdminRole))
                {
                    await roleManager.CreateAsync(new AppRole { Name = AdminRole});
                }

                var user = await userManager.FindByNameAsync(AdminName);
                if (user == null)
                {
                    user = new AppUser { 
                        
                        UserName = AdminName,
                        Email = AdminEmail,
                };
                var result = await userManager.CreateAsync(user , AdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, AdminRole);
                }
                }
        else
        {
            if(!await userManager.IsInRoleAsync(user,AdminRole))
            {
                await userManager.AddToRoleAsync(user, AdminRole);
            }}}}}}