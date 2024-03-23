﻿using AuctionAPI_20_BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI_30_DataAccess.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Auction> Auctions { get; set; }

    public DbSet<Bid> Bids { get; set; }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}