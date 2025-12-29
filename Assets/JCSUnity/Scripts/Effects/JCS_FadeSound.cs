/**
 * $File: JCS_FadeSound.cs $
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
    /// Effect that fade the sound.
    /// </summary>
    public class JCS_FadeSound : MonoBehaviour
    {
        /* Variables */

        private AudioSource mAudioSource = null;

        private JCS_FadeType mType = JCS_FadeType.NONE;

        [Separator("📋 Check Variabless (JCS_FadeSound)")]

        [Tooltip("Do the effect?")]
        [SerializeField]
        [ReadOnly]
        private bool mEffect = false;

        [Separator("⚡️ Runtime Variables (JCS_FadeSound)")]

        [Tooltip("Fade out time.")]
        [SerializeField]
        private float mFadeOutTime = 1.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.UNSCALED_DELTA_TIME;

        [Tooltip("Fade in time.")]
        [SerializeField]
        private float mFadeInTime = 1.0f;

        private float mTargetVolume = 0.0f;
        private float mRecordVolume = 0.0f;

        /* Setter & Getter */

        public void SetFadeOutTime(float t) { mFadeOutTime = t; }
        public void SetFadeInTime(float t) { mFadeInTime = t; }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        public AudioSource audioSource { get { return mAudioSource; } }

        /* Functions */

        private void Update()
        {
            if (!mEffect)
                return;

            DoFadeSound();
        }

        /// <summary>
        /// Set the audio source.
        /// </summary>
        /// <param name="source"></param>
        public void SetAudioSource(AudioSource source)
        {
            if (source == null)
            {
                Debug.LogError("Source assign are null references");
                return;
            }

            mAudioSource = source;

            // record down the volume, 
            // so when is fade in this will become the 
            // target sound!
            mRecordVolume = mAudioSource.volume;
        }

        /// <summary>
        /// Fade out the sound.
        /// </summary>
        /// <param name="target_volume"></param>
        /// <param name="time"></param>
        public void FadeOut(float target_volume, float time)
        {
            if (mType == JCS_FadeType.OUT)
            {
                Debug.LogError("Already fade out");
                return;
            }

            if (mAudioSource == null)
            {
                Debug.LogError("Can't do the fade out effect without audio source");
                return;
            }

            SetFadeOutTime(time);

            mTargetVolume = target_volume;
            mRecordVolume = mAudioSource.volume;

            mType = JCS_FadeType.OUT;
            mEffect = true;
        }

        /// <summary>
        /// Fade in the sound.
        /// </summary>
        /// <param name="target_volume"></param>
        /// <param name="time"></param>
        public void FadeIn(float target_volume, float time)
        {
            if (mType == JCS_FadeType.IN)
            {
                Debug.LogError("Already fade in");
                return;
            }

            if (mAudioSource == null)
            {
                Debug.LogError("Can't do the fade in effect without audio source");
                return;
            }

            SetFadeInTime(time);

            mAudioSource.volume = 0.0f;
            mTargetVolume = target_volume;
            mRecordVolume = target_volume;

            mType = JCS_FadeType.IN;
            mEffect = true;
        }

        /// <summary>
        /// Is the sound fade out?
        /// </summary>
        /// <returns>
        /// true: fade out
        /// false: not fade out
        /// </returns>
        public bool IsFadeOut()
        {
            return mAudioSource.volume <= 0.0f;
        }

        /// <summary>
        /// Is the sound fade in?
        /// </summary>
        /// <returns>
        /// true: fade in
        /// false: not fade in
        /// </returns>
        public bool IsFadeIn()
        {
            return mAudioSource.volume >= 1.0f;
        }

        /// <summary>
        /// Check if the target reach the target volume.
        /// </summary>
        /// <returns>
        /// true: reached.
        /// false, vice versa.
        /// </returns>
        public bool IsReachTargetVolume()
        {
            return mAudioSource.volume == mTargetVolume;
        }

        /// <summary>
        /// Do fade in/out sound logic here.
        /// </summary>
        private void DoFadeSound()
        {
            if (mAudioSource == null)
                return;

            switch (mType)
            {
                case JCS_FadeType.OUT:
                    {
                        mAudioSource.volume -= mRecordVolume / mFadeOutTime * JCS_Time.ItTime(mTimeType);

                        if (mAudioSource.volume <= mTargetVolume)
                        {
                            mEffect = false;
                            mAudioSource.volume = mTargetVolume;
                        }
                    }
                    break;
                case JCS_FadeType.IN:
                    {
                        mAudioSource.volume += mRecordVolume / mFadeInTime * JCS_Time.ItTime(mTimeType);

                        if (mAudioSource.volume >= mTargetVolume)
                        {
                            mEffect = false;
                            mAudioSource.volume = mTargetVolume;
                        }
                    }
                    break;
            }
        }
    }
}
