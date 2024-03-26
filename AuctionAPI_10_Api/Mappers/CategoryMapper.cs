using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_10_Api.Mappers;

public static class CategoryMapper
{
    public static CategoryViewModel MapToViewModel(Category category, IConfiguration configuration)
    {
        if (category.Products != null)
        {
            category.Products.ForEach(p => p.Category = null);
        }

        return new CategoryViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Icon = category.Icon,
            Products = category.Products?.Select(p => ProductMapper.MapToViewModel(p, configuration)).ToList(),
        };
    }
}