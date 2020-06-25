using System;
using System.Collections.Generic;
using System.Text;

namespace Painter.Utilities
{
    public static class ExtensionMethods
    {
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue val)
        {
            key = kvp.Key;
            val = kvp.Value;
        }
    }
}
