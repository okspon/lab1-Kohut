using System;
using IndependentWork12;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine($"=== Дослідження продуктивності та безпеки PLINQ (Ядер CPU: {Environment.ProcessorCount}) ===\n");

// Прогрів JIT
_ = PlinqBenchmarks.HeavyCompute(10007);

// Обов'язкові та проміжний розміри колекцій: 1M, 5M, 10M
PlinqBenchmarks.ComparePerformance(1_000_000);
PlinqBenchmarks.ComparePerformance(5_000_000);
PlinqBenchmarks.ComparePerformance(10_000_000);

// Демонстрація випадку, коли PLINQ не дає виграшу
PlinqBenchmarks.CompareTrivialOperation(1_000_000);

// Демонстрація побічних ефектів та потокобезпечних виправлень
PlinqBenchmarks.DemonstrateThreadSafety();