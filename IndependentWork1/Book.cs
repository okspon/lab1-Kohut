namespace IndependentWork1;

public class Book
{
    // Приватні поля
    private string _title;
    private double _price;

    // Властивість тільки для читання (Read-only)
    public string Title
    {
        get { return _title; }
    }

    // Властивість з get та set
    public double Price
    {
        get { return _price; }
        set { if (value >= 0) _price = value; }
    }

    // Конструктор
    public Book(string title, double price)
    {
        _title = title;
        _price = price;
    }

    //обчислення ціни зі знижкою
    public double CalculateDiscountedPrice(double discountPercent)
    {
        return _price * (1 - discountPercent / 100);
    }
}