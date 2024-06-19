using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
builder.Services.AddDbContext<AplicationDbContext>();

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


public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
}


public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProductId { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public List<Tag> Tags { get; set; }

}


public class AplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    //Este trecho de código está definindo a configuração do modelo para a entidade Product no contexto do Entity Framework Core (EF Core). Ele define o comprimento máximo e o status obrigatório para as propriedades Description, Name e Code da entidade Product. O método OnModelCreating está sendo sobrescrito para personalizar a configuração do modelo.
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>().Property(p => p.Description).HasMaxLength(500).IsRequired(false);
        builder.Entity<Product>().Property(p => p.Name).HasMaxLength(120).IsRequired();
        builder.Entity<Product>().Property(p => p.Code).HasMaxLength(20).IsRequired();
    }


    //docker pull mcr.microsoft.com/mssql/server:2022-latest
    //docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=@Sql2022" -p 1433:1433 --name sqlserver20221 --hostname sql1 -d mcr.microsoft.com/mssql/server:2022-latest

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=Producuts;User Id=sa;Password=@Sql2022;MultipleActiveResultSets=true;Encrypt=YES;TrustServerCertificate=YES;");
    }
}