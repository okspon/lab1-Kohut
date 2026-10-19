using System.Text;

namespace IndependentWork6;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine(" Сценарій 1: Система сповіщень (Інтерфейс INotifier) \n");

        List<INotifier> notifiers = new List<INotifier>
        {
            new EmailNotifier(),
            new SmsNotifier(),
            new PushNotifier()
        };

        foreach (INotifier notifier in notifiers)
        {
            notifier.Send("Ваше замовлення успішно оформлено!");
        }

        Console.WriteLine("\n Сценарій 2: Обробка файлів (Абстрактний клас FileProcessor) \n");

        List<FileProcessor> processors = new List<FileProcessor>
        {
            new TextFileProcessor("document.txt"),
            new XmlFileProcessor("data.xml")
        };

        foreach (FileProcessor processor in processors)
        {
            processor.Process();
        }
    }
}