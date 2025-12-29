/**
 * $File: JCS_FreezeTransformAction.cs $
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
    /// Action that freeze the game object position, rotation and scale.
    /// </summary>
    public class JCS_FreezeTransformAction : JCS_UnityObject
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_FreezeTransformAction)")]

        [Tooltip("Is this action active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Position where this game object freeze at.")]
        [SerializeField]
        private Vector3 mPositionToFreeze = Vector3.zero;

        [Tooltip("Rotation where this game object freeze at.")]
        [SerializeField]
        private Vector3 mRotationToFreeze = Vector3.zero;

        [Tooltip("Scale where this game object freeze at.")]
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

        public bool active { get { return mActive; } set { mActive = value; } }
        public Vector3 positionToFreeze { get { return mPositionToFreeze; } set { mPositionToFreeze = value; } }
        public Vector3 rotationToFreeze { get { return mRotationToFreeze; } set { mRotationToFreeze = value; } }
        public Vector3 scaleToFreeze { get { return mScaleToFreeze; } set { mScaleToFreeze = value; } }
        public JCS_Bool3 freezePosition { get { return mFreezePosition; } set { mFreezePosition = value; } }
        public JCS_Bool3 freezeRotation { get { return mFreezeRotation; } set { mFreezeRotation = value; } }
        public JCS_Bool3 freezeScale { get { return mFreezeScale; } set { mFreezeScale = value; } }
        public bool isLocalPosition
        {
            get { return mIsLocalPosition; }
            set
            {
                mIsLocalPosition = value;

                // get the new freeze position.
                if (mIsLocalPosition)
                    mPositionToFreeze = localPosition;
                else
                    mPositionToFreeze = position;
            }
        }
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

        /* Functions */

        private void Start()
        {
            // record down all the transform info value.
            if (mIsLocalPosition)
                mPositionToFreeze = localPosition;
            else
                mPositionToFreeze = position;

            if (mIsLocalRotation)
                mRotationToFreeze = localEulerAngles;
            else
                mRotationToFreeze = eulerAngles;

            mScaleToFreeze = localScale;
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
            Vector3 newPos = position;
            if (mIsLocalPosition)
                newPos = localPosition;

            /* Freeze position */
            if (mFreezePosition.check1)
                newPos.x = mPositionToFreeze.x;
            if (mFreezePosition.check2)
                newPos.y = mPositionToFreeze.y;
            if (mFreezePosition.check3)
                newPos.z = mPositionToFreeze.z;

            /* Apply new value to transform. */
            if (mIsLocalPosition)
                localPosition = newPos;
            else
                position = newPos;
        }

        /// <summary>
        /// Freeze position.
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

        /// <summary>
        /// Freeze position.
        /// </summary>
        private void DoFreezeScale()
        {
            Vector3 newScale = localScale;

            /* Freeze scale */
            if (mFreezeScale.check1)
                newScale.x = mScaleToFreeze.x;
            if (mFreezeScale.check2)
                newScale.y = mScaleToFreeze.y;
            if (mFreezeScale.check3)
                newScale.z = mScaleToFreeze.z;

            localScale = newScale;
        }
    }
}
