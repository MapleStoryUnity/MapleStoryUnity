/**
 * $File: JCS_DestroyObjectWithTime.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Destroy the game object by time.
    /// </summary>
    public class JCS_DestroyObjectWithTime : MonoBehaviour
    {
        /* Variables */

        private float mTimer = 0;

        private bool mTimesUp = false;

        [Separator("Runtime Variables (JCS_DestroyObjectWithTime)")]

        [Tooltip("Target time to destroy.")]
        [SerializeField]
        [Range(0.0f, 3600.0f)]
        private float mDestroyTime = 10.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("Optional")]

        [Tooltip("While destroying, fade out the game object.")]
        [SerializeField]
        private bool mDestroyWithAlphaEffect = true;

        [Tooltip("Fade out objects that fade out after the time is up.")]
        [SerializeField]
        private List<JCS_FadeObject> mFadeObjects = new List<JCS_FadeObject>();

        [Tooltip("How long it fades.")]
        [SerializeField]
        [Range(0.0f, 60.0f)]
        private float mFadeTime = 1.0f;

        /* Setter & Getter */

        public float destroyTime { get { return mDestroyTime; } set { mDestroyTime = value; } }
        public bool timesUp { get { return mTimesUp; } set { mTimesUp = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        public float fadeTime { get { return mFadeTime; } set { mFadeTime = value; } }
        public List<JCS_FadeObject> fadeObjects { get { return mFadeObjects; } set { mFadeObjects = value; } }

        /* Functions */

        private void Update()
        {
            mTimer += JCS_Time.ItTime(mTimeType);

            if (mDestroyWithAlphaEffect)
            {
                if (mDestroyTime - mTimer <= mFadeTime)
                    FadeOut();
            }

            if (mDestroyTime < mTimer)
            {
                mTimesUp = true;
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Return the first fade object in the array.
        /// </summary>
        /// <returns></returns>
        public JCS_FadeObject GetFadeObject()
        {
            foreach (JCS_FadeObject fo in mFadeObjects)
            {
                if (fo == null)
                    continue;
                return fo;
            }

            return null;
        }

        /// <summary>
        /// Fade out for all the fade objects in list.
        /// </summary>
        private void FadeOut()
        {
            foreach (JCS_FadeObject fo in mFadeObjects)
            {
                if (fo == null)
                    continue;

                fo.fadeTime = mFadeTime;
                fo.FadeOut();
            }
        }
    }
}
