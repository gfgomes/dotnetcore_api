using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AplicationDbContext>();

var app = builder.Build();
var configuration = app.Configuration;

//Console.WriteLine(configuration["Database:SqlServer"]);

app.MapGet("/", () => "Hello World 3!");
app.MapGet("/user", () => new { name = "John", age = 30 });

app.MapGet("/addheader", (HttpResponse response) =>
{
    response.Headers.Append("user", "ggomes");

    return new { name = "Gabriel", age = 36 };

});

//{ "Code":1, "Name": "HD SSD"}
app.MapPost("/saveproduct", (Product product) =>
{
    return $"Produto {product.Code} {product.Name} salvo com sucesso!";
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
