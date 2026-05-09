/**
 * $File: JCS_Pool.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;

namespace JCSUnity
{
    /// <summary>
    /// Pool for all type of data struct.
    /// </summary>
    /// <typeparam name="T"> type </typeparam>
    public class JCS_Pool<T>
    {

        private List<T> objects = null;
        private int mLastUseIndex = 0;


        public List<T> GetObjects() { return objects; }
        public int lastUseIndex { get { return mLastUseIndex; } }

        public JCS_Pool(int num)
        {
            objects = new List<T>(num);
        }

        /// <summary>
        /// Return the object to pool.
        /// </summary>
        public void ReturnToPool()
        {
            // TODO(jenchieh): currently empty..
        }

    }
}
