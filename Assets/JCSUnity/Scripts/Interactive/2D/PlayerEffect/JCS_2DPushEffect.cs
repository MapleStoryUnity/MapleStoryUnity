/**
 * $File: JCS_2DPushEffect.cs $
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
    /// This pushes the player.
    /// </summary>
    public class JCS_2DPushEffect : MonoBehaviour , JCS_2DPlayerEffect
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_2DPushEffect)")]

        [Tooltip("Direction this effect pushes the player.")]
        [SerializeField]
        private JCS_2DFaceType mDirection = JCS_2DFaceType.FACE_LEFT;

        [Tooltip("How many force to push.")]
        [SerializeField]
        private float mPushSpeed = 10.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        /* Functions */

        private void OnTriggerStay(Collider other)
        {
            JCS_Player player = other.GetComponent<JCS_Player>();
            if (player == null)
                return;

            player.VelX += mPushSpeed * -(int)mDirection * JCS_Time.ItTime(mTimeType);
        }

        /// <summary>
        /// Override.
        /// </summary>
        public void Effect()
        {
            // override   
        }
    }
}
