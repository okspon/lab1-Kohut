using System;
using System.Collections.Generic;

namespace IndependentWork11
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }

    public class Sale
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
    }

    public static class DataGenerator
    {
        public static (List<Product> Products, List<Customer> Customers, List<Sale> Sales) Generate(
            int productCount = 60,
            int customerCount = 80,
            int saleCount = 1500,
            int seed = 42)
        {
            var random = new Random(seed);

            string[] categories = ["Електроніка", "Побутова техніка", "Комп'ютери", "Аксесуари", "Смартфони"];
            string[] productPrefixes = ["Pro", "Ultra", "Max", "Lite", "Smart", "Neo", "Air", "Core"];
            string[] cities = ["Рівне", "Київ", "Львів", "Дніпро", "Одеса", "Харків", "Вінниця"];
            string[] firstNames = ["Олена", "Андрій", "Марія", "Дмитро", "Ірина", "Максим", "Софія", "Тарас"];
            string[] lastNames = ["Ковальчук", "Бондаренко", "Ткаченко", "Мельник", "Шевченко", "Кравчук"];

            var products = new List<Product>(productCount);
            for (int i = 1; i <= productCount; i++)
            {
                var category = categories[random.Next(categories.Length)];
                var prefix = productPrefixes[random.Next(productPrefixes.Length)];
                products.Add(new Product
                {
                    Id = i,
                    Name = $"{category} {prefix}-{100 + i}",
                    Category = category,
                    UnitPrice = Math.Round((decimal)(random.NextDouble() * 24500 + 500), 2)
                });
            }

            var customers = new List<Customer>(customerCount);
            for (int i = 1; i <= customerCount; i++)
            {
                var fullName = $"{firstNames[random.Next(firstNames.Length)]} {lastNames[random.Next(lastNames.Length)]}";
                customers.Add(new Customer
                {
                    Id = i,
                    Name = $"{fullName} #{i}",
                    City = cities[random.Next(cities.Length)]
                });
            }

            var startDate = new DateTime(2024, 1, 1);
            int totalDays = (new DateTime(2026, 11, 1) - startDate).Days;

            var sales = new List<Sale>(saleCount);
            for (int i = 1; i <= saleCount; i++)
            {
                sales.Add(new Sale
                {
                    Id = i,
                    ProductId = random.Next(1, productCount + 1),
                    CustomerId = random.Next(1, customerCount + 1),
                    Quantity = random.Next(1, 11),
                    SaleDate = startDate.AddDays(random.Next(totalDays))
                });
            }

            return (products, customers, sales);
        }
    }
}