using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MVCCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCCore.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider isp)
        {
            using (var context = isp.GetRequiredService<ApplicationDbContext>())
            {
                await context.Database.MigrateAsync();

                // Look for any member.
                if (!context.Message.Where(x => x.Email == "ali.zarabi@gmail.com").Any())
                {
                    // DB has been seeded
                    context.Message.Add(new Message
                    {
                        Email = "ali.zarabi@gmail.com",
                        Title = "Hello",
                        Content = "Test Content"
                    });

                    context.SaveChanges();
                }

                var userManager = isp.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = isp.GetRequiredService<RoleManager<IdentityRole>>();

                if (!roleManager.Roles.Where(r => r.Name == "Users").Any())
                    await roleManager.CreateAsync(new IdentityRole() { Name = "Users" });
                if (!roleManager.Roles.Where(r => r.Name == "Admin").Any())
                    await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                if (!roleManager.Roles.Where(r => r.Name == "SuperAdmin").Any())
                    await roleManager.CreateAsync(new IdentityRole() { Name = "SuperAdmin" });

                if (!userManager.Users.Where(u => u.UserName == "ali.zarabi@gmail.com").Any())
                {
                    var sAdmin = new ApplicationUser()
                    {
                        UserName = "ali.zarabi@gmail.com",
                        Email = "ali.zarabi@gmail.com"
                    };

                    await userManager.CreateAsync(sAdmin, "123456");
                    await userManager.AddToRolesAsync(sAdmin, new string[] { "Users", "Admin", "SuperAdmin" });
                }

            }
        }
    }
}
