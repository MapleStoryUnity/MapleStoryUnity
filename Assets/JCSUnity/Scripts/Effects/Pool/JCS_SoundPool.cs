/**
 * $File: JCS_SoundPool.cs $
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
    /// Pool of sound.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_SoundPool : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_SoundPool)")]

        [Tooltip("Pool of the audio clips.")]
        [SerializeField]
        private AudioClip[] mAudioClips = null;

        /* Setter & Getter */

        public AudioClip[] audioClips { get { return mAudioClips; } set { mAudioClips = value; } }

        /* Functions */

        /// <summary>
        /// Get an audio clip from the pool randomly.
        /// </summary>
        /// <returns></returns>
        public AudioClip GetRandomSound()
        {
            if (mAudioClips.Length == 0)
                return null;

            int randIndex = JCS_Random.Range(0, mAudioClips.Length);

            return mAudioClips[randIndex];
        }
    }
}
