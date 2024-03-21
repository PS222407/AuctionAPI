using AuctionAPI_10_Api.RequestModels;
using FluentValidation;

namespace AuctionAPI_10_Api.Validators;

public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Icon).NotEmpty().MaximumLength(1_000_000);
    }
}