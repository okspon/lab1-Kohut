using System;

namespace Lab3
{
    // Варіант 11: Клас CryptoStream
    public class CryptoStream : IDisposable
    {
        private string _algorithm;
        private bool _isStreamOpen;
        private bool _disposed = false;

        public string Algorithm
        {
            get { return _algorithm; }
        }

        public bool IsStreamOpen
        {
            get { return _isStreamOpen; }
        }

        public CryptoStream(string algorithm)
        {
            _algorithm = algorithm;
            _isStreamOpen = true;
            Console.WriteLine("Криптопотік відкрито");
        }

        public void Encrypt(string data)
        {
            if (!_isStreamOpen)
            {
                Console.WriteLine("Потік закритий");
                return;
            }

            Console.WriteLine($"Шифрування даних: {data}");
            Console.WriteLine($"Алгоритм: {_algorithm}");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.WriteLine("Звільнення керованих ресурсів");
                }

                if (_isStreamOpen)
                {
                    Console.WriteLine("Криптопотік закрито");
                    _isStreamOpen = false;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CryptoStream()
        {
            Console.WriteLine("Деструктор викликано");
            Dispose(false);
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== 1. Використання using ===");
            using (CryptoStream stream = new CryptoStream("AES"))
            {
                stream.Encrypt("Hello World");
            }

            Console.WriteLine();

            Console.WriteLine("=== 2. Явний виклик Dispose() ===");
            CryptoStream stream2 = new CryptoStream("RSA");
            stream2.Encrypt("Test data");
            stream2.Dispose();

            Console.WriteLine();

            Console.WriteLine("=== 3. Без Dispose(), через GC ===");
            CreateAndForgetStream(); // Виносимо об'єкт в окремий метод

            // Примусовий запуск збирача сміття
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        // Окремий метод гарантує, що об'єкт втрачає посилання
        static void CreateAndForgetStream()
        {
            CryptoStream stream3 = new CryptoStream("SHA-256");
            stream3.Encrypt("Secret data");
        }
    }
}