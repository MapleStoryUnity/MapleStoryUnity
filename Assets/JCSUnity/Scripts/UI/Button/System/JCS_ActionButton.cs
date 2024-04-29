/**
 * $File: JCS_ActionButton.cs $
 * $Date: 2023-07-29 12:05:00 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2023 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.Events;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Button that accept any event.
    /// </summary>
    public class JCS_ActionButton : JCS_Button
    {
        /* Variables */

        protected EmptyFunction onAction = null;

        [Separator("Runtime Variables (JCS_ActionButton)")]

        [Tooltip("Execute this when it's triggered.")]
        [SerializeField]
        protected UnityEvent mOnAction = null;

        /* Setter & Getter */

        public UnityEvent OnAction { get { return this.mOnAction; } set { this.mOnAction = value; } }

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
            if (onAction != null)
                onAction.Invoke();

            if (mOnAction != null)
                mOnAction.Invoke();
        }
    }
}
