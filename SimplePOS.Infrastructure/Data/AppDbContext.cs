using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimplePOS.Domain.Entities;
using SimplePOS.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Infrastructure.Data
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        public DbSet<Category> Category { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Sale> Sale { get; set; }
        public DbSet<SaleItem> SaleItem { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Category - Product (Uno a muchos)
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            //Client - Sale (Uno a muchos)
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Sales)
                .WithOne(s => s.Client)
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            //Sale - SaleItem (uno a muchos)
            modelBuilder.Entity<Sale>()
                .HasMany(s => s.SaleItem)
                .WithOne(si => si.Sale)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            //SaleItem - Product (muchos a uno)
            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.Product)
                .WithMany(p => p.SaleItem)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            //Ignorar la propiedad que calcula el subtotal en SaleItem
            modelBuilder.Entity<SaleItem>()
                .Ignore(si => si.SubTotal);



            //Configuraciones adicionales


            //Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("varchar(100)");
            });


            //Client 
            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("varchar(100)");

                entity.Property(c => c.Document)
                    .HasMaxLength(100)
                    .HasColumnType("varchar(20)");

                entity.Property(c => c.Email)
                    .HasMaxLength(100)
                    .HasColumnType("varchar(100)");

                entity.Property(c => c.PhoneNumber)
                    .HasMaxLength(20)
                    .HasColumnType("varchar(20)");
            });


            //Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("varchar(150)");
                
                entity.Property(p => p.Description)
                    .HasMaxLength(500)
                    .HasColumnType("varchar(500)");

                entity.Property(p => p.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.Stock)
                    .IsRequired();

                entity.Property(p => p.IsActive)
                    .IsRequired();
            });


            // Sale
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.Property(s => s.Date)
                    .IsRequired();

                entity.Property(s => s.Total)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(s => s.UserId)
                    .HasMaxLength(450)   
                    .HasColumnType("varchar(450)");
            });

            // SaleItem
            modelBuilder.Entity<SaleItem>(entity =>
            {
                entity.Property(si => si.Quantity)
                    .IsRequired();

                entity.Property(si => si.UnitPrice)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
            });
        }


    }
}

