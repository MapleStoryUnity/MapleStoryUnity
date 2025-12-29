/**
 * $File: JCS_SwitchBGMButton.cs $
 * $Date: 2020-06-01 22:02:14 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Button that switches the background music.
    /// </summary>
    public class JCS_SwitchBGMButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_SwitchBGMButton)")]

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

        public override void OnClick()
        {
            JCS_SoundManager.FirstInstance().SwitchBGM(
                mSoundClip,
                mSoundFadeInTime,
                mSoundFadeOutTime);
        }
    }
}
