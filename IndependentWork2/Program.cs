namespace IndependentWork2;

public class Program
{
    public static void Main(string[] args)
    {
        // Встановлюємо Юнікод для коректного відображення символу гривні
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        //Об'єкт через Основний конструктор
        Product product1 = new Product(101, "Laptop", 35000.00m, "Electronics", 15);
        Console.WriteLine($"Товар 1 (основний конструктор): {product1}");

        //Об'єкт через Скорочений конструктор
        Product product2 = new Product(102, "Mouse", 800.00m);
        Console.WriteLine($"Товар 2 (скорочений конструктор): {product2}");

        //Об'єкт через Конструктор копіювання
        Product product3 = new Product(product1);
        Console.WriteLine($"Товар 3 (конструктор копіювання): {product3}");
    }
}