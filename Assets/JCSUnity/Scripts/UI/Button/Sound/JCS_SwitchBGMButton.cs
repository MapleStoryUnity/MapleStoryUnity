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

        [Separator("Runtime Variables (JCS_SwitchBGMButton)")]

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

        public override void OnClick()
        {
            JCS_SoundManager.FirstInstance().SwitchBGM(
                mSoundClip,
                mSoundFadeInTime,
                mSoundFadeOutTime);
        }
    }
}
