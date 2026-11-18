using System;
using System.Text;

namespace PaymentProcessingLab
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var paymentService = new PaymentService();

            Console.WriteLine("1. Демонстрація роботи з власним винятком (ProcessPayment)\n");

            // Успішний сценарій
            try
            {
                paymentService.ProcessPayment(1500.50m);
            }
            catch (PaymentGatewayException ex)
            {
                Console.WriteLine($"Помилка шлюзу [{ex.GatewayName}] (Код: {ex.ErrorCode}): {ex.Message}");
            }

            // Сценарій з викиданням власного винятку
            try
            {
                Console.WriteLine("\nСпроба оплати на суму 75 000 грн...");
                paymentService.ProcessPayment(75000m);
            }
            catch (PaymentGatewayException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Виняток перехоплено!]");
                Console.WriteLine($"Шлюз: {ex.GatewayName}");
                Console.WriteLine($"Код помилки: {ex.ErrorCode}");
                Console.WriteLine($"Повідомлення: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\n2. Демонстрація роботи патерну Result<T> (RefundPayment)\n");

            // Очікувана невдача (транзакція не знайдена)
            string invalidTxn = "TXN-9999";
            Console.WriteLine($"Запит на повернення коштів для {invalidTxn}:");
            Result<bool> failedRefund = paymentService.RefundPayment(invalidTxn);

            if (failedRefund.IsSuccess)
            {
                Console.WriteLine($"Успіх! Кошти повернуто: {failedRefund.Value}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[Очікувана відмова]: {failedRefund.Error}");
                Console.ResetColor();
            }

            // Успішне повернення коштів
            string validTxn = "TXN-1001";
            Console.WriteLine($"\nЗапит на повернення коштів для {validTxn}:");
            Result<bool> successRefund = paymentService.RefundPayment(validTxn);

            if (successRefund.IsSuccess)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[Успіх]: Транзакцію скасовано, кошти повернуто (Статус: {successRefund.Value}).");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"Помилка: {successRefund.Error}");
            }
        }
    }
}