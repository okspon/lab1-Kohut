using System;

namespace IndependentWork19
{
    public class TicketBookingException : Exception
    {
        public string EventCode { get; } = string.Empty;
        public int RequestedSeats { get; }
        public int AvailableSeats { get; }

        // 1. Стандартний конструктор без параметрів
        public TicketBookingException() { }

        // 2. Стандартний конструктор з повідомленням
        public TicketBookingException(string message) : base(message) { }

        // 3. Стандартний конструктор з повідомленням та внутрішнім винятком
        public TicketBookingException(string message, Exception innerException)
            : base(message, innerException) { }

        // 4. Спеціалізований конструктор з бізнес-контекстом
        public TicketBookingException(string message, string eventCode, int requestedSeats, int availableSeats)
            : base(message)
        {
            EventCode = eventCode;
            RequestedSeats = requestedSeats;
            AvailableSeats = availableSeats;
        }
    }

    public static class BookingService
    {
        public static void ReserveSeats(string eventCode, int requestedSeats, int availableSeats)
        {
            if (requestedSeats <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(requestedSeats), "Кількість місць має бути більшою за нуль.");
            }

            if (requestedSeats > availableSeats)
            {
                throw new TicketBookingException(
                    $"Недостатньо вільних місць на подію '{eventCode}'.",
                    eventCode,
                    requestedSeats,
                    availableSeats);
            }

            Console.WriteLine($"[BookingService] Успішно заброньовано {requestedSeats} місць на '{eventCode}'.");
        }
    }
}