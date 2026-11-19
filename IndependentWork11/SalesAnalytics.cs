using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IndependentWork11
{
    public static class SalesAnalytics
    {
        // Пайплайн 1: Найпопулярніші продукти за категоріями (Top-3 за кількістю)
        public static void TopProductsByCategory(List<Sale> sales, List<Product> products)
        {
            var sw = Stopwatch.StartNew();

            var result = sales
                .GroupBy(s => s.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(s => s.Quantity)
                })
                .Join(
                    products,
                    stat => stat.ProductId,
                    prod => prod.Id,
                    (stat, prod) => new
                    {
                        prod.Name,
                        prod.Category,
                        stat.TotalQuantity
                    })
                .GroupBy(x => x.Category)
                .Select(catGroup => new
                {
                    Category = catGroup.Key,
                    Top3 = catGroup
                        .OrderByDescending(x => x.TotalQuantity)
                        .Take(3)
                        .ToList()
                })
                .OrderBy(x => x.Category)
                .ToList();

            sw.Stop();

            Console.WriteLine($" Пайплайн 1: Топ-3 найпопулярніші продукти за категоріями (Час: {sw.Elapsed.TotalMilliseconds:F4} мс / {sw.ElapsedTicks} ticks)");
            foreach (var group in result)
            {
                Console.WriteLine($"Категорія: {group.Category}");
                int rank = 1;
                foreach (var item in group.Top3)
                {
                    Console.WriteLine($"   {rank++}. {item.Name} — продано {item.TotalQuantity} шт.");
                }
            }
            Console.WriteLine();
        }

        // Пайплайн 2: Загальний дохід за містами клієнтів
        public static void RevenueByCustomerCity(List<Sale> sales, List<Product> products, List<Customer> customers)
        {
            var sw = Stopwatch.StartNew();

            var result = sales
                .Join(
                    products,
                    s => s.ProductId,
                    p => p.Id,
                    (s, p) => new
                    {
                        s.CustomerId,
                        Revenue = s.Quantity * p.UnitPrice
                    })
                .Join(
                    customers,
                    sp => sp.CustomerId,
                    c => c.Id,
                    (sp, c) => new
                    {
                        c.City,
                        sp.Revenue
                    })
                .GroupBy(x => x.City)
                .Select(g => new
                {
                    City = g.Key,
                    TotalRevenue = g.Sum(x => x.Revenue),
                    OrdersCount = g.Count()
                })
                .OrderByDescending(x => x.TotalRevenue)
                .ToList();

            sw.Stop();

            Console.WriteLine($" Пайплайн 2: Загальний дохід за містами клієнтів (Час: {sw.Elapsed.TotalMilliseconds:F4} мс / {sw.ElapsedTicks} ticks)");
            foreach (var row in result)
            {
                Console.WriteLine($"  {row.City,-12} | Дохід: {row.TotalRevenue,12:N2} грн | Замовлень: {row.OrdersCount}");
            }
            Console.WriteLine();
        }

        // Пайплайн 3: Продажі за місяцями та роками
        public static void SalesByYearAndMonth(List<Sale> sales, List<Product> products)
        {
            var sw = Stopwatch.StartNew();

            var result = sales
                .Join(
                    products,
                    s => s.ProductId,
                    p => p.Id,
                    (s, p) => new
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

            Console.WriteLine($"Пайплайн 3: Динаміка продажів за роками та місяцями (Час: {sw.Elapsed.TotalMilliseconds:F4} мс / {sw.ElapsedTicks} ticks)");
            foreach (var row in result.Take(12)) // Виводимо перші 12 місяців для компактності
            {
                Console.WriteLine($"  {row.Year}-{row.Month:D2} | Продано одиниць: {row.TotalUnits,5} | Дохід: {row.TotalRevenue,12:N2} грн");
            }
            if (result.Count > 12)
            {
                Console.WriteLine($"  ... (всього сформовано {result.Count} помісячних періодів)");
            }
            Console.WriteLine();
        }

        // Пайплайн 4 (додатковий): Топ-5 VIP-клієнтів (середній чек > 15 000 грн)
        public static void TopVipCustomers(List<Sale> sales, List<Product> products, List<Customer> customers)
        {
            var sw = Stopwatch.StartNew();

            var result = sales
                .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { s.CustomerId, Amount = s.Quantity * p.UnitPrice })
                .Where(x => x.Amount >= 15000m) // Фільтруємо лише великі покупки
                .GroupBy(x => x.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    BigOrders = g.Count(),
                    TotalSpent = g.Sum(x => x.Amount),
                    AverageCheck = g.Average(x => x.Amount)
                })
                .Join(customers, stat => stat.CustomerId, c => c.Id, (stat, c) => new
                {
                    c.Name,
                    c.City,
                    stat.BigOrders,
                    stat.TotalSpent,
                    stat.AverageCheck
                })
                .OrderByDescending(x => x.TotalSpent)
                .Take(5)
                .ToList();

            sw.Stop();

            Console.WriteLine($" Пайплайн 4 (бонус): Топ-5 VIP-клієнтів (замовлення >= 15 000 грн) (Час: {sw.Elapsed.TotalMilliseconds:F4} мс / {sw.ElapsedTicks} ticks)");
            foreach (var vip in result)
            {
                Console.WriteLine($"  {vip.Name,-24} ({vip.City}) | Великих покупок: {vip.BigOrders} | Сума: {vip.TotalSpent,11:N2} грн | Сер. чек: {vip.AverageCheck,9:N2} грн");
            }
            Console.WriteLine();
        }
    }
}