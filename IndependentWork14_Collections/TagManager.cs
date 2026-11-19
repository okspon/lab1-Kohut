using System;
using System.Collections.Generic;

namespace IndependentWork14_Collections
{
    public class TagManager
    {
        private readonly Dictionary<string, HashSet<string>> _articleTags = new(StringComparer.OrdinalIgnoreCase);

        public void AddTag(string articleId, string tag)
        {
            if (!_articleTags.TryGetValue(articleId, out var tags))
            {
                tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                _articleTags[articleId] = tags;
            }
            tags.Add(tag.Trim());
        }

        public bool RemoveTag(string articleId, string tag)
        {
            if (_articleTags.TryGetValue(articleId, out var tags))
            {
                return tags.Remove(tag.Trim());
            }
            return false;
        }

        public HashSet<string> GetTagsForArticle(string articleId)
        {
            return _articleTags.TryGetValue(articleId, out var tags)
                ? new HashSet<string>(tags, StringComparer.OrdinalIgnoreCase)
                : new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        // Об'єднання всіх тегів усіх статей (UnionWith)
        public HashSet<string> GetAllUniqueTags()
        {
            var allTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var tagSet in _articleTags.Values)
            {
                allTags.UnionWith(tagSet);
            }
            return allTags;
        }

        // Спільні теги двох статей (IntersectWith)
        public HashSet<string> FindCommonTags(string articleId1, string articleId2)
        {
            var result = GetTagsForArticle(articleId1);
            var secondTags = GetTagsForArticle(articleId2);
            result.IntersectWith(secondTags);
            return result;
        }

        // Теги, які є в першій статті, але відсутні у другій (ExceptWith)
        public HashSet<string> FindUniqueTagsForFirst(string articleId1, string articleId2)
        {
            var result = GetTagsForArticle(articleId1);
            var secondTags = GetTagsForArticle(articleId2);
            result.ExceptWith(secondTags);
            return result;
        }
    }
}