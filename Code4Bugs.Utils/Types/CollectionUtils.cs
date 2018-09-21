using System;
using System.Collections.Generic;

namespace Code4Bugs.Utils.Types
{
    public static class CollectionUtils
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            var index = 0;
            foreach (var item in enumerable)
            {
                action(item, index++);
            }
        }
        
        public static IList<T> EmptyList<T>()
        {
            return new List<T>(0);
        }
    }
}