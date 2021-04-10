/**
 * $File: JCS_ToggleGamePadButton.cs $
 * $Date: 2017-10-27 12:05:00 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Button that have two callback can toggle each other. (Game Pad)
    /// </summary>
    public class JCS_ToggleGamePadButton
        : JCS_GamePadButton
    {
        /* Variables */

        public ToggleFunc acitveFunc = null;
        public ToggleFunc deactiveFunc = null;

        [Header("** Check Variables (JCS_ToggleGamePadButton) **")]

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
                JCS_Debug.LogError("U have not set the ACTIVE function ptr...");
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
                JCS_Debug.LogError("U have not set the DEACTIVE function ptr...");
                return;
            }

            // do the action.
            deactiveFunc.Invoke();
        }
    }
}
