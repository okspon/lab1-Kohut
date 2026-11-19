using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Timeout;

namespace IndependentWork19
{
    public static class ResilienceScenarios
    {
        // СЦЕНАРІЙ 1: Retry з експоненційною затримкою (Exponential Backoff) та Jitter
        public static async Task RunPaymentRetryScenarioAsync()
        {
            Console.WriteLine(" Сценарій 1: Retry + Exponential Backoff + Jitter (Платіжний шлюз)");
            int attemptCounter = 0;

            var retryPipeline = new ResiliencePipelineBuilder<string>()
                .AddRetry(new RetryStrategyOptions<string>
                {
                    ShouldHandle = new PredicateBuilder<string>().Handle<HttpRequestException>(),
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromMilliseconds(400),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    OnRetry = args =>
                    {
                        Console.WriteLine(
                            $"  [Polly Retry] Спроба #{args.AttemptNumber + 1} не вдалася ({args.Outcome.Exception?.Message}). " +
                            $"Повтор через {args.RetryDelay.TotalMilliseconds:F0} мс...");
                        return ValueTask.CompletedTask;
                    }
                })
                .Build();

            try
            {
                string paymentResult = await retryPipeline.ExecuteAsync(async token =>
                {
                    attemptCounter++;
                    await Task.Delay(50, token);

                    if (attemptCounter < 3)
                    {
                        throw new HttpRequestException($"503 Service Unavailable (збій шлюзу на виклику #{attemptCounter})");
                    }

                    return $"TXN-{Guid.NewGuid().ToString()[..8].ToUpper()} (Оплата підтверджена на виклику #{attemptCounter})";
                });

                Console.WriteLine($"  [Результат] Успіх: {paymentResult}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [Результат] Остаточна помилка оплати: {ex.Message}\n");
            }
        }

        // СЦЕНАРІЙ 2: Circuit Breaker (Автоматичний вимикач для сервісу бонусів)
        public static async Task RunLoyaltyCircuitBreakerScenarioAsync()
        {
            Console.WriteLine(" Сценарій 2: Circuit Breaker (Нестабільна БД бонусної програми)");
            bool isDatabaseHealthy = false;

            var circuitBreakerPipeline = new ResiliencePipelineBuilder<int>()
                .AddCircuitBreaker(new CircuitBreakerStrategyOptions<int>
                {
                    ShouldHandle = new PredicateBuilder<int>().Handle<TimeoutException>(),
                    FailureRatio = 1.0,          // 100% помилок у вибірці
                    MinimumThroughput = 3,       // Мінімум 3 запити для оцінки
                    SamplingDuration = TimeSpan.FromSeconds(10),
                    BreakDuration = TimeSpan.FromSeconds(1), // Час у стані Open перед переходом у Half-Open
                    OnOpened = args =>
                    {
                        Console.WriteLine($"  [Circuit Breaker -> OPEN] Сервіс недоступний! Ланцюг розімкнено на {args.BreakDuration.TotalSeconds} с.");
                        return ValueTask.CompletedTask;
                    },
                    OnClosed = _ =>
                    {
                        Console.WriteLine("  [Circuit Breaker -> CLOSED] Роботу сервісу відновлено, ланцюг замкнено.");
                        return ValueTask.CompletedTask;
                    },
                    OnHalfOpened = _ =>
                    {
                        Console.WriteLine("  [Circuit Breaker -> HALF-OPEN] Пробний запит для перевірки відновлення сервісу...");
                        return ValueTask.CompletedTask;
                    }
                })
                .Build();

            // Виконуємо 6 послідовних запитів
            for (int requestNum = 1; requestNum <= 6; requestNum++)
            {
                // На 5-му запиті імітуємо відновлення бази даних після паузи
                if (requestNum == 5)
                {
                    Console.WriteLine("  ... Очікування 1.2 с для переходу вимикача у Half-Open та відновлення БД ...");
                    await Task.Delay(1200);
                    isDatabaseHealthy = true;
                }

                try
                {
                    int bonusPoints = await circuitBreakerPipeline.ExecuteAsync(async token =>
                    {
                        await Task.Delay(30, token);
                        if (!isDatabaseHealthy)
                        {
                            throw new TimeoutException("Таймаут підключення до LoyaltyDB");
                        }
                        return 450;
                    });

                    Console.WriteLine($"  Запит #{requestNum}: Отримано {bonusPoints} бонусних балів.");
                }
                catch (BrokenCircuitException)
                {
                    Console.WriteLine($"  Запит #{requestNum}: МИТТЄВО ВІДХИЛЕНО (ланцюг у стані Open, навантаження на БД не створюється).");
                }
                catch (TimeoutException ex)
                {
                    Console.WriteLine($"  Запит #{requestNum}: Помилка виклику БД — {ex.Message}");
                }
            }
            Console.WriteLine();
        }

        // СЦЕНАРІЙ 3: Комбінація Timeout + Fallback (Сервіс персональних рекомендацій)
        public static async Task RunRecommendationTimeoutFallbackScenarioAsync()
        {
            Console.WriteLine(" Сценарій 3: Комбінація Timeout + Fallback (Сервіс рекомендацій)");

            var fallbackAndTimeoutPipeline = new ResiliencePipelineBuilder<List<string>>()
                .AddFallback(new FallbackStrategyOptions<List<string>>
                {
                    ShouldHandle = new PredicateBuilder<List<string>>().Handle<TimeoutRejectedException>(),
                    FallbackAction = args =>
                    {
                        Console.WriteLine($"  [Polly Fallback] Спрацював резервний сценарій через {args.Outcome.Exception?.GetType().Name}. Повертаємо кешований топ подій.");
                        var cachedEvents = new List<string> { "Концерт Океан Ельзи (Кеш)", "IT-конференція .NET Day (Кеш)" };
                        return Outcome.FromResultAsValueTask(cachedEvents);
                    }
                })
                .AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = TimeSpan.FromMilliseconds(300),
                    OnTimeout = args =>
                    {
                        Console.WriteLine($"  [Polly Timeout] Перевищено ліміт очікування ({args.Timeout.TotalMilliseconds} мс)!");
                        return ValueTask.CompletedTask;
                    }
                })
                .Build();

            var recommendations = await fallbackAndTimeoutPipeline.ExecuteAsync(async cancellationToken =>
            {
                Console.WriteLine("  [RecommendationService] Пошук персональних рекомендацій (імітація затримки 1000 мс)...");
                // Передаємо cancellationToken, щоб Polly міг коректно перервати повільну операцію
                await Task.Delay(1000, cancellationToken);
                return new List<string> { "Персональна подія #1" };
            });

            Console.WriteLine($"  [Результат] Отримані рекомендації: {string.Join(", ", recommendations)}\n");
        }
    }
}