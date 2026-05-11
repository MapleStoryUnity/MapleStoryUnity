/**
 * $File: JCS_FreezeScaleAction.cs $
 * $Date: 2017-05-11 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using Unity.Mathematics;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Action that freeze the game object scale.
    /// </summary>
    public class JCS_FreezeScaleAction : JCS_UnityObject
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_FreezeScaleAction)")]

        [Tooltip("Is this action active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Scale where this game object freeze at.")]
        [SerializeField]
        private Vector3 mScaleToFreeze = Vector3.zero;

        [Tooltip("Freeze the scale in each axis.")]
        [SerializeField]
        private bool3 mFreezeScale = default;

        /* Setter & Getter */

        public bool active { get { return mActive; } set { mActive = value; } }
        public bool3 freezeScale { get { return mFreezeScale; } set { mFreezeScale = value; } }
        public Vector3 scaleToFreeze { get { return mScaleToFreeze; } set { mScaleToFreeze = value; } }

        /* Functions */

        private void Start()
        {
            // record down all the transform info value.
            mScaleToFreeze = localScale;
        }

        private void LateUpdate()
        {
            if (!mActive)
                return;

            DoFreezeScale();
        }

        /// <summary>
        /// Freeze scale vector.
        /// </summary>
        private void DoFreezeScale()
        {
            Vector3 newScale = localScale;

            /* Freeze scale */
            if (mFreezeScale.x)
                newScale.x = mScaleToFreeze.x;
            if (mFreezeScale.y)
                newScale.y = mScaleToFreeze.y;
            if (mFreezeScale.z)
                newScale.z = mScaleToFreeze.z;

            localScale = newScale;
        }
    }
}
