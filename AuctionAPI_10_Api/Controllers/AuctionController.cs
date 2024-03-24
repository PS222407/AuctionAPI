using AuctionAPI_10_Api.Mappers;
using AuctionAPI_10_Api.RequestModels;
using AuctionAPI_10_Api.ViewModels;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI_10_Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuctionController(
    IAuctionService auctionService,
    IConfiguration configuration,
    IProductService productService,
    IValidator<AuctionRequest> validator)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200)]
    public IActionResult Get()
    {
        return Ok(auctionService.Get().Select(a => AuctionMapper.MapToViewModel(a, configuration)));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public IActionResult Get([FromRoute] int id)
    {
        Auction? auction = auctionService.GetById(id);
        if (auction == null)
        {
            return NotFound();
        }

        AuctionViewModel auctionViewModel = AuctionMapper.MapToViewModel(auction, configuration);

        return Ok(auctionViewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public IActionResult Post([FromBody] AuctionRequest auctionRequest)
    {
        ValidationResult result = validator.Validate(auctionRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }

        if (!productService.Exists(auctionRequest.ProductId))
        {
            return BadRequest(new { Message = "Product not found" });
        }

        Auction auction = new()
        {
            ProductId = auctionRequest.ProductId,
            DurationInSeconds = auctionRequest.DurationInSeconds,
            StartDateTime = auctionRequest.StartDateTime,
        };

        Auction? createdAuction = auctionService.Create(auction);
        if (createdAuction == null)
        {
            return BadRequest(new { Message = "Auction could not be created" });
        }

        return CreatedAtAction("Get", new { id = auction.Id }, AuctionMapper.MapToViewModel(createdAuction, configuration));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public IActionResult Put([FromRoute] int id, [FromBody] AuctionRequest auctionRequest)
    {
        if (!auctionService.Exists(id))
        {
            return NotFound();
        }

        ValidationResult result = validator.Validate(auctionRequest);
        if (!result.IsValid)
        {
            return BadRequest(new { result.Errors });
        }

        if (!productService.Exists(auctionRequest.ProductId))
        {
            return BadRequest("Product not found");
        }

        Auction auction = new()
        {
            Id = id,
            ProductId = auctionRequest.ProductId,
            DurationInSeconds = auctionRequest.DurationInSeconds,
            StartDateTime = auctionRequest.StartDateTime,
        };

        auctionService.Update(auction);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult Delete(int id)
    {
        if (!auctionService.Exists(id))
        {
            return NotFound();
        }

        auctionService.Delete(id);

        return NoContent();
    }
}