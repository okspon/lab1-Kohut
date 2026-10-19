namespace IndependentWork4;

public class AudioBook : Book
{
    private string _narrator;

    public string Narrator => _narrator;

    // Успадковується від Book, передає параметри далі через base(...)
    public AudioBook(string uniqueId, string title, int publicationYear, string author, string narrator) 
        : base(uniqueId, title, publicationYear, author)
    {
        _narrator = narrator;
    }

    public override void DisplayDetails()
    {
        Console.WriteLine($"[ID: {UniqueId}] Аудіокнига: '{Title}' ({PublicationYear} р.), Автор: {Author}, Диктор: {Narrator}");
    }
}
