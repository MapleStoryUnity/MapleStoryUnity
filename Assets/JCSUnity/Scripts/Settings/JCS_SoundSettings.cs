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

        [Separator("Runtime Variables (JCS_SoundSettings)")]

        [Tooltip("The current audio mixer.")]
        public AudioMixer MIXER = null;

        [Tooltip("General Sound fadout time.")]
        [Range(MIN_FADEOUT_TIME, MAX_FADEOUT_TIME)]
        public float SOUND_FADEOUT_TIME = 1.5f;

        [Tooltip("General Sound fadein time.")]
        [Range(MIN_FADEOUT_TIME, MAX_FADEOUT_TIME)]
        public float SOUND_FADEIN_TIME = 1.5f;

        [Tooltip("Keep BGM current scene?")]
        public bool KEEP_BGM_SWITCH_SCENE = false;

        [Header("- Scene Sound")]

        [Tooltip("Smoothly switching the sound between the switching the scene.")]
        public bool SMOOTH_SWITCH_SOUND_BETWEEN_SCENE = true;

        [Tooltip("Clip that will play as background music for this scene.")]
        public AudioClip BACKGROUND_MUSIC = null;

        // Window System
        [Header("- Window System")]

        [Tooltip("Sound to play when open the window clip.")]
        public AudioClip DEFAULT_OPEN_WINDOW_CLIP = null;

        [Tooltip("Sound to play when close the window clip.")]
        public AudioClip DEFAULT_CLOSE_WINDOW_CLIP = null;

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
        public float GetSoundFadeOutTimeBaseOnSetting()
        {
            var sm = JCS_SoundManager.FirstInstance();

            // check if override the setting.
            if (sm.overrideSetting)
            {
                // return the override value.
                return sm.soundFadeOutTime;
            }

            // if not override, 
            // return the basic value.
            return SOUND_FADEOUT_TIME;
        }

        /// <summary>
        /// Get the real sound fade in time base on
        /// the sound manager override setting?
        /// </summary>
        /// <returns> time to fade in the sound </returns>
        public float GetSoundFadeInTimeBaseOnSetting()
        {
            var sm = JCS_SoundManager.FirstInstance();

            // check if override the setting.
            if (sm.overrideSetting)
            {
                // return the override value.
                return sm.soundFadeInTime;
            }

            // if not override, 
            // return the basic value.
            return SOUND_FADEIN_TIME;
        }

        /// <summary>
        /// Make limit so not all the data override the by the new data!
        /// </summary>
        /// <param name="_old"> old data we copy from </param>
        /// <param name="_new"> new data we copy to </param>
        protected override void TransferData(JCS_SoundSettings _old, JCS_SoundSettings _new)
        {
            _new.MIXER = _old.MIXER;

            _new.KEEP_BGM_SWITCH_SCENE = _old.KEEP_BGM_SWITCH_SCENE;
        }
    }
}
