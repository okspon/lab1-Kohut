using System;

namespace Lab2
{
    public class Plane
    {
        private string _airline = "N/A";
        private string _model = "Boeing 737";
        private int _capacity = 150;

        public string Airline
        {
            get => _airline;
            set => _airline = string.IsNullOrWhiteSpace(value) ? "N/A" : value;
        }

        public string Model
        {
            get => _model;
            set => _model = string.IsNullOrWhiteSpace(value) ? "Boeing 737" : value;
        }

        public int Capacity
        {
            get => _capacity;
            set => _capacity = value > 0 ? value : 150;
        }

        public Plane() : this("N/A", "Boeing 737", 150) { }

        public Plane(string airline, string model, int capacity)
        {
            Airline = airline;
            Model = model;
            Capacity = capacity;
        }

        public void Fly() => 
            Console.WriteLine($"Літак {Airline} {Model} (місткість: {Capacity}) піднявся в повітря!");

        ~Plane() => 
            Console.WriteLine($"[Деструктор] {Airline} {Model} вилучено з пам'яті.");
    }

    internal class Program
    {
        private static void Main()
        {
            Plane p1 = new Plane();
            Plane p2 = new Plane("Ryanair", "Airbus A320", 180);
            Plane p3 = new Plane("WizzAir", "Airbus A321", -50);

            p1.Fly();
            p2.Fly();
            p3.Fly();

            p1 = null;
            p2 = null;
            p3 = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
