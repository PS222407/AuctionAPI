using System.Net;
using AuctionAPI_20_BusinessLogic.Exceptions;
using MySqlConnector;
using NuGet.ProjectModel;

namespace AuctionAPI_10_Api.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (InvalidOperationException e) when (e.InnerException is MySqlException &&
                                                  e.InnerException.Message ==
                                                  "Unable to connect to any of the specified MySQL hosts.")
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                Message = "Database is offline",
                Status = context.Response.StatusCode,
            });
        }
        catch (AuctionNotAvailableException e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync(new
            {
                e.Message,
                Status = context.Response.StatusCode,
            });
        }
        catch (BidTooLowException e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                e.Message,
                Status = context.Response.StatusCode,
            });
        }
        catch (FileFormatException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                Message = "Server error",
                Status = context.Response.StatusCode,
                Errors = new
                {
                    Image = new List<string> { "File is not an image" },
                },
            });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                Message = "Server error",
                Status = context.Response.StatusCode,
            });
        }
    }
}