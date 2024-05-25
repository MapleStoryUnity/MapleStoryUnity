/**
 * $File: JCS_Cursor.cs $
 * $Date: 2024-05-05 16:51:42 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2024 by Shen, Jen-Chieh $
 */
using MyBox;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Cursor interface.
    /// </summary>
    public abstract class JCS_Cursor : JCS_Settings<JCS_Cursor>
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_Cursor)")]

        [Tooltip("Show the cursor or not.")]
        [SerializeField]
        protected bool mShowCursor = false;

        /* Setter & Getter */

        public bool ShowCursor { get { return this.mShowCursor; } set { this.mShowCursor = value; } }

        /* Functions */

        protected virtual void Awake()
        {
            instance = CheckSingleton(instance, this);

#if UNITY_EDITOR
            mShowCursor = true;
#else
            Cursor.visible = mShowCursor;
#endif
        }

        private void OnApplicationFocus(bool focusStatus)
        {
            // when get focus do check to not show the cursor
            if (focusStatus)
            {
                Cursor.visible = mShowCursor;
            }
        }

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected override void TransferData(JCS_Cursor _old, JCS_Cursor _new)
        {
            // empty..
        }
    }
}
