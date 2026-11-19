using System;
using IndependentWork19;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("Демонстрація власного винятку TicketBookingException\n");

try
{
    BookingService.ReserveSeats("EVT-2026-KYIV", requestedSeats: 2, availableSeats: 10);
    // Спроба забронювати більше місць, ніж є в наявності
    BookingService.ReserveSeats("EVT-2026-LVIV", requestedSeats: 5, availableSeats: 3);
}
catch (TicketBookingException ex)
{
    Console.WriteLine($"[Перехоплено TicketBookingException] {ex.Message}");
    Console.WriteLine($"  Деталі: Подія = {ex.EventCode}, Запитано = {ex.RequestedSeats}, Доступно = {ex.AvailableSeats}");
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"[Некоректні параметри] {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"[Загальна помилка] {ex.Message}");
}
finally
{
    Console.WriteLine("[Finally] Блок перевірки бронювання завершено.\n");
}

await ResilienceScenarios.RunPaymentRetryScenarioAsync();
await ResilienceScenarios.RunLoyaltyCircuitBreakerScenarioAsync();
await ResilienceScenarios.RunRecommendationTimeoutFallbackScenarioAsync();