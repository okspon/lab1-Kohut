using System;
using System.Collections.Generic;

namespace Lab14
{
    // Кастомний клас для демонстрації роботи з об'єктами
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsInStock { get; set; }

        public Product(string name, decimal price, bool isInStock)
        {
            Name = name;
            Price = price;
            IsInStock = isInStock;
        }
    }

    // Статичний узагальнений клас-утиліта
    public static class CollectionHelper<T>
    {
        /// Повертає кількість елементів у колекції, що задовольняють умову predicate.
        /// Реалізовано через власний цикл foreach без LINQ Enumerable.Count().
        public static int Count(IEnumerable<T> collection, Predicate<T> predicate)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), "Колекція не може бути null.");

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate), "Делегат predicate не може бути null.");

            int matchCount = 0;

            // Власна логіка обходу колекції
            foreach (T item in collection)
            {
                if (predicate(item))
                {
                    matchCount++;
                }
            }

            return matchCount;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Демонстрація 1: List<int>
            List<int> numbers = new List<int> { -10, 5, -3, 0, 12, -45, 8, -1 };
            Console.WriteLine($"Вхідні числа: {string.Join(", ", numbers)}");

            // Передача лямбда-виразу для від'ємних чисел
            int negativeCount = CollectionHelper<int>.Count(numbers, n => n < 0);
            Console.WriteLine($"-> Кількість від'ємних чисел (n < 0): {negativeCount}\n");

            // Демонстрація 2: List<string>
            List<string> words = new List<string> { "C#", "Delegate", "Lambda", "OOP", "Collection", "SDK" };
            Console.WriteLine($"Вхідний список слів: {string.Join(", ", words)}");

            // Передача лямбда-виразу для слів довжиною понад 4 символи
            int longWordsCount = CollectionHelper<string>.Count(words, w => w.Length > 4);
            Console.WriteLine($"-> Кількість слів довжиною > 4 символів: {longWordsCount}\n");

            // Демонстрація 3: List<Product> (Власний клас)
            List<Product> products = new List<Product>
            {
                new Product("Ноутбук", 25000m, true),
                new Product("Мишка", 450m, true),
                new Product("Клавіатура", 1200m, false),
                new Product("Монітор", 8500m, true),
                new Product("Кабель HDMI", 200m, false)
            };

            Console.WriteLine("Список продуктів:");
            foreach (var p in products)
            {
                Console.WriteLine($" - {p.Name}: {p.Price} грн (В наявності: {p.IsInStock})");
            }

            // Лямбда 1: Продукти в наявності
            int availableProducts = CollectionHelper<Product>.Count(products, p => p.IsInStock);
            Console.WriteLine($"\n-> Кількість продуктів у наявності: {availableProducts}");

            // Лямбда 2: Дорогі продукти (> 1000 грн)
            int expensiveProducts = CollectionHelper<Product>.Count(products, p => p.Price > 1000m);
            Console.WriteLine($"-> Кількість продуктів з ціною > 1000 грн: {expensiveProducts}");
        }
    }
}