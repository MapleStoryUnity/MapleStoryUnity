/**
 * $File: JCS_2DAnimPool.cs $
 * $Date: 2017-04-17 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Pool of 2d animation.
    /// </summary>
    public class JCS_2DAnimPool : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_AnimPool)")]

        [Tooltip("Animation pool.")]
        [SerializeField]
        private JCS_2DAnimation[] mAnimPool = null;

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Get a animation from the pool randomly.
        /// </summary>
        /// <returns> animation value. </returns>
        public JCS_2DAnimation GetRandomAnim()
        {
            if (mAnimPool.Length == 0)
                return null;

            int randIndex = JCS_Random.Range(0, mAnimPool.Length);

            return mAnimPool[randIndex];
        }
    }
}
