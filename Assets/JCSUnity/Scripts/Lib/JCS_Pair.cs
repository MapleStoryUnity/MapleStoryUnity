/**
 * $File: JCS_Pair.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Pair data structure.
    /// </summary>
    /// <typeparam name="T"> type A </typeparam>
    /// <typeparam name="U"> type B </typeparam>
    public class JCS_Pair<T, U>
    {
        /* Variables */

        public T pair1 = default;
        public U pair2 = default;

        /* Setter & Getter */

        /* Functions */

        public JCS_Pair()
        {
            // empty..
        }

        public JCS_Pair(T pair1, U pair2)
        {
            Set(pair1, pair2);
        }

        /// <summary>
        /// Set the pair value.
        /// </summary>
        /// <param name="pair1"></param>
        /// <param name="pair2"></param>
        public void Set(T pair1, U pair2)
        {
            this.pair1 = pair1;
            this.pair2 = pair2;
        }
    }
}
