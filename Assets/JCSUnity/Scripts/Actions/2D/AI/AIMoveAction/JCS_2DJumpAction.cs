/**
 * $File: JCS_2DJumpAction.cs $
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
    /// Action does the AI jump action in 2D.
    /// </summary>
    [RequireComponent(typeof(JCS_CharacterControllerInfo))]
    [RequireComponent(typeof(JCS_VelocityInfo))]
    public class JCS_2DJumpAction : JCS_AIAction
    {
        /* Variables */

        // Character Info so we can control the character better.
        private JCS_CharacterControllerInfo mCharacterControllerInfo = null;

        private JCS_VelocityInfo mVelocityInfo = null;

        [Separator("Runtime Varaibles (JCS_2DJumpAction)")]

        [Tooltip("How much force to do one jump.")]
        [SerializeField]
        private float mJumpForce = 10.0f;

        [Tooltip("Possiblity to active this action.")]
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float mPossibility = 20.0f;

        [Tooltip("Time to do one jump.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mTime = 2.0f;

        [Tooltip("Time that will randomly affect the time.")]
        [SerializeField]
        [Range(0.0f, 3.0f)]
        private float mAdjustTime = 1.5f;

        // time to record down the real time to do one jump after we calculate
        // the real time.
        private float mRealTime = 0;

        // timer to do jump.
        private float mTimer = 0;

        // check to see if we can reset our time.
        private bool mJumped = false;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("- Action")]

        [Tooltip("Generate a random walk speed at the initilaize time.")]
        [SerializeField]
        private bool mStartRandomJumpForce = false;

        [Tooltip(@"Addition value to the jump force. For
instance value 5, will generate -5 ~ 5 and add it on to current jump force.")]
        [SerializeField]
        [Range(1.0f, 10.0f)]
        private float mRandomJumpForceRange = 5.0f;

        [Header("- Optional")]

        [Tooltip("Live object animation.")]
        // Animator in order to set the animation.
        [SerializeField]
        private JCS_2DLiveObjectAnimator mLiveObjectAnimator = null;

        // When the we jump, we start check to see if this object is grounded
        // or not, in order to go back and do the animation before we do jump
        // animation.
        private bool mCheckEndJumpAnimation = false;

        // record down the animation before jump, so we can keep on the same
        // animation after jump!
        private JCS_LiveObjectState mAnimStateBeforeJump = JCS_LiveObjectState.STAND;

        /* Setter & Getter */

        // Info Variables
        public CharacterController GetCharacterController() { return mCharacterControllerInfo.GetCharacterController(); }
        public Vector3 velocity { get { return mVelocityInfo.velocity; } set { mVelocityInfo.velocity = value; } }
        public float velX { get { return mVelocityInfo.velX; } set { mVelocityInfo.velX = value; } }
        public float velY { get { return mVelocityInfo.velY; } set { mVelocityInfo.velY = value; } }
        public float velZ { get { return mVelocityInfo.velZ; } set { mVelocityInfo.velZ = value; } }
        public bool isGrounded { get { return mCharacterControllerInfo.isGrounded; } }

        // Action Variables
        public float jumpForce { get { return mJumpForce; } set { mJumpForce = value; } }
        public float adjustTime { get { return mAdjustTime; } set { mAdjustTime = value; } }
        public float time { get { return mTime; } set { mTime = value; } }
        public float possibility { get { return mPossibility; } set { mPossibility = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        /* Functions */

        private void Awake()
        {
            mVelocityInfo = this.GetComponent<JCS_VelocityInfo>();
            mCharacterControllerInfo = this.GetComponent<JCS_CharacterControllerInfo>();

            // try to get the component in the same transform
            if (mLiveObjectAnimator == null)
                mLiveObjectAnimator = this.GetComponent<JCS_2DLiveObjectAnimator>();
        }

        private void Start()
        {
            if (mStartRandomJumpForce)
            {
                mJumpForce += JCS_Random.Range(-mRandomJumpForceRange, mRandomJumpForceRange);
            }
        }

        private void Update()
        {
            if (mCheckEndJumpAnimation)
            {
                if (isGrounded)
                {
                    // continue doing the animation state he left on.
                    mLiveObjectAnimator.DoAnimation(mAnimStateBeforeJump);
                    mCheckEndJumpAnimation = false;
                }
            }

            DoJump();
        }

        /// <summary>
        /// Do the jump by possibility.
        /// </summary>
        public void JumpByPossibility()
        {
            float possibility = JCS_Random.Range(0, 100);

            if (possibility > mPossibility)
                return;

            Jump();
        }

        /// <summary>
        /// Do the jump action.
        /// </summary>
        public void Jump()
        {
            Jump(mJumpForce);
        }
        public void Jump(float force)
        {
            // cannot double jump, to design a double jump plz create another
            // action class to handle the effect.
            if (!isGrounded)
                return;

            velY = force;

            // record down the animation before do jump animation
            mAnimStateBeforeJump = mLiveObjectAnimator.GetCurrentAnimationState();

            // do animation
            mLiveObjectAnimator.DoAnimation(JCS_LiveObjectState.JUMP);

            // next frame re-calculate the time.
            mJumped = true;

            // start check if grounded and end the jump animation.
            mCheckEndJumpAnimation = true;
        }

        /// <summary>
        /// Do jump implementation.
        /// </summary>
        /// <returns> true: do jump, flase: dont do jump </returns>
        private void DoJump()
        {
            if (mJumped)
                RecalculateTimeAndResetTimer();

            mTimer += JCS_Time.ItTime(mTimeType);

            if (mTimer < mRealTime)
                return;

            JumpByPossibility();
        }

        /// <summary>
        /// Recalculate the time and reset the timer.
        /// </summary>
        private void RecalculateTimeAndResetTimer()
        {
            float adjustTime = JCS_Random.Range(-mAdjustTime, mAdjustTime);
            mRealTime = mTime + adjustTime;

            mJumped = false;
            mTimer = 0;
        }
    }
}
