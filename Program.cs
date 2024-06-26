using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<AplicationDbContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();
var configuration = app.Configuration;


app.MapPost("/products", (ProductRequest productRequest, AplicationDbContext dbContext) =>
{
    var caregory = dbContext.Categories.Where(c => c.Id == productRequest.CategoryId).First();
    var product = new Product{
        Code = productRequest.Code,
        Name = productRequest.Name,
        Description = productRequest.Description,
        Category = caregory
    };

    if(productRequest.Tags != null){
        product.Tags = new List<Tag>();
        foreach (var tag in productRequest.Tags){
            product.Tags.Add(new Tag{Name = tag});
        }
    }
    dbContext.Products.Add(product);
    dbContext.SaveChanges();

    return Results.Created($"/products/{product.Id}", product);
    
});

app.MapGet("/products/{id}", ([FromRoute] int id, AplicationDbContext dbContext) =>
{
    var product = dbContext.Products
    .Include(p => p.Category)
    .Include(p => p.Tags)
    .Where(p => p.Id == id).First();
    if(product != null){
        return Results.Ok(product);
    }
    return Results.NotFound();
});

app.MapPut("/products/{id}", ([FromRoute] int id,ProductRequest productRequest ,AplicationDbContext dbContext) =>
{
    var product = dbContext.Products
    .Include(p => p.Tags)
    .Where(p => p.Id == id).First();

    if(product == null){    
        return Results.NotFound();
    }
    product.Code = productRequest.Code;
    product.Name = productRequest.Name;
    product.Description = productRequest.Description;
    product.Category = dbContext.Categories.Where(c => c.Id == productRequest.CategoryId).First();
    product.Tags = new List<Tag>();

    if(productRequest.Tags != null){
        product.Tags = new List<Tag>();
        foreach (var tag in productRequest.Tags){   
            product.Tags.Add(item: new Tag{Name = tag});
        }    
    }
   
    dbContext.SaveChanges();
    return Results.Ok(product);
});





app.MapGet("/", () => "Hello World 3!");
app.MapGet("/user", () => new { name = "John", age = 30 });

app.MapGet("/addheader", (HttpResponse response) =>
{
    response.Headers.Append("user", "ggomes");

    return new { name = "Gabriel", age = 36 };

});

app.MapGet("/getproducts", ([FromQuery] string name, [FromQuery] string code) =>
{
    return new { name, code };
});




app.MapGet("/getproductbyheader", (HttpRequest request) =>
{
    return request.Headers["code"].ToString();
});

app.Run();
