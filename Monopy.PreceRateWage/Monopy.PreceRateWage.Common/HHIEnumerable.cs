using System;
using System.Collections.Generic;

namespace Monopy.PreceRateWage.Common
{
    public enum OptionType
    {
        Add, Delete, Modify
    }

    public static class HHIEnumerable
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource item in source)
            {
                if (seenKeys.Add(keySelector(item)))
                {
                    yield return item;
                }
            }
        }
    }
}