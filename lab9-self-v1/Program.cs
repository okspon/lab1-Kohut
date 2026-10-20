using System;
using System.Collections.Generic;
using System.Text;

namespace lab9_self_v1;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("1. Робота з колекцією об'єктів (List<Product>)");
        var products = new List<Product>
        {
            new Product { Name = "Laptop", Price = 25000 },
            new Product { Name = "Mouse", Price = 500 },
            new Product { Name = "Keyboard", Price = 1500 },
            new Product { Name = "Monitor", Price = 8000 }
        };

        // Map
        var productNames = CollectionUtils.Map(products, p => p.Name);
        Console.WriteLine("Назви товарів: " + string.Join(", ", productNames));

        // MaxBy
        var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
        Console.WriteLine($"Найдорожчий товар: {mostExpensive.Name} ({mostExpensive.Price} грн)");

        // Filter & ForEach
        var expensiveProducts = CollectionUtils.Filter(products, p => p.Price > 1000);
        Console.WriteLine("Товари дорожчі за 1000 грн:");
        CollectionUtils.ForEach(expensiveProducts, p => Console.WriteLine($" - {p.Name}: {p.Price} грн"));

        Console.WriteLine("\n 2. Робота з колекцією чисел (List<int>)");
        var numbers = new List<int> { 12, 5, 8, 23, 42, 1, 19 };

        var squaredNumbers = CollectionUtils.Map(numbers, x => x * x);
        Console.WriteLine("Квадрати чисел: " + string.Join(", ", squaredNumbers));

        var maxNumber = CollectionUtils.MaxBy(numbers, x => x);
        Console.WriteLine($"Максимальне число: {maxNumber}");

        var evenNumbers = CollectionUtils.Filter(numbers, x => x % 2 == 0);
        Console.WriteLine("Парні числа:");
        CollectionUtils.ForEach(evenNumbers, x => Console.Write(x + " "));
        Console.WriteLine();

        Console.WriteLine("\n 3. Робота з колекцією рядків (List<string>)");
        var words = new List<string> { "C#", "Delegates", "Lambda", "LINQ", "Generics" };

        var wordLengths = CollectionUtils.Map(words, w => w.Length);
        Console.WriteLine("Довжини слів: " + string.Join(", ", wordLengths));

        var longestWord = CollectionUtils.MaxBy(words, w => w.Length);
        Console.WriteLine($"Найдовше слово: {longestWord}");

        var shortWords = CollectionUtils.Filter(words, w => w.Length <= 4);
        Console.WriteLine("Слова довжиною <= 4 символів:");
        CollectionUtils.ForEach(shortWords, w => Console.WriteLine($" - {w}"));
    }
}