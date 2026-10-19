namespace IndependentWork6;

// Інтерфейс підходить для контрактів без спільного коду
public interface INotifier
{
    void Send(string message);
}

public class EmailNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine($"[Email] Надсилання листа: \"{message}\"");
    }
}

public class SmsNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine($"[SMS] Надсилання повідомлення: \"{message}\"");
    }
}

public class PushNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine($"[Push] Надсилання push-сповіщення: \"{message}\"");
    }
}