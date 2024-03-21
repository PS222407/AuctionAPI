using AuctionAPI_10_Api.RequestModels;
using FluentValidation;

namespace AuctionAPI_10_Api.Validators;

public class AuctionRequestValidator : AbstractValidator<AuctionRequest>
{
    public AuctionRequestValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(0).LessThan(long.MaxValue);
        RuleFor(x => x.DurationInSeconds).GreaterThan(0).LessThan(int.MaxValue);
        RuleFor(x => x.StartDateTime).NotNull().GreaterThan(DateTime.Parse("01/01/0001 00:00:01"));
    }
}