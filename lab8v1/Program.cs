using System;
using System.Collections.Generic;

namespace Lab8Polymorphism
{
    // 1. БАЗОВИЙ КЛАС
    public class Building
    {
        public string Address { get; set; }

        public Building(string address)
        {
            Address = address;
        }
        public virtual string GetPurpose()
        {
            return $"Адреса: {Address}";
        }
    }

    // 2. ПОХІДНІ КЛАСИ

    // Житловий будинок
    public class House : Building
    {
        public int NumRooms { get; set; }

        public House(string address, int numRooms) : base(address)
        {
            NumRooms = numRooms;
        }

        public override string GetPurpose()
        {
            return $"[Житловий будинок] {base.GetPurpose()} | Призначення: проживання | Кімнат: {NumRooms}";
        }
    }

    // Офісний центр
    public class Office : Building
    {
        public int NumFloors { get; set; }

        public Office(string address, int numFloors) : base(address)
        {
            NumFloors = numFloors;
        }

        public override string GetPurpose()
        {
            return $"[Офісний центр] {base.GetPurpose()} | Призначення: бізнес та робота | Поверхів: {NumFloors}";
        }
    }

    // Магазин
    public class Shop : Building
    {
        public string ProductType { get; set; }

        public Shop(string address, string productType) : base(address)
        {
            ProductType = productType;
        }

        public override string GetPurpose()
        {
            return $"[Торговий магазин] {base.GetPurpose()} | Призначення: продаж товарів ({ProductType})";
        }
    }

    // 3. ТОЧКА ВХОДУ (Демонстрація поліморфізму)
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("ДЕМОНСТРАЦІЯ ПОЛІМОРФІЗМУ ТА АГРЕГАЦІЇ");

            // Колекція базового типу List<Building>, що зберігає різні об'єкти
            List<Building> buildings = new List<Building>
            {
                new House("вул. Хрещатик, 10", 4),
                new Office("просп. Перемоги, 25", 12),
                new Shop("вул. Соборна, 5", "Електроніка"),
                new House("вул. Зелена, 18", 2),
                new Shop("вул. Садова, 12", "Продукти харчування")
            };

            //збирання результатів
            List<string> purposeSummary = new List<string>();

            //динамічне зв'язування обирає потрібний метод під час виконання
            int index = 1;
            foreach (Building b in buildings)
            {
                // Поліморфний виклик
                string info = b.GetPurpose();

                Console.WriteLine($"Виклик #{index}: {info}");

                // Агрегація результатів
                purposeSummary.Add($"{index}. {info}");
                index++;
            }

            // Вивід агрегованого списку
            Console.WriteLine(" ");
            Console.WriteLine("ПІДСУМКОВИЙ АГРЕГОВАНИЙ СПИСОК:");

            foreach (string item in purposeSummary)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"\nВсього оброблено об'єктів: {buildings.Count}");
        }
    }
}