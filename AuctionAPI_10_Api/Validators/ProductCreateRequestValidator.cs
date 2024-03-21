using AuctionAPI_10_Api.RequestModels;
using FluentValidation;

namespace AuctionAPI_10_Api.Validators;

public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
{
    public ProductCreateRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1_000_000);
        RuleFor(x => x.CategoryId).NotNull().GreaterThan(0).LessThan(long.MaxValue);
        RuleFor(x => x.Image).NotNull();
    }
}