using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab17v11
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public override string ToString() => $"[Ticket #{Id} | {Type} | {Price:C}]";
    }

    public class Participant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();

        public override string ToString() => $"Participant: {Name} (Id: {Id}) - Tickets count: {Tickets.Count}";
    }

    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Participant> Participants { get; set; } = new List<Participant>();

        public override string ToString() => $"Event #{Id}: '{Name}' (Participants: {Participants.Count})";
    }

    public static class EventExtensions
    {
        // Custom Extension для Варіанту 11
        public static decimal TotalTicketRevenue(this IEnumerable<Event> events)
        {
            if (events == null) return 0m;

            return events
                .SelectMany(e => e.Participants)
                .SelectMany(p => p.Tickets)
                .Sum(t => t.Price);
        }

        // Власний метод для зручної пагінації
        public static IEnumerable<T> GetPage<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (pageIndex < 1) throw new ArgumentOutOfRangeException(nameof(pageIndex));
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));

            return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            List<Event> events = SeedData();

            // 1. SelectMany
            Console.WriteLine("[Завдання 1] Використання SelectMany");
            Console.WriteLine("Отримання всіх квитків усіх учасників усіх подій у 1 списку:");

            var allTickets = events
                .SelectMany(e => e.Participants)
                .SelectMany(p => p.Tickets);

            int ticketCounter = 1;
            foreach (var ticket in allTickets)
            {
                Console.WriteLine($"  {ticketCounter++}. {ticket}");
            }
            Console.WriteLine($"Усього знайдено квитків: {allTickets.Count()}\n");

            // 2. Пагінація
            Console.WriteLine("[Завдання 2] Пагінація подій (по 2 події на сторінку) ");
            int pageSize = 2;
            int totalEvents = events.Count;
            int totalPages = (int)Math.Ceiling((double)totalEvents / pageSize);

            for (int page = 1; page <= totalPages; page++)
            {
                Console.WriteLine($"\n--- Сторінка {page} з {totalPages} ---");
                var pageItems = events.GetPage(page, pageSize);

                foreach (var ev in pageItems)
                {
                    Console.WriteLine($"  - {ev}");
                    foreach (var p in ev.Participants)
                    {
                        Console.WriteLine($"      * Учасник: {p.Name} (Квитків: {p.Tickets.Count})");
                    }
                }
            }
            Console.WriteLine();

            // 3. Custom Extension
            Console.WriteLine("[Завдання 3] Кастомний метод розширення TotalTicketRevenue()");
            decimal totalRevenue = events.TotalTicketRevenue();
            Console.WriteLine($"Загальний дохід від продажу всіх квитків: {totalRevenue:C}\n");

        }

        private static List<Event> SeedData()
        {
            return new List<Event>
            {
                new Event
                {
                    Id = 1, Name = "IT Forum 2026",
                    Participants = new List<Participant>
                    {
                        new Participant { Id = 101, Name = "Олексій Коваленко", Tickets = new List<Ticket> { new Ticket { Id = 1, Type = "VIP", Price = 1500m }, new Ticket { Id = 2, Type = "Standard", Price = 500m } } },
                        new Participant { Id = 102, Name = "Олена Бойко", Tickets = new List<Ticket> { new Ticket { Id = 3, Type = "Student", Price = 250m } } }
                    }
                },
                new Event
                {
                    Id = 2, Name = "Rock Fest",
                    Participants = new List<Participant>
                    {
                        new Participant { Id = 103, Name = "Іван Мельник", Tickets = new List<Ticket> { new Ticket { Id = 4, Type = "Fan Zone", Price = 800m }, new Ticket { Id = 5, Type = "Fan Zone", Price = 800m } } }
                    }
                },
                new Event
                {
                    Id = 3, Name = "GameDev Conference",
                    Participants = new List<Participant>
                    {
                        new Participant { Id = 104, Name = "Марія Шевченко", Tickets = new List<Ticket> { new Ticket { Id = 6, Type = "VIP Premium", Price = 2000m } } },
                        new Participant { Id = 105, Name = "Тарас Сидоренко", Tickets = new List<Ticket> { new Ticket { Id = 7, Type = "Standard", Price = 600m }, new Ticket { Id = 8, Type = "Standard", Price = 600m } } }
                    }
                },
                new Event
                {
                    Id = 4, Name = "CyberSecurity Workshop",
                    Participants = new List<Participant>
                    {
                        new Participant { Id = 106, Name = "Дмитро Ткаченко", Tickets = new List<Ticket> { new Ticket { Id = 9, Type = "Online Access", Price = 400m } } }
                    }
                },
                new Event
                {
                    Id = 5, Name = "AI Innovation Summit",
                    Participants = new List<Participant>
                    {
                        new Participant { Id = 107, Name = "Анна Кравчук", Tickets = new List<Ticket> { new Ticket { Id = 10, Type = "VIP", Price = 2500m } } }
                    }
                }
            };
        }
    }
}
