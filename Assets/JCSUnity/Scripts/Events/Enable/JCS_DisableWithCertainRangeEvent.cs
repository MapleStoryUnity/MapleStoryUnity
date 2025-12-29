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

        [Separator("⚡️ Runtime Variables (JCS_DisableWithCertainRangeEvent)")]

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

        [Header("🔍 Optional")]

        [Tooltip("Fade before disable?")]
        [SerializeField]
        private bool mFadeEffect = false;

        [Tooltip("Distance starting to fade.")]
        [Range(0.0f, 1000.0f)]
        [SerializeField]
        private float mFadeDistance = 0.0f;

        private bool mFaded = false;

        /* Setter & Getter */

        public bool fadeEffect { get { return mFadeEffect; } set { mFadeEffect = value; } }
        public Vector3 targetPosition { get { return mTargetPosition; } set { mTargetPosition = value; } }
        public void SetTarget(JCS_UnityObject trans)
        {
            // update target position.
            mTarget = trans;

            // update the target position too.
            mTargetPosition = transform.position;
        }
        public float range { get { return mRange; } set { mRange = value; } }

        /* Functions */

        private void Awake()
        {
            mJCSFadeObject = GetComponent<JCS_FadeObject>();
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
                distance = Vector3.Distance(transform.localPosition, mTargetPosition);
            }
            else
            {
                // get the distance between self's and target's.
                distance = Vector3.Distance(transform.position, mTargetPosition);
            }

            if (distance < mRange)
            {
                // disable the game object.
                gameObject.SetActive(false);
            }

            // check fade effect enable?
            if (!mFadeEffect || mFaded)
                return;

            if (distance < mFadeDistance)
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
                if (JCS_GameSettings.FirstInstance().debugMode)
                    Debug.LogError("Can't set the position without target transform.");

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
