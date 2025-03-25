/**
 * $File: JCS_2DWalkAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Action does the AI walk action in 2D.
    /// </summary>
    [RequireComponent(typeof(JCS_CharacterControllerInfo))]
    [RequireComponent(typeof(JCS_VelocityInfo))]
    public class JCS_2DWalkAction : JCS_AIAction
    {
        /* Variables */

        public enum Status
        {
            IDLE = 0,
            LEFT = -1,
            RIGHT = 1
        };

        // Character Info so we can control the character better.
        private JCS_CharacterControllerInfo mCharacterControllerInfo = null;

        private JCS_VelocityInfo mVelocityInfo = null;

        [Separator("Runtime Varaibles (JCS_2DWalkAction)")]

        [Tooltip("Speed of the action.")]
        [SerializeField]
        private float mWalkSpeed = 10.0f;

        [Tooltip("Possibility to walk LEFT way.")]
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float mToLeft = 50.0f;

        [Tooltip("Possibility to walk RIGHT way.")]
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float mToRight = 50.0f;

        [Tooltip("Possibility to IDLE.")]
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float mToIdle = 50.0f;

        [Tooltip("Possiblity to active this action.")]
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float mPossibility = 80.0f;

        [Header("- Time")]

        [Tooltip("Time to do one walk.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mTimeZone = 2.0f;

        [Tooltip("Time that will randomly affect the Time Zone.")]
        [SerializeField]
        [Range(0.0f, 3.0f)]
        private float mAdjustTimeZone = 1.5f;

        // time to record down the real time to do one walk
        // action after we calculate the real time.
        private float mRealTimeZone = 0;

        // timer to do walk.
        private float mTimer = 0;

        // check to see if we can reset our time zone.
        private bool mWalked = false;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("- Action")]

        [Tooltip("Generate a random walk speed at the initilaize time.")]
        [SerializeField]
        private bool mStartRandomWalkSpeed = false;

        [Tooltip(@"Addition value to the walk speed. For
instance value 5, will generate -5 ~ 5 and add it on to current walk speed.")]
        [SerializeField]
        [Range(1, 10)]
        private float mRandomWalkSpeedRange = 5;

        [Header("- Track Effect")]

        [Tooltip("If get mad will start tracking the object that make this object mad.")]
        [SerializeField]
        private bool mMadEffect = true;

        [Tooltip("If Mad Effect is on this object is needed.")]
        [SerializeField]
        private JCS_AttackerRecorder mAttackRecorder = null;

        [Header("- Optional")]

        [Tooltip("Live object animation.")]
        [SerializeField]
        private JCS_2DLiveObjectAnimator mLiveObjectAnimator = null;

        /* Setter & Getter */

        public CharacterController GetCharacterController() { return this.mCharacterControllerInfo.GetCharacterController(); }
        public Vector3 Velocity { get { return this.mVelocityInfo.Velocity; } set { this.mVelocityInfo.Velocity = value; } }
        public float VelX { get { return mVelocityInfo.VelX; } set { mVelocityInfo.VelX = value; } }
        public float VelY { get { return mVelocityInfo.VelY; } set { mVelocityInfo.VelY = value; } }
        public float VelZ { get { return mVelocityInfo.VelZ; } set { mVelocityInfo.VelZ = value; } }
        public bool isGrounded { get { return this.mCharacterControllerInfo.isGrounded; } }
        public Vector3 MoveSpeed { get { return mVelocityInfo.MoveSpeed; } set { mVelocityInfo.MoveSpeed = value; } }
        public Vector3 RecordSpeed { get { return mVelocityInfo.RecordSpeed; } set { mVelocityInfo.RecordSpeed = value; } }
        public float RecordSpeedX { get { return this.mVelocityInfo.RecordSpeed.x; } set { this.mVelocityInfo.RecordSpeedX = value; } }
        public float RecordSpeedY { get { return this.mVelocityInfo.RecordSpeed.y; } set { this.mVelocityInfo.RecordSpeedY = value; } }
        public float RecordSpeedZ { get { return this.mVelocityInfo.RecordSpeed.z; } set { this.mVelocityInfo.RecordSpeedZ = value; } }
        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        // Track Effects
        public bool MadEffect { get { return this.mMadEffect; } set { this.mMadEffect = value; } }

        /* Functions */

        private void Awake()
        {
            mVelocityInfo = this.GetComponent<JCS_VelocityInfo>();
            mCharacterControllerInfo = this.GetComponent<JCS_CharacterControllerInfo>();

            // try to get the component in the same transform
            if (mAttackRecorder == null)
                mAttackRecorder = this.GetComponent<JCS_AttackerRecorder>();
            if (mLiveObjectAnimator == null)
                mLiveObjectAnimator = this.GetComponent<JCS_2DLiveObjectAnimator>();
        }

        private void Start()
        {
            if (mStartRandomWalkSpeed)
            {
                mWalkSpeed += JCS_Random.Range(-mRandomWalkSpeedRange, mRandomWalkSpeedRange);
            }
        }

        private void Update()
        {
            DoWalk();
        }

        /// <summary>
        /// Do the walk action depends on possibility.
        /// </summary>
        public void WalkByPossiblity()
        {
            float possibility = JCS_Random.Range(0, 100);

            // possibility check
            if (possibility > mPossibility)
                return;

            // start the algorithm to see if we find the direction to do,
            // if not it will just go randomly.
            WalkDirectionPossibility();
        }

        /// <summary>
        /// Recusive function that will find the direction
        /// and do the walk.
        /// </summary>
        public void WalkDirectionPossibility()
        {
            // direction we are going to use next.
            Status direction = Status.IDLE;


            // if is already get attack do the mad effect.
            if (mMadEffect && mAttackRecorder != null)
            {
                Transform lastAttacker = mAttackRecorder.LastAttacker;

                // if the last attacker does not exist, do nothing.
                if (lastAttacker != null)
                {
                    // if does exist, start following the attacker.
                    if (lastAttacker.position.x < this.transform.position.x)
                        direction = Status.LEFT;
                    else
                        direction = Status.RIGHT;

                    // do that direction and return.
                    WalkByStatus(direction);
                    return;
                }
            }

            // record down how what does success to go with.
            int resultCounter = 0;

            float leftPossiblity = JCS_Random.Range(0, 100);
            float idlePossiblity = JCS_Random.Range(0, 100);
            float rightPossiblity = JCS_Random.Range(0, 100);

            if (idlePossiblity < mToIdle)
            {
                // success to do idle
                direction = Status.IDLE;
                ++resultCounter;
            }

            if (leftPossiblity < mToLeft)
            {
                // success to do left
                direction = Status.LEFT;
                ++resultCounter;
            }

            if (rightPossiblity < mToRight)
            {
                // success to do right
                direction = Status.RIGHT;
                ++resultCounter;
            }

            // if there are multiple result do randomly
            if (resultCounter >= 2)
                WalkRandomly();
            // else if we successfully find the direction, use the direction
            // algorithm found.
            else
                WalkByStatus(direction);
        }

        /// <summary>
        /// Sometimes if the algorithm cannot find which direction to go.
        /// Just use the function instead of keep finding the direction.
        ///
        /// For example, all three possiblity (Idle, Left, Right)
        /// can be set to 100 percent. Than it will always have to
        /// possiblity of direction to go, which mean the object could
        /// not decide which direction to go.
        /// </summary>
        public void WalkRandomly()
        {
            int result = JCS_Random.Range(-1, 1 + 1);

            WalkByStatus(result);
        }

        /// <summary>
        /// Walk depends on the status.
        /// </summary>
        /// <param name="direction"></param>
        public void WalkByStatus(int direction)
        {
            WalkByStatus((Status)direction);
        }

        /// <summary>
        /// Walk depends on the status.
        /// </summary>
        /// <param name="status"></param>
        public void WalkByStatus(Status status)
        {
            switch (status)
            {
                case Status.IDLE:
                    {
                        Walk(0);        // stop walking

                        if (mLiveObjectAnimator != null)
                            mLiveObjectAnimator.DoAnimation(JCS_LiveObjectState.STAND);
                    }
                    break;

                case Status.LEFT:
                    {
                        Walk(JCS_Mathf.ToNegative(mWalkSpeed));

                        if (mLiveObjectAnimator != null)
                            mLiveObjectAnimator.DoAnimation(JCS_LiveObjectState.WALK);
                    }
                    break;

                case Status.RIGHT:
                    {
                        Walk(JCS_Mathf.ToPositive(mWalkSpeed));

                        if (mLiveObjectAnimator != null)
                            mLiveObjectAnimator.DoAnimation(JCS_LiveObjectState.WALK);
                    }
                    break;
            }
        }

        /// <summary>
        /// Walk in default speed.
        /// </summary>
        public void Walk()
        {
            Walk(mWalkSpeed);
        }

        /// <summary>
        /// Walk with pass in speed.
        /// </summary>
        /// <param name="speed"> walk speed </param>
        public void Walk(float speed)
        {
            // cannot change direction while in air.
            if (!isGrounded)
                return;

            RecordSpeedX = speed;

            mWalked = true;
        }

        /// <summary>
        /// Do the walk algorithm.
        /// </summary>
        private void DoWalk()
        {
            if (mWalked)
                ResetTimeZone();

            mTimer += JCS_Time.ItTime(mTimeType);

            if (mTimer < mRealTimeZone)
                return;

            WalkByPossiblity();
        }

        /// <summary>
        /// Algorithm to calculate the time to do walk action include direction.
        /// </summary>
        private void ResetTimeZone()
        {
            float adjustTime = JCS_Random.Range(-mAdjustTimeZone, mAdjustTimeZone);
            mRealTimeZone = mTimeZone + adjustTime;

            mWalked = false;
            mTimer = 0;
        }
    }
}
