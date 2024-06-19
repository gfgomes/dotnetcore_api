using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

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

public class Product
{
    public string Code { get; set; }
    public string Name { get; set; }

}


public class AplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }


    //docker pull mcr.microsoft.com/mssql/server:2022-latest
    //docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=@Sql2022" -p 1433:1433 --name sqlserver20221 --hostname sql1 -d mcr.microsoft.com/mssql/server:2022-latest

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;");
    }
}