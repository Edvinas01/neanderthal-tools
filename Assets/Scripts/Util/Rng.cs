using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Util
{
    public static class Rng
    {
        public static T GetRandom<T>(this IReadOnlyList<T> items)
        {
            return items.Count == 0 ? default : items[Random.Range(0, items.Count)];
        }
    }
}
