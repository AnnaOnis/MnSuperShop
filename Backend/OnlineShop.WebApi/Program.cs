using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.EF;
using OnlineShop.Data.EF.Repositoryes;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;
using OnlineShop.IdentityPasswordHasher;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepositoryEF>();
builder.Services.AddScoped<IAccountRepository, AccountRepositoryEF>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddSingleton<IAppPasswordHasher, IdentityPasswordHasher>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();

