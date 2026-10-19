namespace IndependentWork4;

public class LibraryItem
{
    // Приватні поля
    private string _title;
    private int _publicationYear;
    private string _uniqueId; // Додано в пункті 4

    // Публічні властивості
    public string Title => _title;
    public int PublicationYear => _publicationYear;
    public string UniqueId => _uniqueId; // Read-only властивість (пункт 4)

    // Конструктор з урахуванням uniqueId
    public LibraryItem(string uniqueId, string title, int publicationYear)
    {
        _uniqueId = uniqueId;
        _title = title;
        _publicationYear = publicationYear;
    }

    // Віртуальний метод
    public virtual void DisplayDetails()
    {
        Console.WriteLine($"[ID: {UniqueId}] '{Title}' ({PublicationYear} р.)");
    }
}