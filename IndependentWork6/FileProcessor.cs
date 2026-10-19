namespace IndependentWork6;

// Абстрактний клас підходить для повторного використання спільного коду (Template Method)
public abstract class FileProcessor
{
    public string FilePath { get; }

    protected FileProcessor(string filePath)
    {
        FilePath = filePath;
    }

    // Спільні кроки, реалізовані в базовому класі
    public void OpenFile()
    {
        Console.WriteLine($"[Відкриття] Файл '{FilePath}' успішно відкрито.");
    }

    public void CloseFile()
    {
        Console.WriteLine($"[Закриття] Файл '{FilePath}' закрито.\n");
    }

    // Абстрактний метод, який мусять реалізувати похідні класи
    public abstract void ProcessContent();

    // Загальний процес обробки (Шаблонний метод)
    public void Process()
    {
        OpenFile();
        ProcessContent();
        CloseFile();
    }
}

public class TextFileProcessor : FileProcessor
{
    public TextFileProcessor(string filePath) : base(filePath) { }

    public override void ProcessContent()
    {
        Console.WriteLine($"[Обробка] Зчитування та аналіз текстового файлу '{FilePath}'...");
    }
}

public class XmlFileProcessor : FileProcessor
{
    public XmlFileProcessor(string filePath) : base(filePath) { }

    public override void ProcessContent()
    {
        Console.WriteLine($"[Обробка] Валідація та парсинг XML-структури файлу '{FilePath}'...");
    }
}