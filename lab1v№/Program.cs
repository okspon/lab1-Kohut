using System;

namespace lab1v11
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Налаштування відображення кирилиці

            Plane plane1 = new Plane("МАУ", "Boeing 737", 186);
            Plane plane2 = new Plane("Ryanair", "Boeing 737-800", 189);
            Plane plane3 = new Plane("Wizz Air", "Airbus A321neo", 239);

            Console.WriteLine("=== Перевірка польотів літаків ===");
            
            plane1.Fly();
            plane2.Fly();
            plane3.Fly();

            Console.WriteLine($"\nПочаткова місткість {plane3.Capacity} пасажирів.");
            plane3.Capacity = 240; // Зміна значення через set
            Console.WriteLine($"Оновлена місткість: {plane3.Capacity} пасажирів.");
        }
    }
}