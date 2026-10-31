using System;
using System.Collections.Generic;

namespace Lab15v11
{
    // Клас для представлення об'єкта в кеші
    public class CacheItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }

        public CacheItem(string key, string value)
        {
            Key = key;
            Value = value;
            CreatedAt = DateTime.Now;
        }

        public override string ToString() => $"[{Key}]: \"{Value}\" (створено: {CreatedAt:HH:mm:ss})";
    }

    // Менеджер кешу, що поєднує HashSet, Queue та Stack
    public class CacheManager
    {
        private readonly int _capacity;
        
        // 1. HashSet: швидка перевірка унікальності ключів за O(1)
        private readonly HashSet<string> _keys;

        // 2. Queue: порядок витіснення за принципом FIFO (Oldest First)
        private readonly Queue<string> _evictionQueue;

        // 3. Stack: історія останніх звернень / дій (LIFO)
        private readonly Stack<string> _accessHistory;

        // Словник для збереження самих даних кешу
        private readonly Dictionary<string, CacheItem> _storage;

        public CacheManager(int capacity)
        {
            _capacity = capacity;
            _keys = new HashSet<string>();
            _evictionQueue = new Queue<string>();
            _accessHistory = new Stack<string>();
            _storage = new Dictionary<string, CacheItem>();
        }

        //РОБОТА З HASHSET ТА QUEUE
        public void Add(string key, string value)
        {
            Console.WriteLine($"\n[Спроба додати]: Key = '{key}', Value = '{value}'");

            // Використання HashSet.Contains() за O(1)
            if (_keys.Contains(key))
            {
                Console.WriteLine($"  -> Ключ '{key}' вже існує в кеші. Оновлюємо значення.");
                _storage[key].Value = value;
                RecordAccess(key);
                return;
            }

            // Перевірка переповнення кешу (витіснення за чергою Queue - FIFO)
            if (_keys.Count >= _capacity)
            {
                EvictOldest();
            }

            // Додавання елемента
            _keys.Add(key);                           // HashSet
            _evictionQueue.Enqueue(key);              // Queue
            _storage[key] = new CacheItem(key, value);
            
            RecordAccess(key);
            Console.WriteLine($"  -> Успішно додано ключ '{key}'. Поточний розмір кешу: {_keys.Count}/{_capacity}");
        }

        // Видалення найстарішого елемента (Queue - FIFO)
        private void EvictOldest()
        {
            if (_evictionQueue.Count == 0) return;

            // Отримуємо найстаріший ключ з черги
            string oldestKey = _evictionQueue.Dequeue(); // Queue.Dequeue()
            
            // Видаляємо ключ із HashSet та словника
            _keys.Remove(oldestKey);                     // HashSet.Remove()
            _storage.Remove(oldestKey);

            Console.WriteLine($"  [КЕШ ПЕРЕПОВНЕНО] Видалено найстаріший елемент (FIFO): '{oldestKey}'");
        }

        // Отримання елемента з кешу
        public CacheItem Get(string key)
        {
            Console.WriteLine($"\n[Пошук в кеші]: Key = '{key}'");
            
            // HashSet.Contains() забезпечує швидку перевірку
            if (_keys.Contains(key))
            {
                Console.WriteLine($"  -> Знайдено за O(1)!");
                RecordAccess(key);
                return _storage[key];
            }

            Console.WriteLine($"  -> Ключ '{key}' відсутній у кеші.");
            return null;
        }

        //РОБОТА ЗІ STACK 
        private void RecordAccess(string key)
        {
            _accessHistory.Push(key); // Stack.Push() - фіксуємо звернення
        }

        public void PrintAccessHistory()
        {
            Console.WriteLine("\nІсторія останніх звернень до кешу (Stack - LIFO)");
            if (_accessHistory.Count == 0)
            {
                Console.WriteLine("Історія порожня.");
                return;
            }

            Console.WriteLine($"Останнє звернення (Peek): '{_accessHistory.Peek()}'");
            Console.WriteLine("Повний список звернень (від найновішого до найстарішого):");
            
            int index = 1;
            foreach (var key in _accessHistory)
            {
                Console.WriteLine($"  {index++}. {key}");
            }
        }

        public void PopLastAccess()
        {
            if (_accessHistory.Count > 0)
            {
                string popped = _accessHistory.Pop(); // Stack.Pop()
                Console.WriteLine($"\n[Undo/Pop history] Видалено останній запис з історії звернень: '{popped}'");
            }
        }

        public void PrintCurrentCacheState()
        {
            Console.WriteLine("\nПоточний стан кешу");
            Console.WriteLine($"Унікальні ключі (HashSet count = {_keys.Count}): [ {string.Join(", ", _keys)} ]");
            
            if (_evictionQueue.Count > 0)
            {
                Console.WriteLine($"Наступний на видалення при переповненні (Queue Peek): '{_evictionQueue.Peek()}'");
            }
            
            Console.WriteLine("Вміст кешу:");
            foreach (var item in _storage.Values)
            {
                Console.WriteLine($"  - {item}");
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // Створюємо кеш ємністю 3 елементи
            CacheManager cache = new CacheManager(capacity: 3);

            // 1. Демонстрація додавання та переповнення черги (Queue/HashSet)
            Console.WriteLine("\nStep 1: Наповнення кешу");
            cache.Add("user_1", "Іван Іваненко");
            cache.Add("user_2", "Петро Петренко");
            cache.Add("user_3", "Олена Сидоренко");

            cache.PrintCurrentCacheState();

            // 2. Додаємо 4-й елемент — має спрацювати FIFO витіснення ("user_1" зникне)
            Console.WriteLine("\nStep 2: Додавання понаднормового елемента");
            cache.Add("user_4", "Марія Коваль");

            cache.PrintCurrentCacheState();

            // 3. Демонстрація пошуку елементів (HashSet Contains O(1))
            Console.WriteLine("\nStep 3: Перевірка наявності елементів");
            cache.Get("user_1"); // Вже видалено
            cache.Get("user_3"); // Наявний

            // 4. Демонстрація роботи з дублікатами
            Console.WriteLine("\nStep 4: Спроба повторного додавання існуючого ключа");
            cache.Add("user_2", "Петро Петренко (Оновлено)");

            // 5. Демонстрація історії звернень (Stack LIFO)
            cache.PrintAccessHistory();

            // Видалення з вершини стеку
            cache.PopLastAccess();
            cache.PrintAccessHistory();

        }
    }
}