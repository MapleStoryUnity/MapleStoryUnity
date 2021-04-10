/**
 * $File: JCS_PfHeap.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;

namespace JCSUnity
{
    /// <summary>
    /// Path finding heap.
    /// </summary>
    public class JCS_PfHeap<T>
        where T : IHeapItem<T>
    {
        /* Variables */

        private T[] mItems;
        private int mCurrentItemCount;

        /* Setter & Getter */

        public int Count { get { return mCurrentItemCount; } }

        /* Functions */

        public JCS_PfHeap(int maxHeapSize)
        {
            mItems = new T[maxHeapSize];
        }

        /// <summary>
        /// Add a new item to the heap.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            item.HeapIndex = mCurrentItemCount;
            mItems[mCurrentItemCount] = item;
            SortUp(item);
            ++mCurrentItemCount;
        }

        /// <summary>
        /// Remove the first item from the heap.
        /// </summary>
        /// <returns></returns>
        public T RemoveFirst()
        {
            T firstItem = mItems[0];
            --mCurrentItemCount;
            mItems[0] = mItems[mCurrentItemCount];
            mItems[0].HeapIndex = 0;
            SortDown(mItems[0]);

            return firstItem;
        }

        /// <summary>
        /// Update the item by another item.
        /// </summary>
        /// <param name="item"></param>
        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        /// <summary>
        /// Check contains the item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return Equals(mItems[item.HeapIndex], item);
        }

        /// <summary>
        /// Sort down the item in the heap.
        /// </summary>
        /// <param name="item"></param>
        private void SortDown(T item)
        {
            while (true)
            {
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = item.HeapIndex * 2 + 2;
                int swapIndex = 0;

                if (childIndexLeft < mCurrentItemCount)
                {
                    swapIndex = childIndexLeft;

                    if (childIndexRight < mCurrentItemCount)
                    {
                        if (mItems[childIndexLeft].CompareTo(mItems[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (item.CompareTo(mItems[swapIndex]) < 0)
                    {
                        Swap(item, mItems[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Sort up the item in the heap.
        /// </summary>
        /// <param name="item"></param>
        private void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                T parentItem = mItems[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else
                {
                    break;
                }

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        /// <summary>
        /// Swap the two items.
        /// </summary>
        /// <param name="itemA"></param>
        /// <param name="itemB"></param>
        private void Swap(T itemA, T itemB)
        {
            mItems[itemA.HeapIndex] = itemB;
            mItems[itemB.HeapIndex] = itemA;

            int itemAIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
        }
    }

    /// <summary>
    /// Heap interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex
        {
            get;
            set;
        }
    }
}
