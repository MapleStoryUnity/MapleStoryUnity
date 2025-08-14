/**
 * $File: JCS_2DTrackAction.cs $
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
    /// Track a game object on 2D.
    /// </summary>
    public class JCS_2DTrackAction : MonoBehaviour , JCS_IAction
    {
        /* Variables */

        [Separator("Check Variables (JCS_2DTrackAction)")]

        [SerializeField]
        [ReadOnly]
        private float mIndex = 0;

        [SerializeField]
        [ReadOnly]
        private int mOrderIndex = 0;

        [SerializeField]
        [ReadOnly]
        private bool mFollowing = true;

        [Separator("Runtime Variables (JCS_2DTrackAction)")]

        [Tooltip("Transform we want to target.")]
        [SerializeField]
        private Transform mTargetTransform = null;

        [Tooltip("Which plane we want to move.")]
        [SerializeField]
        private JCS_Axis mAxis = JCS_Axis.AXIS_Z;

        private Vector3 mVelocity = Vector3.zero;

        //-- Smooth Track
        [Tooltip("Invers of speed, if smooth track is enable use ")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 10.0f)]
        private float mMoveFriction = 0.2f;

        //-- Hard Track
        [Tooltip("If smooth track is disable use ")]
        [SerializeField]
        private float mMoveSpeed = 10.0f;

        [Tooltip("Accept range on hard track.")]
        [SerializeField]
        private float mAccpetRange = 0.8f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

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

        public Transform targetTransform { get { return mTargetTransform; } set { mTargetTransform = value; } }
        public float moveFriction { get { return mMoveFriction; } set { mMoveFriction = value; } }
        public float moveSpeed { get { return mMoveSpeed; } set { mMoveSpeed = value; } }
        public float index { get { return mIndex; } set { mIndex = value; } }
        public int orderIndex { get { return mOrderIndex; } set { mOrderIndex = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }
        public bool following { get { return mFollowing; } set { mFollowing = value; } }
        public bool hardOnX { get { return mHardOnX; } set { mHardOnX = value; } }
        public bool hardOnY { get { return mHardOnY; } set { mHardOnY = value; } }
        public bool hardOnZ { get { return mHardOnZ; } set { mHardOnZ = value; } }

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
        /// Follow the target game object.
        /// </summary>
        private void FollowObject()
        {
            Vector3 targetPos = mTargetTransform.position;

            // use this effect you can shoot up to down!
            // if I set the index correctly!
            if (mOrderEffect)
                targetPos += mGap * mIndex;

            Vector3 newPos = transform.position;

            SmoothTrack(ref targetPos, ref newPos);
            HardTrack(ref targetPos, ref newPos);


           // apply force
            newPos += mVelocity * JCS_Time.ItTime(mTimeType);

            transform.position = newPos;
        }

        /// <summary>
        /// Keep the same direction.
        /// </summary>
        private void KeepOnSameDirection()
        {
            transform.position += mVelocity * JCS_Time.ItTime(mTimeType);
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
