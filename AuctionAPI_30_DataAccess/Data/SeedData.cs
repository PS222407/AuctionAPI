using AuctionAPI_20_BusinessLogic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionAPI_30_DataAccess.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using DataContext context = new(serviceProvider.GetRequiredService<DbContextOptions<DataContext>>());

        try
        {
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        Name = "Product 1",
                    }
                );
            }

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new IdentityUser
                    {
                        Id = "0206A018-5AC6-492D-AB99-10105193D384",
                        Email = "jensramakers@gmail.com",
                        UserName = "jens ramakers",
                        PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "Pa$$w0rd!"),
                    }
                );
            }

            if (!context.UserRoles.Any())
            {
                IdentityRole roleAdmin = context.Roles.First(r => r.Name == "Admin");
                context.UserRoles.Add(new IdentityUserRole<string>
                {
                    UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                    RoleId = roleAdmin.Id
                });
            }
            
            context.SaveChanges();
        }
        catch (MySqlConnector.MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}