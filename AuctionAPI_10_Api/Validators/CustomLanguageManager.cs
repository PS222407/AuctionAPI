using System.Text.Json;
using FluentValidation.Resources;

namespace AuctionAPI_10_Api.Validators;

public class CustomLanguageManager : LanguageManager
{
    public CustomLanguageManager()
    {
        AddTranslation("en", "NotNullValidator", JsonSerializer.Serialize(new { key = "validation.required", propertyName = "{PropertyName}" }));
        AddTranslation("en", "NotEmptyValidator", JsonSerializer.Serialize(new { key = "validation.required", propertyName = "{PropertyName}" }));
        AddTranslation("en", "MaximumLengthValidator", JsonSerializer.Serialize(new { key = "validation.max_length", propertyName = "{PropertyName}", maxLength = "{MaxLength}", totalLength = "{TotalLength}" }));
        AddTranslation("en", "GreaterThanValidator", JsonSerializer.Serialize(new { key = "validation.number_min", propertyName = "{PropertyName}", minValue = "{ComparisonValue}" }));
    }
}