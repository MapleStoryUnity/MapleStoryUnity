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

        [Separator("⚡️ Runtime Variables (JCS_TriggerSwitchBGMEvent)")]

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

        public AudioClip soundClip { get { return mSoundClip; } set { mSoundClip = value; } }
        public float soundFadeInTime { get { return mSoundFadeInTime; } set { mSoundFadeInTime = value; } }
        public float soundFadeOutTime { get { return mSoundFadeOutTime; } set { mSoundFadeOutTime = value; } }

        /* Functions */

        private void OnTriggerEnter(Collider other)
        {
            JCS_SoundManager.FirstInstance().SwitchBGM(
                mSoundClip, 
                mSoundFadeInTime, 
                mSoundFadeOutTime);
        }
    }
}
