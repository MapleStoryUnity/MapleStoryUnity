/**
 * $File: JCS_ToggleButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Button that have two callback can toggle each other.
    /// </summary>
    public class JCS_ToggleButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        /* Variables */

        public Action onActive = null;
        public Action onDeactive = null;

        [Separator("⚡️ Runtime Variables (JCS_ToggleButton)")]

        [Tooltip("Return true if toggle button currently active.")]
        [SerializeField]
        protected bool mActive = false;

        [Tooltip("Execute this when toggle is on.")]
        [SerializeField]
        protected UnityEvent mOnAction = null;

        [Tooltip("Execute this when toggle is off.")]
        [SerializeField]
        protected UnityEvent mOffAction = null;

        /* Setter & Getter */

        public bool active
        {
            get { return mActive; }
            set
            {
                if (mActive != value)
                {
                    Toggle();
                    mActive = value;
                }
            }
        }
        public UnityEvent onAction { get { return mOnAction; } set { mOnAction = value; } }
        public UnityEvent offAction { get { return mOffAction; } set { mOffAction = value; } }

        /* Functions */

        /// <summary>
        /// On click event.
        /// </summary>
        public override void OnClick()
        {
            Toggle();
        }

        /// <summary>
        /// Toggle the button.
        /// </summary>
        public void Toggle()
        {
            // do the toggle function.
            if (mActive)
                // active do deactive.
                DoDeactiveFunc();
            else
                // deactive do active.
                DoActiveFunc();

            // toggle the boolean
            mActive = !mActive;
        }

        /// <summary>
        /// Invoke active callback.
        /// </summary>
        public void DoActiveFunc()
        {
            if (onActive != null)
                onActive.Invoke();

            if (mOnAction != null)
                mOnAction.Invoke();
        }

        /// <summary>
        /// Invoke deactive callback.
        /// </summary>
        public void DoDeactiveFunc()
        {
            if (onDeactive != null)
                onDeactive.Invoke();

            if (mOffAction != null)
                mOffAction.Invoke();
        }
    }
}
