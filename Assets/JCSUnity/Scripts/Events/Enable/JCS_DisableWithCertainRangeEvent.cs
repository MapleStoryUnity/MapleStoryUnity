/**
 * $File: JCS_DisableWithCertainRangeEvent.cs $
 * $Date: 2016-11-12 21:30:00 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Disable the the current game object 
    /// with in the certain range.
    /// </summary>
    [RequireComponent(typeof(JCS_FadeObject))]
    public class JCS_DisableWithCertainRangeEvent : MonoBehaviour
    {
        /* Variables */

        private JCS_FadeObject mJCSFadeObject = null;

        [Separator("Runtime Variables (JCS_DisableWithCertainRangeEvent)")]

        [Tooltip("Use local position instead of global position.")]
        [SerializeField]
        private bool mUseLocal = false;

        [Tooltip("Target check with in the range.")]
        [SerializeField]
        private JCS_UnityObject mTarget = null;

        [Tooltip("Target position, do not have to pass in transform.")]
        [SerializeField]
        private Vector3 mTargetPosition = Vector3.zero;

        [Tooltip("Disable with in this range.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField]
        private float mRange = 0.0f;

        [Header("- Optional")]

        [Tooltip("Fade before disable?")]
        [SerializeField]
        private bool mFadeEffect = false;

        [Tooltip("Distance starting to fade.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField]
        private float mFadeDistance = 0.0f;

        private bool mFaded = false;

        /* Setter & Getter */

        public bool FadeEffect { get { return this.mFadeEffect; } set { this.mFadeEffect = value; } }
        public Vector3 TargetPosition { get { return this.mTargetPosition; } set { this.mTargetPosition = value; } }
        public void SetTarget(JCS_UnityObject trans)
        {
            // update target position.
            this.mTarget = trans;

            // update the target position too.
            this.mTargetPosition = this.transform.position;
        }
        public float Range { get { return this.mRange; } set { this.mRange = value; } }

        /* Functions */

        private void Awake()
        {
            mJCSFadeObject = this.GetComponent<JCS_FadeObject>();
        }

        private void Update()
        {
            UpdateTargetPosition();

            DisableWithInRange();
        }

        private void OnEnable()
        {
            mFaded = false;
        }

        /// <summary>
        /// Disable the game object when whith in the range.
        /// </summary>
        private void DisableWithInRange()
        {
            float distance = 0;
            if (mUseLocal)
            {
                distance = Vector3.Distance(this.transform.localPosition, this.mTargetPosition);
            }
            else
            {
                // get the distance between self's and target's.
                distance = Vector3.Distance(this.transform.position, this.mTargetPosition);
            }

            if (distance < this.mRange)
            {
                // disable the game object.
                this.gameObject.SetActive(false);
            }

            // check fade effect enable?
            if (!mFadeEffect || mFaded)
                return;

            if (distance < this.mFadeDistance)
            {
                mJCSFadeObject.FadeOut();
                mFaded = true;
            }
        }

        /// <summary>
        /// Get the tartget's transform position / local position.
        /// </summary>
        private void UpdateTargetPosition()
        {
            if (mTarget == null)
            {
                if (JCS_GameSettings.instance.DEBUG_MODE)
                    JCS_Debug.LogError("Can't set the position without target transform.");

                return;
            }

            if (mUseLocal)
            {
                mTargetPosition = mTarget.transform.localPosition;
            }
            else
            {
                // get the target position.
                mTargetPosition = mTarget.transform.position;
            }
        }

    }
}
