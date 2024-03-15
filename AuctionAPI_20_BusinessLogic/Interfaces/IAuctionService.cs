﻿using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Interfaces;

public interface IAuctionService
{
    public List<Auction> Get();
    
    public bool Create(Auction auction);
    
    public Auction? GetById(long id);
    
    public bool Update(Auction auction);
    
    public bool Delete(long id);
}