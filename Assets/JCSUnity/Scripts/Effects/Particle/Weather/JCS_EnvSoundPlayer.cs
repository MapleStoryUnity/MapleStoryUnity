/**
 * $File: JCS_EnvironmentSoundPlayer.cs $
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
    /// Sound player specific for environment sound.
    /// </summary>
    public class JCS_EnvSoundPlayer : JCS_SoundPlayer
    {
        /* Variables */

        [Separator("Initialize Variables (JCS_EnvSoundPlayer)")]

        [Tooltip("Sound to play for environment sound.")]
        [SerializeField]
        private AudioClip mEnvSound = null;

        /* Setter & Getter */

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            mAudioSource.loop = true;

            // NOTE(JenChieh): does not connect to bgm yet.
            mAudioSource.clip = mEnvSound;
            mAudioSource.Play();
        }
    }
}
