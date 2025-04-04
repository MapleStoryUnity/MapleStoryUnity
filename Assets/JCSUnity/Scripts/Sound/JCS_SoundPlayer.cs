﻿/**
 * $File: JCS_SoundPlayer.cs $
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
    /// Sound Player for any object that need to player 
    /// sound effect.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class JCS_SoundPlayer : MonoBehaviour
    {
        /* Variables */

        private AudioSource mAudioSource = null;

        [Separator("Runtime Variables (JCS_SoundPlayer)")]

        [Tooltip("Sound setting type for this sound player.")]
        [SerializeField]
        private JCS_SoundSettingType mSoundSettingType = JCS_SoundSettingType.NONE;

        /* Setter & Getter */

        public AudioSource GetAudioSource() { return this.mAudioSource; }
        public JCS_SoundSettingType SoundSettingType { get { return this.mSoundSettingType; } }

        /* Functions */

        protected virtual void Awake()
        {
            mAudioSource = this.GetComponent<AudioSource>();
        }

        protected virtual void Start()
        {
            // let sound manager know we are here and ready
            // to be manage be him(Sound Manager)
            JCS_SoundManager.instance.AssignSoundSource(mSoundSettingType, GetAudioSource());
        }

        /// <summary>
        /// Play AC by method depends on the attachement of the SP.
        /// </summary>
        /// <param name="sp"> Used sound player, if null use global sound player. </param>
        /// <param name="ac"> Target audio clip. </param>
        /// <param name="method"> Method to play. </param>
        /// <param name="volume"> Sound volume. </param>
        public static void PlayByAttachment(AudioClip clip, JCS_SoundMethod method = JCS_SoundMethod.PLAY_SOUND, float volume = 1.0f)
        {
            PlayByAttachment(null, clip, method);
        }
        public static void PlayByAttachment(JCS_SoundPlayer player, AudioClip clip, JCS_SoundMethod method = JCS_SoundMethod.PLAY_SOUND, float volume = 1.0f)
        {
            JCS_SoundPlayer useSp = player;
            if (useSp == null)
                useSp = JCS_SoundManager.instance.GlobalSoundPlayer();
            useSp.PlayOneShotByMethod(clip, method, volume);
        }

        /// <summary>
        /// Play one shot of sound.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="type"></param>
        public void PlayOneShot(AudioClip clip, JCS_SoundSettingType type)
        {
            var ss = JCS_SoundSettings.instance;

            float volume = 0;
            switch (type)
            {
                case JCS_SoundSettingType.BGM:
                    volume = ss.GetBGM_Volume();
                    break;
                case JCS_SoundSettingType.EFFECT:
                    volume = ss.GetEffect_Volume();
                    break;
                case JCS_SoundSettingType.SKILL:
                    volume = ss.GetSkill_Volume();
                    break;
            }

            PlayOneShot(clip, volume);
        }

        /// <summary>
        /// Play one shot of sound.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="volume"></param>
        public void PlayOneShot(AudioClip clip, float volume = 1.0f)
        {
            if (clip == null)
                return;

            GetAudioSource().PlayOneShot(clip, volume);
        }

        /// <summary>
        /// play sound when is not playing any sound.
        /// </summary>
        public void PlayOneShotWhileNotPlaying(AudioClip clip, float volume = 1.0f)
        {
            if (GetAudioSource() == null)
                return;

            if (GetAudioSource().isPlaying)
                return;

            PlayOneShot(clip, volume);
        }

        /// <summary>
        /// Stop all the sound then play the this sound.
        /// </summary>
        public void PlayOneShotInterrupt(AudioClip clip, float volume = 1.0f)
        {
            if (GetAudioSource() == null)
                return;

            GetAudioSource().Stop();

            PlayOneShot(clip, volume);
        }

        /// <summary>
        /// Play sound by method.
        /// </summary>
        /// <param name="clip"> clip to play. </param>
        /// <param name="method"> method of play. </param>
        /// <param name="volume"> volume to play. </param>
        public void PlayOneShotByMethod(AudioClip clip, JCS_SoundMethod method, float volume = 1.0f)
        {
            switch (method)
            {
                case JCS_SoundMethod.PLAY_SOUND:
                    PlayOneShot(clip, volume);
                    break;
                case JCS_SoundMethod.PLAY_SOUND_WHILE_NOT_PLAYING:
                    PlayOneShotWhileNotPlaying(clip, volume);
                    break;
                case JCS_SoundMethod.PLAY_SOUND_INTERRUPT:
                    PlayOneShotInterrupt(clip, volume);
                    break;
            }
        }
    }
}
