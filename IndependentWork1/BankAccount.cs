namespace IndependentWork1;

public class BankAccount
{
    // Приватні поля
    private string _accountNumber;
    private decimal _balance;

    // Властивості
    public string AccountNumber
    {
        get { return _accountNumber; }
    }

    public decimal Balance
    {
        get { return _balance; }
    }

    // Конструктор
    public BankAccount(string accountNumber, decimal initialBalance)
    {
        _accountNumber = accountNumber;
        _balance = initialBalance;
    }

    //зняття коштів з перевіркою стану
    public bool Withdraw(decimal amount)
    {
        if (amount <= _balance)
        {
            _balance -= amount;
            return true; // Зняття успішне
        }
        return false; // Недостатньо коштів
    }
}