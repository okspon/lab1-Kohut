namespace lab9_self_v1;

public class Product
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public override string ToString()
    {
        return $"{Name} - {Price} грн";
    }
}