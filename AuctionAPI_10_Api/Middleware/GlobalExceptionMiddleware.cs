using MySqlConnector;

namespace AuctionAPI_10_Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    
    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InvalidOperationException e) when (e.InnerException is MySqlException && e.InnerException.Message == "Unable to connect to any of the specified MySQL hosts.")
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                Message = "Database is offline",
                Status = 500,
            });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                Message = "Server error",
                Status = 500,
            });
        }
    }
}