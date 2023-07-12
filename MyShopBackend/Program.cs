using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShopBackend.Data;

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

var app = builder.Build();

app.UseCors(policy =>
{
    policy
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin();
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
app.MapPost("/update_product_fb", UpdateProductFromBody);
app.MapPost("/update_product_fq", UpdateProductFromQuery);

app.Run();


Task<Product[]> GetProducts(AppDbContext dbContext)
{
    return dbContext.Products.ToArrayAsync();
}
async Task<IResult> GetProductById([FromQuery] Guid id, [FromServices] AppDbContext dbContext)
{
    var product = await dbContext.Products.FirstAsync(p => p.Id == id);
    if(product == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(product);
}

async Task AddProduct([FromBody] Product product, [FromServices] AppDbContext dbContext)
{
    await dbContext.Products.AddAsync(product);
    await dbContext.SaveChangesAsync();
}

async Task<IResult> RemoveProduct([FromQuery] Guid id, [FromServices] AppDbContext dbContext)
{
    var product = await dbContext.Products.FirstAsync(p => p.Id == id);
    if (product == null)
    {
        return Results.NotFound();
    }   
    dbContext.Products.Remove(product);
    await dbContext.SaveChangesAsync();
    return Results.Ok();
}

async Task UpdateProductFromBody([FromBody] Product updatedProduct, 
                                 [FromServices] AppDbContext dbContext)
{
    dbContext.Products.Update(updatedProduct);
    await dbContext.SaveChangesAsync();
}

async Task<IResult> UpdateProductFromQuery([FromQuery] Guid productId,
                                  [FromQuery] string name,
                                  [FromQuery] decimal price, 
                                  [FromServices] AppDbContext dbContext)
{
    var product = await dbContext.Products.FirstAsync(p => p.Id == productId);
    if (product == null)
    {
        return Results.NotFound();
    }
    product.Price = price;
    product.Name = name;
    dbContext.Products.Update(product);
    await dbContext.SaveChangesAsync();
    return Results.Ok();
}