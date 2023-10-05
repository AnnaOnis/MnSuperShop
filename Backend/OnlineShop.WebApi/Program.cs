using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Data.EF;
using OnlineShop.Data.EF.Repositoryes;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;
using OnlineShop.IdentityPasswordHasher;
using OnlineShop.WebApi.Configurations;
using OnlineShop.WebApi.Sevices;

var builder = WebApplication.CreateBuilder(args);

JwtConfig jwtConfig = builder.Configuration
   .GetRequiredSection("JwtConfig")
   .Get<JwtConfig>()!;
if (jwtConfig is null)
{
    throw new InvalidOperationException("JwtConfig is not configured");
}
builder.Services.AddSingleton(jwtConfig);
builder.Services.AddSingleton<ITokenService, TokenService>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           IssuerSigningKey = new SymmetricSecurityKey(jwtConfig.SigningKeyBytes),
           ValidateIssuerSigningKey = true,
           ValidateLifetime = true,
           RequireExpirationTime = true,
           RequireSignedTokens = true,

           ValidateAudience = true,
           ValidateIssuer = true,
           ValidAudiences = new[] { jwtConfig.Audience },
           ValidIssuer = jwtConfig.Issuer
       };
   });
builder.Services.AddAuthorization();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepositoryEF>();
builder.Services.AddScoped<IAccountRepository, AccountRepositoryEF>();
builder.Services.AddScoped<ICartRepository, CartRepositoryEF>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWorkEf>();
builder.Services.AddSingleton<IAppPasswordHasher, IdentityPasswordHasher>();

builder.Services.AddScoped<AccountService>();


var dbPath = "myapp.db";
builder.Services.AddDbContext<AppDbContext>(
   options => options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestHeaders
                            | HttpLoggingFields.ResponseHeaders
                            | HttpLoggingFields.RequestBody
                            | HttpLoggingFields.ResponseBody;
});


var app = builder.Build();

app.Use(async (context, next) =>
{
    var userAgent = context.Request.Headers.UserAgent;
    if (userAgent.ToString().Contains("Edg"))
    {
        await next();
    }
    else
    {
        await context.Response.WriteAsync("You are not using MS Edge! Please, use MS Edge!");
    }

});

app.UseCors(policy =>
{
    policy
        .WithOrigins("https://localhost:5001")
        .AllowAnyHeader()
        .AllowAnyMethod();
});


// Configure the HTTP request pipeline.

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

