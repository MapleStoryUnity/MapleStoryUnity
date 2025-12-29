/**
 * $File: JCS_FreezeRotationAction.cs $
 * $Date: 2017-05-11 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Action that freeze the game object rotation.
    /// </summary>
    public class JCS_FreezeRotationAction : JCS_UnityObject
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_FreezeTransformAction)")]

        [Tooltip("Is this action active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Rotation where game object freeze at.")]
        [SerializeField]
        private Vector3 mRotationToFreeze = Vector3.zero;

        [Tooltip("Freeze in the local space.")]
        [SerializeField]
        private bool mIsLocalRotation = true;

        [Tooltip("Freeze the rotation in each axis.")]
        [SerializeField]
        private JCS_Bool3 mFreezeRotation = JCS_Bool3.allFalse;

        /* Setter & Getter */

        public bool active { get { return mActive; } set { mActive = value; } }
        public Vector3 rotationToFreeze { get { return mRotationToFreeze; } set { mRotationToFreeze = value; } }
        public bool isLocalRotation
        {
            get { return mIsLocalRotation; }
            set
            {
                mIsLocalRotation = value;

                // get the new freeze position.
                if (mIsLocalRotation)
                    mRotationToFreeze = localEulerAngles;
                else
                    mRotationToFreeze = eulerAngles;
            }
        }
        public JCS_Bool3 freezeRotation { get { return mFreezeRotation; } set { mFreezeRotation = value; } }

        /* Functions */

        private void LateUpdate()
        {
            if (!mActive)
                return;

            DoFreezeRotation();
        }

        /// <summary>
        /// Freeze rotation.
        /// </summary>
        private void DoFreezeRotation()
        {
            Vector3 newRot = eulerAngles;
            if (mIsLocalRotation)
                newRot = localEulerAngles;

            /* Freeze euler angles */
            if (mFreezeRotation.check1)
                newRot.x = mRotationToFreeze.x;
            if (mFreezeRotation.check2)
                newRot.y = mRotationToFreeze.y;
            if (mFreezeRotation.check3)
                newRot.z = mRotationToFreeze.z;

            if (mIsLocalRotation)
                localEulerAngles = newRot;
            else
                eulerAngles = newRot;
        }
    }
}
