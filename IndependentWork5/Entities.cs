namespace IndependentWork5;

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return $"Товар: '{Name}', Ціна: {Price:C}";
    }
}

public class Customer
{
    public string FullName { get; set; }
    public string Email { get; set; }

    public Customer(string fullName, string email)
    {
        FullName = fullName;
        Email = email;
    }

    public override string ToString()
    {
        return $"Клієнт: {FullName} ({Email})";
    }
}

public class Order
{
    public int OrderId { get; set; }
    public decimal TotalAmount { get; set; }

    public Order(int orderId, decimal totalAmount)
    {
        OrderId = orderId;
        TotalAmount = totalAmount;
    }

    public override string ToString()
    {
        return $"Замовлення №{OrderId} на суму: {TotalAmount:C}";
    }
}