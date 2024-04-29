/**
 * $File: JCS_ToggleGamepadButton.cs $
 * $Date: 2017-10-27 12:05:00 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.Events;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Button that have two callback can toggle each other. (Gamepad)
    /// </summary>
    public class JCS_ToggleGamepadButton : JCS_GamepadButton
    {
        /* Variables */

        public EmptyFunction onActive = null;
        public EmptyFunction onDeactive = null;

        [Separator("Check Variables (JCS_ToggleGamepadButton)")]

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

        public bool Active
        {
            get { return this.mActive; }
            set
            {
                if (this.mActive != value)
                {
                    Toggle();
                    this.mActive = value;
                }
            }
        }
        public UnityEvent OnAction { get { return this.mOnAction; } set { this.mOnAction = value; } }
        public UnityEvent OffAction { get { return this.mOffAction; } set { this.mOffAction = value; } }

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
