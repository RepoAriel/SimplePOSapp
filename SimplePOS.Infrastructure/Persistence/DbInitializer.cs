using Microsoft.AspNetCore.Identity;
using SimplePOS.Domain.Entities;
using SimplePOS.Infrastructure.Data;
using SimplePOS.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (!context.Category.Any())
            {
                var rand = new Random();
                var categoryNames = new[] { "Bebidas", "Snacks", "Electronica", "Ropa", "Libros" };
                var categories = categoryNames.Select(name => new Category { Name = name }).ToList();
                context.Category.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // Seed de Productos
            if (!context.Product.Any())
            {
                var rand = new Random();
                var categories = context.Category.ToList();
                var products = new List<Product>();

                foreach (var category in categories)
                {
                    for (int i = 1; i <= 20; i++) // ⬅️ CUIDADO: este for antes tenía un error
                    {
                        var productName = $"{category.Name} Producto {i}";
                        products.Add(new Product
                        {
                            Name = productName,
                            Description = $"Descripcion de {productName}",
                            Price = Math.Round((decimal)(rand.NextDouble() * 100 + 1), 2),
                            Stock = rand.Next(10, 100),
                            IsActive = true,
                            PhotoURL = $"https://via.placeholder.com/300x300.png?text={Uri.EscapeDataString(productName)}",
                            CategoryId = category.Id
                        });
                    }
                }

                context.Product.AddRange(products);
                await context.SaveChangesAsync();
            }

            // Seed de Clientes
            if (!context.Client.Any())
            {
                var clients = new List<Client>();
                for (int i = 1; i <= 50; i++)
                {
                    clients.Add(new Client
                    {
                        Name = $"Cliente {i}",
                        Document = $"DOC-{1000 + i}",
                        Email = $"cliente{i}@example.com",
                        PhoneNumber = $"555-010{i:D2}",
                        PhotoURL = $"https://via.placeholder.com/100x100.png?text=C{i}"
                    });
                }
                context.Client.AddRange(clients);
                await context.SaveChangesAsync();
            }

            // Seed de Ventas
            if (!context.Sale.Any())
            {
                var rand = new Random();
                var clients = context.Client.ToList();
                var products = context.Product.ToList();

                var sales = new List<Sale>();
                for (int i = 0; i < 30; i++)
                {
                    var client = clients[rand.Next(clients.Count)];
                    int itemsCount = rand.Next(1, 6);
                    var saleItems = new List<SaleItem>();
                    var productSelected = new HashSet<int>();
                    decimal totalSale = 0m;

                    for (int j = 0; j < itemsCount; j++)
                    {
                        Product product;
                        do
                        {
                            product = products[rand.Next(products.Count)];
                        } while (productSelected.Contains(product.Id));

                        productSelected.Add(product.Id);
                        int quantity = rand.Next(1, 5);
                        decimal unitPrice = product.Price;
                        decimal subTotal = quantity * unitPrice;

                        totalSale += subTotal;

                        saleItems.Add(new SaleItem
                        {
                            ProductId = product.Id,
                            Quantity = quantity,
                            UnitPrice = unitPrice
                        });
                    }

                    var sale = new Sale
                    {
                        ClientId = client.Id,
                        Date = DateTime.Now.AddDays(-rand.Next(30)),
                        Total = totalSale,
                        SaleItem = saleItems
                    };
                    sales.Add(sale);
                }

                context.Sale.AddRange(sales);
                await context.SaveChangesAsync();
            }
        }
    }
}
