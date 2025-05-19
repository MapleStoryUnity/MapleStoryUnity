/**
 * $File: JCS_Audio.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2025 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.Audio;

namespace JCSUnity
{
    /// <summary>
    /// Audio utilities.
    /// </summary>
    public static class JCS_Audio
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Return a float value from the audio mixer.
        /// </summary>
        public static float GetFloat(AudioMixer mixer, string parameter)
        {
            float val;
            mixer.GetFloat(parameter, out val);
            return val;
        }

        /// <summary>
        /// Convert volume level to decibel.
        /// </summary>
        public static float Volume2Decibel(float volume)
        {
            return Mathf.Log10(volume) * 20;
        }

        /// <summary>
        /// Convert decibel to volume level.
        /// </summary>
        public static float Decibel2Volume(float dB)
        {
            return Mathf.Pow(10f, dB / 20f);
        }

        /// <summary>
        /// Return the volume level from the audio mixer.
        /// </summary>
        public static float GetVolume(AudioMixer mixer, string parameter)
        {
            float dB = GetFloat(mixer, parameter);

            return Decibel2Volume(dB);
        }

        /// <summary>
        /// Set the audio mixer's volume.
        /// </summary>
        public static void SetVolume(AudioMixer mixer, string parameter, float volume)
        {
            float dB = Volume2Decibel(volume);

            mixer.SetFloat(parameter, dB);
        }

        /// <summary>
        /// Same with function `AudioSource.PlayClipAtPoint` with different
        /// default `spatialBlend` value.
        /// </summary>
        public static AudioSource PlayClipAtPoint(
            AudioClip clip,
            Vector3 position,
            float volume,
            float spatialBlend)
        {
            if (clip == null)
                return null;

            var gameObject = new GameObject("One shot audio");
            gameObject.transform.position = position;
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.spatialBlend = spatialBlend;
            audioSource.volume = volume;
            audioSource.Play();

            DestroyClip(audioSource);

            return audioSource;
        }

        /// <summary>
        /// Destroy the clip by its clip length.
        /// </summary>
        public static void DestroyClip(AudioSource source)
        {
            DestroyClip(source, source.clip);
        }
        public static void DestroyClip(AudioSource source, AudioClip clip)
        {
            if (source.loop)
                return;

            Object.Destroy(source.gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
        }
    }
}
