/**
 * $File: JCS_FreezeRotationAction.cs $
 * $Date: 2017-05-11 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Action that freeze the gameobject rotation.
    /// </summary>
    public class JCS_FreezeRotationAction : JCS_UnityObject
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_FreezeTransformAction) **")]

        [Tooltip("Is this action active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Rotation where gameobject freeze at.")]
        [SerializeField]
        private Vector3 mRotationToFreeze = Vector3.zero;

        [Tooltip("Freeze in the local space.")]
        [SerializeField]
        private bool mIsLocalRotation = true;

        [Tooltip("Freeze the rotation in each axis.")]
        [SerializeField]
        private JCS_Bool3 mFreezeRotation = JCS_Bool3.allFalse;

        /* Setter & Getter */

        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public Vector3 RotationToFreeze { get { return this.mRotationToFreeze; } set { this.mRotationToFreeze = value; } }
        public bool IsLocalRotation
        {
            get { return this.mIsLocalRotation; }
            set
            {
                this.mIsLocalRotation = value;

                // get the new freeze position.
                if (mIsLocalRotation)
                    this.mRotationToFreeze = this.LocalEulerAngles;
                else
                    this.mRotationToFreeze = this.EulerAngles;
            }
        }
        public JCS_Bool3 FreezeRotation { get { return this.mFreezeRotation; } set { this.mFreezeRotation = value; } }

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
            Vector3 newRot = this.EulerAngles;
            if (IsLocalRotation)
                newRot = this.LocalEulerAngles;

            /* Freeze euler angles */
            if (mFreezeRotation.check1)
                newRot.x = mRotationToFreeze.x;
            if (mFreezeRotation.check2)
                newRot.y = mRotationToFreeze.y;
            if (mFreezeRotation.check3)
                newRot.z = mRotationToFreeze.z;

            if (IsLocalRotation)
                this.LocalEulerAngles = newRot;
            else
                this.EulerAngles = newRot;
        }
    }
}
