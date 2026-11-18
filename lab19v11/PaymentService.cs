using System;
using System.Collections.Generic;

namespace PaymentProcessingLab
{
    public class PaymentService
    {
        private readonly string _gatewayName = "LiqPay / Stripe Gateway";
        
        // Імітація бази успішних транзакцій
        private readonly HashSet<string> _completedTransactions = new HashSet<string>
        {
            "TXN-1001",
            "TXN-1002",
            "TXN-1003"
        };

        public void ProcessPayment(decimal amount)
        {
            if (amount <= 0)
            {
                throw new PaymentGatewayException(
                    "Некоректна сума платежу. Сума повинна бути більшою за нуль.",
                    _gatewayName,
                    "ERR_INVALID_AMOUNT"
                );
            }

            // Імітація технічної помилки платіжного шлюзу при великій сумі
            if (amount > 50000)
            {
                throw new PaymentGatewayException(
                    "Платіжний шлюз відхилив транзакцію через перевищення ліміту безпеки.",
                    _gatewayName,
                    "ERR_GATEWAY_LIMIT_EXCEEDED"
                );
            }

            string newTxnId = $"TXN-{new Random().Next(2000, 9999)}";
            _completedTransactions.Add(newTxnId);
            Console.WriteLine($"[PaymentService] Платіж на суму {amount} грн успішно проведено. ID: {newTxnId}");
        }

        public Result<bool> RefundPayment(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
            {
                return Result<bool>.Failure("Ідентифікатор транзакції не може бути порожнім.");
            }

            if (!_completedTransactions.Contains(transactionId))
            {
                return Result<bool>.Failure($"Транзакцію з ID '{transactionId}' не знайдено в системі.");
            }

            _completedTransactions.Remove(transactionId);
            return Result<bool>.Success(true);
        }
    }
}