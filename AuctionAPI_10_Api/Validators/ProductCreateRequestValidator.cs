using AuctionAPI_10_Api.RequestModels;
using FluentValidation;

namespace AuctionAPI_10_Api.Validators;

public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
{
    public ProductCreateRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().MaximumLength(255);
        RuleFor(x => x.PriceInCents).NotNull().GreaterThan(0).LessThan(int.MaxValue);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1_000_000);
        RuleFor(x => x.CategoryId).NotNull().GreaterThan(0).LessThan(long.MaxValue).WithName("Category");
        RuleFor(x => x.Image).NotNull();
    }
}