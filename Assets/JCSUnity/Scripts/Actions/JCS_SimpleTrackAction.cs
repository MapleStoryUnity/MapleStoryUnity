/**
 * $File: JCS_SimpleTrackAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// The action that moves toward a position.
    /// </summary>
    public class JCS_SimpleTrackAction : JCS_UnityObject
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_SimpleTrackAction) **")]

        [Tooltip("Target position to track.")]
        [SerializeField]
        private Vector3 mTargetPos = Vector3.zero;

        [Tooltip("How fast it moves toward to the target position?")]
        [SerializeField]
        [Range(0.01f, 10.0f)]
        private float mFriction = 0.2f;

        [Tooltip("Don't track on x-axis?")]
        [SerializeField]
        private bool mIgnoreX = false;

        [Tooltip("Don't track on y-axis?")]
        [SerializeField]
        private bool mIgnoreY = false;

        [Tooltip("Don't track on z-axis?")]
        [SerializeField]
        private bool mIgnoreZ = false;

        /* Setter & Getter */

        public Vector3 TargetPosition { get { return this.mTargetPos; } set { this.mTargetPos = value; } }
        public float Friction { get { return this.mFriction; } set { this.mFriction = value; } }
        public bool IgnoreX { get { return this.mIgnoreX; } set { this.mIgnoreX = value; } }
        public bool IgnoreY { get { return this.mIgnoreY; } set { this.mIgnoreY = value; } }
        public bool IgnoreZ { get { return this.mIgnoreZ; } set { this.mIgnoreZ = value; } }

        /* Functions */

        private void Update()
        {
            Vector3 tempTargetPost = mTargetPos;

            if (mIgnoreX)
                tempTargetPost.x = this.LocalPosition.x;
            if (mIgnoreY)
                tempTargetPost.y = this.LocalPosition.y;
            if (mIgnoreZ)
                tempTargetPost.z = this.LocalPosition.z;

            this.LocalPosition += (tempTargetPost - LocalPosition) / mFriction * Time.deltaTime;
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
