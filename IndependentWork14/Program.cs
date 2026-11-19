using System;
using System.Net.Http;
using IndependentWork14;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var policy = new RetryPolicy(
    maxRetries: 3,
    initialDelay: TimeSpan.FromMilliseconds(300),
    jitterFactor: 0.25);


// СЦЕНАРІЙ 1: Операція спочатку падає з тимчасовою помилкою, потім успішна

Console.WriteLine("Сценарій 1: Тимчасовий збій API -> Успіх після повторів");
int apiCalls = 0;

try
{
    decimal rate = policy.Execute(
        operation: () =>
        {
            apiCalls++;
            if (apiCalls <= 2)
            {
                throw new TimeoutException($"Таймаут відповіді банківського API (виклик #{apiCalls})");
            }
            return 41.85m;
        },
        shouldRetry: ex => ex is TimeoutException or HttpRequestException);

    Console.WriteLine($"[Main] Отримано актуальний курс USD: {rate} грн\n");
}
catch (Exception ex)
{
    Console.WriteLine($"[Main] Неочікувана помилка: {ex.Message}\n");
}

// СЦЕНАРІЙ 2: Операція завжди падає -> викидання винятку після всіх спроб
Console.WriteLine("Сценарій 2: Постійний збій -> Вичерпання maxRetries та виняток");
int dbCalls = 0;

try
{
    string data = policy.Execute<string>(
        operation: () =>
        {
            dbCalls++;
            throw new HttpRequestException($"503 Service Unavailable (спроба підключення #{dbCalls})");
        },
        shouldRetry: ex => ex is TimeoutException or HttpRequestException);

    Console.WriteLine($"[Main] Дані: {data}\n");
}
catch (Exception ex)
{
    Console.WriteLine($"[Main] Перехоплено остаточний виняток після {dbCalls} спроб: [{ex.GetType().Name}] {ex.Message}\n");
}

// СЦЕНАРІЙ 3: Помилка, яку НЕ варто повторювати (фільтр shouldRetry)
Console.WriteLine("Сценарій 3: Критична помилка (shouldRetry = false -> негайний вихід)");
try
{
    policy.Execute<int>(
        operation: () => throw new UnauthorizedAccessException("Недійсний API-ключ доступу"),
        shouldRetry: ex => ex is TimeoutException or HttpRequestException);
}
catch (UnauthorizedAccessException ex)
{
    Console.WriteLine($"[Main] Повторів не виконувалось. Перехоплено критичну помилку: {ex.Message}\n");
}