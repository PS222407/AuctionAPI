using AuctionAPI_10_Api.RequestModels;
using FluentValidation;

namespace AuctionAPI_10_Api.Validators;

public class OrderRequestValidator : AbstractValidator<OrderRequest>
{
    public OrderRequestValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().GreaterThan(0);
    }
}