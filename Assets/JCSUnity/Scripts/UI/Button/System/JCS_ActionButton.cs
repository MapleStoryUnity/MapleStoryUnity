/**
 * $File: JCS_ActionButton.cs $
 * $Date: 2023-07-29 12:05:00 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2023 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Button that accept any event.
    /// </summary>
    public class JCS_ActionButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        /* Variables */

        protected Action onAction = null;

        [Separator("Runtime Variables (JCS_ActionButton)")]

        [Tooltip("Execute this when it's triggered.")]
        [SerializeField]
        protected UnityEvent mOnActionUE = null;

        /* Setter & Getter */

        public UnityEvent onActionUE { get { return mOnActionUE; } set { mOnActionUE = value; } }

        /* Functions */

        /// <summary>
        /// On click event.
        /// </summary>
        public override void OnClick()
        {
            Execute();
        }

        /// <summary>
        /// Execute the action.
        /// </summary>
        public void Execute()
        {
            onAction?.Invoke();

            mOnActionUE?.Invoke();
        }
    }
}
