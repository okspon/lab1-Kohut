namespace lab12v11;

public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly List<T> _items = new();

    public T? GetById(int id)
    {
        return _items.FirstOrDefault(x => x.Id == id);
    }

    public IEnumerable<T> GetAll()
    {
        return _items.ToList();
    }

    public void Add(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (_items.Any(x => x.Id == item.Id))
            throw new InvalidOperationException($"Елемент з Id = {item.Id} вже існує.");

        _items.Add(item);
    }

    public void Update(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        var existingIndex = _items.FindIndex(x => x.Id == item.Id);
        if (existingIndex == -1)
            throw new KeyNotFoundException($"Елемент з Id = {item.Id} не знайдено.");

        _items[existingIndex] = item;
    }

    public void Delete(int id)
    {
        var existingItem = GetById(id);
        if (existingItem == null)
            throw new KeyNotFoundException($"Елемент з Id = {id} не знайдено.");

        _items.Remove(existingItem);
    }
}