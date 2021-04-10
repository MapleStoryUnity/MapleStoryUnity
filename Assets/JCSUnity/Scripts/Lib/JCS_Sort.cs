/**
 * $File: JCS_Sort.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JCSUnity
{
    /// <summary>
    /// Sorting object.
    /// </summary>
    public class JCS_Sort<T>
        where T : JCS_SortingObject
    {
        /* Variables */

        // NOTE(JenChieh): not sure template support this kind of feature.
        private T[] mArray = null;  // any array with this sort

        /* Setter & Getter */

        public T[] Array { get { return this.mArray; } }

        /* Functions */

        public JCS_Sort() { }

        /// <summary>
        /// Add the list of the sorting object to the array.
        /// </summary>
        /// <param name="arr"></param>
        public void AddAll(T[] arr)
        {
            // copy array
            mArray = arr;
        }

        /// <summary>
        /// Add the list of the sorting object to the array.
        /// </summary>
        /// <param name="arr"></param>
        public void AddAll(List<T> arr)
        {
            AddAll(arr.ToArray());
        }

        /// <summary>
        /// Use insertion sort method to sort the object.
        /// </summary>
        /// <returns>List of sorted sorting object.</returns>
        public T[] InsertionSort()
        {
            for (int i = 0; i < mArray.Length; ++i)
            {
                for (int j = i; j > 0; --j)
                {
                    if (mArray[j].OrderIndex < mArray[j - 1].OrderIndex)
                    {
                        T temp = mArray[j];
                        mArray[j] = mArray[j - 1];
                        mArray[j - 1] = temp;
                    }
                }
            }
            return mArray;
        }
    }
}
