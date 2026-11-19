using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace IndependentWork12
{
    public static class PlinqBenchmarks
    {
        public static List<int> GenerateData(int count, int seed = 42)
        {
            var random = new Random(seed);
            var list = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                // Генеруємо числа від 1000 до 50000 для відчутної обчислювальної складності
                list.Add(random.Next(1000, 50000));
            }
            return list;
        }

        /// Обчислювально інтенсивна операція: перевірка на простоту + математичні перетворення
        public static double HeavyCompute(int number)
        {
            bool isPrime = true;
            int limit = (int)Math.Sqrt(number);
            for (int i = 2; i <= limit; i++)
            {
                if (number % i == 0)
                {
                    isPrime = false;
                    break;
                }
            }

            double acc = isPrime ? number * 1.5 : number * 0.5;
            for (int k = 1; k <= 15; k++)
            {
                acc += Math.Sin(number + k) * Math.Cos(k) + Math.Sqrt(acc + k);
            }
            return acc;
        }

        /// Порівняння продуктивності LINQ та PLINQ на заданому розмірі колекції
        public static void ComparePerformance(int size)
        {
            Console.WriteLine($"Експеримент: {size:N0} елементів (важка обчислювальна операція)");
            var data = GenerateData(size);

            // 1. Звичайний послідовний LINQ
            var swLinq = Stopwatch.StartNew();
            var linqResult = data
                .Where(x => x % 2 != 0)
                .Select(HeavyCompute)
                .ToList();
            swLinq.Stop();

            // 2. Паралельний PLINQ
            var swPlinq = Stopwatch.StartNew();
            var plinqResult = data
                .AsParallel()
                .Where(x => x % 2 != 0)
                .Select(HeavyCompute)
                .ToList();
            swPlinq.Stop();

            double speedup = (double)swLinq.ElapsedMilliseconds / Math.Max(1, swPlinq.ElapsedMilliseconds);

            Console.WriteLine($"  LINQ:        {swLinq.ElapsedMilliseconds,6} мс (оброблено: {linqResult.Count:N0})");
            Console.WriteLine($"  PLINQ:       {swPlinq.ElapsedMilliseconds,6} мс (оброблено: {plinqResult.Count:N0})");
            Console.WriteLine($"  Прискорення: {speedup:F2}x (використано логічних ядер: {Environment.ProcessorCount})\n");
        }

        /// Контрольний тест: коли PLINQ може бути повільнішим за LINQ (дешева операція)
        public static void CompareTrivialOperation(int size = 1_000_000)
        {
            Console.WriteLine($"Контрольний тест: {size:N0} елементів (дуже дешева операція x * 2)");
            var data = GenerateData(size);

            var swLinq = Stopwatch.StartNew();
            var linqResult = data.Where(x => x > 25000).Select(x => x * 2).ToList();
            swLinq.Stop();

            var swPlinq = Stopwatch.StartNew();
            var plinqResult = data.AsParallel().Where(x => x > 25000).Select(x => x * 2).ToList();
            swPlinq.Stop();

            Console.WriteLine($"  LINQ:  {swLinq.Elapsed.TotalMilliseconds,6:F3} мс");
            Console.WriteLine($"  PLINQ: {swPlinq.Elapsed.TotalMilliseconds,6:F3} мс (накладні витрати на розподіл потоків)\n");
        }

        /// <summary>
        /// Дослідження безпеки (побічні ефекти / Race Condition) та їх виправлення
        /// </summary>
        public static void DemonstrateThreadSafety()
        {
            Console.WriteLine("Дослідження безпеки PLINQ: побічні ефекти та Race Condition");
            var data = Enumerable.Range(1, 1_000_000).ToList();
            int expected = data.Count(x => x % 2 == 0);
            Console.WriteLine($"Очікувана кількість парних чисел: {expected:N0}\n");

            // 1. НЕБЕЗПЕЧНИЙ варіант: модифікація спільної змінної без синхронізації
            Console.WriteLine("1. Небезпечний PLINQ (спільна змінна unsafeCounter++ без lock):");
            for (int run = 1; run <= 3; run++)
            {
                int unsafeCounter = 0;
                _ = data.AsParallel()
                    .Where(x => x % 2 == 0)
                    .Select(x =>
                    {
                        unsafeCounter++; // Race condition! Операція не атомарна
                        return x;
                    })
                    .ToList();

                int lost = expected - unsafeCounter;
                Console.WriteLine($"   Прогін #{run}: отримано {unsafeCounter:N0} (втрачено {lost:N0} інкрементів!)");
            }

            // 2. ВИПРАВЛЕННЯ А: використання Interlocked.Increment
            var swInterlocked = Stopwatch.StartNew();
            int interlockedCounter = 0;
            _ = data.AsParallel()
                .Where(x => x % 2 == 0)
                .Select(x =>
                {
                    Interlocked.Increment(ref interlockedCounter);
                    return x;
                })
                .ToList();
            swInterlocked.Stop();
            Console.WriteLine($"\n2. Виправлення через Interlocked.Increment: {interlockedCounter:N0} (коректно, час: {swInterlocked.ElapsedMilliseconds} мс)");

            // 3. ВИПРАВЛЕННЯ Б: використання блокування lock
            var swLock = Stopwatch.StartNew();
            int lockedCounter = 0;
            object syncRoot = new();
            _ = data.AsParallel()
                .Where(x => x % 2 == 0)
                .Select(x =>
                {
                    lock (syncRoot)
                    {
                        lockedCounter++;
                    }
                    return x;
                })
                .ToList();
            swLock.Stop();
            Console.WriteLine($"3. Виправлення через lock:                  {lockedCounter:N0} (коректно, час: {swLock.ElapsedMilliseconds} мс)");

            // 4. ВИПРАВЛЕННЯ В (найкраща практика): потокобезпечна колекція ConcurrentBag<T> + чистий PLINQ
            var bag = new ConcurrentBag<int>();
            data.AsParallel()
                .Where(x => x % 2 == 0)
                .ForAll(x => bag.Add(x));
            Console.WriteLine($"4. Виправлення через ConcurrentBag<int>:    {bag.Count:N0} (коректно, без втрати даних)");
        }
    }
}