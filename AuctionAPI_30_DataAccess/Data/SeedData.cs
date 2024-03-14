using System.Security.Cryptography;
using AuctionAPI_20_BusinessLogic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

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
                        Description = "Description 1",
                        ImageUrl = "https://via.placeholder.com/150",
                    }
                );
            }

            if (!context.Users.Any())
            {
                IdentityUser identityUser = new()
                {
                    Id = "54cd4b50-f61e-4e73-a0e8-f072fad42269",
                    UserName = "jensramakers@gmail.com",
                    NormalizedUserName = "JENSRAMAKERS@GMAIL.COM",
                    Email = "jensramakers@gmail.com",
                    NormalizedEmail = "JENSRAMAKERS@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEOPismypAVGGQWvtW1z7RmvLmqsE9/rBbrjTgWRHrqYO7HN+AEFz6TSRWMGf51YPZA==",
                    SecurityStamp = "ZJSHNNDGIW6S55JFM5FZVA63N35UA32M",
                    ConcurrencyStamp = "1f8b3dfd-dcc5-4d1c-a85b-21630eab2fb5",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                };

                PasswordHasher<IdentityUser> passwordHasher = new();  
                passwordHasher.HashPassword(identityUser, "Pa$$w0rd!"); 
                
                context.Users.AddRange(identityUser);
            }

            if (!context.UserRoles.Any())
            {
                IdentityRole roleAdmin = context.Roles.First(r => r.Name == "Admin");
                context.UserRoles.Add(new IdentityUserRole<string>
                {
                    UserId = "54cd4b50-f61e-4e73-a0e8-f072fad42269",
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