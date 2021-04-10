/**
 * $File: JCS_TransformPool.cs $
 * $Date: 2017-04-17 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Pool of transform.
    /// </summary>
    public class JCS_TransformPool
        : MonoBehaviour
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_ObjectPool) **")]

        [Tooltip("Pool to randomly choose a transform.")]
        [SerializeField]
        private Transform[] mTransPool = null;

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Get a transform from the pool randomly.
        /// </summary>
        /// <returns> transform value. </returns>
        public Transform GetRandomObject()
        {
            if (mTransPool.Length == 0)
                return null;

            int randIndex = JCS_Random.Range(0, mTransPool.Length);

            return mTransPool[randIndex];
        }
    }
}
