/**
 * $File: JCS_WaitDestroySoundEndEvent.cs $
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
    /// Ensure while playing the sound, this game object does not 
    /// get destroyed.
    /// 
    /// Careful this won't destroy it self!!!
    /// </summary>
    [RequireComponent(typeof(JCS_DestroyReminder))]
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_WaitDestroySoundEndEvent : MonoBehaviour
    {
        /* Variables */

        private bool mSoundPlayed = false;
        private JCS_SoundPlayer mSoundPlayer = null;

        [Separator("⚡️ Runtime Variables (JCS_WaitDestroySoundEndEvent)")]

        [Tooltip("Audio clip to plays.")]
        [SerializeField]
        private AudioClip mAudioClip = null;

        /* Setter & Getter */

        public void SetAudioClip(AudioClip ac) { mAudioClip = ac; }

        /* Functions */

        private void Awake()
        {
            mSoundPlayer = GetComponent<JCS_SoundPlayer>();
        }

        private void Update()
        {
            if (mSoundPlayed)
                return;

            mSoundPlayer.PlayOneShot(mAudioClip);

            mSoundPlayed = true;
        }
    }
}
