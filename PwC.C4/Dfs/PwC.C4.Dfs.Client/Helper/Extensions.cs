using System.Collections.Generic;

namespace PwC.C4.Dfs.Client.Helper
{
    internal static class Extensions
    {
        public static bool Empty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }
    }
}
