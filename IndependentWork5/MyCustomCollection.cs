using System.Collections;

namespace IndependentWork5;

public class MyCustomCollection : IEnumerable<string>
{
    private readonly List<string> _items = new List<string>();

    public void Add(string item)
    {
        _items.Add(item);
    }

    // Реалізація обобщеного IEnumerable<T>
    public IEnumerator<string> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    // Реалізація необобщеного IEnumerable
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}