using System;
using System.Collections.Generic;

namespace Lab6vN
{
    // 1. БАЗОВИЙ КЛАС: Instrument
    public class Instrument
    {
        // Приватні поля
        private string _brand;
        private decimal _price;

        // Публічні властивості
        public string Brand
        {
            get => _brand;
            set => _brand = !string.IsNullOrWhiteSpace(value) ? value : "Невідомий бренд";
        }

        public decimal Price
        {
            get => _price;
            set => _price = value >= 0 ? value : 0;
        }

        // Конструктор базового класу
        public Instrument(string brand, decimal price)
        {
            Brand = brand;
            Price = price;
        }

        // Віртуальний метод для перевизначення (override)
        public virtual void PlaySound()
        {
            Console.WriteLine($"[Instrument] Грає якийсь музичний інструмент бренду {Brand}.");
        }

        // Невіртуальний метод для демонстрації приховування (new)
        public string GetInstrumentType()
        {
            return "Базовий інструмент";
        }
    }

    // 2. ПОХІДНИЙ КЛАС 1: Guitar
    public class Guitar : Instrument
    {
        public int NumStrings { get; set; }

        // Конструктор, що викликає базовий через base(...)
        public Guitar(string brand, decimal price, int numStrings) 
            : base(brand, price)
        {
            NumStrings = numStrings > 0 ? numStrings : 6;
        }

        // Перевизначення віртуального методу (Поліморфізм)
        public override void PlaySound()
        {
            Console.WriteLine($"[Guitar] Гітара {Brand} ({NumStrings} струн) видає бренькіт: Brrr-strum!");
        }

        // Унікальний метод класу Guitar
        public void Strum()
        {
            Console.WriteLine($"[Guitar] Робимо ефектний чіткий грайливий удари по {NumStrings} струнах!");
        }

        // Демонстрація new: Приховування невіртуального методу базового класу
        public new string GetInstrumentType()
        {
            return "Акустична/Електро Гітара";
        }
    }

    // 3. ПОХІДНИЙ КЛАС 2: Piano
    public class Piano : Instrument
    {
        public int NumKeys { get; set; }

        // Конструктор, що викликає базовий через base(...)
        public Piano(string brand, decimal price, int numKeys) 
            : base(brand, price)
        {
            NumKeys = numKeys > 0 ? numKeys : 88;
        }

        // Перевизначення віртуального методу (Поліморфізм)
        public override void PlaySound()
        {
            Console.WriteLine($"[Piano] Піаніно {Brand} ({NumKeys} клавіш) грає мелодію: Plink-plonk!");
        }

        // Унікальний метод класу Piano
        public void PressPedal()
        {
            Console.WriteLine($"[Piano] Натиснуто педаль сустейну на {Brand}. Звук стає об'ємнішим та тривалішим.");
        }
    }

    // 4. ГОЛОВНИЙ КЛАС ПРОГРАМИ: Program
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("1. СТВОРЕННЯ ОБ'ЄКТІВ І ВИКЛИК УНІКАЛЬНИХ МЕТОДІВ");
            Guitar myGuitar = new Guitar("Fender", 1200m, 6);
            Piano myPiano = new Piano("Yamaha", 3500m, 88);

            myGuitar.Strum();
            myPiano.PressPedal();
            Console.WriteLine();

            Console.WriteLine("2. ДЕМОНСТРАЦІЯ ПОЛІМОРФІЗМУ (override)");
            // Створюємо список, що містить посилання типу базового класу Instrument
            List<Instrument> orchestra = new List<Instrument>
            {
                new Instrument("Generic sound Inc.", 100m),
                myGuitar,
                myPiano
            };

            // Виклик PlaySound() через посилання на Instrument
            foreach (var instrument in orchestra)
            {
                // Завдяки override виконується версія методу ДІЙСНОГО об'єкта в пам'яті (Динамічне зв'язування)
                instrument.PlaySound();
            }
            Console.WriteLine();

            Console.WriteLine("3. ДЕМОНСТРАЦІЯ РІЗНИЦІ МІЖ override ТА new");

            // а) Виклик через посилання похідного типу (Guitar)
            Console.WriteLine($"Виклик через посилання типу Guitar:");
            Console.WriteLine($"  GetInstrumentType(): {myGuitar.GetInstrumentType()}");
            myGuitar.PlaySound();

            Console.WriteLine();

            // б) Приведення посилання до базового типу (Instrument)
            Instrument guitarAsInstrument = myGuitar;
            Console.WriteLine($"Виклик того ж об'єкта через посилання типу Instrument:");
            // new використовує Статичне зв'язування -> викликається метод базового класу
            Console.WriteLine($"  GetInstrumentType(): {guitarAsInstrument.GetInstrumentType()} (викликано new-метод з Instrument)");
            // override використовує Динамічне зв'язування -> викликається перевизначений метод похідного класу
            guitarAsInstrument.PlaySound();

            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }
    }
}