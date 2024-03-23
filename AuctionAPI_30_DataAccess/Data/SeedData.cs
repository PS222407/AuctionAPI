using AuctionAPI_20_BusinessLogic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace AuctionAPI_30_DataAccess.Data;

public class SeedData
{
    private readonly DataContext _dbContext;

    public SeedData(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ResetDatabaseAndSeed()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.MigrateAsync();
        _dbContext.ChangeTracker.Clear();

        try
        {
            SeedUsersAndRoles();
            SeedCategories();
            SeedProducts();
            await _dbContext.SaveChangesAsync();
            SeedAuctions();
            SeedBids();

            await _dbContext.SaveChangesAsync();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void SeedUsersAndRoles()
    {
        _dbContext.Users.AddRange(
            new User
            {
                Id = "0206A018-5AC6-492D-AB99-10105193D384",
                Email = "admin@gmail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Password123!"),
            },
            new User
            {
                Id = "3FEF01FF-C53F-43B1-96BE-9D806DEC8652",
                Email = "employee@gmail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Password123!"),
            }
        );
        
        _dbContext.Roles.AddRange(
            new Role
            {
                Id = "8977148E-C765-410F-9A58-0C7D054E4536", Name = "Admin",
            },
            new Role
            {
                Id = "81659B09-5665-4E61-ACB9-5C43E28BE6A4", Name = "Employee",
            }
        );

        _dbContext.SaveChanges();
        
        List<User> users = _dbContext.Users.ToList();
        List<Role> roles = _dbContext.Roles.ToList();
        users.Find(u => u.Email == "admin@gmail.com")!.Roles.Add(roles.First(r => r.Name == "Admin"));
        users.Find(u => u.Email == "employee@gmail.com")!.Roles.Add(roles.First(r => r.Name == "Employee"));
    }

    private void SeedCategories()
    {
        _dbContext.Categories.AddRange(
            new Category
            {
                Id = 1,
                Name = "Horeca",
                Icon =
                    "<svg xmlns=\"http://www.w3.org/2000/svg\" class=\"icon icon-tabler icon-tabler-tools-kitchen-2\" width=\"44\" height=\"44\" viewBox=\"0 0 24 24\" stroke-width=\"1.5\" stroke=\"#2c3e50\" fill=\"none\" stroke-linecap=\"round\" stroke-linejoin=\"round\">\n  <path stroke=\"none\" d=\"M0 0h24v24H0z\" fill=\"none\"/>\n  <path d=\"M19 3v12h-5c-.023 -3.681 .184 -7.406 5 -12zm0 12v6h-1v-3m-10 -14v17m-3 -17v3a3 3 0 1 0 6 0v-3\" />\n</svg>",
            },
            new Category
            {
                Id = 2,
                Name = "Gadgets",
                Icon =
                    "<svg xmlns=\"http://www.w3.org/2000/svg\" class=\"icon icon-tabler icon-tabler-fidget-spinner\" width=\"44\" height=\"44\" viewBox=\"0 0 24 24\" stroke-width=\"1.5\" stroke=\"#2c3e50\" fill=\"none\" stroke-linecap=\"round\" stroke-linejoin=\"round\">\n  <path stroke=\"none\" d=\"M0 0h24v24H0z\" fill=\"none\"/>\n  <path d=\"M18 16v.01\" />\n  <path d=\"M6 16v.01\" />\n  <path d=\"M12 5v.01\" />\n  <path d=\"M12 12v.01\" />\n  <path d=\"M12 1a4 4 0 0 1 2.001 7.464l.001 .072a3.998 3.998 0 0 1 1.987 3.758l.22 .128a3.978 3.978 0 0 1 1.591 -.417l.2 -.005a4 4 0 1 1 -3.994 3.77l-.28 -.16c-.522 .25 -1.108 .39 -1.726 .39c-.619 0 -1.205 -.14 -1.728 -.391l-.279 .16l.007 .231a4 4 0 1 1 -2.212 -3.579l.222 -.129a3.998 3.998 0 0 1 1.988 -3.756l.002 -.071a4 4 0 0 1 -1.995 -3.265l-.005 -.2a4 4 0 0 1 4 -4z\" />\n</svg>",
            },
            new Category
            {
                Id = 3,
                Name = "Food",
                Icon =
                    "<svg xmlns=\"http://www.w3.org/2000/svg\" class=\"icon icon-tabler icon-tabler-chef-hat\" width=\"44\" height=\"44\" viewBox=\"0 0 24 24\" stroke-width=\"1.5\" stroke=\"#2c3e50\" fill=\"none\" stroke-linecap=\"round\" stroke-linejoin=\"round\">\n  <path stroke=\"none\" d=\"M0 0h24v24H0z\" fill=\"none\"/>\n  <path d=\"M12 3c1.918 0 3.52 1.35 3.91 3.151a4 4 0 0 1 2.09 7.723l0 7.126h-12v-7.126a4 4 0 1 1 2.092 -7.723a4 4 0 0 1 3.908 -3.151z\" />\n  <path d=\"M6.161 17.009l11.839 -.009\" />\n</svg>",
            }
        );
    }

    private void SeedProducts()
    {
        _dbContext.Products.AddRange(
            new Product
            {
                Name = "Cutlery Set",
                Description = "A set of cutlery",
                ImageIsExternal = true,
                ImageUrl =
                    "https://www.ikea.com/nl/en/images/products/gamman-24-piece-cutlery-set-stainless-steel__0713267_pe729383_s5.jpg?f=s",
                CategoryId = 1,
            },
            new Product
            {
                Name = "Afzuigkap compleet met motor + LED verlichting | 1530m3 per uur | 900x560x250(h)mm",
                Description =
                    "Afzuigkap compleet met motor \n\nAfmeting 900x560x250(h)mm\n\nMateriaal RVS 430\n\nSpanning 230V\n\nFrequentie 50Hz\n\nEl.vermogen (kW) 0,246\n\nAf. aansluitmond mm 200\n\nCap. m3 lucht/uur 1530\n\nAfm. filters 2x 344x356(h)mm 1x 206x356(h)mm\n\nLED verlichting 2x3\n\nAantal filters 3\n\nGeluidsniveau (dB) 65\n\nType filter RVS lamellen\n\nMet regelaar\n\nMet motor \n\nGewicht 33 kg ",
                ImageIsExternal = true,
                ImageUrl =
                    "https://cdn.webshopapp.com/shops/291748/files/339546304/1000x1000x2/combisteel-afzuigkap-compleet-met-motor-led-verlic.webp",
                CategoryId = 1,
            },
            new Product
            {
                Name = "Fidget Spinner",
                Description = "A fidget spinner",
                ImageIsExternal = true,
                ImageUrl =
                    "https://cdn.webshopapp.com/shops/44056/files/334148720/890x820x2/fidget-fidget-spinner-classic-red.jpg",
                CategoryId = 2,
            },
            new Product
            {
                Name = "Fidget Cube",
                Description = "A fidget cube",
                ImageIsExternal = true,
                ImageUrl = "https://image.smythstoys.com/original/desktop/197796_7.jpg",
                CategoryId = 2,
            },
            new Product
            {
                Name = "Pizza",
                Description = "A delicious pizza",
                ImageIsExternal = true,
                ImageUrl =
                    "https://www.moulinex-me.com/medias/?context=bWFzdGVyfHJvb3R8MTQzNTExfGltYWdlL2pwZWd8aGNlL2hmZC8xNTk2ODYyNTc4NjkxMC5qcGd8MmYwYzQ4YTg0MTgzNmVjYTZkMWZkZWZmMDdlMWFlMjRhOGIxMTQ2MTZkNDk4ZDU3ZjlkNDk2MzMzNDA5OWY3OA",
                CategoryId = 3,
            },
            new Product
            {
                Name = "Burger",
                Description = "A delicious burger",
                ImageIsExternal = true,
                ImageUrl = "https://www.outofhome-shops.nl/files/202202/dist/3d91e961ea0f0abc6ee29aabe8dddc10.jpg",
                CategoryId = 3,
            },
            new Product
            {
                Name = "Pasta",
                Description = "A delicious pasta",
                ImageIsExternal = true,
                ImageUrl = "https://www.culy.nl/wp-content/uploads/2023/09/3_pasta-all-assassina-recept-1024x683.jpg",
                CategoryId = 3,
            }
        );
    }

    private void SeedAuctions()
    {
        _dbContext.Auctions.AddRange(
            new Auction
            {
                Id = 1,
                ProductId = 1,
                StartDateTime = DateTime.Parse("2023-09-01T12:00:00"),
                DurationInSeconds = 3600,
            },
            new Auction
            {
                Id = 2,
                ProductId = 2,
                StartDateTime = DateTime.Parse("2023-09-01T12:00:00"),
                DurationInSeconds = 3600,
            },
            new Auction
            {
                Id = 3,
                ProductId = 3,
                StartDateTime = DateTime.Parse("2023-09-01T12:00:00"),
                DurationInSeconds = 3600,
            }
        );
    }

    private void SeedBids()
    {
        _dbContext.Bids.AddRange(
            new Bid
            {
                Id = 1,
                UserId = "3FEF01FF-C53F-43B1-96BE-9D806DEC8652",
                AuctionId = 1,
                PriceInCents = 1000,
                CreatedAt = DateTime.Parse("2023-09-01T12:00:00"),
            },
            new Bid
            {
                Id = 2,
                UserId = "3FEF01FF-C53F-43B1-96BE-9D806DEC8652",
                AuctionId = 1,
                PriceInCents = 1100,
                CreatedAt = DateTime.Parse("2023-09-01T12:00:00"),
            },
            new Bid
            {
                Id = 3,
                UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                AuctionId = 1,
                PriceInCents = 1200,
                CreatedAt = DateTime.Parse("2023-09-01T12:00:00"),
            },
            new Bid
            {
                Id = 4,
                UserId = "3FEF01FF-C53F-43B1-96BE-9D806DEC8652",
                AuctionId = 1,
                PriceInCents = 1300,
                CreatedAt = DateTime.Parse("2023-09-01T12:00:00"),
            },
            new Bid
            {
                Id = 5,
                UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                AuctionId = 1,
                PriceInCents = 1400,
                CreatedAt = DateTime.Parse("2023-09-01T12:00:00"),
            },
            new Bid
            {
                Id = 6,
                UserId = "3FEF01FF-C53F-43B1-96BE-9D806DEC8652",
                AuctionId = 1,
                PriceInCents = 1500,
                CreatedAt = DateTime.Parse("2023-09-01T12:00:00"),
            },
            new Bid
            {
                Id = 7,
                UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                AuctionId = 1,
                PriceInCents = 1600,
                CreatedAt = DateTime.Parse("2023-09-01T12:00:00"),
            },
            new Bid
            {
                Id = 8,
                UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                AuctionId = 1,
                PriceInCents = 1700,
                CreatedAt = DateTime.Parse("2023-09-01T12:00:00"),
            },
            new Bid
            {
                Id = 9,
                UserId = "3FEF01FF-C53F-43B1-96BE-9D806DEC8652",
                AuctionId = 1,
                PriceInCents = 1800,
                CreatedAt = DateTime.Parse("2023-09-01T12:00:00"),
            },
            new Bid
            {
                Id = 10,
                UserId = "3FEF01FF-C53F-43B1-96BE-9D806DEC8652",
                AuctionId = 2,
                PriceInCents = 1000,
                CreatedAt = DateTime.Parse("2023-09-01T12:00:00"),
            },
            new Bid
            {
                Id = 11,
                UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                AuctionId = 2,
                PriceInCents = 1100,
                CreatedAt = DateTime.Parse("2023-09-01T12:00:00"),
            },
            new Bid
            {
                Id = 12,
                UserId = "3FEF01FF-C53F-43B1-96BE-9D806DEC8652",
                AuctionId = 3,
                PriceInCents = 1200,
                CreatedAt = DateTime.Parse("2023-09-01T12:00:00"),
            },
            new Bid
            {
                Id = 13,
                UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                AuctionId = 3,
                PriceInCents = 1300,
                CreatedAt = DateTime.Parse("2023-09-01T12:00:00"),
            },
            new Bid
            {
                Id = 14,
                UserId = "3FEF01FF-C53F-43B1-96BE-9D806DEC8652",
                AuctionId = 3,
                PriceInCents = 1400,
                CreatedAt = DateTime.Parse("2023-09-01T12:00:00"),
            },
            new Bid
            {
                Id = 15,
                UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                AuctionId = 3,
                PriceInCents = 1500,
                CreatedAt = DateTime.Parse("2023-09-01T12:00:00"),
            }
        );
    }
}