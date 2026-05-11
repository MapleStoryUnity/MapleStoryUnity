/**
 * $File: JCS_FreezeTransformAction.cs $
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
        private bool3 mFreezePosition = default;

        [Tooltip("Freeze the rotation in each axis.")]
        [SerializeField]
        private bool3 mFreezeRotation = default;

        [Tooltip("Freeze the scale in each axis.")]
        [SerializeField]
        private bool3 mFreezeScale = default;

        /* Setter & Getter */

        public bool active { get { return mActive; } set { mActive = value; } }
        public Vector3 positionToFreeze { get { return mPositionToFreeze; } set { mPositionToFreeze = value; } }
        public Vector3 rotationToFreeze { get { return mRotationToFreeze; } set { mRotationToFreeze = value; } }
        public Vector3 scaleToFreeze { get { return mScaleToFreeze; } set { mScaleToFreeze = value; } }
        public bool3 freezePosition { get { return mFreezePosition; } set { mFreezePosition = value; } }
        public bool3 freezeRotation { get { return mFreezeRotation; } set { mFreezeRotation = value; } }
        public bool3 freezeScale { get { return mFreezeScale; } set { mFreezeScale = value; } }
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
            if (mFreezePosition.x)
                newPos.x = mPositionToFreeze.x;
            if (mFreezePosition.y)
                newPos.y = mPositionToFreeze.y;
            if (mFreezePosition.z)
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
            if (mFreezeRotation.x)
                newRot.x = mRotationToFreeze.x;
            if (mFreezeRotation.y)
                newRot.y = mRotationToFreeze.y;
            if (mFreezeRotation.z)
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
