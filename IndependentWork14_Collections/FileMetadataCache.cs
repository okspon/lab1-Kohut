using System;
using System.Collections.Generic;

namespace IndependentWork14_Collections
{
    public class FileMetadata
    {
        public string FileName { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        public string Hash { get; set; } = string.Empty;

        public override string ToString() =>
            $"{FileName} | Розмір: {Size} байт | Змінено: {LastModified:yyyy-MM-dd HH:mm} | SHA256: {Hash}";
    }

    public class FileMetadataCache
    {
        // Використовуємо StringComparer.OrdinalIgnoreCase, оскільки файлові шляхи у Windows регістронезалежні
        private readonly Dictionary<string, FileMetadata> _cache = new(StringComparer.OrdinalIgnoreCase);

        public int Count => _cache.Count;

        public void AddOrUpdate(string filePath, FileMetadata metadata)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
            ArgumentNullException.ThrowIfNull(metadata);

            _cache[filePath] = metadata;
        }

        public bool TryAdd(string filePath, FileMetadata metadata)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
            ArgumentNullException.ThrowIfNull(metadata);

            return _cache.TryAdd(filePath, metadata);
        }

        public FileMetadata? Get(string filePath)
        {
            return _cache.TryGetValue(filePath, out var metadata) ? metadata : null;
        }

        public bool Contains(string filePath)
        {
            return _cache.ContainsKey(filePath);
        }

        public bool Remove(string filePath)
        {
            return _cache.Remove(filePath);
        }
    }
}