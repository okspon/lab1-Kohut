using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace Lab18v12
{
    // 1. ДИСЦИПЛІНА ТА СУТНОСТІ СЦЕНАРІЮ

    /// Процесор файлів з імітацією тимчасових помилок доступу do файлу.
    public class FileProcessor
    {
        private int _attemptCount = 0;

        /// Метод завантаження продуктів з файлу.
        /// Перші 2 рази вибиває FileNotFoundException, на 3-й — успіх.
        public List<string> LoadProductNames(string path)
        {
            _attemptCount++;
            Console.WriteLine($"   [FileProcessor] Спроба №{_attemptCount} читання файлу за шляхом: '{path}'");

            if (_attemptCount <= 2)
            {
                throw new FileNotFoundException($"[Імітація] Файл за шляхом '{path}' не знайдено (Спроба {_attemptCount}).");
            }

            // Успішний результат на 3-й спробі
            return new List<string>
            {
                "Ноутбук Lenovo Legion",
                "Смартфон Samsung Galaxy",
                "Навушники Sony WH-1000XM5"
            };
        }
    }

    /// Мережевий клієнт з імітацією тимчасових мережевих помилок.
    public class NetworkClient
    {
        private int _attemptCount = 0;

        /// Метод отримання продуктів через API.
        /// Перші 3 рази вибиває HttpRequestException, на 4-й — успіх.

        public List<string> GetProductsFromApi(string url)
        {
            _attemptCount++;
            Console.WriteLine($"   [NetworkClient] Спроба №{_attemptCount} запиту do API за адресою: '{url}'");

            if (_attemptCount <= 3)
            {
                throw new HttpRequestException($"[Імітація] Помилка мережевого з'єднання (HTTP 503) під час запиту do '{url}' (Спроба {_attemptCount}).");
            }

            // Успішний результат на 4-й спробі
            return new List<string>
            {
                "Монітор Dell UltraSharp",
                "Механічна клавіатура Keychron",
                "Миша Logitech MX Master 3S"
            };
        }
    }

    // 2. УЗАГАЛЬНЕНИЙ RETRY HELPER

    public static class RetryHelper
    {

        public static T ExecuteWithRetry<T>(
            Func<T> operation,
            int maxRetries = 4,
            int initialDelayMs = 500,
            Predicate<Exception>? shouldRetry = null)
        {
            int currentAttempt = 0;

            while (true)
            {
                try
                {
                    currentAttempt++;
                    return operation();
                }
                catch (Exception ex)
                {
                    // Перевіряємо, чи дозволено повторювати спробу для даного винятку
                    if (shouldRetry != null && !shouldRetry(ex))
                    {
                        Console.WriteLine($"   [RetryHelper] Виняток {ex.GetType().Name} НЕ підпадає під правило shouldRetry! Повтор скасовано.");
                        throw;
                    }

                    if (currentAttempt > maxRetries)
                    {
                        Console.WriteLine($"   [RetryHelper] Перевищено ліміт спроб ({maxRetries}). Передаємо виняток вище.");
                        throw;
                    }

                    // Формула експоненційної затримки: delay = initialDelay * (2 ^ (attempt - 1))
                    int delayMs = initialDelayMs * (int)Math.Pow(2, currentAttempt - 1);

                    Console.WriteLine($"   [RetryHelper] Перехоплено помилку: {ex.GetType().Name} - '{ex.Message}'");
                    Console.WriteLine($"   [RetryHelper] Спроба {currentAttempt}/{maxRetries} не вдалася. Очікування {delayMs} мс перед наступною спробою...\n");

                    Thread.Sleep(delayMs);
                }
            }
        }
    }

    // 3. ТОЧКА ВХОДУ (MAIN) ТА ДЕМОНСТРАЦІЯ

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;


            // Визначення правила shouldRetry: повторювати ТІЛЬКИ для FileNotFoundException та HttpRequestException
            Predicate<Exception> shouldRetry = ex => ex is FileNotFoundException || ex is HttpRequestException;

            // СЦЕНАРІЙ 1: FileProcessor (FileNotFoundException 2 рази, потім успіх)
            Console.WriteLine("[Сценарій 1] Читання продуктів з файлу (FileProcessor)");
            try
            {
                var fileProcessor = new FileProcessor();
                List<string> fileProducts = RetryHelper.ExecuteWithRetry(
                    operation: () => fileProcessor.LoadProductNames("data/products.txt"),
                    maxRetries: 4,
                    initialDelayMs: 400,
                    shouldRetry: shouldRetry
                );

                Console.WriteLine("\n[УСПІХ] Продукти успішно завантажені з файлу:");
                foreach (var product in fileProducts)
                {
                    Console.WriteLine($"  • {product}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ПОМИЛКА] Операцію читання файлу не вдалося виконати: {ex.Message}");
            }

            // СЦЕНАРІЙ 2: NetworkClient (HttpRequestException 3 рази, потім успіх)
            Console.WriteLine(" [Сценарій 2] Запит продуктів з API (NetworkClient)");
            try
            {
                var networkClient = new NetworkClient();
                List<string> apiProducts = RetryHelper.ExecuteWithRetry(
                    operation: () => networkClient.GetProductsFromApi("https://api.store.com/products"),
                    maxRetries: 4,
                    initialDelayMs: 300,
                    shouldRetry: shouldRetry
                );

                Console.WriteLine("\n[УСПІХ] Продукти успішно отримані з API:");
                foreach (var product in apiProducts)
                {
                    Console.WriteLine($"  • {product}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ПОМИЛКА] Операцію запиту do API не вдалося виконати: {ex.Message}");
            }

            // СЦЕНАРІЙ 3: Демонстрація фільтрації через shouldRetry (Критична помилка)
            Console.WriteLine("[Сценарій 3] Демонстрація непідтримуваного винятку (InvalidOperationException) ");
            try
            {
                RetryHelper.ExecuteWithRetry<int>(
                    operation: () => throw new InvalidOperationException("Критична помилка конфігурації!"),
                    maxRetries: 3,
                    initialDelayMs: 200,
                    shouldRetry: shouldRetry
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[УСПІШНЕ ПЕРЕХОПЛЕННЯ] RetryHelper зупинив виконання для {ex.GetType().Name}, як і очікувалося.");
            }

            Console.ReadKey();
        }
    }
}

