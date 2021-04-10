/**
 * $File: JCS_SoundPoolAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// A pool of audio clips.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_SoundPoolAction
        : MonoBehaviour
        , JCS_Action
    {
        /* Variables */

        private JCS_SoundPlayer mSoundPlayer = null;

        [Header("** Runtime Variables (JCS_SoundPoolAction) **")]

        [Tooltip("Pool of audio clips.")]
        [SerializeField]
        private AudioClip[] mAudioClips = null;

        [Tooltip("Sound type you want to organized.")]
        [SerializeField]
        private JCS_SoundSettingType mSoundSettingType = JCS_SoundSettingType.NONE;

        /* Setter & Getter */

        public AudioClip[] AudioClips { get { return this.mAudioClips; } set { this.mAudioClips = value; } }
        public JCS_SoundSettingType SoundSettingType { get { return this.mSoundSettingType; } set { this.mSoundSettingType = value; } }

        /* Functions */

        private void Awake()
        {
            this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
        }

        /// <summary>
        /// Randomly play a sound from the pool.
        /// </summary>
        public void PlayRandomSound()
        {
            if (mAudioClips.Length == 0)
                return;

            int randIndex = JCS_Random.Range(0, mAudioClips.Length);

            if (mAudioClips[randIndex] == null)
            {
                JCS_Debug.LogError("You inlcude a null references in he audio pool");
                return;
            }

            float soundVolume = JCS_SoundSettings.instance.GetSoundBaseOnType(mSoundSettingType);
            mSoundPlayer.PlayOneShot(mAudioClips[randIndex], soundVolume);
        }
    }
}
