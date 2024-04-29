/**
 * $File: JCS_GamepadButtonUIHandler.cs $
 * $Date: 2017-10-18 12:19:55 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Switch the button UI depends on if the gamepad connected?
    /// </summary>
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class JCS_GamepadButtonUIHandler : MonoBehaviour
    {
        /* Variables */

        private Image mNormalImage = null;
        private Button mButton = null;

        [Separator("Initialize Variables (JCS_GamepadButtonUIHandler)")]

        [Tooltip("Change the selection to sprite swap at awake?")]
        [SerializeField]
        private bool mActiveSpriteSwapAtAwake = true;

        [Header("- Keyboard")]

        [Tooltip("Sprite when button is normal. (Keyboard)")]
        [SerializeField]
        private Sprite mKNormalSprite = null;

        [Tooltip("Sprite when button is hover. (Keyboard)")]
        [SerializeField]
        private Sprite mKHighlightedSprite = null;

        [Tooltip("Sprite when button is pressed. (Keyboard)")]
        [SerializeField]
        private Sprite mKPressedSprite = null;

        [Tooltip("Sprite when button is disabled. (Keyboard)")]
        [SerializeField]
        private Sprite mKDisabledSprite = null;

        [Header("- Gamepad")]

        [Tooltip("Sprite when button is normal. (Joystick)")]
        [SerializeField]
        private Sprite mJNormalSprite = null;

        [Tooltip("Sprite when button is hover. (Joystick)")]
        [SerializeField]
        private Sprite mJHighlightedSprite = null;

        [Tooltip("Sprite when button is pressed. (Joystick)")]
        [SerializeField]
        private Sprite mJPressedSprite = null;

        [Tooltip("Sprite when button is disabled. (Joystick)")]
        [SerializeField]
        private Sprite mJDisabledSprite = null;

        /* Setter & Getter */

        public bool ActiveSpriteSwapAtAwake { get { return this.mActiveSpriteSwapAtAwake; } set { this.mActiveSpriteSwapAtAwake = value; } }

        public Sprite KNormalSprite { get { return this.mKNormalSprite; } set { this.mKNormalSprite = value; } }
        public Sprite KHighlightedSprite { get { return this.mKHighlightedSprite; } set { this.mKHighlightedSprite = value; } }
        public Sprite KPressedSprite { get { return this.mKPressedSprite; } set { this.mKPressedSprite = value; } }
        public Sprite KDisabledSprite { get { return this.mKDisabledSprite; } set { this.mKDisabledSprite = value; } }

        public Sprite JNormalSprite { get { return this.mJNormalSprite; } set { this.mJNormalSprite = value; } }
        public Sprite JHighlightedSprite { get { return this.mJHighlightedSprite; } set { this.mJHighlightedSprite = value; } }
        public Sprite JPressedSprite { get { return this.mJPressedSprite; } set { this.mJPressedSprite = value; } }
        public Sprite JDisabledSprite { get { return this.mJDisabledSprite; } set { this.mJDisabledSprite = value; } }

        /* Functions */

        private void Awake()
        {
            this.mNormalImage = this.GetComponent<Image>();
            this.mButton = this.GetComponent<Button>();

            // chage it to swip swap selection.
            if (mActiveSpriteSwapAtAwake)
                mButton.transition = Selectable.Transition.SpriteSwap;

            /* Register callback */
            {
                JCS_Input.joystickPluggedCallback += JoystickPluggedCallback;
                JCS_Input.joystickUnPluggedCallback += JoystickUnPluggedCallback;
            }

            JCS_Input.InputCallbackOnce();
        }

        private void OnDestroy()
        {
            /* Deregister callback */
            {
                JCS_Input.joystickPluggedCallback -= JoystickPluggedCallback;
                JCS_Input.joystickUnPluggedCallback -= JoystickUnPluggedCallback;
            }
        }

        /// <summary>
        /// When joystick plugged.
        /// </summary>
        protected virtual void JoystickPluggedCallback()
        {
            this.mNormalImage.sprite = mJNormalSprite;

            SpriteState newSpriteState = new SpriteState();

            newSpriteState.highlightedSprite = mJHighlightedSprite;
            newSpriteState.pressedSprite = mJPressedSprite;
            newSpriteState.disabledSprite = mJDisabledSprite;

            this.mButton.spriteState = newSpriteState;
        }

        /// <summary>
        /// When joystick un-plugged.
        /// </summary>
        protected virtual void JoystickUnPluggedCallback()
        {
            this.mNormalImage.sprite = mKNormalSprite;

            SpriteState newSpriteState = new SpriteState();

            newSpriteState.highlightedSprite = mKHighlightedSprite;
            newSpriteState.pressedSprite = mKPressedSprite;
            newSpriteState.disabledSprite = mKDisabledSprite;

            this.mButton.spriteState = newSpriteState;
        }

    }
}
