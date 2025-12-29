/**
 * $File: JCS_SoundSettings.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.Audio;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Sound Setting for each theme.
    /// </summary>
    public class JCS_SoundSettings : JCS_Settings<JCS_SoundSettings>
    {
        /* Variables */

        public const float MAX_FADEOUT_TIME = 5.0f;
        public const float MIN_FADEOUT_TIME = 0.0f;

        [Separator("⚡️ Runtime Variables (JCS_SoundSettings)")]

        [Tooltip("The current audio mixer.")]
        public AudioMixer mixer = null;

        [Tooltip("General Sound fadout time.")]
        [SerializeField]

        [Range(MIN_FADEOUT_TIME, MAX_FADEOUT_TIME)]
        private float mTimeOut = 1.5f;

        [Tooltip("General Sound fadein time.")]
        [SerializeField]
        [Range(MIN_FADEOUT_TIME, MAX_FADEOUT_TIME)]
        private float mTimeIn = 1.5f;

        [Tooltip("Keep BGM current scene?")]
        public bool keepBGMSwitchScene = false;

        [Header("🔍 Scene Sound")]

        [Tooltip("Smoothly switching the sound between the switching the scene.")]
        public bool smoothSwithBetweenScene = true;

        [Tooltip("Clip that will play as background music for this scene.")]
        public AudioClip clipBGM = null;

        // Window System
        [Header("🔍 Window System")]

        [Tooltip("Sound to play when open the window clip.")]
        public AudioClip clipWindowOpen = null;

        [Tooltip("Sound to play when close the window clip.")]
        public AudioClip clipWindowClose = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            CheckInstance(this);
        }

        /// <summary>
        /// Get the real sound fade out time base on 
        /// the sound manager override setting.
        /// </summary>
        /// <returns> timeto fade out the music. </returns>
        public float TimeOut()
        {
            var sm = JCS_SoundManager.FirstInstance();

            // check if override the setting.
            if (sm.overrideSetting)
            {
                // return the override value.
                return sm.timeOut;
            }

            // if not override, 
            // return the basic value.
            return mTimeOut;
        }

        /// <summary>
        /// Get the real sound fade in time base on
        /// the sound manager override setting.
        /// </summary>
        /// <returns> time to fade in the sound </returns>
        public float TimeIn()
        {
            var sm = JCS_SoundManager.FirstInstance();

            // check if override the setting.
            if (sm.overrideSetting)
            {
                // return the override value.
                return sm.timeIn;
            }

            // if not override, 
            // return the basic value.
            return mTimeIn;
        }

        /// <summary>
        /// Make limit so not all the data override the by the new data!
        /// </summary>
        /// <param name="_old"> old data we copy from </param>
        /// <param name="_new"> new data we copy to </param>
        protected override void TransferData(JCS_SoundSettings _old, JCS_SoundSettings _new)
        {
            _new.mixer = _old.mixer;

            _new.mTimeIn = _old.mTimeIn;
            _new.mTimeOut = _old.mTimeOut;
            _new.keepBGMSwitchScene = _old.keepBGMSwitchScene;
        }
    }
}
