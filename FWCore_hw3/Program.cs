
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Json;

internal class Program
{
    static void Main(string[] args)
    {

        using (ApplicationContext db = new())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Ноутбук Lenovo IdeaPad",
                    Price = 54999.99m,
                    StockQuantity = 12,
                    Description = "15.6'' Full HD, Intel i5, 16GB RAM, SSD 512GB",
                    TemporaryData = 0
                },
                new Product
                {
                    Name = "Смартфон Samsung Galaxy S23",
                    Price = 79999.50m,
                    StockQuantity = 5,
                    Description = "6.1'' AMOLED, Snapdragon 8 Gen 2, 8GB RAM, 256GB",
                    TemporaryData = 1
                }
            };
            db.Products.AddRange(products);

            db.SaveChanges();
        }

    }
}

internal class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? Description { get; set; }
    public byte TemporaryData { get; set; }

}

internal class ApplicationContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=FWCore_hw3;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(builder =>
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Price).HasColumnType("DECIMAL(10, 2)");
            builder.Property(e => e.StockQuantity).HasDefaultValue(0);
            builder.Property(e => e.Description).IsRequired(false);
            builder.HasAlternateKey(e => e.Name);
            builder.Ignore(e => e.TemporaryData);
            builder.ToTable("StoreProducts");
            builder.ToTable(e => e.HasCheckConstraint("Price", "PRICE >= 0"));
        });
    }
}


