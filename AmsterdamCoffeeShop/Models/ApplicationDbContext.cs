using Microsoft.EntityFrameworkCore;

namespace AmsterdamCoffeeShop.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            // Seed data for coffee products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Espresso", Description = "Strong coffee brewed by forcing hot water through finely-ground coffee beans.", Price = 3.50m, Category = "Hot Drinks", IsAvailable = true, ImageUrl = "/images/espresso.jpg" },
                new Product { Id = 2, Name = "Cappuccino", Description = "Espresso-based coffee drink with steamed milk foam.", Price = 4.20m, Category = "Hot Drinks", IsAvailable = true, ImageUrl = "/images/cappuccino.jpg" },
                new Product { Id = 3, Name = "Latte", Description = "Coffee drink made with espresso and steamed milk.", Price = 4.50m, Category = "Hot Drinks", IsAvailable = true, ImageUrl = "/images/latte.jpg" },
                new Product { Id = 4, Name = "Iced Coffee", Description = "Chilled coffee served with ice and optional milk.", Price = 4.00m, Category = "Cold Drinks", IsAvailable = true, ImageUrl = "/images/iced-coffee.jpg" },
                new Product { Id = 5, Name = "Americano", Description = "Diluted espresso with hot water, similar strength to drip coffee.", Price = 3.80m, Category = "Hot Drinks", IsAvailable = true, ImageUrl = "/images/americano.jpg" }
            );
        }
    }
}
