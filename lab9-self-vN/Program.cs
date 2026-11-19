using System;
using System.Collections.Generic;
using lab9_self_vN;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// 1. Колекція об'єктів: List<Product>
Console.WriteLine("=== 1. Робота з колекцією List<Product> ===");
var products = new List<Product>
{
    new Product { Name = "Ноутбук", Category = "Електроніка", Price = 32000 },
    new Product { Name = "Мишка", Category = "Периферія", Price = 650 },
    new Product { Name = "Клавіатура", Category = "Периферія", Price = 1800 },
    new Product { Name = "Монітор", Category = "Електроніка", Price = 9500 }
};

// Map: форматуємо інформацію про товари зі знижкою 10%
var discountedInfo = CollectionUtils.Map(products, p => $"{p.Name}: {p.Price * 0.9m:F0} грн (зі знижкою)");
Console.WriteLine("Акційні ціни:");
CollectionUtils.ForEach(discountedInfo, info => Console.WriteLine($"  * {info}"));

// MaxBy: найдорожчий товар
var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
Console.WriteLine($"Найдорожчий товар: {mostExpensive.Name} ({mostExpensive.Price} грн)");

// Filter: товари дорожчі за 1500 грн
var expensiveProducts = CollectionUtils.Filter(products, p => p.Price > 1500);
Console.WriteLine("Товари дорожчі за 1500 грн:");
CollectionUtils.ForEach(expensiveProducts, p => Console.WriteLine($"  - {p.Name} ({p.Price} грн)"));

// 2. Колекція цілих чисел: List<int>
Console.WriteLine("\n=== 2. Робота з колекцією List<int> ===");
var numbers = new List<int> { 4, 15, 8, 23, 42, 11, 6 };

// Filter: лише парні числа
var evenNumbers = CollectionUtils.Filter(numbers, n => n % 2 == 0);
Console.WriteLine("Парні числа: " + string.Join(", ", evenNumbers));

// Map: піднесення до квадрату
var squares = CollectionUtils.Map(evenNumbers, n => n * n);
Console.WriteLine("Квадрати парних чисел: " + string.Join(", ", squares));

// MaxBy: число з найбільшою остачею від ділення на 10
var maxLastDigit = CollectionUtils.MaxBy(numbers, n => n % 10);
Console.WriteLine($"Число з найбільшою останньою цифрою: {maxLastDigit}");

// ForEach: підрахунок суми через зовнішню змінну в лямбді
int sum = 0;
CollectionUtils.ForEach(numbers, n => sum += n);
Console.WriteLine($"Сума всіх чисел: {sum}");

// 3. Колекція рядків: List<string>
Console.WriteLine("\n=== 3. Робота з колекцією List<string> ===");
var languages = new List<string> { "C#", "Python", "TypeScript", "Go", "Rust" };

// MaxBy: найдовша назва мови програмування
var longestName = CollectionUtils.MaxBy(languages, lang => lang.Length);
Console.WriteLine($"Найдовша назва мови: {longestName} ({longestName.Length} символів)");

// Filter: мови, назва яких довша за 2 символи
var filteredLangs = CollectionUtils.Filter(languages, lang => lang.Length > 2);

// Map: перетворення у верхній регістр із додаванням довжини
var formattedLangs = CollectionUtils.Map(filteredLangs, lang => $"{lang.ToUpper()} [{lang.Length}]");

Console.Write("Відфільтровані та відформатовані мови: ");
CollectionUtils.ForEach(formattedLangs, item => Console.Write(item + " "));
Console.WriteLine();

public class Product
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
}