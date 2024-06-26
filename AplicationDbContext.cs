using Microsoft.EntityFrameworkCore;

public class AplicationDbContext : DbContext
{
    public AplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    //Este trecho de código está definindo a configuração do modelo para a entidade Product no contexto do Entity Framework Core (EF Core). Ele define o comprimento máximo e o status obrigatório para as propriedades Description, Name e Code da entidade Product. O método OnModelCreating está sendo sobrescrito para personalizar a configuração do modelo.
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>().Property(p => p.Description).HasMaxLength(500).IsRequired(false);
        builder.Entity<Product>().Property(p => p.Name).HasMaxLength(120).IsRequired();
        builder.Entity<Product>().Property(p => p.Code).HasMaxLength(20).IsRequired();
        builder.Entity<Category>().ToTable("Categories");
    }

    //docker pull mcr.microsoft.com/mssql/server:2022-latest
    //docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=@Sql2022" -p 1433:1433 --name sqlserver20221 --hostname sql1 -d mcr.microsoft.com/mssql/server:2022-latest

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=Producuts;User Id=sa;Password=@Sql2022;MultipleActiveResultSets=true;Encrypt=YES;TrustServerCertificate=YES;");
    }
}