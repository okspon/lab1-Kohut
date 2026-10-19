namespace IndependentWork4;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // 1. Створення об'єкта Book
        Book book = new Book("B-001", "Тіні забутих предків", 1911, "Михайло Коцюбинський");
        book.DisplayDetails();

        // 2. Створення об'єкта Journal
        Journal journal = new Journal("J-102", "Науковий вісник", 2024, 5);
        journal.DisplayDetails();

        // 3. Створення об'єкта AudioBook (успадковано від Book)
        AudioBook audioBook = new AudioBook("A-501", "Кобзар", 1840, "Тарас Шевченко", "Олександр Бондаренко");
        audioBook.DisplayDetails();
    }
}