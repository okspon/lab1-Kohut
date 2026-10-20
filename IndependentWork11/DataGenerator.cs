using System;
using System.Collections.Generic;

namespace IndependentWork11;

public static class DataGenerator
{
    public static (List<Product> products, List<Customer> customers, List<Sale> sales) GenerateData()
    {
        var random = new Random(42); // Фіксований Seed для відтворюваності

        var categories = new[] { "Electronics", "Books", "Clothing", "Home & Kitchen" };
        var cities = new[] { "Київ", "Львів", "Одеса", "Харків", "Дніпро", "Рівне" };

        var products = new List<Product>();
        for (int i = 1; i <= 60; i++)
        {
            products.Add(new Product
            {
                Id = i,
                Name = $"Товар_{i}",
                Category = categories[random.Next(categories.Length)],
                UnitPrice = random.Next(50, 5000)
            });
        }

        var customers = new List<Customer>();
        for (int i = 1; i <= 80; i++)
        {
            customers.Add(new Customer
            {
                Id = i,
                Name = $"Клієнт_{i}",
                City = cities[random.Next(cities.Length)]
            });
        }

        var sales = new List<Sale>();
        var startDate = new DateTime(2025, 1, 1);
        for (int i = 1; i <= 200; i++)
        {
            sales.Add(new Sale
            {
                Id = i,
                ProductId = random.Next(1, products.Count + 1),
                CustomerId = random.Next(1, customers.Count + 1),
                Quantity = random.Next(1, 10),
                SaleDate = startDate.AddDays(random.Next(0, 365))
            });
        }

        return (products, customers, sales);
    }
}