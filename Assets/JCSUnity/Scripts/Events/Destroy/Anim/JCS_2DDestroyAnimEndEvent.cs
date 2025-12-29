/**
 * $File: JCS_2DDestroyAnimEndEvent.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Destroy the game object after done playing the 2D animation.
    /// </summary>
    [RequireComponent(typeof(JCS_2DAnimation))]
    public class JCS_2DDestroyAnimEndEvent : MonoBehaviour
    {
        /* Variables */

        private JCS_2DAnimation m2DAnimation = null;

        [Separator("⚡️ Runtime Variables (JCS_2DDestroyAnimEndEvent)")]

        [Tooltip("How many times the animation plays before destorying.")]
        [SerializeField]
        private int mLoopTimes = 1;

        // Loop count.
        private int mLoopCount = 0;

        /* Setter & Getter */

        public int loopTimes { get { return mLoopTimes; } set { mLoopTimes = value; } }

        /* Functions */

        private void Awake()
        {
            m2DAnimation = GetComponent<JCS_2DAnimation>();
        }

        private void LateUpdate()
        {
            if (!m2DAnimation.isDonePlaying)
                return;

            ++mLoopCount;

            if (mLoopCount <= mLoopTimes)
                Destroy(gameObject);
            else
            {
                // play once more.
                m2DAnimation.Play();
            }
        }
    }
}
