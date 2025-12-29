/**
 * $File: JCS_Enum.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2025 by Shen, Jen-Chieh $
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace JCSUnity
{
    /// <summary>
    /// Enumerator utilities.
    /// </summary>
    public static class JCS_Enum
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Enum typed version casting.
        /// Source: http://stackoverflow.com/questions/972307/can-you-loop-through-all-enum-values
        /// </summary>
        public static IEnumerable<T> GetValues<T>()
        {
            return GetValues<T, T>();
        }
        public static IEnumerable<U> GetValues<T, U>()
        {
            return Enum.GetValues(typeof(T)).Cast<U>();
        }

        /// <summary>
        /// Get the value for each enum, in the type of `Enum`.
        /// </summary>
        public static IEnumerable<Enum> GetValuesE<T>()
        {
            return GetValues<T, Enum>();
        }

        /// <summary>
        /// Convert enum values to array.
        /// </summary>
        public static ICollection<T> ToArray<T>()
        {
            return GetValues<T>().ToArray();
        }

        /// <summary>
        /// Convert enum values to list.
        /// </summary>
        public static ICollection<T> ToList<T>()
        {
            return GetValues<T>().ToList();
        }

        /// <summary>
        /// Retrieves an array of the names of the constants 
        /// in a specified enumeration.
        /// </summary>
        public static string[] GetNames<T>()
        {
            return Enum.GetNames(typeof(T));
        }

        /// <summary>
        /// Return the length of an enumerator.
        /// </summary>
        /// <typeparam name="T"> Enum type. </typeparam>
        /// <returns> Size of the enum listed. </returns>
        public static int Size<T>()
        {
            return GetNames<T>().Length;
        }
    }
}
