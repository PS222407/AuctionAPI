using System.Text;
using AuctionAPI_10_Api.Hub;
using AuctionAPI_10_Api.Middleware;
using AuctionAPI_10_Api.RequestModels;
using AuctionAPI_10_Api.Services;
using AuctionAPI_10_Api.Validators;
using AuctionAPI_20_BusinessLogic.Interfaces;
using AuctionAPI_20_BusinessLogic.Services;
using AuctionAPI_30_DataAccess.Data;
using AuctionAPI_30_DataAccess.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<IBidService, BidService>();
builder.Services.AddScoped<IBidRepository, BidRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IValidator<BidRequest>, BidRequestValidator>();
builder.Services.AddScoped<IValidator<AuctionRequest>, AuctionRequestValidator>();
builder.Services.AddScoped<IValidator<CategoryRequest>, CategoryRequestValidator>();
builder.Services.AddScoped<IValidator<ProductCreateRequest>, ProductCreateRequestValidator>();
builder.Services.AddScoped<IValidator<ProductUpdateRequest>, ProductUpdateRequestValidator>();
builder.Services.AddScoped<IValidator<UserRequest>, UserRequestValidator>();
builder.Services.AddScoped<IValidator<RefreshTokenRequest>, RefreshTokenRequestValidator>();

builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                          throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 35)),
        mySqlOptions => mySqlOptions.MigrationsAssembly("AuctionAPI_30_DataAccess")
    ));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>(),
        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Get<string>(),
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Get<string>()!)),
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins(builder.Configuration.GetValue<string>("FrontendUrl"))
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

WebApplication app = builder.Build();

app.UseCors();

app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
    RequestPath = "/api",
});

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MainHub>("/api/mainHub");

app.Run();