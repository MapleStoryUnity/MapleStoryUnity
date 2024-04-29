/**
 * $File: JCS_TriggerSwitchBGMEvent.cs $
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
    /// Trigger switch the bgm.
    /// </summary>
    public class JCS_TriggerSwitchBGMEvent : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_TriggerSwitchBGMEvent)")]

        [Tooltip("Sound you want to load.")]
        [SerializeField]
        private AudioClip mSoundClip = null;

        [Tooltip("Time to fade in the sound.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mSoundFadeInTime = 1.0f;

        [Tooltip("Time to fade out the sound.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mSoundFadeOutTime = 1.0f;

        /* Setter & Getter */

        public AudioClip SoundClip { get { return this.mSoundClip; } set { this.mSoundClip = value; } }
        public float SoundFadeInTime { get { return this.mSoundFadeInTime; } set { this.mSoundFadeInTime = value; } }
        public float SoundFadeOutTime { get { return this.mSoundFadeOutTime; } set { this.mSoundFadeOutTime = value; } }

        /* Functions */

        private void OnTriggerEnter(Collider other)
        {
            JCS_SoundManager.instance.SwitchBackgroundMusic(
                mSoundClip, 
                mSoundFadeInTime, 
                mSoundFadeOutTime);
        }
    }
}
