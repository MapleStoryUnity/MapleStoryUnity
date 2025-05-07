/**
 * $File: JCS_Loop.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2025 by Shen, Jen-Chieh $
 */
using System;
using System.Collections.Generic;

namespace JCSUnity
{
    /// <summary>
    /// Loop operations.
    /// </summary>
    public static class JCS_Loop
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Loop in range.
        /// </summary>
        /// <param name="start"> Start index. </param>
        /// <param name="end"> End index. </param>
        /// <param name="action"> Action to execute in range. </param>
        public static void Range(int start, int end, Action<int> action)
        {
            for (int count = start; count < end; ++count)
            {
                action?.Invoke(count);
            }
        }

        /// <summary>
        /// Loops a specific number of times.
        /// </summary>
        /// <param name="times"> Times to loop over. </param>
        /// <param name="action"> Action to execute each times. </param>
        public static void Times(int times, Action<int> action)
        {
            Range(0, times, action);
        }

        /// <summary>
        /// Like `foreach` but with index.
        /// </summary>
        /// <typeparam name="T"> The type of the item. </typeparam>
        /// <param name="coll"> The collection. </param>
        /// <param name="action"> Action to execute. </param>
        public static void ForEach<T>(ICollection<T> coll, Action<T> action)
        {
            ForEach(coll, (count, item) =>
            {
                action?.Invoke(item);
            });
        }
        public static void ForEach<T>(ICollection<T> coll, Action<int, T> action)
        {
            int count = 0;

            foreach (T item in coll)
            {
                action?.Invoke(count, item);

                ++count;
            }
        }

        /// <summary>
        /// Loop through dictionary.
        /// </summary>
        /// <param name="dict"> The dictionary. </param>
        /// <param name="action"> Action to execute. </param>
        public static void ForEach<T, V>(IDictionary<T, V> dict, Action<KeyValuePair<T, V>> action)
        {
            ForEach(dict, (_, entry) =>
            {
                action?.Invoke(entry);
            });
        }
        public static void ForEach<T, V>(IDictionary<T, V> dict, Action<int, KeyValuePair<T, V>> action)
        {
            int count = 0;

            foreach (KeyValuePair<T, V> entry in dict)
            {
                action?.Invoke(count, entry);

                ++count;
            }
        }

        /// <summary>
        /// Loop through enumerator.
        /// </summary>
        /// <param name="action"> The callback. </param>
        public static void ForEach<T>(Action<T> action)
           where T : Enum
        {
            ForEach<T>((_, item) =>
            {
                action?.Invoke(item);
            });
        }
        public static void ForEach<T>(Action<int, T> action)
            where T : Enum
        {
            int count = 0;

            foreach (T item in Enum.GetValues(typeof(T)))
            {
                action?.Invoke(count, item);

                ++count;
            }
        }
    }
}
