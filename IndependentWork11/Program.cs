using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace IndependentWork11;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var (products, customers, sales) = DataGenerator.GenerateData();

        Console.WriteLine($"Згенеровано даних: Продукти={products.Count}, Клієнти={customers.Count}, Продажі={sales.Count}\n");

        Pipeline1_TopProductsByCategory(products, sales);
        Pipeline2_RevenueByCustomerCity(products, customers, sales);
        Pipeline3_SalesByMonthAndYear(products, sales);
        Pipeline4_TopSpendingCustomers(products, customers, sales);
    }

    // Пайплайн 1: Найпопулярніші продукти за категоріями
    public static void Pipeline1_TopProductsByCategory(List<Product> products, List<Sale> sales)
    {
        Console.WriteLine("Пайплайн 1: Найпопулярніші продукти за категоріями");
        var sw = Stopwatch.StartNew();

        var result = sales
            .GroupBy(s => s.ProductId)
            .Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(s => s.Quantity) })
            .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { p.Name, p.Category, s.TotalQuantity })
            .GroupBy(p => p.Category)
            .Select(g => new
            {
                Category = g.Key,
                TopProducts = g.OrderByDescending(x => x.TotalQuantity).Take(3).ToList()
            }).ToList();

        sw.Stop();

        foreach (var cat in result)
        {
            Console.WriteLine($"Категорія: {cat.Category}");
            foreach (var prod in cat.TopProducts)
            {
                Console.WriteLine($"  - {prod.Name}: {prod.TotalQuantity} шт.");
            }
        }
        Console.WriteLine($"Час виконання: {sw.Elapsed.TotalMilliseconds:F3} ms ({sw.ElapsedTicks} ticks)\n");
    }

    // Пайплайн 2: Загальний дохід за містами клієнтів
    public static void Pipeline2_RevenueByCustomerCity(List<Product> products, List<Customer> customers, List<Sale> sales)
    {
        Console.WriteLine("Пайплайн 2: Загальний дохід за містами клієнтів");
        var sw = Stopwatch.StartNew();

        var result = sales
            .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { s.CustomerId, Revenue = s.Quantity * p.UnitPrice })
            .Join(customers, s => s.CustomerId, c => c.Id, (s, c) => new { c.City, s.Revenue })
            .GroupBy(c => c.City)
            .Select(g => new { City = g.Key, TotalRevenue = g.Sum(x => x.Revenue) })
            .OrderByDescending(x => x.TotalRevenue)
            .ToList();

        sw.Stop();

        foreach (var item in result)
        {
            Console.WriteLine($"Місто: {item.City,-8} | Дохід: {item.TotalRevenue,10:C}");
        }
        Console.WriteLine($"Час виконання: {sw.Elapsed.TotalMilliseconds:F3} ms ({sw.ElapsedTicks} ticks)\n");
    }

    // Пайплайн 3: Продажі за місяцями та роками
    public static void Pipeline3_SalesByMonthAndYear(List<Product> products, List<Sale> sales)
    {
        Console.WriteLine("Пайплайн 3: Продажі за місяцями та роками");
        var sw = Stopwatch.StartNew();

        var result = sales
            .Join(products, s => s.ProductId, p => p.Id, (s, p) => new
            {
                s.SaleDate.Year,
                s.SaleDate.Month,
                s.Quantity,
                Revenue = s.Quantity * p.UnitPrice
            })
            .GroupBy(x => new { x.Year, x.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                TotalUnits = g.Sum(x => x.Quantity),
                TotalRevenue = g.Sum(x => x.Revenue)
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToList();

        sw.Stop();

        foreach (var item in result.Take(5)) // Виводимо перші 5 місяців для стислості
        {
            Console.WriteLine($"Рік: {item.Year}, Місяць: {item.Month:D2} | Одиниць: {item.TotalUnits,3} | Дохід: {item.TotalRevenue,10:C}");
        }
        Console.WriteLine($"Час виконання: {sw.Elapsed.TotalMilliseconds:F3} ms ({sw.ElapsedTicks} ticks)\n");
    }

    // Пайплайн 4: Топ-3 клієнти за загальною сумою покупок
    public static void Pipeline4_TopSpendingCustomers(List<Product> products, List<Customer> customers, List<Sale> sales)
    {
        Console.WriteLine("Пайплайн 4: Топ-3 клієнти за витратами");
        var sw = Stopwatch.StartNew();

        var result = sales
            .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { s.CustomerId, Revenue = s.Quantity * p.UnitPrice })
            .GroupBy(x => x.CustomerId)
            .Select(g => new { CustomerId = g.Key, TotalSpent = g.Sum(x => x.Revenue) })
            .Join(customers, x => x.CustomerId, c => c.Id, (x, c) => new { c.Name, c.City, x.TotalSpent })
            .OrderByDescending(x => x.TotalSpent)
            .Take(3)
            .ToList();

        sw.Stop();

        foreach (var item in result)
        {
            Console.WriteLine($"Клієнт: {item.Name,-10} | Місто: {item.City,-8} | Витрачено: {item.TotalSpent,10:C}");
        }
        Console.WriteLine($"Час виконання: {sw.Elapsed.TotalMilliseconds:F3} ms ({sw.ElapsedTicks} ticks)\n");
    }
}