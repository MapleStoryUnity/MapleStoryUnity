/**
 * $File: JCS_OneShotObject.cs $
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
    /// One shot object.
    /// </summary>
    public class JCS_OneShotObject : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_OneShotObject)")]

        [Tooltip("Effect on which axis?")]
        [SerializeField]
        private JCS_Axis mAxis = JCS_Axis.AXIS_X;

        [Tooltip("Do the effect on awake event.")]
        [SerializeField]
        private bool mEffectOnAwake = true;

        [Tooltip("Loop one shot?")]
        [SerializeField]
        private bool mLoop = false;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Tooltip("Force in the last position.")]
        [SerializeField]
        private bool mStayAtLastPosition = false;

        [Tooltip("How fast it moves.")]
        [SerializeField]
        private float mMoveSpeed = 5.0f;

        [Tooltip("How far it moves.")]
        [SerializeField]
        private float mDistance = 100.0f;

        private bool mEffect = false;

        private Vector3 mRecordPosition = Vector3.zero;
        private float mDistanceRecorder = 0.0f;

        /* Setter & Getter */

        public JCS_Axis axis { get { return mAxis; } set { mAxis = value; } }
        public bool effectOnAwake { get { return mEffectOnAwake; } set { mEffectOnAwake = value; } }
        public bool loop { get { return mLoop; } set { mLoop = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }
        public bool stayAtLastPosition { get { return mStayAtLastPosition; } set { mStayAtLastPosition = value; } }
        public float moveSpeed { get { return mMoveSpeed; } set { mMoveSpeed = value; } }
        public float distance { get { return mDistance; } set { mDistance = value; } }

        /* Functions */

        private void Start()
        {
            mRecordPosition = transform.position;
            mEffect = mEffectOnAwake;

            if (mStayAtLastPosition)
                mLoop = false;
        }

        private void Update()
        {
            if (!mEffect)
                return;

            mDistanceRecorder += JCS_Mathf.AbsoluteValue(mMoveSpeed) * JCS_Time.ItTime(mTimeType);

            if (mDistanceRecorder > mDistance)
            {
                mDistanceRecorder = 0;

                if (!mStayAtLastPosition)
                    transform.position = mRecordPosition;

                // if is not loop disable the effect
                if (!mLoop)
                    mEffect = false;
            }


            DoMovement(mAxis, mMoveSpeed);

        }

        /// <summary>
        /// Play one shot the object.
        /// </summary>
        /// <param name="pos"> Target position. </param>
        public void PlayOneShot(Vector3 pos)
        {
            PlayOneShot(pos, mDistance);
        }
        /// <summary>
        /// Play one shot the object.
        /// </summary>
        /// <param name="pos"> Target position. </param>
        /// <param name="distance"> How far this one shot object moves. </param>
        public void PlayOneShot(Vector3 pos, float distance)
        {
            mRecordPosition = pos;
            mDistance = distance;
            mEffect = true;
        }
        
        /// <summary>
        /// Do the movement.
        /// </summary>
        /// <param name="ax"></param>
        /// <param name="speed"></param>
        private void DoMovement(JCS_Axis ax, float speed)
        {
            float dt = JCS_Time.ItTime(mTimeType);

            switch (ax)
            {
                case JCS_Axis.AXIS_X:
                    transform.position += new Vector3(speed, 0, 0) * dt;
                    break;
                case JCS_Axis.AXIS_Y:
                    transform.position += new Vector3(0, speed, 0) * dt;
                    break;
                case JCS_Axis.AXIS_Z:
                    transform.position += new Vector3(0, 0, speed) * dt;
                    break;
            }
        }
    }
}
