/**
 * $File: JCS_SoundEffect.cs $
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
    /// Sound Effect use for environment!
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class JCS_SoundEffect : MonoBehaviour
    {
        /* Variables */

        private AudioSource mAudioSource = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            this.mAudioSource = this.GetComponent<AudioSource>();

            // Check to see if there is audio attach in the game or not!
            if (mAudioSource.clip == null)
                JCS_Debug.LogError("Sound Effect Object with out audio clip init...");
        }
    }
}
