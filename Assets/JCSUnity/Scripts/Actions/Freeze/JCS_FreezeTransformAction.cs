/**
 * $File: JCS_FreezeTransformAction.cs $
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
    /// Action that freeze the gameobject position, rotation and scale.
    /// </summary>
    public class JCS_FreezeTransformAction : JCS_UnityObject
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_FreezeTransformAction) **")]

        [Tooltip("Is this action active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Position where this gameobject freeze at.")]
        [SerializeField]
        private Vector3 mPositionToFreeze = Vector3.zero;

        [Tooltip("Rotation where this gameobject freeze at.")]
        [SerializeField]
        private Vector3 mRotationToFreeze = Vector3.zero;

        [Tooltip("Scale where this gameobject freeze at.")]
        [SerializeField]
        private Vector3 mScaleToFreeze = Vector3.zero;

        [Tooltip("Freeze in the local space.")]
        [SerializeField]
        private bool mIsLocalPosition = true;

        [Tooltip("Freeze in the local space.")]
        [SerializeField]
        private bool mIsLocalRotation = true;

        [Tooltip("Freeze the position in each axis.")]
        [SerializeField]
        private JCS_Bool3 mFreezePosition = JCS_Bool3.allFalse;

        [Tooltip("Freeze the rotation in each axis.")]
        [SerializeField]
        private JCS_Bool3 mFreezeRotation = JCS_Bool3.allFalse;

        [Tooltip("Freeze the scale in each axis.")]
        [SerializeField]
        private JCS_Bool3 mFreezeScale = JCS_Bool3.allFalse;

        /* Setter & Getter */

        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public Vector3 PositionToFreeze { get { return this.mPositionToFreeze; } set { this.mPositionToFreeze = value; } }
        public Vector3 RotationToFreeze { get { return this.mRotationToFreeze; } set { this.mRotationToFreeze = value; } }
        public Vector3 ScaleToFreeze { get { return this.mScaleToFreeze; } set { this.mScaleToFreeze = value; } }
        public JCS_Bool3 FreezePosition { get { return this.mFreezePosition; } set { this.mFreezePosition = value; } }
        public JCS_Bool3 FreezeRotation { get { return this.mFreezeRotation; } set { this.mFreezeRotation = value; } }
        public JCS_Bool3 FreezeScale { get { return this.mFreezeScale; } set { this.mFreezeScale = value; } }
        public bool IsLocalPosition
        {
            get { return this.mIsLocalPosition; }
            set
            {
                this.mIsLocalPosition = value;

                // get the new freeze position.
                if (mIsLocalPosition)
                    this.mPositionToFreeze = this.LocalPosition;
                else
                    this.mPositionToFreeze = this.Position;
            }
        }
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

        /* Functions */

        private void Start()
        {
            // record down all the transform info value.
            if (IsLocalPosition)
                this.mPositionToFreeze = this.LocalPosition;
            else
                this.mPositionToFreeze = this.Position;

            if (mIsLocalRotation)
                this.mRotationToFreeze = this.LocalEulerAngles;
            else
                this.mRotationToFreeze = this.EulerAngles;

            this.mScaleToFreeze = this.LocalScale;
        }

        private void LateUpdate()
        {
            if (!mActive)
                return;

            DoFreeze();
        }

        /// <summary>
        /// Do freezing logic here...
        /// </summary>
        private void DoFreeze()
        {
            DoFreezePosition();
            DoFreezeRotation();
            DoFreezeScale();
        }

        /// <summary>
        /// Freeze transform data.
        /// </summary>
        private void DoFreezePosition()
        {
            Vector3 newPos = this.Position;
            if (IsLocalPosition)
                newPos = this.LocalPosition;

            /* Freeze position */
            if (mFreezePosition.check1)
                newPos.x = mPositionToFreeze.x;
            if (mFreezePosition.check2)
                newPos.y = mPositionToFreeze.y;
            if (mFreezePosition.check3)
                newPos.z = mPositionToFreeze.z;

            /* Apply new value to transform. */
            if (IsLocalPosition)
                this.LocalPosition = newPos;
            else
                this.Position = newPos;
        }

        /// <summary>
        /// Freeze position.
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

        /// <summary>
        /// Freeze position.
        /// </summary>
        private void DoFreezeScale()
        {
            Vector3 newScale = this.LocalScale;

            /* Freeze scale */
            if (mFreezeScale.check1)
                newScale.x = mScaleToFreeze.x;
            if (mFreezeScale.check2)
                newScale.y = mScaleToFreeze.y;
            if (mFreezeScale.check3)
                newScale.z = mScaleToFreeze.z;

            this.LocalScale = newScale;
        }
    }
}
