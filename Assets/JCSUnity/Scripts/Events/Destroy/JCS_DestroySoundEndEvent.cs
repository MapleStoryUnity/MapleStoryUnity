/**
 * $File: JCS_DestroySoundEndEvent.cs $
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
    /// Destroy the game object after the sound is done playing.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_DestroySoundEndEvent : MonoBehaviour
    {
        /* Variables */

        private JCS_SoundPlayer mSoundPlayer = null;

        /* Setter & Getter */

        public void SetAudioClipAndPlayOneShot(AudioClip clip)
        {
            if (mSoundPlayer == null)
                mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            this.mSoundPlayer.audioSource.clip = clip;
            this.mSoundPlayer.PlayOneShot(clip);
        }

        /* Functions */

        private void Update()
        {
            if (mSoundPlayer.audioSource.isPlaying)
                return;

            Destroy(this.gameObject);
        }
    }
}
