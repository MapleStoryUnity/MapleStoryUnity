/**
 * $File: JCS_SoundPoolAction.cs $
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
    /// A pool of audio clips.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_SoundPoolAction : MonoBehaviour , JCS_IAction
    {
        /* Variables */

        private JCS_SoundPlayer mSoundPlayer = null;

        [Separator("Runtime Variables (JCS_SoundPoolAction)")]

        [Tooltip("Pool of audio clips.")]
        [SerializeField]
        private AudioClip[] mAudioClips = null;

        /* Setter & Getter */

        public AudioClip[] AudioClips { get { return this.mAudioClips; } set { this.mAudioClips = value; } }

        /* Functions */

        private void Awake()
        {
            this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
        }

        /// <summary>
        /// Randomly play a sound from the pool.
        /// </summary>
        public void PlayRandom()
        {
            if (mAudioClips.Length == 0)
                return;

            int randIndex = JCS_Random.Range(0, mAudioClips.Length);

            if (mAudioClips[randIndex] == null)
            {
                Debug.LogError("You inlcude a null references in he audio pool");
                return;
            }

            mSoundPlayer.PlayOneShot(mAudioClips[randIndex]);
        }
    }
}
