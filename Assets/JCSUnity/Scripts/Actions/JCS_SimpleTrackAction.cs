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

        [Separator("Runtime Variables (JCS_SimpleTrackAction)")]

        [Tooltip("Target transform; if null use target position instead.")]
        [SerializeField]
        private Transform mTarget = null;

        [Tooltip("Target position to track.")]
        [SerializeField]
        private Vector3 mTargetPos = Vector3.zero;

        [Tooltip("How fast it moves toward to the target position?")]
        [SerializeField]
        [Range(0.01f, 10.0f)]
        private float mFriction = 0.2f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

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

        public Vector3 TargetPosition { get { return this.mTargetPos; } set { this.mTargetPos = value; } }
        public float Friction { get { return this.mFriction; } set { this.mFriction = value; } }
        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }
        public bool IgnoreX { get { return this.mIgnoreX; } set { this.mIgnoreX = value; } }
        public bool IgnoreY { get { return this.mIgnoreY; } set { this.mIgnoreY = value; } }
        public bool IgnoreZ { get { return this.mIgnoreZ; } set { this.mIgnoreZ = value; } }
        public bool LocalTarget { get { return this.mLocalTarget; } set { this.mLocalTarget = value; } }
        public bool LocalSelf { get { return this.mLocalSelf; } set { this.mLocalSelf = value; } }

        /* Functions */

        private void Update()
        {
            Vector3 newPos = mTargetPos;

            if (mTarget)
            {
                newPos = (mLocalTarget) ? mTarget.transform.localPosition : mTarget.transform.position;
            }

            if (mIgnoreX)
                newPos.x = (mLocalSelf) ? LocalPosition.x : Position.x;
            if (mIgnoreY)
                newPos.y = (mLocalSelf) ? LocalPosition.y : Position.y;
            if (mIgnoreZ)
                newPos.z = (mLocalSelf) ? LocalPosition.z : Position.z;

            float time = JCS_Time.DeltaTime(mDeltaTimeType);

            if (mLocalSelf)
                this.LocalPosition += (newPos - LocalPosition) / mFriction * time;
            else
                this.Position += (newPos - Position) / mFriction * time;
        }

        /// <summary>
        /// Change the target position on x axis by adding it.
        /// </summary>
        /// <param name="val">Value to add.</param>
        public void DeltaTargetPosX(float val)
        {
            mTargetPos = JCS_Util.IncVecX(mTargetPos, val);
        }

        /// <summary>
        /// Change the target position on y axis by adding it.
        /// </summary>
        /// <param name="val">Value to add.</param>
        public void DeltaTargetPosY(float val)
        {
            mTargetPos = JCS_Util.IncVecY(mTargetPos, val);
        }

        /// <summary>
        /// Change the target position on z axis by adding it.
        /// </summary>
        /// <param name="val">Value to add.</param>
        public void DeltaTargetPosZ(float val)
        {
            mTargetPos = JCS_Util.IncVecZ(mTargetPos, val);
        }
    }
}
