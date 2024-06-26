using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<AplicationDbContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();
var configuration = app.Configuration;

//Console.WriteLine(configuration["Database:SqlServer"]);

//{ "Code":1, "Name": "HD SSD"}
app.MapPost("/products", (ProductRequest productRequest, AplicationDbContext dbContext) =>
{
    var caregory = dbContext.Categories.Where(c => c.Id == productRequest.CategoryId).First();
    var product = new Product{
        Code = productRequest.Code,
        Name = productRequest.Name,
        Description = productRequest.Description,
        Category = caregory
    };
    dbContext.Products.Add(product);
    dbContext.SaveChanges();

    return Results.Created($"/products/{product.Id}", product);
    
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

app.MapGet("/getproducts/{code}", ([FromRoute] string code) =>
{
    return new { code };
});


app.MapGet("/getproductbyheader", (HttpRequest request) =>
{
    return request.Headers["code"].ToString();
});

app.Run();
