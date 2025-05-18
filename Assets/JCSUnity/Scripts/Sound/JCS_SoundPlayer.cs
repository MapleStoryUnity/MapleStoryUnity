/**
 * $File: JCS_SoundPlayer.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

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

        protected AudioSource mAudioSource = null;

        /* Setter & Getter */

        public AudioSource audioSource { get { return mAudioSource; } }

        /* Functions */

        protected virtual void Awake()
        {
            mAudioSource = this.GetComponent<AudioSource>();

            if (mAudioSource.outputAudioMixerGroup == null)
            {
                Debug.Log("The output audio mixer is empty: " + this.name);
            }
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
        /// <param name="volume"></param>
        public void PlayOneShot(AudioClip clip, float volume = 1.0f)
        {
            if (clip == null)
                return;

            mAudioSource.PlayOneShot(clip, volume);
        }

        /// <summary>
        /// play sound when is not playing any sound.
        /// </summary>
        public void PlayOneShotWhileNotPlaying(AudioClip clip, float volume = 1.0f)
        {
            if (mAudioSource == null)
                return;

            if (mAudioSource.isPlaying)
                return;

            PlayOneShot(clip, volume);
        }

        /// <summary>
        /// Stop all the sound then play the this sound.
        /// </summary>
        public void PlayOneShotInterrupt(AudioClip clip, float volume = 1.0f)
        {
            if (mAudioSource == null)
                return;

            mAudioSource.Stop();

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
