using System;

namespace PaymentProcessingLab
{
    public class PaymentGatewayException : Exception
    {
        public string GatewayName { get; }
        public string ErrorCode { get; }

        // Стандартні конструктори
        public PaymentGatewayException() 
            : base("Виникла помилка під час взаємодії з платіжним шлюзом.") { }

        public PaymentGatewayException(string message) 
            : base(message) { }

        public PaymentGatewayException(string message, Exception innerException) 
            : base(message, innerException) { }

        // Кастомні конструктори з додатковими властивостями варіанту
        public PaymentGatewayException(string message, string gatewayName, string errorCode)
            : base(message)
        {
            GatewayName = gatewayName;
            ErrorCode = errorCode;
        }

        public PaymentGatewayException(string message, string gatewayName, string errorCode, Exception innerException)
            : base(message, innerException)
        {
            GatewayName = gatewayName;
            ErrorCode = errorCode;
        }
    }
}