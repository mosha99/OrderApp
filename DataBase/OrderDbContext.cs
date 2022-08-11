using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase;

public class OrderDbContext : DbContext
{
    public DbSet<Order> orders { get; set; }
    public DbSet<Person> people { get; set; }
    public DbSet<Product> products { get; set; }
    public DbSet<OrderItem> orderItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.;Database=OrderDb;Trusted_Connection=True;");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(e =>
        {
            e.HasKey(p => p.OrderId);
            //e.HasOne(p => p.Orderperson).WithMany().HasForeignKey(p => p.OrderpersonId);
            //e.HasMany(x => x.orderItems).WithOne().HasForeignKey(p => p.ItemOrderId);

        });
        modelBuilder.Entity<Person>(e =>
        {
            e.HasKey(p => p.PersonId);
        });
        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(p => p.ProductId);
            //e.HasMany(x => x.orderItems).WithOne().HasForeignKey(p => p.ItemProductId);


        });
        modelBuilder.Entity<OrderItem>(e =>
        {
            e.HasKey(p => p.OrderItemId);
            //e.HasOne(x => x.Itemorder).WithMany().HasForeignKey(p => p.ItemOrderId);
            //e.HasOne(x => x.Itemproduct).WithMany().HasForeignKey(p => p.ItemProductId);
        });

        base.OnModelCreating(modelBuilder);
    }
}
