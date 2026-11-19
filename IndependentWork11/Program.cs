using System;
using IndependentWork11;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var (products, customers, sales) = DataGenerator.Generate(
    productCount: 60,
    customerCount: 80,
    saleCount: 1500,
    seed: 42);

Console.WriteLine($"Згенеровано тестовий набір (Seed=42): {products.Count} товарів, {customers.Count} клієнтів, {sales.Count} продажів.\n");

// Прогрів JIT-компілятора (перший виклик LINQ-ланцюжка ініціалізує вирази)
SalesAnalytics.TopProductsByCategory(sales, products);
SalesAnalytics.RevenueByCustomerCity(sales, products, customers);
SalesAnalytics.SalesByYearAndMonth(sales, products);
SalesAnalytics.TopVipCustomers(sales, products, customers);