namespace IndependentWork1;

public class Program
{
    public static void Main(string[] args)
    {
        //Тестування класу Book
        Book book = new Book("Кобзар", 350.0);
        double discountedPrice = book.CalculateDiscountedPrice(15); // знижка 15%
        Console.WriteLine($"Книга: '{book.Title}', початкова ціна: {book.Price} грн");
        Console.WriteLine($"Ціна зі знижкою 15%: {discountedPrice:F2} грн\n");

        //Тестування класу BankAccount
        BankAccount account = new BankAccount("UA1234567890", 1000.00m);
        Console.WriteLine($"Рахунок: {account.AccountNumber}, поточний баланс: {account.Balance} грн");
        
        decimal withdrawAmount = 400.00m;
        if (account.Withdraw(withdrawAmount))
        {
            Console.WriteLine($"Успішно знято {withdrawAmount} грн. Новий баланс: {account.Balance} грн");
        }
        else
        {
            Console.WriteLine($"Недостатньо коштів для зняття {withdrawAmount} грн!");
        }
        Console.WriteLine();

        //Тестування класу Student
        Student student = new Student("Олексій Ковальчук", 4.8);
        Console.WriteLine($"Студент: {student.FullName}, середній бал: {student.AverageScore}");
        if (student.IsExcellentStudent())
        {
            Console.WriteLine("Статус: Відмінник/Стипендіат");
        }
        else
        {
            Console.WriteLine("Статус: Звичайний бал");
        }
    }
}