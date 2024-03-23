using AuctionAPI_10_Api.RequestModels;
using FluentValidation;

namespace AuctionAPI_10_Api.Validators;

public class UserRequestValidator : AbstractValidator<UserRequest>
{
    public UserRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(255);
    }
}