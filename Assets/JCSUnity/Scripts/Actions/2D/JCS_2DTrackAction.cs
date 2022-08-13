/**
 * $File: JCS_2DTrackAction.cs $
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
    /// Track a gameobject on 2D.
    /// </summary>
    public class JCS_2DTrackAction : MonoBehaviour , JCS_IAction
    {
        /* Variables */

        [Header("** Check Variables (JCS_2DTrackAction) **")]

        [SerializeField]
        private float mIndex = 0;

        [SerializeField]
        private int mOrderIndex = 0;

        [SerializeField]
        private bool mFollowing = true;

        [Header("** Runtime Variables (JCS_2DTrackAction) **")]

        [Tooltip("Transform we want to target.")]
        [SerializeField]
        private Transform mTargetTransform = null;

        [Tooltip("Which plane we want to move.")]
        [SerializeField]
        private JCS_Axis mAxis = JCS_Axis.AXIS_Z;

        private Vector3 mVelocity = Vector3.zero;

        //-- Smooth Track
        [Tooltip("Invers of speed, if smooth track is enable use this.")]
        [SerializeField]
        private float mMoveFriction = 0.2f;

        //-- Hard Track
        [Tooltip("If smooth track is disable use this.")]
        [SerializeField]
        private float mMoveSpeed = 10.0f;

        [Tooltip("Accept range on hard track.")]
        [SerializeField]
        private float mAccpetRange = 0.8f;

        [Tooltip("Hard track on x-axis.")]
        [SerializeField]
        private bool mHardOnX = false;

        [Tooltip("Hard track on y-axis.")]
        [SerializeField]
        private bool mHardOnY = false;

        [Tooltip("Hard track on z-axis.")]
        [SerializeField]
        private bool mHardOnZ = false;

        [Header("- Order")]

        [Tooltip("Set the track action order.")]
        [SerializeField]
        private bool mOrderEffect = false;

        [Tooltip("Gap between each order.")]
        [SerializeField]
        private Vector3 mGap = Vector3.zero;

        /* Setter & Getter */

        public Transform TargetTransform { get { return this.mTargetTransform; } set { this.mTargetTransform = value; } }
        public float MoveFriction { get { return this.mMoveFriction; } set { this.mMoveFriction = value; } }
        public float MoveSpeed { get { return this.mMoveSpeed; } set { this.mMoveSpeed = value; } }
        public float Index { get { return this.mIndex; } set { this.mIndex = value; } }
        public int OrderIndex { get { return this.mOrderIndex; } set { this.mOrderIndex = value; } }
        public bool Following { get { return this.mFollowing; } set { this.mFollowing = value; } }
        public bool HardOnX { get { return this.mHardOnX; } set { this.mHardOnX = value; } }
        public bool HardOnY { get { return this.mHardOnY; } set { this.mHardOnY = value; } }
        public bool HardOnZ { get { return this.mHardOnZ; } set { this.mHardOnZ = value; } }

        /* Functions */

        private void Update()
        {
            if (mTargetTransform == null)
                mFollowing = false;

            if (mFollowing)
                FollowObject();
            else
                KeepOnSameDirection();
        }

        /// <summary>
        /// Follow the target gameobject.
        /// </summary>
        private void FollowObject()
        {
            Vector3 targetPos = mTargetTransform.position;

            // use this effect you can shoot up to down!
            // if I set the index correctly!
            if (mOrderEffect)
                targetPos += mGap * mIndex;

            Vector3 newPos = this.transform.position;

            SmoothTrack(ref targetPos, ref newPos);
            HardTrack(ref targetPos, ref newPos);


           // apply force
            newPos += mVelocity * Time.deltaTime;

            this.transform.position = newPos;
        }

        /// <summary>
        /// Keep the same direction.
        /// </summary>
        private void KeepOnSameDirection()
        {
            this.transform.position += mVelocity * Time.deltaTime;
        }

        /// <summary>
        /// Smooth tracking.
        /// </summary>
        private void SmoothTrack(ref Vector3 targetPos, ref Vector3 newPos)
        {

            switch (mAxis)
            {
                case JCS_Axis.AXIS_X:
                    mVelocity.x = 0;
                    mVelocity.y = (targetPos.y - newPos.y) / mMoveFriction;
                    mVelocity.z = (targetPos.z - newPos.z) / mMoveFriction;
                    break;
                case JCS_Axis.AXIS_Y:

                    mVelocity.x = (targetPos.x - newPos.x) / mMoveFriction;
                    mVelocity.y = 0;
                    mVelocity.z = (targetPos.z - newPos.z) / mMoveFriction;
                    break;
                case JCS_Axis.AXIS_Z:
                    mVelocity.x = (targetPos.x - newPos.x) / mMoveFriction;
                    mVelocity.y = (targetPos.y - newPos.y) / mMoveFriction;
                    mVelocity.z = 0;
                    break;
            }
        }

        /// <summary>
        /// Hark tracking.
        /// </summary>
        private void HardTrack(ref Vector3 targetPos, ref Vector3 newPos)
        {
            if (mHardOnX)
            {
                if (targetPos.x < newPos.x - mAccpetRange)
                    mVelocity.x = -mMoveSpeed;
                else if (targetPos.x > newPos.x + mAccpetRange)
                    mVelocity.x = mMoveSpeed;
                else
                {
                    mVelocity.x = 0;
                }
            }

            if (mHardOnY)
            {
                if (targetPos.y < newPos.y - mAccpetRange)
                    mVelocity.y = -mMoveSpeed;
                else if (targetPos.y > newPos.y + mAccpetRange)
                    mVelocity.y = mMoveSpeed;
                else
                {
                    mVelocity.y = 0;
                }
            }

            if (mHardOnZ)
            {
                if (targetPos.z < newPos.z - mAccpetRange)
                    mVelocity.z = -mMoveSpeed;
                else if (targetPos.z > newPos.z + mAccpetRange)
                    mVelocity.z = mMoveSpeed;
                else
                {
                    mVelocity.z = 0;
                }
            }
        }
    }
}
