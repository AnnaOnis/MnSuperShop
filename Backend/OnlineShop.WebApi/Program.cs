using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.EF;
using OnlineShop.Data.EF.Repositoryes;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbPath = "myapp.db";
builder.Services.AddDbContext<AppDbContext>(
   options => options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepositoryEF>();
builder.Services.AddScoped<IAccountRepository, AccountRepositoryEF>();
builder.Services.AddScoped<AccountService>();

var app = builder.Build();

app.UseCors(policy =>
{
    policy
        .WithOrigins("https://localhost:5001")
        .AllowAnyHeader()
        .AllowAnyMethod();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.MapGet("/get_products", GetProducts);
//app.MapGet("/get_product", GetProductById);
//app.MapPost("/add_product", AddProduct);
//app.MapPost("/remove_product", RemoveProduct);
//app.MapPost("/update_product", UpdateProduct);

app.Run();

