/**
 * $File: JCS_GamePadAnimationHandler.cs $
 * $Date: 2017-10-31 11:22:17 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Handle animation using 'JCS_2DAnimator' depends on weather
    /// the game pad is plug or unplugn.
    /// </summary>
    public class JCS_GamePadAnimationHandler : MonoBehaviour
    {
        /* Variables */

        /// <summary>
        /// Target sprite set to handle.
        /// </summary>
        [System.Serializable]
        public struct AnimationSet
        {
            [Tooltip("Object sprite target to replace with.")]
            public JCS_2DAnimator animator;

            [Tooltip("Animation to use if gamepad not connected.")]
            [Range(0, 1)]
            public int keyboardAnimationId;

            [Tooltip("Animation to use if gamepad connected.")]
            [Range(0, 1)]
            public int gamePadAnimationId;
        };


        [Header("** Runtime Variables (JCS_GamePadSpriteHandler) **")]

        [Tooltip("Any animation element you want to change depends on controller connection.")]
        public List<AnimationSet> animationSets = null;


        /* Setter & Getter */

        /* Functions */

        private void Start()
        {
            this.animationSets = JCS_Util.RemoveEmptySlot(this.animationSets);

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
            foreach (AnimationSet ss in animationSets)
            {
                if (ss.animator == null)
                    continue;

                // play the gamepad animation
                ss.animator.DoAnimation(ss.gamePadAnimationId);
            }
        }

        /// <summary>
        /// When joystick un-plugged.
        /// </summary>
        protected virtual void JoystickUnPluggedCallback()
        {
            foreach (AnimationSet ss in animationSets)
            {
                if (ss.animator == null)
                    continue;

                // play the keyboard animation.
                ss.animator.DoAnimation(ss.keyboardAnimationId);
            }
        }
    }
}
