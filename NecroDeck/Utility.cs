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
        public static T FirstOrDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate, T defaultValue)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (var element in source)
            {
                if (predicate(element))
                {
                    return element;
                }
            }

            return defaultValue;
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
        internal static ulong ListToBitflag(List<int> cards)
        {
            ulong rest = 0;
            foreach (int num in cards)
            {
                if (num >= 0 && num <= 60) // Ensure the number is within the valid range
                {
                    rest |= (1UL << num);
                }
            }
            return rest;
        }
        internal static List<int> BitFlagToList(ulong cardsInHandBitflag)
        {
            List<int> intList = new List<int>();
            for (int i = 0; i <= 60; i++)
            {
                if ((cardsInHandBitflag & (1UL << i)) != 0)
                {
                    intList.Add(i);
                }
            }
            return intList;
        }
    }
}
