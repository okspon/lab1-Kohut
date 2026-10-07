using System;
using System.Collections.Generic;

namespace OOP_Lecture6_7_Demo
{
    // Інтерфейс логування
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void LogInfo(string message) => Console.WriteLine($"[INFO]: {message}");
        public void LogError(string message) => Console.Error.WriteLine($"[ERROR]: {message}");
    }

    // Абстрактний базовий клас
    public abstract class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product()
        {
            Name = "Unnamed";
            Price = 0m;
        }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public void PrintBaseDetails()
        {
            Console.WriteLine($"Товар: {Name} | Базова ціна: {Price:C}");
        }

        // Абстрактний метод
        public abstract decimal CalculateDiscountedPrice(decimal discountPercentage);
    }

    // Похідний клас 1
    public class Electronics : Product
    {
        public int WarrantyMonths { get; set; }

        public Electronics() : base() { WarrantyMonths = 12; }

        public Electronics(string name, decimal price, int warranty) : base(name, price)
        {
            WarrantyMonths = warranty;
        }

        public override decimal CalculateDiscountedPrice(decimal discountPercentage)
        {
            decimal discount = Price * (discountPercentage / 100m);
            return Price - discount;
        }
    }

    // Похідний клас 2
    public class Food : Product
    {
        public DateTime ExpirationDate { get; set; }

        public Food() : base() { ExpirationDate = DateTime.Now.AddDays(7); }

        public Food(string name, decimal price, DateTime expirationDate) : base(name, price)
        {
            ExpirationDate = expirationDate;
        }

        public override decimal CalculateDiscountedPrice(decimal discountPercentage)
        {
            decimal effectiveDiscount = (ExpirationDate - DateTime.Now).TotalDays < 3 
                ? discountPercentage * 2 
                : discountPercentage;

            if (effectiveDiscount > 90) effectiveDiscount = 90;

            return Price - (Price * (effectiveDiscount / 100m));
        }
    }

    // Узагальнений інтерфейс
    public interface IRepository<T>
    {
        void Add(T item);
        T Get(int index);
        List<T> GetAll();
    }

    // Узагальнений клас із обмеженнями (Generic constraints)
    public class ProductRepository<T> : IRepository<T> where T : Product, new()
    {
        private readonly List<T> _items = new List<T>();

        public void Add(T item)
        {
            _items.Add(item);
        }

        public T Get(int index)
        {
            if (index >= 0 && index < _items.Count)
                return _items[index];

            throw new ArgumentOutOfRangeException(nameof(index), "Індекс поза межами масиву.");
        }

        public List<T> GetAll() => _items;

        public T CreateDefaultProduct()
        {
            return new T();
        }
    }

    // Узагальнений метод
    public static class Utility
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }

    // Композиція з інтерфейсами
    public class InventoryService<T> where T : Product, new()
    {
        private readonly IRepository<T> _repository;
        private readonly ILogger _logger;

        public InventoryService(IRepository<T> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public void RegisterProduct(T product)
        {
            _repository.Add(product);
            _logger.LogInfo($"Товар '{product.Name}' успішно додано до складу.");
        }

        public void DisplayDiscountedCatalog(decimal discount)
        {
            _logger.LogInfo($"Каталог товарів зі знижкою {discount}%:");
            foreach (var item in _repository.GetAll())
            {
                decimal finalPrice = item.CalculateDiscountedPrice(discount);
                Console.WriteLine($"• {item.Name} | Початкова: {item.Price:C} -> Зі знижкою: {finalPrice:C}");
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("ДЕМОНСТРАЦІЯ ЛЕКЦІЙ №6 ТА №7 (Generics, Abstract, Interfaces)\n");

            // Виклик Generic-методу
            Console.WriteLine("Узагальнений метод Utility.Swap<T>");
            int val1 = 100, val2 = 500;
            Console.WriteLine($"До Swap: val1 = {val1}, val2 = {val2}");
            Utility.Swap(ref val1, ref val2);
            Console.WriteLine($"Після Swap: val1 = {val1}, val2 = {val2}\n");

            // Демонстрація електроніки
            Console.WriteLine("Робота зі складом Електроніки");
            ILogger logger = new ConsoleLogger();
            IRepository<Electronics> electronicsRepo = new ProductRepository<Electronics>();
            InventoryService<Electronics> electronicsService = new InventoryService<Electronics>(electronicsRepo, logger);

            electronicsService.RegisterProduct(new Electronics("Смартфон", 25000m, 24));
            electronicsService.RegisterProduct(new Electronics("Ноутбук", 45000m, 12));
            electronicsService.DisplayDiscountedCatalog(10);
            Console.WriteLine();

            // Демонстрація продуктів харчування
            Console.WriteLine("Робота зі складом Продуктів");
            IRepository<Food> foodRepo = new ProductRepository<Food>();
            InventoryService<Food> foodService = new InventoryService<Food>(foodRepo, logger);

            foodService.RegisterProduct(new Food("Молоко", 45m, DateTime.Now.AddDays(1)));
            foodService.RegisterProduct(new Food("Сир", 180m, DateTime.Now.AddDays(10)));
            foodService.DisplayDiscountedCatalog(15);
        }
    }
}