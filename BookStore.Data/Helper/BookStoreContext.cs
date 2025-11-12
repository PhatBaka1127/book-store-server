using BookStore.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Data.Helper
{
    public partial class BookStoreContext : DbContext
    {
        public BookStoreContext() { }
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
                IConfigurationRoot configuration = builder.Build();
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().Property(x => x.Id);
            modelBuilder.Entity<Category>().Property(x => x.Id);
            modelBuilder.Entity<User>().Property(x => x.Id);
            modelBuilder.Entity<Order>().Property(x => x.Id);
            modelBuilder.Entity<OrderDetail>().HasKey(od => new { od.OrderId, od.BookId });
            modelBuilder.Entity<Rating>().HasKey(r => new { r.BookId, r.OrderId });
            modelBuilder.Entity<Cart>().HasKey(c => new { c.BookId, c.UserId });

            modelBuilder.Entity<OrderDetail>()
                            .HasOne(od => od.Book)
                            .WithMany(b => b.OrderDetails)
                            .HasForeignKey(od => od.BookId)
                            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Cart>()
                            .HasOne(c => c.Book)
                            .WithMany(b => b.Carts)
                            .HasForeignKey(c => c.BookId)
                            .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
        }
    }
}