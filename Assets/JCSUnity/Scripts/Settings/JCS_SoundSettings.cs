/**
 * $File: JCS_SoundSettings.cs $
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
    /// Sound Setting for each theme.
    /// </summary>
    public class JCS_SoundSettings : JCS_Settings<JCS_SoundSettings>
    {
        /* Variables */

        public const float MAX_SOUND_VOLUME = 1.0f;
        public const float MIN_SOUND_VOLUME = 0.0f;

        public const float MAX_SOUND_FADEOUT_TIME = 5.0f;
        public const float MIN_SOUND_FADEOUT_TIME = 0.0f;

        [Separator("Runtime Variables (JCS_SoundSettings)")]

        [Tooltip("If true, mute BGM.")]
        public bool BGM_MUTE = false;

        [Tooltip("If true, mute SFX.")]
        public bool EFFECT_MUTE = false;

        [Tooltip("If true, mute player SFX.")]
        public bool PERFONAL_EFFECT_MUTE = false;

        [Tooltip("Background music [Default: 0.4f]")]
        [Range(MIN_SOUND_VOLUME, MAX_SOUND_VOLUME)]
        public float BGM_SOUND = 0.4f;

        [Tooltip("Sound from other player/environment [Default: 0.4]")]
        [Range(MIN_SOUND_VOLUME, MAX_SOUND_VOLUME)]
        public float SFX_SOUND = 0.4f;

        [Tooltip("Sound from player [Default: 0.4f]")]
        [Range(MIN_SOUND_VOLUME, MAX_SOUND_VOLUME)]
        public float SKILLS_SOUND = 0.4f;

        [Tooltip("General Sound fadout time.")]
        [Range(MIN_SOUND_FADEOUT_TIME, MAX_SOUND_FADEOUT_TIME)]
        public float SOUND_FADEOUT_TIME = 1.5f;

        [Tooltip("General Sound fadein time.")]
        [Range(MIN_SOUND_FADEOUT_TIME, MAX_SOUND_FADEOUT_TIME)]
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

        public float GetBGM_Volume() { return BGM_SOUND; }
        public void SetBGM_Volume(float volume)
        {
            BGM_SOUND = volume;
            JCS_SoundManager.instance.GetBGMAudioSource().volume = volume;
        }
        public float GetSFXSound_Volume() { return SFX_SOUND; }
        public void SetSFXSound_Volume(float volume)
        {
            SFX_SOUND = volume;
            JCS_SoundManager.instance.SetSoundVolume(JCS_SoundSettingType.SFX_SOUND, volume);
        }
        public void SetSkillsSound_Volume(float volume)
        {
            SKILLS_SOUND = volume;
            JCS_SoundManager.instance.SetSoundVolume(JCS_SoundSettingType.SKILLS_SOUND, volume);
        }
        public float GetSkillsSound_Volume() { return SKILLS_SOUND; }
        public float GetSoundVolume(JCS_SoundSettingType type)
        {
            switch (type)
            {
                case JCS_SoundSettingType.BGM_SOUND:
                    return GetBGM_Volume();
                case JCS_SoundSettingType.SFX_SOUND:
                    return GetSFXSound_Volume();
                case JCS_SoundSettingType.SKILLS_SOUND:
                    return GetSkillsSound_Volume();
            }

            JCS_Debug.LogError("Get unknown volume...");
            return 0;
        }
        public void SetSoudnVolume(JCS_SoundSettingType type, float volume)
        {
            switch (type)
            {
                case JCS_SoundSettingType.NONE: return;
                case JCS_SoundSettingType.BGM_SOUND: BGM_SOUND = volume; break;
                case JCS_SoundSettingType.SFX_SOUND: SFX_SOUND = volume; break;
                case JCS_SoundSettingType.SKILLS_SOUND: SKILLS_SOUND = volume; break;
            }

            JCS_SoundManager.instance.SetSoundVolume(type, volume);
        }

        /* Functions */

        private void Awake()
        {
            instance = CheckSingleton(instance, this);
        }

        /// <summary>
        /// Get the real sound fade out time base on 
        /// the sound manager override setting.
        /// </summary>
        /// <returns> timeto fade out the music. </returns>
        public float GetSoundFadeOutTimeBaseOnSetting()
        {
            var sm = JCS_SoundManager.instance;

            // check if override the setting.
            if (sm.OverrideSetting)
            {
                // return the override value.
                return sm.SoundFadeOutTime;
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
            var sm = JCS_SoundManager.instance;

            // check if override the setting.
            if (sm.OverrideSetting)
            {
                // return the override value.
                return sm.SoundFadeInTime;
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
            _new.BGM_MUTE = _old.BGM_MUTE;
            _new.EFFECT_MUTE = _old.EFFECT_MUTE;
            _new.PERFONAL_EFFECT_MUTE = _old.PERFONAL_EFFECT_MUTE;
            _new.BGM_SOUND = _old.BGM_SOUND;
            _new.SFX_SOUND = _old.SFX_SOUND;
            _new.SKILLS_SOUND = _old.SKILLS_SOUND;

            _new.KEEP_BGM_SWITCH_SCENE = _old.KEEP_BGM_SWITCH_SCENE;
        }
    }
}
