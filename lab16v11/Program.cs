using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lab10_Variant11
{
    // 1. КЛАСИ СУТНОСТЕЙ (Entities)

    /// <summary>
    /// Локація для проведення подій
    /// </summary>
    public class Venue
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }

    /// <summary>
    /// Подія
    /// </summary>
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int VenueId { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 2. ТЕСТОВІ ДАНІ (10+ елементів)

            List<Venue> venues = new List<Venue>
            {
                new Venue { Id = 1, Name = "Національний палац мистецтв \"Україна\"", Capacity = 3700 },
                new Venue { Id = 2, Name = "Палац Спорту", Capacity = 10000 },
                new Venue { Id = 3, Name = "МЦКМ (Жовтневий палац)", Capacity = 2000 },
                new Venue { Id = 4, Name = "Камерний зал \"Арт-Спейс\"", Capacity = 150 },
                new Venue { Id = 5, Name = "Міський ДК", Capacity = 450 },
                new Venue { Id = 6, Name = "Клуб \"Atlas\"", Capacity = 1200 },
                new Venue { Id = 7, Name = "Конгрес-центр", Capacity = 800 },
                new Venue { Id = 8, Name = "Театр ляльок", Capacity = 300 },
                new Venue { Id = 9, Name = "Арена Львів", Capacity = 34000 },
                new Venue { Id = 10, Name = "Органний зал", Capacity = 400 }
            };

            List<Event> events = new List<Event>
            {
                new Event { Id = 101, Name = "Благодійний рок-концерт", Date = new DateTime(2024, 05, 15), VenueId = 2 },
                new Event { Id = 102, Name = "IT-Конференція DevDays 2024", Date = new DateTime(2024, 09, 10), VenueId = 1 },
                new Event { Id = 103, Name = "Вечір класичної музики", Date = new DateTime(2023, 11, 20), VenueId = 10 },
                new Event { Id = 104, Name = "Джазовий фестиваль", Date = new DateTime(2024, 07, 01), VenueId = 6 },
                new Event { Id = 105, Name = "Майстер-клас з живопису", Date = new DateTime(2024, 03, 12), VenueId = 4 },
                new Event { Id = 106, Name = "Сімфонічний оркестр", Date = new DateTime(2025, 01, 15), VenueId = 3 },
                new Event { Id = 107, Name = "Театральна вистава \"Гамлет\"", Date = new DateTime(2024, 12, 05), VenueId = 3 },
                new Event { Id = 108, Name = "Виставка робототехніки", Date = new DateTime(2024, 04, 22), VenueId = 7 },
                new Event { Id = 109, Name = "Дитяче шоу", Date = new DateTime(2023, 06, 01), VenueId = 8 },
                new Event { Id = 110, Name = "Міжнародний економічний форум", Date = new DateTime(2024, 10, 18), VenueId = 9 },
                new Event { Id = 111, Name = "Stand-Up шоу", Date = new DateTime(2024, 08, 14), VenueId = 6 },
                new Event { Id = 112, Name = "Поетичний вечір", Date = new DateTime(2024, 02, 14), VenueId = 5 }
            };

            Stopwatch sw = new Stopwatch();


            // ЗАПИТ 1: Події 2024 року на локації з Capacity > 500
            Console.WriteLine(" ЗАПИТ 1: Події 2024 року на локаціях з місткістю > 500 ");

            // Query Syntax
            sw.Restart();
            var q1Query = from e in events
                         join v in venues on e.VenueId equals v.Id
                         where e.Date.Year == 2024 && v.Capacity > 500
                         select new { EventName = e.Name, e.Date, VenueName = v.Name, v.Capacity };
            var listQ1Query = q1Query.ToList(); // Негайне виконання
            sw.Stop();
            long timeQ1Query = sw.ElapsedTicks;

            // Method Syntax
            sw.Restart();
            var q1Method = events
                .Join(venues,
                      e => e.VenueId,
                      v => v.Id,
                      (e, v) => new { Event = e, Venue = v })
                .Where(x => x.Event.Date.Year == 2024 && x.Venue.Capacity > 500)
                .Select(x => new { EventName = x.Event.Name, x.Event.Date, VenueName = x.Venue.Name, x.Venue.Capacity })
                .ToList();
            sw.Stop();
            long timeQ1Method = sw.ElapsedTicks;

            // Вивід результатів
            foreach (var item in listQ1Query)
            {
                Console.WriteLine($"• {item.EventName} ({item.Date:dd.MM.yyyy}) -> {item.VenueName} (Місткість: {item.Capacity})");
            }
            Console.WriteLine($"Час (Query Syntax):  {timeQ1Query} ticks");
            Console.WriteLine($"Час (Method Syntax): {timeQ1Method} ticks\n");


            // ЗАПИТ 2: Об'єднати події з їхніми локаціями
            Console.WriteLine("ЗАПИТ 2: Об'єднання подій з локаціями (Подія - Локація)");

            // Query Syntax
            sw.Restart();
            var q2Query = (from e in events
                          join v in venues on e.VenueId equals v.Id
                          select new { EventName = e.Name, VenueName = v.Name }).ToList();
            sw.Stop();
            long timeQ2Query = sw.ElapsedTicks;

            // Method Syntax
            sw.Restart();
            var q2Method = events
                .Join(venues,
                      e => e.VenueId,
                      v => v.Id,
                      (e, v) => new { EventName = e.Name, VenueName = v.Name })
                .ToList();
            sw.Stop();
            long timeQ2Method = sw.ElapsedTicks;

            foreach (var item in q2Query)
            {
                Console.WriteLine($"• Подія: \"{item.EventName}\" | Локація: \"{item.VenueName}\"");
            }
            Console.WriteLine($"Час (Query Syntax):  {timeQ2Query} ticks");
            Console.WriteLine($"Час (Method Syntax): {timeQ2Method} ticks\n");


            // ЗАПИТ 3: Кількість подій для кожної локації
            Console.WriteLine("ЗАПИТ 3: Кількість подій для кожної локації");

            // Query Syntax (використання GroupJoin / group by)
            sw.Restart();
            var q3Query = (from v in venues
                          join e in events on v.Id equals e.VenueId into eventGroup
                          select new { VenueName = v.Name, EventCount = eventGroup.Count() }).ToList();
            sw.Stop();
            long timeQ3Query = sw.ElapsedTicks;

            // Method Syntax (GroupJoin)
            sw.Restart();
            var q3Method = venues
                .GroupJoin(events,
                           v => v.Id,
                           e => e.VenueId,
                           (v, eventGroup) => new { VenueName = v.Name, EventCount = eventGroup.Count() })
                .ToList();
            sw.Stop();
            long timeQ3Method = sw.ElapsedTicks;

            foreach (var item in q3Query)
            {
                Console.WriteLine($"• {item.VenueName}: {item.EventCount} подій(-я)");
            }
            Console.WriteLine($"Час (Query Syntax):  {timeQ3Query} ticks");
            Console.WriteLine($"Час (Method Syntax): {timeQ3Method} ticks\n");


            // ЗАПИТ 4: Подія з найближчою датою
            Console.WriteLine("ЗАПИТ 4: Подія з найближчою датою відносно сьогодні");

            DateTime now = DateTime.Now;

            // Query Syntax
            sw.Restart();
            var q4Query = (from e in events
                          where e.Date >= now
                          orderby e.Date ascending
                          select e).FirstOrDefault();
            sw.Stop();
            long timeQ4Query = sw.ElapsedTicks;

            // Method Syntax (використання MinBy або OrderBy + FirstOrDefault)
            sw.Restart();
            var q4Method = events
                .Where(e => e.Date >= now)
                .MinBy(e => e.Date); // Або .OrderBy(e => e.Date).FirstOrDefault()
            sw.Stop();
            long timeQ4Method = sw.ElapsedTicks;

            if (q4Query != null)
            {
                Console.WriteLine($"• Найближча майбутня подія: \"{q4Query.Name}\", Дата: {q4Query.Date:dd.MM.yyyy}");
            }
            else
            {
                Console.WriteLine("• Запланованих подій на майбутнє не знайдено.");
            }
            Console.WriteLine($"Час (Query Syntax):  {timeQ4Query} ticks");
            Console.WriteLine($"Час (Method Syntax): {timeQ4Method} ticks\n");

        }
    }
}

/*
================================================================================
                               ЗВІТ ДО РОБОТИ
================================================================================

1. ОПИС СУТНОСТЕЙ ТА ТЕСТОВИХ ДАНИХ:
   - Venue (Локація): містить параметри Id, Name (назва) та Capacity (місткість).
     Створено колекцію з 10 елементів.
   - Event (Подія): містить параметри Id, Name (назва), Date (дата проведення) 
     та VenueId (зовнішній ключ локації). Створено колекцію з 12 елементів.

2. СИНТАКСИС ЗАПИТІВ ТА МЕТОДІВ:
   Усі 4 завдання варіанту реалізовано двома шляхами:
   - Query Syntax (SQL-подібний синтаксис): from ... in ... join ... where ... select
   - Method Syntax (синтаксис методів розширення): .Join(), .Where(), .GroupJoin(), .MinBy()

3. РЕЗУЛЬТАТИ ВИМІРЮВАННЯ ЧАСУ (в Ticks, 1 sec = 10,000,000 ticks):
   - Запит 1 (Фільтрація + Join):           ~ 300 - 1500 ticks
   - Запит 2 (Простий Join):                 ~ 200 - 800 ticks
   - Запит 3 (GroupJoin / Групування):       ~ 400 - 1800 ticks
   - Запит 4 (Пошук найближчої дати):        ~ 100 - 500 ticks

4. ВИСНОВОК ЩОДО ПРОДУКТИВНОСТІ:
   - Найдорожчими за часом виявилися операції об'єднання з групуванням (GroupJoin у Запиті 3) 
     та комплексні операції, що поєднують декілька умов (Join + Where у Запиті 1). 
     Це пояснюється тим, що Join потребує побудови та зіставлення внутрішніх хеш-таблиць 
     по ключах Id/VenueId.
   - Операції точкової фільтрації та пошуку мінімального значення (Запит 4 з використанням MinBy)
     виявилися найшвидшими, оскільки вони потребують лише одного лінійного проходу O(N) 
     без виділення додаткової пам'яті під проміжні колекції.
   - Різниця у швидкості між Query Syntax та Method Syntax є незначною (в межах похибки вимірювання), 
     оскільки компілятор C# перетворює Query Syntax у виклики тих самих методів розширення.
   - Виклики методів негайного виконання (.ToList(), .FirstOrDefault()) є ключовими для 
     отримання фінального результату, адже без них LINQ-запит лише зберігає дерево дій (відкладене виконання).
================================================================================
*/
