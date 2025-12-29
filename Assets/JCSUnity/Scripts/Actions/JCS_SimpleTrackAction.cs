/**
 * $File: JCS_SimpleTrackAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// The action that moves toward a position.
    /// </summary>
    public class JCS_SimpleTrackAction : JCS_UnityObject
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_SimpleTrackAction)")]

        [Tooltip("Target transform; if null use target position instead.")]
        [SerializeField]
        private Transform mTarget = null;

        [Tooltip("Target position to track.")]
        [SerializeField]
        private Vector3 mTargetPos = Vector3.zero;

        [Tooltip("How fast it moves toward to the target position?")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 10.0f)]
        private float mFriction = 0.2f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Tooltip("Don't track on x-axis?")]
        [SerializeField]
        private bool mIgnoreX = false;

        [Tooltip("Don't track on y-axis?")]
        [SerializeField]
        private bool mIgnoreY = false;

        [Tooltip("Don't track on z-axis?")]
        [SerializeField]
        private bool mIgnoreZ = false;

        [Tooltip("Use local variables for target instead.")]
        [SerializeField]
        private bool mLocalTarget = false;

        [Tooltip("Use local variables for self instead.")]
        [SerializeField]
        private bool mLocalSelf = false;

        /* Setter & Getter */

        public Vector3 targetPosition { get { return mTargetPos; } set { mTargetPos = value; } }
        public float friction { get { return mFriction; } set { mFriction = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }
        public bool ignoreX { get { return mIgnoreX; } set { mIgnoreX = value; } }
        public bool ignoreY { get { return mIgnoreY; } set { mIgnoreY = value; } }
        public bool ignoreZ { get { return mIgnoreZ; } set { mIgnoreZ = value; } }
        public bool localTarget { get { return mLocalTarget; } set { mLocalTarget = value; } }
        public bool localSelf { get { return mLocalSelf; } set { mLocalSelf = value; } }

        /* Functions */

        private void Update()
        {
            Vector3 newPos = mTargetPos;

            if (mTarget)
            {
                newPos = (mLocalTarget) ? mTarget.transform.localPosition : mTarget.transform.position;
            }

            if (mIgnoreX)
                newPos.x = (mLocalSelf) ? localPosition.x : position.x;
            if (mIgnoreY)
                newPos.y = (mLocalSelf) ? localPosition.y : position.y;
            if (mIgnoreZ)
                newPos.z = (mLocalSelf) ? localPosition.z : position.z;

            float time = JCS_Time.ItTime(mTimeType);

            if (mLocalSelf)
                localPosition += (newPos - localPosition) / mFriction * time;
            else
                position += (newPos - position) / mFriction * time;
        }

        /// <summary>
        /// Change the target position on x axis by adding it.
        /// </summary>
        /// <param name="val">Value to add.</param>
        public void DeltaTargetPosX(float val)
        {
            mTargetPos = JCS_Vector.IncVecX(mTargetPos, val);
        }

        /// <summary>
        /// Change the target position on y axis by adding it.
        /// </summary>
        /// <param name="val">Value to add.</param>
        public void DeltaTargetPosY(float val)
        {
            mTargetPos = JCS_Vector.IncVecY(mTargetPos, val);
        }

        /// <summary>
        /// Change the target position on z axis by adding it.
        /// </summary>
        /// <param name="val">Value to add.</param>
        public void DeltaTargetPosZ(float val)
        {
            mTargetPos = JCS_Vector.IncVecZ(mTargetPos, val);
        }
    }
}
