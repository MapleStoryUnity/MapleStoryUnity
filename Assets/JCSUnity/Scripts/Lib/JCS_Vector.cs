/**
 * $File: $
 * $Date: $
 * $Reveision: $
 * $Creator: Tony Wei, Jen-Chieh Shen (Modefied) $
 */
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Vector data structure implementation.
    /// </summary>
    public class JCS_Vector<T>
    {
        private T[] m_array = null;

        public T[] array { get { return m_array; } }
        public int length { get { return m_array.Length; } }
        public bool empty { get { return (length == 0); } }

        public JCS_Vector(int length = 0)
        {
            m_array = new T[length];
        }

        /// <summary>
        /// add one element at the end of the m_array
        /// </summary>
        /// <param name="target">new element.</param>
        public T[] push(T target)
        {
            T[] newm_array = new T[length + 1];

            for (int i = 0; i < length; i++)
            {
                newm_array[i] = m_array[i];
            }
            newm_array[length] = target;

            m_array = newm_array;
            return m_array;
        }

        /// <summary>
        /// remove first element and return it
        /// </summary>
        public T shift()
        {
            if (length == 0) return default(T);

            T[] newm_array = new T[length - 1];
            T element = m_array[0];

            for (int i = 0; i < length - 1; i++)
            {
                newm_array[i] = m_array[i + 1];
            }
            m_array = newm_array;
            return element;
        }

        /// <summary>
        /// remove the element of index, and return the element
        /// </summary>
        /// <param name="index">Target index.</param>
        public T slice(int index)
        {
            if (index >= 0 && index <= m_array.Length - 1)
            {
                T[] newarray = new T[length - 1];
                T result = m_array[index];

                for (int i = 0; i < index; i++)
                {
                    newarray[i] = m_array[i];
                }
                for (int j = index; j < length - 1; j++)
                {
                    newarray[j] = m_array[j + 1];
                }

                m_array = newarray;
                return result;
            }
            else
            {
                Debug.Log("JCS_Vector slice: index out of range, failed to slice.");
                return default(T);
            }
        }

        /// <summary>
        /// remove the element of index, and return the element
        /// </summary>
        /// <param name="target">Target object.</param>
        public T slice(T target)
        {
            bool isFound = false;
            int index = 0;

            for (int i = 0; i < length; i++)
            {
                if (m_array[i].Equals(target))
                {
                    //found elements
                    index = i;
                    isFound = true;
                }
            }

            if (isFound)
            {
                return slice(index);
            }
            else
            {
                Debug.Log("JCS_Vector slice: target object not found in vector.");
                return default(T);
            }
        }

        /// <summary>
        /// Return GameObject with index
        /// </summary>
        public T at(int index)
        {
            if (index >= 0 && index <= m_array.Length - 1)
                return m_array[index];
            else
                Debug.Log("JCS_Vector::(129)::Out of Range!");

            return default(T);
        }

        /// <summary>
        /// Set the object by index.
        /// </summary>
        /// <param name="index">Target index.</param>
        /// <param name="val">Object value.</param>
        public void set(int index, T val)
        {
            if (index >= 0 && index <= m_array.Length - 1)
                this.m_array[index] = val;
            else
                Debug.Log("JCS_Vector::(139)::Out of Range!");
        }

        /// <summary>
        /// Clear the array.
        /// </summary>
        public void clear()
        {
            // Call init directly clear the vector.
            // TODO(JenChieh): not sure this will cause memory leak.
            m_array = null;
            m_array = new T[0];
        }

    }
}


