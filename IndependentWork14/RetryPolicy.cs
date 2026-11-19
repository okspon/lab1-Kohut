using System;
using System.Threading;

namespace IndependentWork14
{
    public class RetryPolicy
    {
        private readonly int _maxRetries;
        private readonly TimeSpan _initialDelay;
        private readonly double _jitterFactor;
        private readonly Random _random = new();

        public RetryPolicy(int maxRetries, TimeSpan initialDelay, double jitterFactor = 0.2)
        {
            if (maxRetries < 0)
                throw new ArgumentOutOfRangeException(nameof(maxRetries), "Кількість повторів не може бути від'ємною.");
            if (initialDelay < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(initialDelay), "Початкова затримка не може бути від'ємною.");
            if (jitterFactor < 0 || jitterFactor > 1)
                throw new ArgumentOutOfRangeException(nameof(jitterFactor), "Коефіцієнт jitter має бути в межах [0, 1].");

            _maxRetries = maxRetries;
            _initialDelay = initialDelay;
            _jitterFactor = jitterFactor;
        }

        public T Execute<T>(Func<T> operation, Func<Exception, bool>? shouldRetry = null)
        {
            ArgumentNullException.ThrowIfNull(operation);

            int retryAttempt = 0;

            while (true)
            {
                try
                {
                    Console.WriteLine($"[RetryPolicy] Спроба #{retryAttempt + 1} (повтор #{retryAttempt} із {_maxRetries})...");
                    T result = operation();
                    Console.WriteLine($"[RetryPolicy] Операція успішно виконана на спробі #{retryAttempt + 1}.");
                    return result;
                }
                catch (Exception ex)
                {
                    bool canRetry = retryAttempt < _maxRetries && (shouldRetry == null || shouldRetry(ex));

                    if (!canRetry)
                    {
                        Console.WriteLine(
                            $"[RetryPolicy] Повтор неможливий (виконано повторів: {retryAttempt}/{_maxRetries}, " +
                            $"тип винятку: {ex.GetType().Name}). Прокидаємо виняток далі.");
                        throw;
                    }

                    // Обов'язкова формула exponential backoff з jitter:
                    // delay = initialDelay * Math.Pow(2, retryAttempt) * (1 + random.NextDouble() * jitterFactor * 2 - jitterFactor)
                    double multiplier = Math.Pow(2, retryAttempt) * (1 + _random.NextDouble() * _jitterFactor * 2 - _jitterFactor);
                    TimeSpan delay = TimeSpan.FromMilliseconds(Math.Max(0, _initialDelay.TotalMilliseconds * multiplier));

                    Console.WriteLine(
                        $"[RetryPolicy] Помилка на спробі #{retryAttempt + 1}: [{ex.GetType().Name}] {ex.Message}. " +
                        $"Очікування {delay.TotalMilliseconds:F0} мс перед наступною спробою...");

                    Thread.Sleep(delay);
                    retryAttempt++;
                }
            }
        }
    }
}