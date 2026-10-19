namespace IndependentWork4;

public class Book : LibraryItem
{
    private string _author;

    public string Author => _author;

    // Конструктор викликає base(...) та передає uniqueId у базовий клас
    public Book(string uniqueId, string title, int publicationYear, string author) 
        : base(uniqueId, title, publicationYear)
    {
        _author = author;
    }

    public override void DisplayDetails()
    {
        Console.WriteLine($"[ID: {UniqueId}] Книга: '{Title}' ({PublicationYear} р.), Автор: {Author}");
    }
}