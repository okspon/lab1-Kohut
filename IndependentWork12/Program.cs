using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace IndependentWork12;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("1. ДОСЛІДЖЕННЯ ПРОДУКТИВНОСТІ: LINQ vs PLINQ");
        int[] sizes = { 1_000_000, 5_000_000, 10_000_000 };

        foreach (int size in sizes)
        {
            Console.WriteLine($"\nТестування на колекції з {size:N0} елементів");
            List<int> data = GenerateData(size);

            // Прогрів JIT
            data.Take(100).Where(IsPrime).ToList();

            // 1. LINQ
            var sw = Stopwatch.StartNew();
            int linqCount = data.Where(IsPrime).Count();
            sw.Stop();
            long linqTime = sw.ElapsedMilliseconds;
            Console.WriteLine($"LINQ:  {linqTime} мс (Знайдено простих чисел: {linqCount})");

            // 2. PLINQ
            sw.Restart();
            int plinqCount = data.AsParallel().Where(IsPrime).Count();
            sw.Stop();
            long plinqTime = sw.ElapsedMilliseconds;
            Console.WriteLine($"PLINQ: {plinqTime} мс (Знайдено простих чисел: {plinqCount})");

            double speedup = (double)linqTime / (plinqTime > 0 ? plinqTime : 1);
            Console.WriteLine($"Прискорення (Speedup): {speedup:F2}x");
        }

        Console.WriteLine("\n2. ДОСЛІДЖЕННЯ БЕЗПЕКИ ТА ПОБІЧНИХ ЕФЕКТІВ");
        DemonstrateRaceCondition();
        DemonstrateFixWithInterlocked();
    }

    // Обчислювально інтенсивна операція: перевірка на просте число
    private static bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        var boundary = (int)Math.Floor(Math.Sqrt(number));
        for (int i = 3; i <= boundary; i += 2)
        {
            if (number % i == 0) return false;
        }
        return true;
    }

    private static List<int> GenerateData(int count)
    {
        var random = new Random(42);
        var list = new List<int>(count);
        for (int i = 0; i < count; i++)
        {
            list.Add(random.Next(1, 100_000));
        }
        return list;
    }

    // Демонстрація Race Condition (некоректна лічба через відсутність синхронізації)
    private static void DemonstrateRaceCondition()
    {
        Console.WriteLine("\n[Тест 1: Небезпечний PLINQ з побічним ефектом (Race Condition)]");
        List<int> numbers = Enumerable.Range(1, 1_000_000).ToList();

        int unsafeCounter = 0;

        // Помилка: Кілька потоків одночасно змінюють спільно використовувану змінну без синхронізації
        numbers.AsParallel().ForAll(n =>
        {
            if (n % 2 == 0)
            {
                unsafeCounter++; // НЕ ПОТОКОБЕЗПЕЧНО!
            }
        });

        Console.WriteLine($"Очікувана кількість парних чисел: 500,000");
        Console.WriteLine($"Фактично пораховано (Unsafe PLINQ): {unsafeCounter}");
        Console.WriteLine($"Результат коректний? -> {unsafeCounter == 500000}");
    }

    // Виправлення проблеми за допомогою атомарних операцій (Interlocked)
    private static void DemonstrateFixWithInterlocked()
    {
        Console.WriteLine("\n[Тест 2: Виправлений PLINQ з атомарними операціями (Interlocked)]");
        List<int> numbers = Enumerable.Range(1, 1_000_000).ToList();

        int safeCounter = 0;

        numbers.AsParallel().ForAll(n =>
        {
            if (n % 2 == 0)
            {
                Interlocked.Increment(ref safeCounter); // ПОТОКОБЕЗПЕЧНО!
            }
        });

        Console.WriteLine($"Фактично пораховано (Safe PLINQ):   {safeCounter}");
        Console.WriteLine($"Результат коректний? -> {safeCounter == 500000}");
    }
}