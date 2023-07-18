using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShopBackend.Data;
using MyShopBackend.Data.Repositoryes;

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

app.MapGet("/get_products", GetProducts);
app.MapGet("/get_product", GetProductById);
app.MapPost("/add_product", AddProduct);
app.MapPost("/remove_product", RemoveProduct);
app.MapPost("/update_product", UpdateProduct);


app.Run();


async Task<IResult> GetProducts(
                    [FromServices] IRepository<Product> repository, 
                    CancellationToken cancellationToken)
{
    try
    {
        var products = await repository.GetAll(cancellationToken);
        return Results.Ok(products);
    }
    catch (InvalidOperationException) 
    {
        return Results.NoContent();
    } 
    
}
async Task<IResult> GetProductById(
                    [FromQuery] Guid id, 
                    [FromServices] IRepository<Product> repository, 
                    CancellationToken cancellationToken)
{
    try
    {
        var product = await repository.GetById(id, cancellationToken);
        return Results.Ok(product);
    }
    catch(InvalidOperationException)
    {
        return Results.NotFound();
    }
}

async Task<IResult> AddProduct(
            [FromBody] Product product, 
            [FromServices] IRepository<Product> repository, 
            CancellationToken cancellationToken)
{
    try
    {
        await repository.Add(product, cancellationToken);
        return Results.Ok();
    }
    catch (ArgumentNullException)
    {
        return Results.NotFound();
    }
}

async Task<IResult> RemoveProduct(
                    [FromQuery] Guid id, 
                    [FromServices] IRepository<Product> repository,
                    CancellationToken cancellationToken)
{
    try
    {
        await repository.Delete(id, cancellationToken);
        return Results.Ok();
    }
    catch (InvalidOperationException)
    {
        return Results.NotFound();
    }
}

async Task<IResult> UpdateProduct([FromBody] Product updatedProduct, 
                                 [FromServices] IRepository<Product> repository,
                                 CancellationToken cancellationToken)
{
    try
    {
        await repository.Update(updatedProduct, cancellationToken);
        return Results.Ok();
    }
    catch(ArgumentNullException)
    {
        return Results.NotFound();
    }

}
