namespace IndependentWork2;

public class Product
{
    //Приватні поля
    private int _id;
    private string _name;
    private decimal _price;
    private string _category;
    private int _stockCount;

    //Публічні read-only властивості
    public int Id => _id;
    public string Name => _name;
    public decimal Price => _price;
    public string Category => _category;
    public int StockCount => _stockCount;

    //Конструктор 1 (Основний - приймає всі 5 параметрів)
    public Product(int id, string name, decimal price, string category, int stockCount)
    {
        _id = id;
        _name = name;
        _price = price;
        _category = category;
        _stockCount = stockCount;
    }

    //Конструктор 2 (Скорочений - викликає основний через : this(...))
    public Product(int id, string name, decimal price)
        : this(id, name, price, "Uncategorized", 0)
    {
    }

    //Конструктор 3 (Копіювання - викликає основний через : this(...))
    public Product(Product other)
        : this(other.Id, other.Name, other.Price, other.Category, other.StockCount)
    {
    }

    //Перевизначений метод ToString()
    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Price: {Price:C}, Category: {Category}, Stock: {StockCount}";
    }
}