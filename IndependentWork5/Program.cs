using System.IO;
using System.Text;

namespace IndependentWork5;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("1. Поліморфізм через System.Object (ToString)");
        List<object> objects = new List<object>
        {
            new Product("Ноутбук", 35000.00m),
            new Customer("Іван Петренко", "ivan@example.com"),
            new Order(1001, 35000.00m)
        };

        foreach (object obj in objects)
        {
            // Поліморфний виклик: перевизначений ToString() викликається відповідно до реального типу об'єкта
            Console.WriteLine(obj.ToString());
        }

        Console.WriteLine("\n 2. Поліморфізм через IEnumerable<T>");
        MyCustomCollection collection = new MyCustomCollection();
        collection.Add("Елемент 1");
        collection.Add("Елемент 2");
        collection.Add("Елемент 3");

        // Поліморфний перебір за допомогою foreach через інтерфейс IEnumerable
        foreach (string item in collection)
        {
            Console.WriteLine($"Отримано з власної колекції: {item}");
        }

        Console.WriteLine("\n 3. Поліморфізм через абстрактний клас System.IO.Stream");
        string testFilePath = "test_file.txt";
        File.WriteAllText(testFilePath, "Hello, Stream Polymorphism!");

        using (FileStream fileStream = new FileStream(testFilePath, FileMode.Open, FileAccess.Read))
        using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("Memory Stream Data")))
        {
            Console.Write("FileStream: ");
            ProcessStream(fileStream);

            Console.Write("MemoryStream: ");
            ProcessStream(memoryStream);
        }

        // Видаляємо тестовий файл
        if (File.Exists(testFilePath))
        {
            File.Delete(testFilePath);
        }
    }

    // Метод працює з будь-яким потоком завдяки поліморфізму абстрактного класу Stream
    public static void ProcessStream(Stream stream)
    {
        byte[] buffer = new byte[10];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string text = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Console.WriteLine($"Прочитано {bytesRead} байт: \"{text}\"");
    }
}