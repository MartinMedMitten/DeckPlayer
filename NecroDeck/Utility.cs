using System;
using System.Collections.Generic;
using System.Linq;

namespace NecroDeck
{
    static class Utility
    {

        public static int FirstIndexOf<T>(this IList<T> list, Func<T, bool> func)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (func(list[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public static IEnumerable<T> ExceptItem<T>(this IEnumerable<T> e, T item)
        {
            foreach (var x in e)
            {
                if (!x.Equals(item))
                {
                    yield return x;
                }
            }
        }
        public static IEnumerable<T> ConcatItem<T>(this IEnumerable<T> e, T item)
        {
            foreach (var x in e)
            {
                yield return x;
            }
            yield return item;
        }

        public static T With<T>(this T t, Action<T> a)
        {
            a(t);
            return t;
        }

        public static void Enqueue<T>(this Stack<T> s, T item)
        {
            s.Push(item);
        }
        public static T Dequeue<T>(this Stack<T> s)
        {
            return s.Pop();
        }

    }
}
