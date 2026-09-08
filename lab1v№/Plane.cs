using System;

namespace lab1v11
{
    public class Plane
    {
        private string airline;
        private string model;

        public int Capacity { get; set; }

        public Plane(string airline, string model, int capacity)
        {
            this.airline = airline;
            this.model = model;
            Capacity = capacity;
        }

        ~Plane()
        {
            Console.WriteLine($"[Деструктор]: Об'єкт літака {model} видалено з пам'яті.");
        }

        public void Fly()
        {
            Console.WriteLine($"Літак {model} авіакомпанії \"{airline}\" (місткість: {Capacity} пас.) успішно набрав висоту та виконує політ.");
        }
    }
}