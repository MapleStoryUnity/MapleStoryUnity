/**
 * $File: JCS_ShootAllAngle.cs $
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
    /// Action that shoot in all angle.
    /// </summary>
    [RequireComponent(typeof(JCS_ShootAction))]
    public class JCS_ShootAllAngle : MonoBehaviour, JCS_IAction
    {
        /* Variables */

        private JCS_ShootAction mShootAction = null;

        [Separator("📋 Check Variabless (JCS_ShootAllAngle)")]

        [Tooltip("Check if the enemy can shoot or not depends on the delay time!")]
        [SerializeField]
        [ReadOnly]
        private bool mCanShoot = true;

        [Separator("⚡️ Runtime Variables (JCS_ShootAllAngle)")]

        [Tooltip("Automatically shoot bullets itself in frame.")]
        [SerializeField]
        private bool mAutoShootInFrame = false;

        [Tooltip("Automatically shoot bullets itself in order.")]
        [SerializeField]
        private bool mAutoShootByOrder = false;

        [Tooltip("Degree per bullet shoot.")]
        [SerializeField]
        [Range(1.0f, 360.0f)]
        private float mDegreePerShoot = 10.0f;

        [Tooltip("How long it takes to shoot a bullet.")]
        [SerializeField]
        [Range(0.01f, 15.0f)]
        private float mDelayTime = 1.0f;

        [Tooltip("Time that will randomly affect the time.")]
        [SerializeField]
        [Range(0.0f, 3.0f)]
        private float mAdjustTime = 1.5f;

        [Tooltip("Axis the bullet shoots.")]
        [SerializeField]
        private JCS_Axis mShootAxis = JCS_Axis.AXIS_Z;

        private float mDelayTimer = 0.0f;

        private bool mShooted = false;

        private float mRealTime = 0.0f;

        private float mCount = 0.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public bool canShoot { get { return mCanShoot; } set { mCanShoot = value; } }
        public bool autoShootInFrame { get { return mAutoShootInFrame; } set { mAutoShootInFrame = value; } }
        public bool autoShootByOrder { get { return mAutoShootByOrder; } set { mAutoShootByOrder = value; } }
        public float degreePerShoot { get { return mDegreePerShoot; } set { mDegreePerShoot = value; } }
        public float delayTime { get { return mDelayTime; } set { mDelayTime = value; } }
        public float adjustTime { get { return mAdjustTime; } set { mAdjustTime = value; } }
        public JCS_Axis shootAxis { get { return mShootAxis; } set { mShootAxis = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        /* Functions */

        private void Awake()
        {
            mShootAction = GetComponent<JCS_ShootAction>();

            mShootAction.canShoot = false;

            // disable auto shoot be default
            mShootAction.autoShoot = false;
        }

        private void Update()
        {
            if (mAutoShootInFrame)
                AutoShootActionByFrame();

            if (mAutoShootByOrder)
                AutoShootActionByOrder();
        }

        /// <summary>
        /// Shoot bullets in all angle in current frame.
        /// </summary>
        public void ShootAllAngleByFrame()
        {
            Vector3 newRotation = transform.localEulerAngles;
            Vector3 recordRotation = newRotation;

            for (int count = 0; count < 360 / mDegreePerShoot; ++count)
            {
                switch (mShootAxis)
                {
                    case JCS_Axis.AXIS_X:
                        newRotation.x = mDegreePerShoot * count;
                        break;

                    case JCS_Axis.AXIS_Y:
                        newRotation.y = mDegreePerShoot * count;
                        break;

                    case JCS_Axis.AXIS_Z:
                        newRotation.z = mDegreePerShoot * count;
                        break;
                }

                transform.localEulerAngles = newRotation;

                mShootAction.Shoot();
            }
            transform.localEulerAngles = recordRotation;

            mShooted = true;
        }

        /// <summary>
        /// Shoot a bullet at a time in oder by degree.
        /// </summary>
        public void ShootAllAngleByOrder()
        {
            Vector3 newRotation = transform.localEulerAngles;
            Vector3 recordRotation = newRotation;

            switch (mShootAxis)
            {
                case JCS_Axis.AXIS_X:
                    newRotation.x = mDegreePerShoot * mCount;
                    break;

                case JCS_Axis.AXIS_Y:
                    newRotation.y = mDegreePerShoot * mCount;
                    break;

                case JCS_Axis.AXIS_Z:
                    newRotation.z = mDegreePerShoot * mCount;
                    break;
            }

            transform.localEulerAngles = newRotation;

            mShootAction.Shoot();

            ++mCount;

            transform.localEulerAngles = recordRotation;

            mShooted = true;
        }

        /// <summary>
        /// Automatically shoot bullets in all angle in current frame.
        /// </summary>
        private void AutoShootActionByFrame()
        {
            if (!mCanShoot)
                return;

            if (mShooted)
                RecalculateTimeAndResetTimer();

            mDelayTimer += JCS_Time.ItTime(mTimeType);

            if (mRealTime < mDelayTimer)
            {
                ShootAllAngleByFrame();

                // reset timer
                mDelayTimer = 0;
            }
        }

        /// <summary>
        /// Automatically shoot by the timer.
        /// </summary>
        private void AutoShootActionByOrder()
        {
            if (!mCanShoot)
                return;

            if (mShooted)
                RecalculateTimeAndResetTimer();

            mDelayTimer += JCS_Time.ItTime(mTimeType);

            if (mRealTime < mDelayTimer)
            {
                ShootAllAngleByOrder();

                // reset timer
                mDelayTimer = 0;
            }
        }

        /// <summary>
        /// Recalculate the time and reset the timer.
        /// </summary>
        private void RecalculateTimeAndResetTimer()
        {
            float adjustTime = JCS_Random.Range(-mAdjustTime, mAdjustTime);
            mRealTime = mDelayTime + adjustTime;

            mShooted = false;
            mDelayTimer = 0;
        }
    }
}
