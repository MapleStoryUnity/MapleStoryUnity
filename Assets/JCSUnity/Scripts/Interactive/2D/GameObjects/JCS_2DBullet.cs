/**
 * $File: JCS_2DBullet.cs $
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
    /// For scripter and programmer,
    /// Override this if you want to implement this class
    /// </summary>
    [RequireComponent(typeof(JCS_3DGoStraightAction))]
    [RequireComponent(typeof(JCS_DestroyObjectWithTime))]
    [RequireComponent(typeof(JCS_HitCountEvent))]
    [RequireComponent(typeof(JCS_3DDestroyAnimEffect))]
    public class JCS_2DBullet : JCS_Bullet
    {
        /* Variables */

        private JCS_3DGoStraightAction mGoStraightAction = null;

        [Header("- Absorb Effect")]

        [Tooltip("Enable/Disable absorb effect.")]
        [SerializeField] private bool mAbsorbEffect = false;

        [Tooltip("Time to absorb the bullet.")]
        [SerializeField]
        [Range(0.01f, 5.0f)]
        private float mTimeToAbsorb = 0.25f;

        [Tooltip("How fast it absorb back.")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 10.0f)]
        private float mAbsorbBackFriction = 0.25f;

        [Tooltip("Randomize the time to absorb at init time.")]
        [SerializeField]
        [Range(0, 3.0f)]
        private float mRandomAbsorbTime = 0.5f;

        [Tooltip("Within this range is acceptable to active/deactive the effect.")]
        [SerializeField]
        [Range(0.1f, 1.0f)]
        private float mAcceptTimeRange = 0.8f;

        private float mAbsorbEffectTimer = 0;
        private float mRecordMoveSpeed = 0;

        [Header("- Degree Change")]

        [Tooltip("")]
        [SerializeField]
        private bool mDegreeChangeEffect = false;

        [Tooltip("Do this effect periodically?")]
        [SerializeField]
        private bool mContinousDegreeChange = false;

        [Tooltip("")]
        [SerializeField]
        [Range(0.1f, 10.0f)]
        private float mTimeToDegreeChange = 0.5f;

        // timer in order to know when to do the effect.
        private float mDegreeTimer = 0;

        [Tooltip("")]
        [SerializeField]
        private bool mRandDegreeX = false;
        [Tooltip("")]
        [SerializeField]
        [Range(0, 360)]
        private float mRandDegreeRangeX = 45f;

        [Tooltip("")]
        [SerializeField]
        private bool mRandDegreeY = false;
        [Tooltip("")]
        [SerializeField]
        [Range(0, 360)]
        private float mRandDegreeRangeY = 45f;

        [Tooltip("")]
        [SerializeField]
        private bool mRandDegreeZ = false;
        [Tooltip("")]
        [SerializeField]
        [Range(0, 360)]
        private float mRandDegreeRangeZ = 45f;

        [Header("- Init Look by Type Action")]

        [SerializeField]
        private bool mInitLookByTypeAction = false;

        [Tooltip("Do this effect periodically?")]
        [SerializeField]
        [Range(1, 50)]
        private int mContinousLookCount = 1;

        // counter see if read the count.
        private int mContinousLookCounter = 0;

        [Tooltip("Time until to do the effect or periodically.")]
        [SerializeField]
        [Range(0.1f, 10.0f)]
        private float mTimeToLook = 0.5f;

        [Tooltip("Time do add it to time to look.")]
        [SerializeField]
        [Range(0.0f, 3.0f)]
        private float mRandomTimeToLook = 0.5f;

        // timer in order to know when to do the effect.
        private float mLookTimer = 0;

        [Tooltip("If this effect is active plz attach.")]
        [SerializeField]
        private JCS_2DInitLookByTypeAction mInitLookAction = null;

        /* Setter & Getter */

        public override float moveSpeed
        {
            get
            {
                return mMoveSpeed;
            }

            set
            {
                mMoveSpeed = value;
                mGoStraightAction.moveSpeed = value;
                mRecordMoveSpeed = value;
            }
        }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            mGoStraightAction = this.GetComponent<JCS_3DGoStraightAction>();

            if (mRandomAbsorbTime != 0)
                mTimeToAbsorb += JCS_Random.Range(-mRandomAbsorbTime, mRandomAbsorbTime);

            if (mInitLookAction == null)
                mInitLookAction = this.GetComponent<JCS_2DInitLookByTypeAction>();

            if (mRandomTimeToLook != 0)
                mTimeToLook += JCS_Random.Range(-mRandomTimeToLook, mRandomTimeToLook);
        }

        private void Start()
        {
            // set all action to this move speed.
            mGoStraightAction.moveSpeed = mMoveSpeed;

            // record down the move speed
            mRecordMoveSpeed = mMoveSpeed;
        }

        private void Update()
        {
            DoAbsorbEffect();
            DoDegreeChangeEffect();
            DoInitLookByTypeEffect();
        }

        /// <summary>
        /// 
        /// </summary>
        private void DoAbsorbEffect()
        {
            float dt = JCS_Time.ItTime(mTimeType);

            if (mAbsorbEffect)
            {
                mAbsorbEffectTimer += dt;

                if (mTimeToAbsorb < mAbsorbEffectTimer)
                {
                    // start the effect
                    mGoStraightAction.moveSpeed += (0.1f - mGoStraightAction.moveSpeed) / mTimeToAbsorb * Time.deltaTime;

                    if (JCS_Util.WithInRange(-mAcceptTimeRange, mAcceptTimeRange, mGoStraightAction.moveSpeed))
                    {
                        //mGoStraightAction.MoveSpeed = mRecordMoveSpeed;

                        // end the effect.
                        mAbsorbEffect = false;
                    }
                }
            }
            else
            {
                mGoStraightAction.moveSpeed += (mRecordMoveSpeed - mGoStraightAction.moveSpeed) / mAbsorbBackFriction * dt;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DoDegreeChangeEffect()
        {
            if (!mDegreeChangeEffect)
                return;

            mDegreeTimer += JCS_Time.ItTime(mTimeType);

            if (mDegreeTimer < mTimeToDegreeChange)
                return;

            // prepare random
            Vector3 randVal = new Vector3(
                mRandDegreeRangeX,
                mRandDegreeRangeY,
                mRandDegreeRangeZ);

            JCS_Bool3 checkers = new JCS_Bool3(
                mRandDegreeX,
                mRandDegreeY,
                mRandDegreeZ);

            // apply it.
            transform.eulerAngles = JCS_Vector.ApplyRandVector3(transform.eulerAngles, randVal, checkers);

            // reset timer
            mDegreeTimer = 0;

            // conitnue the effect then dont disable the effect!
            if (!mContinousDegreeChange)
            {
                // end the effect!
                mDegreeChangeEffect = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DoInitLookByTypeEffect()
        {
            if (!mInitLookByTypeAction)
                return;

            if (mInitLookAction == null)
                return;

            mLookTimer += JCS_Time.ItTime(mTimeType);

            if (mLookTimer < mTimeToLook)
                return;

            // count once
            ++mContinousLookCounter;

            mInitLookAction.LockOnInit();

            // reset timer.
            mLookTimer = 0;

            // if is continous look counter reach the set
            // look count! Deactive the effect.
            if (mContinousLookCount == mContinousLookCounter)
            {
                // end the effect if not continous.
                mInitLookByTypeAction = false;
            }
        }
    }
}
