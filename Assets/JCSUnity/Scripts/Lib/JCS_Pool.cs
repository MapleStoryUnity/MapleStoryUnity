/**
 * $File: JCS_Pool.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Pool for all type of data struct.
    /// </summary>
    /// <typeparam name="T"> type </typeparam>
    public class JCS_Pool<T>
    {

        private JCS_Vector<T> objects = null;
        private int mLastUseIndex = 0;


        public JCS_Vector<T> GetObjects() { return this.objects; }
        public int LastUseIndex { get { return this.mLastUseIndex; } }

        public JCS_Pool(int num)
        {
            objects = new JCS_Vector<T>(num);
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
