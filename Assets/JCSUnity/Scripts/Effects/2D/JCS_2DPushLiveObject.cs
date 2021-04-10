/**
 * $File: JCS_2DPushLiveObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Effect that pushes the 2d live object.
    /// </summary>
    public class JCS_2DPushLiveObject
        : MonoBehaviour
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_2DPushLiveObject) **")]

        [Tooltip("Continuing push the effected object.")]
        [SerializeField]
        private bool mContinuousPush = false;

        [Tooltip("How much force it pushed.")]
        [SerializeField] [Range(1.0f, 100.0f)]
        private float mPushForce = 10.0f;

        /* Setter & Getter */

        public bool ContinuousPush { get { return this.mContinuousPush; } set { this.mContinuousPush = value; } }
        public float PushForce { get { return this.mPushForce; } set { this.mPushForce = value; } }

        /* Functions */

        private void OnTriggerEnter(Collider other)
        {
            CheckAvailableToPush(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!mContinuousPush)
                return;

            CheckAvailableToPush(other);
        }

        /// <summary>
        /// Do once the push effect.
        /// </summary>
        public void PushEffect(JCS_2DLiveObject liveObject)
        {
            // knock back the object.
            liveObject.KnockBack(mPushForce, this.transform);
        }

        /// <summary>
        /// Check if the object are ok to do push.
        /// </summary>
        /// <param name="other"></param>
        private void CheckAvailableToPush(Collider other)
        {
            JCS_2DLiveObject liveObject = other.GetComponent<JCS_2DLiveObject>();
            if (liveObject == null)
                return;

            // do the effect.
            PushEffect(liveObject);
        }
    }
}
