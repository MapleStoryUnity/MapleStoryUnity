/**
 * $File: JCS_2DSideScrollerPlayer.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Character controll of player
    /// </summary>
    [RequireComponent(typeof(JCS_2DSideScrollerPlayerAudioController))]
    [RequireComponent(typeof(JCS_2DAnimator))]
    [RequireComponent(typeof(JCS_OrderLayerObject))]
    public class JCS_2DSideScrollerPlayer : JCS_Player
    {
        /* Variables */

        protected JCS_2DAnimator mJCS2DAnimator = null;
        protected JCS_OrderLayerObject mOrderLayerObject = null;

        //-- Action (Ladder, Rope)
        [Separator("Check Variables (JCS_2DSideScrollerPlayer)")]

        //-- Facing
        [SerializeField]
        [ReadOnly]
        private JCS_2DFaceType mFace = JCS_2DFaceType.FACE_LEFT;

        //-- Climbing
        [Tooltip("See if there are ladder object so we can climb.")]
        [SerializeField]
        [ReadOnly]
        private bool mCanLadder = false;

        [Tooltip("See if there are rope object so we can climb.")]
        [SerializeField]
        [ReadOnly]
        private bool mCanRope = false;

        [Tooltip("Object that we can climb on.")]
        [SerializeField]
        private JCS_2DClimbableObject m2DClimbingObject = null;

        private bool mExitingClimbing = false;
        private bool mStartClimbing = false;

        [Tooltip("Count of the current jump.")]
        [SerializeField]
        [ReadOnly]
        private int mJumpCount = 0;

        [Tooltip("")]
        [SerializeField]
        [ReadOnly]
        private JCS_ClimbMoveType mClimbMoveType = JCS_ClimbMoveType.IDLE;

        [Separator("Runtime Variables (JCS_2DSideScrollerPlayer)")]

        [Tooltip("Character's status.")]
        [SerializeField]
        private JCS_2DCharacterState mCharacterState = JCS_2DCharacterState.NORMAL;

        //-- Jumping
        [Header("Jump")]

        [Tooltip("Type that this character can do.")]
        [SerializeField]
        private JCS_JumpeType mJumpType = JCS_JumpeType.BASIC_JUMP;

        private bool mDoubleJump = false;       // trigger to check
        private bool mTripleJump = false;       // trigger to check

        [Tooltip("Force apply when it jump. (Horizontal)")]
        [SerializeField]
        protected float[] mJumpYForces = null;

        [Tooltip("Force apply when it jump. (Vertical)")]
        [SerializeField]
        protected float[] mJumpXForces = null;

        [Tooltip("Maxinum speed of free fall object.")]
        [SerializeField]
        private float mTerminalSpeed = 20;

        [Tooltip("")]
        [SerializeField]
        private bool[] mForceXAfterJump = null;

        //-- Animator Control
        [Header("Animation Settings")]

        [Tooltip("Animation display when it jump event occurs.")]
        [SerializeField]
        private RuntimeAnimatorController[] mJumpAnim = null;

        [Tooltip("Animation sprite's offset for jump effect.")]
        [SerializeField]
        private Vector3[] mJumpAnimOffset = null;

        //-- Audio Control
        protected JCS_2DSideScrollerPlayerAudioController mAudioController = null;

        // once we call Physics.IgnoreCollision will call
        // OnTriggerEnter and OnTriggerExit once, 
        // to prevent this happen trigger this for checking.
        private bool mResetingCollision = false;

        //-- GENERAL
        [Header("Others")]

        [Tooltip("")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 10.0f)]
        private float mAirFriction = 0.5f;

        private bool mAttackedInAir = false;

        [Tooltip("Can the player down jump the platform.")]
        [SerializeField]
        private bool mCanDownJump = false;

        [Header("Control Key")]

        [Tooltip("Key press up.")]
        [SerializeField]
        private KeyCode mUpKey = KeyCode.UpArrow;
        [Tooltip("Key press down.")]
        [SerializeField]
        private KeyCode mDownKey = KeyCode.DownArrow;
        [Tooltip("Key press right.")]
        [SerializeField]
        private KeyCode mRightKey = KeyCode.RightArrow;
        [Tooltip("Key press left.")]
        [SerializeField]
        private KeyCode mLeftKey = KeyCode.LeftArrow;
        [Tooltip("Key press jump.")]
        [SerializeField]
        private KeyCode mJumpKey = KeyCode.LeftAlt;

        [Tooltip("Key press climb up.")]
        [SerializeField]
        private KeyCode mClimbUpKey = KeyCode.UpArrow;
        [Tooltip("Key press climb down.")]
        [SerializeField]
        private KeyCode mClimbDownKey = KeyCode.DownArrow;

        [Header("Climb")]

        [SerializeField]
        private bool mAutoClimb = false;

        [SerializeField]
        private JCS_ClimbMoveType mAutoClimbDirection = JCS_ClimbMoveType.MOVE_UP;

        [Tooltip("Force when u exit the climbing state.")]
        [SerializeField]
        private float mExitClimbForceY = 10;

        [Header("Hit")]

        [Tooltip("Trigger to enable hit effect.")]
        [SerializeField]
        private bool mHitEffect = true;

        [Tooltip("Velocity when get hit. (Horizontal) ")]
        [SerializeField]
        private float mHitVelX = 5;

        [Tooltip("Velocity when get hit. (Vertical) ")]
        [SerializeField]
        private float mHitVelY = 5;

        private bool mJustClimbOnTopOfBox = false;

        /* Setter & Getter */

        public bool justClimbOnTopOfBox { get { return mJustClimbOnTopOfBox; } set { mJustClimbOnTopOfBox = value; } }
        public bool autoClimb { get { return mAutoClimb; } set { mAutoClimb = value; } }
        public JCS_ClimbMoveType autoClimbDirection { get { return mAutoClimbDirection; } set { mAutoClimbDirection = value; } }
        public JCS_ClimbMoveType climbMoveType { get { return mClimbMoveType; } }
        public bool resetingCollision { get { return mResetingCollision; } set { mResetingCollision = value; } }
        public bool exitingClimbing { get { return mExitingClimbing; } set { mExitingClimbing = value; } }
        public JCS_2DCharacterState characterState { get { return mCharacterState; } set { mCharacterState = value; } }
        public bool canLadder { get { return mCanLadder; } set { mCanLadder = value; } }
        public bool canRope { get { return mCanRope; } set { mCanRope = value; } }
        public void SetJumpType(JCS_JumpeType type) { mJumpType = type; }
        public JCS_JumpeType GetJumpType() { return mJumpType; }
        public JCS_2DAnimator GetCharacterAnimator() { return mJCS2DAnimator; }
        public bool isGrounded() { return mCharacterController.isGrounded; }
        public JCS_2DSideScrollerPlayerAudioController GetAudioController() { return mAudioController; }
        public int jumpCount { get { return mJumpCount; } }
        public JCS_2DFaceType face { get { return mFace; } }
        public JCS_OrderLayerObject orderLayerObject { get { return mOrderLayerObject; } }

        public KeyCode upKey { get { return mUpKey; } set { mUpKey = value; } }
        public KeyCode downKey { get { return mDownKey; } set { mDownKey = value; } }
        public KeyCode rightKey { get { return mRightKey; } set { mRightKey = value; } }
        public KeyCode leftKey { get { return mLeftKey; } set { mLeftKey = value; } }
        public KeyCode jumpKey { get { return mJumpKey; } set { mJumpKey = value; } }
        public KeyCode climbUpKey { get { return mClimbUpKey; } set { mClimbUpKey = value; } }
        public KeyCode climbDownKey { get { return mClimbDownKey; } set { mClimbDownKey = value; } }

        public JCS_2DClimbableObject climbableObject { get { return m2DClimbingObject; } set { m2DClimbingObject = value; } }


        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            mAudioController = GetComponent<JCS_2DSideScrollerPlayerAudioController>();
            mJCS2DAnimator = GetComponent<JCS_2DAnimator>();
            mOrderLayerObject = GetComponent<JCS_OrderLayerObject>();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            ProcessState();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        public override void ControlEnable(bool act)
        {
            var gs = JCS_GameSettings.FirstInstance();

            // enable all the component here
            if (act)
            {
                if (gs.cameraType != JCS_CameraType.MULTI_TARGET)
                {
                    AudioListener al = mAudioController.GetAudioListener();
                    if (al != null)
                        al.enabled = true;
                }
                mIsControllable = true;
            }
            // diable all the component here
            else
            {
                if (gs.cameraType != JCS_CameraType.MULTI_TARGET)
                {
                    AudioListener al = mAudioController.GetAudioListener();
                    if (al != null)
                        al.enabled = false;
                }
                mIsControllable = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="facing"></param>
        public void TurnFace(int facing)
        {
            TurnFace((JCS_2DFaceType)facing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void TurnFace(JCS_2DFaceType type)
        {
            Vector3 originalScale = gameObject.transform.localScale;
            float absoluteOriginalScale = JCS_Mathf.AbsoluteValue(originalScale.x);
            gameObject.transform.localScale = new Vector3((int)type * absoluteOriginalScale, originalScale.y, originalScale.z);

            mFace = type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void Jump(JCS_JumpeType type)
        {
            switch (type)
            {
                case JCS_JumpeType.BASIC_JUMP:
                    BasicJump(mJumpYForces[0]);
                    break;
                case JCS_JumpeType.DOUBLE_JUMP:
                    DoubleJump(mJumpYForces[0], mJumpYForces[1]);
                    break;
                case JCS_JumpeType.TRIPLE_JUMP:
                    TripleJump(mJumpYForces[0], mJumpYForces[1], mJumpYForces[2]);
                    break;
                case JCS_JumpeType.DOUBLE_JUMP_FORCE:

                    break;
                case JCS_JumpeType.TRIPLE_JUMP_FORCE:

                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="force"></param>
        /// <returns></returns>
        public bool BasicJump(float force = 10)
        {
            if (characterState == JCS_2DCharacterState.CLIMBING)
                return false;

            if (mExitingClimbing)
                return false;

            if (!isGrounded())
                return false;

            if (!mIsControllable)
                return false;

            if (isInAttackStage())
                return false;

            velY = force;

            if (mForceXAfterJump[0])
                velX = (mJumpXForces[0] * -(int)mFace);

            mDoubleJump = false;
            mTripleJump = false;

            mAudioController.BasicJumpSound();

            DoJumpAnimEffect(0);

            ++mJumpCount;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstForce"></param>
        /// <param name="secodForce"></param>
        /// <returns></returns>
        public bool DoubleJump(float firstForce = 10, float secodForce = 10)
        {
            if (characterState == JCS_2DCharacterState.CLIMBING)
                return false;

            if (mExitingClimbing)
                return false;

            if (!mIsControllable)
                return false;

            if (mAttackedInAir)
                return false;

            if (isInAttackStage())
                return false;

            // Jump the first jump
            bool firstJump = BasicJump(firstForce);

            if (mJumpCount == 0 && !isGrounded())
                mDoubleJump = false;

            // check if do the second jump
            if (!firstJump && !mDoubleJump)
            {
                velY = secodForce;
                mDoubleJump = true;
                mAudioController.DoubleJumpSound();

                if (mForceXAfterJump[1])
                    velX = (mJumpXForces[1] * -(int)mFace);

                DoJumpAnimEffect(1);

                ++mJumpCount;

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstForce"></param>
        /// <param name="secodForce"></param>
        /// <param name="thirdForce"></param>
        /// <returns></returns>
        public bool TripleJump(float firstForce = 10, float secodForce = 10, float thirdForce = 10)
        {
            if (characterState == JCS_2DCharacterState.CLIMBING)
                return false;

            if (!mIsControllable)
                return false;

            if (mAttackedInAir)
                return false;

            if (isInAttackStage())
                return false;

            // get current double jump succeed or not
            bool currentDoubleJump = DoubleJump(firstForce, secodForce);

            if (mJumpCount == 1 && !isGrounded())
                mTripleJump = false;

            // if current double jump not succeed
            // if double jump succeed 
            if (!currentDoubleJump &&
                !mTripleJump &&
                mDoubleJump)
            {
                velY = thirdForce;
                mTripleJump = true;
                mAudioController.TripleJumpSound();

                if (mForceXAfterJump[2])
                    velX = (mJumpXForces[2] * -(int)mFace);

                DoJumpAnimEffect(2);

                ++mJumpCount;

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ToggleFace()
        {
            if (mFace == JCS_2DFaceType.FACE_LEFT)
                mFace = JCS_2DFaceType.FACE_RIGHT;
            else
                mFace = JCS_2DFaceType.FACE_LEFT;

            TurnFace((int)mFace);
        }

        /// <summary>
        /// Move right with defualt speed.
        /// </summary>
        public void MoveRight()
        {
            MoveRight(mMoveSpeed);
        }

        /// <summary>
        /// move right with pass in speed.
        /// </summary>
        /// <param name="vel"> speed </param>
        public void MoveRight(float vel)
        {
            // cannot move during climbing
            if (characterState == JCS_2DCharacterState.CLIMBING)
                return;

            if (!mIsControllable)
                return;

            bool isAttacking = isInAttackStage();

            if (isGrounded() || mExitingClimbing)
            {
                if (!isAttacking)
                    mVelocity.x = vel;
                mExitingClimbing = false;
            }
            // in air
            else
            {
                mVelocity.x += (0 - mVelocity.x) * mAirFriction * JCS_Time.ItTime(mTimeType);
            }

            if (isAttacking)
                return;

            TurnFace(JCS_2DFaceType.FACE_RIGHT);
            DoAnimation(JCS_LiveObjectState.WALK);
        }

        /// <summary>
        /// Move left with defualt speed.
        /// </summary>
        public void MoveLeft()
        {
            MoveLeft(mMoveSpeed);
        }

        /// <summary>
        /// Move left with pass in speed.
        /// </summary>
        /// <param name="vel"> speed </param>
        public void MoveLeft(float vel)
        {
            // cannot move during climbing
            if (characterState == JCS_2DCharacterState.CLIMBING)
                return;

            if (!mIsControllable)
                return;

            bool isAttacking = isInAttackStage();

            if (isGrounded() || mExitingClimbing)
            {
                if (!isAttacking)
                    mVelocity.x = -vel;
                mExitingClimbing = false;
            }
            // in air
            else
            {
                mVelocity.x += (0 - mVelocity.x) * mAirFriction * JCS_Time.ItTime(mTimeType);
            }

            if (isAttacking)
                return;

            TurnFace(JCS_2DFaceType.FACE_LEFT);
            DoAnimation(JCS_LiveObjectState.WALK);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Stand()
        {
            if (mStartClimbing)
                return;

            if (isInAttackStage())
                return;

            // In Air
            if (!isGrounded())
                return;

            DoAnimation(JCS_LiveObjectState.STAND);
            mVelocity.x = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Attack()
        {
            // cannot attack during climbing
            if (characterState == JCS_2DCharacterState.CLIMBING)
                return;

            if (!mIsControllable || isInAttackStage())
                return;

            if (isGrounded())
                mVelocity.x = 0;
            else
                mAttackedInAir = true;

            DoAnimation(JCS_LiveObjectState.RAND_ATTACK);
            mAudioController.AttackSound();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Jump()
        {
            if (IsDownJump())
                return;

            Jump(GetJumpType());
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Alert()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Ladder()
        {
            DoAnimation(JCS_LiveObjectState.LADDER);

            mVelocity.x = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Rope()
        {
            DoAnimation(JCS_LiveObjectState.ROPE);

            mVelocity.x = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Die()
        {
            mVelocity = Vector3.zero;

            DoAnimation(JCS_LiveObjectState.DIE);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dance()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Swim()
        {
            DoAnimation(JCS_LiveObjectState.SWIM);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Fly()
        {
            DoAnimation(JCS_LiveObjectState.FLY);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Sit()
        {

        }

        /// <summary>
        /// When player get hit.
        /// </summary>
        public override void Hit()
        {
            if (!mHitEffect)
            {
                Debug.LogError("You call the function without checking the hit effect?");
                return;
            }

            ExitClimbing(0);

            int randDirection = JCS_Random.Range(0, 2);        // 0 ~ 1

            // if 0 push right, else if 1 push left
            float pushVel = (randDirection == 0) ? mHitVelX : -mHitVelX;

            // apply force as velocity
            mVelocity.x += pushVel;

            // hop a bit. (velcotiy y axis)
            mVelocity.y += mHitVelY;


            DoAnimation(JCS_LiveObjectState.STAND);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Ghost()
        {

        }

        /// <summary>
        /// Climb and teleport are the same key.
        /// </summary>
        public virtual void ClimbOrTeleport()
        {
            if (isInAttackStage())
                return;

            // check the direction correctly or not.
            // 這個讓原本就在梯子上面的玩家不再往上爬!
            if (!CheckClimbDirection())
                return;

            // set mode to climbing.
            characterState = JCS_2DCharacterState.CLIMBING;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Prone()
        {
            // cannot prone in the air.
            if (!isGrounded() && velY != 0)
                return;

            /**
             * NOTE(jenchieh): i dont think these two return really effect
             * anything.
             */
            //if (mStartClimbing)
            //    return;

            //// cannot prone while climbing.
            //if (CharacterState == JCS_2DCharacterState.CLIMBING)
            //    return;

            if (canRope || canLadder)
            {
                if (CheckClimbDirection())
                {
                    characterState = JCS_2DCharacterState.CLIMBING;
                    return;
                }
            }

            DoAnimation(JCS_LiveObjectState.PRONE);

            mVelocity.x = 0;
        }

        /// <summary>
        /// Check if the player down jumping.
        /// </summary>
        /// <returns></returns>
        public bool IsDownJump()
        {
            if (!mCanDownJump)
                return false;

            if (JCS_Input.GetKey(jumpKey) && JCS_Input.GetKey(downKey))
                return true;

            return false;
        }

        /// <summary>
        /// Check if we are still in the attack stage.
        /// </summary>
        /// <returns>
        /// true: in attack stage.
        /// false: not in attack stage.
        /// </returns>
        protected bool isInAttackStage()
        {
            JCS_LiveObjectState lastState = (JCS_LiveObjectState)GetCharacterAnimator().currentAnimId;
            if (lastState == JCS_LiveObjectState.RAND_ATTACK &&
                !GetCharacterAnimator().currentAnimation.isDonePlaying)
                return true;

            return false;
        }

        /// <summary>
        /// Do the animation by passin the state.
        /// </summary>
        /// <param name="state"> state </param>
        /// <returns>
        /// true: play animation success.
        /// false: failed to play animation.
        /// </returns>
        protected bool DoAnimation(JCS_LiveObjectState state)
        {
            if (GetCharacterAnimator().currentAnimation != null)
            {
                if (GetCharacterAnimator().currentAnimId == (int)JCS_LiveObjectState.RAND_ATTACK)
                {
                    if (!GetCharacterAnimator().currentAnimation.isDonePlaying)
                        return false;
                }
            }

            if (state != JCS_LiveObjectState.RAND_ATTACK)
            {
                if (characterState != JCS_2DCharacterState.CLIMBING)
                {
                    JCS_LiveObjectState lastState = (JCS_LiveObjectState)GetCharacterAnimator().currentAnimId;

                    if (lastState == JCS_LiveObjectState.JUMP && !isGrounded())
                        return false;
                }
            }

            if (state == JCS_LiveObjectState.RAND_ATTACK)
                GetCharacterAnimator().PlayOneShot((int)JCS_LiveObjectState.RAND_ATTACK);
            else
                GetCharacterAnimator().DoAnimation((int)state);
            DoSound(state);

            return true;
        }

        /// <summary>
        /// Do the sound base on the state we are in.
        /// </summary>
        /// <param name="state"> state we are in. </param>
        protected void DoSound(JCS_LiveObjectState state)
        {
            GetAudioController().PlaySoundByPlayerState(state);
        }

        /// <summary>
        /// Process State Pattern.
        /// </summary>
        private void ProcessState()
        {
            switch (mCharacterState)
            {
                case JCS_2DCharacterState.NORMAL:
                    {
                        CharacterNormal();
                    }
                    break;
                case JCS_2DCharacterState.CLIMBING:
                    {
                        CharacterClimbing();
                    }
                    break;
                case JCS_2DCharacterState.ATTACK:
                    {

                    }
                    break;
            }
        }

        /// <summary>
        /// While Character is standing.
        /// </summary>
        private void CharacterNormal()
        {
            // if is in the AIR
            if (!isGrounded())
            {
                var gs = JCS_GameSettings.FirstInstance();

                // apply gravity
                mVelocity.y += (JCS_Physics.GRAVITY *
                    JCS_Time.ItTime(mTimeType) *
                    gs.gravityProduct);

                /* TODO!! */
                if (!mJustClimbOnTopOfBox)
                    DoAnimation(JCS_LiveObjectState.JUMP);

                // apply terminal speed
                if (mVelocity.y < -mTerminalSpeed)
                {
                    mVelocity.y = -mTerminalSpeed;
                }
            }
            // if is on the ground
            else
            {
                mJustClimbOnTopOfBox = false;

                Stand();
                mAttackedInAir = false;

                mJumpCount = 0;

                // if on the ground and is in attack stage, 
                // dont move the character in x-axis.
                if (isInAttackStage())
                    mVelocity.x = 0;
            }

            mStartClimbing = false;
        }

        /// <summary>
        /// While character are climbing.
        /// </summary>
        private void CharacterClimbing()
        {
            mJumpCount = 0;

            if (!mStartClimbing)
            {
                if (!mAutoClimb)
                {
                    if (!GetCharacterAnimator().IsInState((int)JCS_LiveObjectState.STAND) &&
                        !GetCharacterAnimator().IsInState((int)JCS_LiveObjectState.JUMP) &&
                        !GetCharacterAnimator().IsInState((int)JCS_LiveObjectState.WALK))
                    {
                        ExitClimbing(0);
                        return;
                    }
                }

                if (m2DClimbingObject == null)
                {
                    ExitClimbing(0);
                    return;
                }

                mStartClimbing = true;
            }

            if (canLadder)
                Ladder();
            else if (canRope)
                Rope();
            else
            {
                ExitClimbing(0);
                return;
            }

            // while climbing zero jump count.
            mJumpCount = 0;

            mClimbMoveType = JCS_ClimbMoveType.IDLE;

            if (mAutoClimb)
            {
                mClimbMoveType = mAutoClimbDirection;
            }
            else
            {
                // process input
                if (JCS_Input.GetKey(climbUpKey))
                    mClimbMoveType = JCS_ClimbMoveType.MOVE_UP;
                else if (JCS_Input.GetKey(mClimbDownKey))
                    mClimbMoveType = JCS_ClimbMoveType.MOVE_DOWN;
                else
                {
                    mClimbMoveType = JCS_ClimbMoveType.IDLE;
                    GetCharacterAnimator().StopAnimationInFrame();
                }
            }

            bool climbing = false;

            // process velocity
            switch (mClimbMoveType)
            {
                case JCS_ClimbMoveType.IDLE:
                    mVelocity.y = 0;
                    break;
                case JCS_ClimbMoveType.MOVE_UP:
                    mVelocity.y = mMoveSpeed;
                    climbing = true;
                    break;
                case JCS_ClimbMoveType.MOVE_DOWN:
                    mVelocity.y = -mMoveSpeed;
                    climbing = true;
                    break;
            }

            if (climbing)
            {
                if (m2DClimbingObject == null)
                {
                    ExitClimbing(0);
                    return;
                }

                // let x axis the same as ladder x axis.
                Vector3 newPos = transform.position;
                newPos.x = m2DClimbingObject.transform.position.x;
                transform.position = newPos;

                // start the animation agian
                GetCharacterAnimator().PlayAnimationInFrame();
            }
            else
            {
                if (JCS_Input.GetKey(jumpKey))
                {

                    if (JCS_Input.GetKey(leftKey) ||
                        JCS_Input.GetKey(rightKey))
                    {
                        ExitClimbing();
                        mExitingClimbing = true;
                    }
                }
            }

            if (isGrounded())
                ExitClimbing(0);
        }

        /// <summary>
        /// Spawn a animation for jumping effect.
        /// </summary>
        /// <param name="index"></param>
        private void DoJumpAnimEffect(int index)
        {
            if (mJumpAnim.Length < index)
                return;

            if (mJumpAnim.Length != mJumpAnimOffset.Length)
                return;

            if (mJumpAnim.Length == 0)
                return;

            if (mJumpAnim[index] == null)
                return;

            GameObject gameObject =
                JCS_Util.SpawnAnimateObjectDeathEvent(
                    mJumpAnim[index],
                    mOrderLayerObject.sortingOrder - 1);

            Vector3 newPos = Vector3.zero;
            Vector3 tempOffset = mJumpAnimOffset[index];

            // change the position depends on the scale.
            if (transform.localScale.x < 0)
                tempOffset.x = -tempOffset.x;

            // assign new position for the effect.
            newPos = transform.localPosition + tempOffset;

            // assign to effect's transform.
            gameObject.transform.localPosition = newPos;
            gameObject.transform.localScale = transform.localScale;
        }

        /// <summary>
        /// Call this when exit climbing status.
        /// </summary>
        private void ExitClimbing()
        {
            ExitClimbing(mExitClimbForceY);
        }
        /// <summary>
        /// Call this when exit climbing status.
        /// </summary>
        /// <param name="jumpForce"> force to exit the climbing object. </param>
        private void ExitClimbing(float jumpForce)
        {
            mCharacterState = JCS_2DCharacterState.NORMAL;

            if (jumpForce != 0)
            {
                // when exit give a litte jump
                /**
                 * ATTENSION(jenchieh): this make Unity's CharacterController
                 * component 'isGrounded' not accurate. Use this carefully.
                 */
                mVelocity.y = jumpForce;
            }

            // start the animation agian
            GetCharacterAnimator().PlayAnimationInFrame();

            if (jumpForce != 0)
            {
                canLadder = false;
                canRope = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CheckClimbDirection()
        {
            // no climbing object, no climbing!
            if (m2DClimbingObject == null)
                return false;

            if (m2DClimbingObject.IsOpTopOfLeanPlatform(this) && !JCS_Input.GetKey(mClimbDownKey))
                return false;

            // when is in air, character can climb what ever
            if (!isGrounded())
                return true;

            JCS_ClimbMoveType keyInput = JCS_ClimbMoveType.IDLE;

            // process input
            if (JCS_Input.GetKey(climbUpKey))
                keyInput = JCS_ClimbMoveType.MOVE_UP;
            else if (JCS_Input.GetKey(mClimbDownKey))
                keyInput = JCS_ClimbMoveType.MOVE_DOWN;


            switch (keyInput)
            {
                case JCS_ClimbMoveType.IDLE: return true;

                case JCS_ClimbMoveType.MOVE_UP:
                    {
                        if (transform.position.y < m2DClimbingObject.transform.position.y)
                            return true;
                    }
                    break;
                case JCS_ClimbMoveType.MOVE_DOWN:
                    {
                        if (transform.position.y > m2DClimbingObject.transform.position.y)
                            return true;
                    }
                    break;
            }

            return false;

        }
    }
}
