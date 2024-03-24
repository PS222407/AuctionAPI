using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Models;

namespace AuctionAPI_10_Api.Mappers;

public static class ProductMapper
{
    public static ProductViewModel MapToViewModel(Product product, IConfiguration configuration)
    {
        if (product.Category != null)
        {
            product.Category.Products = null;
        }

        if (product.Auctions != null)
        {
            product.Auctions.ForEach(a => a.Product = null);
        }

        return new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.ImageIsExternal ? product.ImageUrl : $"{configuration["BackendUrl"]}{product.ImageUrl}",
            Category = product.Category == null ? null : CategoryMapper.MapToViewModel(product.Category, configuration),
            Auctions = product.Auctions?.Select(a => AuctionMapper.MapToViewModel(a, configuration)).ToList(),
        };
    }
}