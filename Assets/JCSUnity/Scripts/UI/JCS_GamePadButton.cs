/**
 * $File: JCS_GamepadButton.cs $
 * $Date: 2017-10-07 04:37:53 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Button will listen to the gamepad. Note it compatible with PC/desktop as well.
    /// </summary>
    public abstract class JCS_GamepadButton : JCS_Button
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_GamepadButton)")]

        [Tooltip("Ignore the top two variables, listen to any key on the keyboard/gamepad.")]
        [SerializeField]
        protected bool mListenToAnyKey = false;

        [Tooltip("Key action type.")]
        [SerializeField]
        protected JCS_KeyActionType mKeyActionType = JCS_KeyActionType.KEY_DOWN;

        [Header("- Keyboard")]

        [Tooltip("Key to trigger this button.")]
        [SerializeField]
        protected KeyCode mKKeyToListen = KeyCode.None;

        [Header("- Gamepad")]

        [Tooltip("Key to trigger this button.")]
        [SerializeField]
        protected JCS_JoystickButton mJKeyToListen = JCS_JoystickButton.NONE;

        [Tooltip("Which joystick should listen?")]
        [SerializeField]
        protected JCS_JoystickId mJoystickLitener = JCS_JoystickId.ANY;

        [Header("- Sound")]

        [Tooltip("Sound player for 3D sounds calculation.")]
        [SerializeField]
        protected JCS_SoundPlayer mSoundPlayer = null;

        [Tooltip("Sound when button is pressed.")]
        [SerializeField]
        protected AudioClip mButtonClickSound = null;

        [Tooltip("Sound method.")]
        [SerializeField]
        protected JCS_SoundMethod mSoundMethod = JCS_SoundMethod.PLAY_SOUND;

        /* Setter & Getter */

        public KeyCode kKeyToListen { get { return this.mKKeyToListen; } set { this.mKKeyToListen = value; } }
        public JCS_JoystickButton jKeyToListen { get { return this.mJKeyToListen; } set { this.mJKeyToListen = value; } }
        public JCS_JoystickId joystickLitener { get { return this.mJoystickLitener; } set { this.mJoystickLitener = value; } }
        public bool listenToAnyKey { get { return this.mListenToAnyKey; } set { this.mListenToAnyKey = value; } }
        public JCS_KeyActionType keyActionType { get { return this.mKeyActionType; } set { this.mKeyActionType = value; } }

        public AudioClip buttonClickSound { get { return this.mButtonClickSound; } set { this.mButtonClickSound = value; } }
        public JCS_SoundMethod soundMethod { get { return this.mSoundMethod; } set { this.mSoundMethod = value; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            if (mSoundPlayer == null)
                this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
        }

        protected virtual void Update()
        {
            if (mListenToAnyKey)
            {
                if (JCS_Input.IsAnyKeyBuffer(mKeyActionType))
                {
                    ButtonClick();
                    PlayButtonClickSound();
                }
            }
            else
            {

                if (// listen to keyboard.
                    JCS_Input.GetKeyByAction(mKeyActionType, mKKeyToListen) ||
                    // listen to game pad.
                    JCS_Input.GetJoystickKeyByAction(mKeyActionType, mJoystickLitener, mJKeyToListen))
                {
                    ButtonClick();
                    PlayButtonClickSound();
                }
            }
        }

        /// <summary>
        /// Play the button click sound.
        /// </summary>
        private void PlayButtonClickSound()
        {
            JCS_SoundPlayer.PlayByAttachment(mSoundPlayer, mButtonClickSound, mSoundMethod);
        }
    }
}
