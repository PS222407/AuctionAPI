using AuctionAPI_10_Api.RequestModels;
using FluentValidation;

namespace AuctionAPI_10_Api.Validators;

public class BidRequestValidator : AbstractValidator<BidRequest>
{
    public BidRequestValidator()
    {
        RuleFor(x => x.AuctionId).GreaterThan(0).LessThan(long.MaxValue);
        RuleFor(x => x.PriceInCents).GreaterThan(0).LessThan(int.MaxValue / 100);
    }
}