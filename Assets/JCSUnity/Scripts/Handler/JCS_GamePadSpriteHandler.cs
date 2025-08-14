/**
 * $File: JCS_GamepadSpriteHandler.cs $
 * $Date: 2017-10-18 13:53:59 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Handle any sprite object when controller callback active.
    /// </summary>
    public class JCS_GamepadSpriteHandler : MonoBehaviour
    {
        /* Variables */

        /// <summary>
        /// Target sprite set to handle.
        /// </summary>
        [System.Serializable]
        public struct SpriteSet
        {
            [Tooltip("Object sprite target to replace with.")]
            public JCS_UnityObject objectSprite;

            [Tooltip("Sprite to use if gamepad not connected.")]
            public Sprite keyboardSprite;

            [Tooltip("Sprite to use if gamepad connected.")]
            public Sprite gamePadSprite;
        };

        [Separator("Runtime Variables (JCS_GamepadSpriteHandler)")]

        [Tooltip("Any sprite element you want to change depends on controller connection.")]
        public List<SpriteSet> spriteSets = null;

        /* Setter & Getter */

        /* Functions */

        private void Start()
        {
            spriteSets = JCS_Array.RemoveEmpty(spriteSets);

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
            foreach (SpriteSet ss in spriteSets)
            {
                if (ss.objectSprite == null)
                    continue;

                // set to gamepad's sprite.
                ss.objectSprite.localSprite = ss.gamePadSprite;
            }
        }

        /// <summary>
        /// When joystick un-plugged.
        /// </summary>
        protected virtual void JoystickUnpluggedCallback()
        {
            foreach (SpriteSet ss in spriteSets)
            {
                if (ss.objectSprite == null)
                    continue;

                // set to gamepad's sprite.
                ss.objectSprite.localSprite = ss.keyboardSprite;
            }
        }
    }
}
