namespace IndependentWork4;

public class Journal : LibraryItem
{
    private int _issueNumber;

    public int IssueNumber => _issueNumber;

    // Конструктор викликає base(...) та передає uniqueId у базовий клас
    public Journal(string uniqueId, string title, int publicationYear, int issueNumber) 
        : base(uniqueId, title, publicationYear)
    {
        _issueNumber = issueNumber;
    }

    public override void DisplayDetails()
    {
        Console.WriteLine($"[ID: {UniqueId}] Журнал: '{Title}' ({PublicationYear} р.), Випуск №{IssueNumber}");
    }
}