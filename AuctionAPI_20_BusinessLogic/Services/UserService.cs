﻿using AuctionAPI_20_BusinessLogic.DataModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_20_BusinessLogic.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public List<Auction> GetWonAuctions(string userId)
    {
        List<WonAuctionsDataModel> wonAuctions = userRepository.GetWonAuctions(userId);

        return wonAuctions.Select(x => new Auction
        {
            Id = x.a__id,
            ProductId = x.a__ProductId,
            StartDateTime = x.a__StartDateTime,
            DurationInSeconds = x.a__DurationInSeconds,
            Product = new Product
            {
                Id = x.p__Id,
                Name = x.p__Name,
                Description = x.p__Description,
                ImageUrl = x.p__ImageUrl,
                CategoryId = x.p__CategoryId,
                ImageIsExternal = x.p__ImageIsExternal,
            },
            Bids =
            [
                new Bid
                {
                    Id = x.b__Id,
                    AuctionId = x.b__AuctionId,
                    PriceInCents = x.b__PriceInCents,
                    CreatedAt = x.b__CreatedAt,
                    UserId = x.b__UserId,
                },
            ],
        }).ToList();
    }
}