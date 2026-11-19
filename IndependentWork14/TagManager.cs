using System;
using System.Collections.Generic;

namespace IndependentWork14
{
    public class TagManager
    {
        // ArticleId -> Множина унікальних тегів (без урахування регістру)
        private readonly Dictionary<string, HashSet<string>> _articleTags =
            new(StringComparer.OrdinalIgnoreCase);

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

        public HashSet<string> GetAllUniqueTags()
        {
            var allTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var tags in _articleTags.Values)
            {
                allTags.UnionWith(tags); // Об'єднання множин
            }
            return allTags;
        }

        public HashSet<string> FindCommonTags(string articleId1, string articleId2)
        {
            if (!_articleTags.TryGetValue(articleId1, out var tags1) ||
                !_articleTags.TryGetValue(articleId2, out var tags2))
            {
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }

            var common = new HashSet<string>(tags1, StringComparer.OrdinalIgnoreCase);
            common.IntersectWith(tags2); // Перетин множин
            return common;
        }

        public HashSet<string> FindUniqueTagsForFirstArticle(string articleId1, string articleId2)
        {
            if (!_articleTags.TryGetValue(articleId1, out var tags1))
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var difference = new HashSet<string>(tags1, StringComparer.OrdinalIgnoreCase);
            if (_articleTags.TryGetValue(articleId2, out var tags2))
            {
                difference.ExceptWith(tags2); // Різниця множин
            }
            return difference;
        }
    }
}