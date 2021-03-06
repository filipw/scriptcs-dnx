﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptCs.Dnx.Core.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }

            var collection = enumerable as ICollection<T>;
            if (collection != null)
            {
                return collection.Count == 0;
            }

            return !enumerable.Any();
        }
    }
}
