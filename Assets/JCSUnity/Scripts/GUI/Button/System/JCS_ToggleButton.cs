/**
 * $File: JCS_ToggleButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    public delegate void ToggleFunc();

    /// <summary>
    /// Button that have two callback can toggle each other.
    /// </summary>
    public class JCS_ToggleButton
        : JCS_Button
    {
        /* Variables */

        public ToggleFunc acitveFunc = null;
        public ToggleFunc deactiveFunc = null;

        [Header("** Runtime Variables (JCS_ToggleButton) **")]

        [Tooltip("Is the toggle button currently active?")]
        [SerializeField]
        protected bool mActive = false;

        /* Setter & Getter */

        public bool Active {
            get { return this.mActive; }
            set {
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
        public override void JCS_OnClickCallback()
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
            if (acitveFunc == null)
            {
                JCS_Debug.LogError(
                    "You have not set the ACTIVE function ptr...");
                return;
            }

            // do the action.
            acitveFunc.Invoke();
        }

        /// <summary>
        /// Invoke deactive callback.
        /// </summary>
        public void DoDeactiveFunc()
        {
            if (deactiveFunc == null)
            {
                JCS_Debug.LogError(
                    "You have not set the DEACTIVE function ptr...");
                return;
            }

            // do the action.
            deactiveFunc.Invoke();
        }
    }
}
