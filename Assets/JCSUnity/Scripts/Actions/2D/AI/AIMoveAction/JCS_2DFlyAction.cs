/**
 * $File: JCS_2DFlyAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Action does the AI fly action on 2D.
    /// </summary>
    [RequireComponent(typeof(JCS_CharacterControllerInfo))]
    [RequireComponent(typeof(JCS_VelocityInfo))]
    public class JCS_2DFlyAction : JCS_AIAction
    {
        /* Variables */

        public enum StatusX
        {
            IDLE = 0,

            LEFT = -1,
            RIGHT = 1
        };

        public enum StatusY
        {
            IDLE = 0,

            DOWN = -1,
            UP = 1,
        };

        private JCS_VelocityInfo mVelocityInfo = null;
        private JCS_CharacterControllerInfo mCharacterControllerInfo = null;

        [Header("** Runtime Varaibles (JCS_2DFlyAction) **")]

        [Tooltip("Speed of flying on x-axis.")]
        [SerializeField]
        private float mFlyForceX = 10.0f;

        [Tooltip("Speed of flying on y-axis.")]
        [SerializeField]
        private float mFlyForceY = 10.0f;

        [Header("- Activate")]

        [Tooltip("Possiblity of going UP.")]
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float mToUp = 50.0f;

        [Tooltip("Possiblity of going DOWN.")]
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float mToDown = 50.0f;

        [Tooltip("Possiblity of going LEFT.")]
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float mToLeft = 50.0f;

        [Tooltip("Possiblity of going RIGHT.")]
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float mToRight = 50.0f;

        [Tooltip("Possibility to IDLE in horizontal direction.")]
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float mToIdleHorizontal = 50.0f;

        [Tooltip("Possibility to IDLE in vertical direction.")]
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float mToIdleVetical = 50.0f;

        [Tooltip("Possiblity to active this action.")]
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float mPossibility = 80.0f;

        [Header("** Time Settings (JCS_2DFlyAction) **")]

        [Tooltip("Time to do one Fly.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mTimeZone = 2.0f;

        [Tooltip("Time that will randomly affect the Time Zone.")]
        [SerializeField]
        [Range(0.0f, 3.0f)]
        private float mAdjustTimeZone = 1.5f;

        // time to record down the real time to do one fly
        // action after we calculate the real time.
        private float mRealTimeZone = 0.0f;

        // timer to do fly.
        private float mTimer = 0.0f;

        // check to see if we can reset our time zone.
        private bool mFlyed = false;

        [Header("- Space Limitation")]

        [Tooltip("Lowest height the object can go.")]
        [SerializeField]
        [Range(-1000.0f, 1000.0f)]
        private float mMinHeight = -500.0f;

        [Tooltip("Highest height the object can go.")]
        [SerializeField]
        [Range(-1000.0f, 1000.0f)]
        private float mMaxHeight = 500.0f;

        [Header("- Track")]

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

        [Tooltip(@"Check this to make the object ignore all the platform at initialize time.")]
        [SerializeField]
        private bool mIgnorePlatform = true;

        [Header("- Sound")]

        [Tooltip("Sound while flying.")]
        [SerializeField]
        private AudioClip mFlySound = null;

        [Tooltip("Sound player to play sounds.")]
        [SerializeField]
        private JCS_SoundPlayer mSoundPlayer = null;

        /* Setter & Getter */

        public CharacterController GetCharacterController() { return this.mCharacterControllerInfo.GetCharacterController(); }
        public Vector3 Velocity { get { return this.mVelocityInfo.Velocity; } set { this.mVelocityInfo.Velocity = value; } }
        public float VelX { get { return mVelocityInfo.VelX; } set { mVelocityInfo.VelX = value; } }
        public float VelY { get { return mVelocityInfo.VelY; } set { mVelocityInfo.VelY = value; } }
        public float VelZ { get { return mVelocityInfo.VelZ; } set { mVelocityInfo.VelZ = value; } }
        public bool isGrounded { get { return this.mCharacterControllerInfo.isGrounded; } }
        public Vector3 MoveSpeed { get { return mVelocityInfo.MoveSpeed; } set { mVelocityInfo.MoveSpeed = value; } }
        public Vector3 RecordSpeed { get { return mVelocityInfo.RecordSpeed; } set { mVelocityInfo.RecordSpeed = value; } }

        public bool MadEffect { get { return this.mMadEffect; } set { this.mMadEffect = value; } }

        /* Functions */

        private void Awake()
        {
            this.mVelocityInfo = this.GetComponent<JCS_VelocityInfo>();
            this.mCharacterControllerInfo = this.GetComponent<JCS_CharacterControllerInfo>();

            // try to get the sound
            if (mSoundPlayer == null)
                this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            // try to get the component in the same transform
            if (mAttackRecorder == null)
                mAttackRecorder = this.GetComponent<JCS_AttackerRecorder>();
            if (mLiveObjectAnimator == null)
                mLiveObjectAnimator = this.GetComponent<JCS_2DLiveObjectAnimator>();
        }

        private void Start()
        {
            PlayFlySound();

            // ignore all the tags.
            JCS_2DFlyActionIgnore[] tags = Resources.FindObjectsOfTypeAll<JCS_2DFlyActionIgnore>();
            foreach (JCS_2DFlyActionIgnore tag in tags)
            {
                Physics.IgnoreCollision(tag.GetComponent<Collider>(),
                    this.GetCharacterController());
            }

            // ignore all platforms
            if (mIgnorePlatform)
            {
                // get all the paltform in the scene
                JCS_2DPositionPlatform[] platforms = Resources.FindObjectsOfTypeAll<JCS_2DPositionPlatform>();
                foreach (JCS_2DPositionPlatform platform in platforms)
                {
                    if (platform.CannotBeGoThrough)
                        return;

                    Physics.IgnoreCollision(platform.GetPlatformCollider(),
                        this.GetCharacterController());
                    Physics.IgnoreCollision(platform.GetPlatformTrigger(),
                        this.GetCharacterController());
                }
            }
        }

        private void Update()
        {
            DoFly();
        }

        private void LateUpdate()
        {
            // check after limit,
            // so before rendering will fix the position.
            SpaceLimitCheck();
        }

        /// <summary>
        /// Calculate the possiblity and see if do the fly action.
        /// </summary>
        public void FlyByPossiblity()
        {
            float possibility = JCS_Random.Range(0, 100);

            // possibility check
            if (possibility > mPossibility)
                return;

            // start the algorithm to see if we
            // find the direction to do,
            // if not it will just go randomly.
            FlyDirectionByPossiblity();
        }

        /// <summary>
        /// Fly to a direction base on possibilities.
        /// </summary>
        public void FlyDirectionByPossiblity()
        {
            // direction we are going to use next.
            StatusX directionX = StatusX.IDLE;
            StatusY directionY = StatusY.IDLE;

            // if is already get attack do the mad effect.
            if (mMadEffect && mAttackRecorder != null)
            {
                Transform lastAttacker = mAttackRecorder.LastAttacker;

                // if the last attacker does not exist,
                // do nothing.
                if (lastAttacker != null)
                {
                    // NOTE(JenChieh): if does exist, start
                    // following the attacker.

                    // X-axis
                    if (lastAttacker.position.x < this.transform.position.x)
                        directionX = StatusX.LEFT;
                    else
                        directionX = StatusX.RIGHT;

                    // Y-axis
                    if (lastAttacker.position.y < this.transform.position.y)
                        directionY = StatusY.DOWN;
                    else
                        directionY = StatusY.UP;

                    // do that direction and return.
                    FlyByStatus(directionX, directionY);
                    return;
                }
            }

            // record down how what does success to go with.
            int resultCounterX = 0;
            int resultCounterY = 0;

            float idlePossiblityX = JCS_Random.Range(0, 100);
            float leftPossiblity = JCS_Random.Range(0, 100);
            float rightPossiblity = JCS_Random.Range(0, 100);

            if (idlePossiblityX < mToIdleVetical)
            {
                // success to do idle in x axis
                directionX = StatusX.IDLE;
                ++resultCounterX;
            }

            if (leftPossiblity < mToLeft)
            {
                // success to do left
                directionX = StatusX.LEFT;
                ++resultCounterX;
            }

            if (rightPossiblity < mToRight)
            {
                // success to do right
                directionX = StatusX.RIGHT;
                ++resultCounterX;
            }


            float idlePossiblityY = JCS_Random.Range(0, 100);
            float upPossiblity = JCS_Random.Range(0, 100);
            float downPossiblity = JCS_Random.Range(0, 100);

            if (idlePossiblityY < mToIdleHorizontal)
            {
                // success to do idle in y axis
                directionY = StatusY.IDLE;
                ++resultCounterY;
            }

            if (downPossiblity < mToDown)
            {
                // success to do left
                directionY = StatusY.DOWN;
                ++resultCounterY;
            }

            if (upPossiblity < mToUp)
            {
                // success to do right
                directionY = StatusY.UP;
                ++resultCounterY;
            }

            // if there are multiple result do randomly
            if (resultCounterX >= 2 &&
                resultCounterY >= 2)
                FlyRandomly();
            // else if we successfully find the direction,
            // use the direction algorithm found.
            else
                FlyByStatus(directionX, directionY);
        }

        /// <summary>
        /// Calculate the result x and y axis by pass passing
        /// -1, 0, 1 in order to process Fly Action base on Status
        /// enum class.
        /// </summary>
        public void FlyRandomly()
        {
            // get the result -1 ~ 1
            int resultX = JCS_Random.Range(-1, 1 + 1);
            int resultY = JCS_Random.Range(-1, 1 + 1);

            // Porcess by status!
            // 0 idle, -1 left/down, 1 right/up
            FlyByStatus(resultX, resultY);
        }

        /// <summary>
        /// Process velocity and animation by
        /// passing the status. (Integer)
        /// </summary>
        /// <param name="statusX"> status in x axis </param>
        /// <param name="statusY"> status in y axis </param>
        public void FlyByStatus(int statusX, int statusY)
        {
            FlyByStatus((StatusX)statusX, (StatusY)statusY);
        }
        /// <summary>
        /// Process velocity and animation by
        /// passing the status. (Enum)
        /// </summary>
        /// <param name="statusX"> status in x axis </param>
        /// <param name="statusY"> status in y axis </param>
        public void FlyByStatus(StatusX statusX, StatusY statusY)
        {
            // process vertical axis (x-axis)
            switch (statusX)
            {
                case StatusX.IDLE:
                    FlyX(0);
                    break;
                case StatusX.LEFT:
                    FlyX(JCS_Mathf.ToNegative(mFlyForceX));
                    break;
                case StatusX.RIGHT:
                    FlyX(JCS_Mathf.ToPositive(mFlyForceX));
                    break;
            }

            // process horizontal axis (y-axis)
            switch (statusY)
            {
                case StatusY.IDLE:
                    FlyY(0);
                    break;
                case StatusY.DOWN:
                    FlyY(JCS_Mathf.ToNegative(mFlyForceY));
                    break;
                case StatusY.UP:
                    FlyY(JCS_Mathf.ToPositive(mFlyForceY));
                    break;
            }
        }


        /// <summary>
        /// Fly on x-axis.
        /// </summary>
        public void FlyX()
        {
            FlyX(mFlyForceX);
        }
        /// <summary>
        /// Fly on x-axis.
        /// </summary>
        /// <param name="force"> foce to apply </param>
        public void FlyX(float force)
        {
            mVelocityInfo.RecordSpeedX = force;

            mFlyed = true;
        }

        /// <summary>
        /// Fly on y-axis.
        /// </summary>
        public void FlyY()
        {
            FlyY(mFlyForceY);
        }
        /// <summary>
        /// Fly on y-axis.
        /// </summary>
        /// <param name="force"> foce to apply </param>
        public void FlyY(float force)
        {
            mVelocityInfo.VelY = force;

            mFlyed = true;
        }

        /// <summary>
        /// Do the fly algorithmns
        /// </summary>
        private void DoFly()
        {
            if (mFlyed)
                ResetTimeZone();

            mTimer += Time.deltaTime;

            if (mTimer < mRealTimeZone)
                return;

            FlyByPossiblity();
        }

        /// <summary>
        /// Algorithm to calculate the time to do
        /// fly action include direction.
        /// </summary>
        private void ResetTimeZone()
        {
            float adjustTime = JCS_Random.Range(-mAdjustTimeZone, mAdjustTimeZone);
            mRealTimeZone = mTimeZone + adjustTime;

            mFlyed = false;
            mTimer = 0;
        }

        /// <summary>
        /// If the object out of space we set,
        /// limit it. (Let the object not go anywhere.)
        /// </summary>
        private void SpaceLimitCheck()
        {
            Vector3 newPos = this.transform.position;

            if (this.transform.position.y < mMinHeight)
                newPos.y = mMinHeight;

            if (this.transform.position.y > mMaxHeight)
                newPos.y = mMaxHeight;

            this.transform.position = newPos;
        }

        /// <summary>
        /// Sound Settings stuff,
        /// Play the flying sound and loop the sound.
        /// </summary>
        private void PlayFlySound()
        {
            // play fly sound,
            // fly sound should be looped.
            if (mSoundPlayer != null)
            {
                mSoundPlayer.GetAudioSource().loop = true;
                mSoundPlayer.GetAudioSource().clip = mFlySound;
                mSoundPlayer.GetAudioSource().Play();
            }
        }

    }
}
