using AuctionAPI_20_BusinessLogic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI_30_DataAccess.Data;

public class DataContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }
    
    public DbSet<Auction> Auctions { get; set; }
    
    public DbSet<Bid> Bids { get; set; }
}