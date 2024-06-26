﻿using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using AuctionAPI_30_DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI_30_DataAccess.Repositories;

public class OrderRepository(DataContext dbContext) : IOrderRepository
{
    public Order? Create(Order order)
    {
        dbContext.Orders.Add(order);
        return dbContext.SaveChanges() > 0 ? order : null;
    }

    public Order? GetBy(string id)
    {
        return dbContext.Orders.Include(o => o.Product).FirstOrDefault(o => o.Id == id);
    }

    public bool Update(Order order)
    {
        dbContext.Orders.Update(order);
        return dbContext.SaveChanges() > 0;
    }

    public Order? GetByExternalPaymentId(string id)
    {
        return dbContext.Orders.FirstOrDefault(o => o.ExternalPaymentId == id);
    }

    public List<Order> GetByUserId(string userId)
    {
        return dbContext.Orders.Include(o => o.Product).Where(o => o.UserId == userId).ToList();
    }
}