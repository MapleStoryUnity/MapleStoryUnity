/**
 * $File: JCS_FadeSound.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Effect that fade the sound.
    /// </summary>
    public class JCS_FadeSound
        : MonoBehaviour
    {
        /* Variables */

        private AudioSource mAudioSource = null;

        private JCS_FadeType mType = JCS_FadeType.NONE;

        [Header("** Check Variables (JCS_FadeSound) **")]

        [Tooltip("Do the effect?")]
        [SerializeField]
        private bool mEffect = false;

        [Header("** Runtime Variables (JCS_FadeSound) **")]

        [Tooltip("Fade out time.")]
        [SerializeField]
        private float mFadeOutTime = 1.0f;

        [Tooltip("Fade in time.")]
        [SerializeField]
        private float mFadeInTime = 1.0f;

        private float mTargetVolume = 0.0f;
        private float mRecordVolume = 0.0f;

        /* Setter & Getter */

        public void SetFadeOutTime(float t) { this.mFadeOutTime = t; }
        public void SetFadeInTime(float t) { this.mFadeInTime = t; }

        public AudioSource GetAudioSource() { return this.mAudioSource; }

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
                JCS_Debug.LogError("Source assign are null references");
                return;
            }

            this.mAudioSource = source;

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
            if (mType == JCS_FadeType.FADE_OUT)
            {
                JCS_Debug.LogError("Already fade out");
                return;
            }

            if (mAudioSource == null)
            {
                JCS_Debug.LogError("Can't do the fade out effect without audio source");
                return;
            }

            SetFadeOutTime(time);

            mTargetVolume = target_volume;
            mRecordVolume = mAudioSource.volume;

            mType = JCS_FadeType.FADE_OUT;
            mEffect = true;
        }

        /// <summary>
        /// Fade in the sound.
        /// </summary>
        /// <param name="target_volume"></param>
        /// <param name="time"></param>
        public void FadeIn(float target_volume, float time)
        {
            if (mType == JCS_FadeType.FADE_IN)
            {
                JCS_Debug.LogError("Already fade in");
                return;
            }

            if (mAudioSource == null)
            {
                JCS_Debug.LogError("Can't do the fade in effect without audio source");
                return;
            }

            SetFadeInTime(time);

            mAudioSource.volume = 0.0f;
            mTargetVolume = target_volume;
            mRecordVolume = target_volume;

            mType = JCS_FadeType.FADE_IN;
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
            switch (mType)
            {
                case JCS_FadeType.FADE_OUT:
                    {
                        mAudioSource.volume -= mRecordVolume / mFadeOutTime * Time.deltaTime;

                        if (mAudioSource.volume <= mTargetVolume)
                        {
                            mEffect = false;
                            mAudioSource.volume = mTargetVolume;
                        }
                    }
                    break;
                case JCS_FadeType.FADE_IN:
                    {
                        mAudioSource.volume += mRecordVolume / mFadeInTime * Time.deltaTime;

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
