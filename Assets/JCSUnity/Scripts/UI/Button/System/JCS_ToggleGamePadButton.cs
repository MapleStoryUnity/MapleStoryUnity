/**
 * $File: JCS_ToggleGamepadButton.cs $
 * $Date: 2017-10-27 12:05:00 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

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

        [Header("** Check Variables (JCS_ToggleGamepadButton) **")]

        [Tooltip("Return true if toggle button currently active.")]
        [SerializeField]
        protected bool mActive = false;

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
            if (onActive == null)
            {
                JCS_Debug.LogError("U have not set the ACTIVE function ptr...");
                return;
            }

            // do the action.
            onActive.Invoke();
        }

        /// <summary>
        /// Invoke deactive callback.
        /// </summary>
        public void DoDeactiveFunc()
        {
            if (onDeactive == null)
            {
                JCS_Debug.LogError("U have not set the DEACTIVE function ptr...");
                return;
            }

            // do the action.
            onDeactive.Invoke();
        }
    }
}
