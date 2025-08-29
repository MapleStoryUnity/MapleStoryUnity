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

        [Header("Keyboard")]

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

        [Header("Gamepad")]

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

        public bool activeSpriteSwapAtAwake { get { return mActiveSpriteSwapAtAwake; } set { mActiveSpriteSwapAtAwake = value; } }

        public Sprite kNormalSprite { get { return mKNormalSprite; } set { mKNormalSprite = value; } }
        public Sprite kHighlightedSprite { get { return mKHighlightedSprite; } set { mKHighlightedSprite = value; } }
        public Sprite kPressedSprite { get { return mKPressedSprite; } set { mKPressedSprite = value; } }
        public Sprite kDisabledSprite { get { return mKDisabledSprite; } set { mKDisabledSprite = value; } }

        public Sprite jNormalSprite { get { return mJNormalSprite; } set { mJNormalSprite = value; } }
        public Sprite jHighlightedSprite { get { return mJHighlightedSprite; } set { mJHighlightedSprite = value; } }
        public Sprite jPressedSprite { get { return mJPressedSprite; } set { mJPressedSprite = value; } }
        public Sprite jDisabledSprite { get { return mJDisabledSprite; } set { mJDisabledSprite = value; } }

        /* Functions */

        private void Awake()
        {
            mNormalImage = GetComponent<Image>();
            mButton = GetComponent<Button>();

            // chage it to swip swap selection.
            if (mActiveSpriteSwapAtAwake)
                mButton.transition = Selectable.Transition.SpriteSwap;

            /* Register callback */
            {
                JCS_Input.onJoystickPlugged += JoystickPluggedCallback;
                JCS_Input.onJoystickUnplugged += JoystickUnpluggedCallback;
            }

            JCS_Input.InputCallbackOnce();
        }

        private void OnDestroy()
        {
            /* Deregister callback */
            {
                JCS_Input.onJoystickPlugged -= JoystickPluggedCallback;
                JCS_Input.onJoystickUnplugged -= JoystickUnpluggedCallback;
            }
        }

        /// <summary>
        /// When joystick plugged.
        /// </summary>
        protected virtual void JoystickPluggedCallback()
        {
            mNormalImage.sprite = mJNormalSprite;

            SpriteState newSpriteState = new SpriteState();

            newSpriteState.highlightedSprite = mJHighlightedSprite;
            newSpriteState.pressedSprite = mJPressedSprite;
            newSpriteState.disabledSprite = mJDisabledSprite;

            mButton.spriteState = newSpriteState;
        }

        /// <summary>
        /// When joystick un-plugged.
        /// </summary>
        protected virtual void JoystickUnpluggedCallback()
        {
            mNormalImage.sprite = mKNormalSprite;

            SpriteState newSpriteState = new();

            newSpriteState.highlightedSprite = mKHighlightedSprite;
            newSpriteState.pressedSprite = mKPressedSprite;
            newSpriteState.disabledSprite = mKDisabledSprite;

            mButton.spriteState = newSpriteState;
        }

    }
}
