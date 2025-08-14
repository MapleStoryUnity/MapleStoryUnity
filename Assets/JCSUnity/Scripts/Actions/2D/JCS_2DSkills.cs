/**
 * $File: JCS_2DSkills.cs $
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
    /// Do not use add this script directly to Unity.
    /// This is for Scripter only file.
    /// </summary>
    public abstract class JCS_2DSkills : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_2DSkills)")]

        [Tooltip("While by pressing this key active skill.")]
        [SerializeField]
        protected KeyCode mKeyCode = KeyCode.None;

        [Tooltip("Main animation player 1.")]
        [SerializeField]
        protected RuntimeAnimatorController mMainAnim1 = null;

        [Tooltip("Main animation player 2.")]
        [SerializeField]
        protected RuntimeAnimatorController mMainAnim2 = null;

        [Tooltip("Animation position offset 1.")]
        [SerializeField]
        protected Vector3 mAnimPosOffset1 = Vector3.zero;

        [Tooltip("Animation position offset 2.")]
        [SerializeField]
        protected Vector3 mAnimPosOffset2 = Vector3.zero;

        [Tooltip("Sprite render layer.")]
        [SerializeField]
        protected int mOrderLayer = 15;

        [Tooltip("animate action type displayed in the skills.")]
        [SerializeField]
        protected JCS_2DAnimActionType mAnimType = JCS_2DAnimActionType.DESTROY;

        [Tooltip("Stay with the parent game object.")]
        [SerializeField]
        protected bool mStayWithActiveTarget = true;

        [Tooltip("Time before activates the action.")]
        [SerializeField]
        protected float mTimeToActive = 0.0f;

        protected bool mAction = false;           // trigger action.

        [Tooltip("Delay time after activates this skills.")]
        [SerializeField]
        protected float mActiveTime = 0.0f;

        protected bool mAfterDelay = false;       // trigger after delay.

        protected float mActionTimer = 0;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        protected JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("- Spawn")]

        [Tooltip("Spawn the same position as this game object.")]
        [SerializeField]
        protected bool mSamePosition = true;

        [Tooltip("Spawn the same rotation as this game object.")]
        [SerializeField]
        protected bool mSameRotation = true;

        [Tooltip("Spawn the same scale as this game object.")]
        [SerializeField]
        protected bool mSameScale = true;

        [Header("- Sound")]

        [Tooltip("Optional sound player for 3D sounds calculation.")]
        [SerializeField]
        protected JCS_SoundPlayer mSoundPlayer = null;

        [Tooltip("Sound when plays this skill.")]
        [SerializeField]
        protected AudioClip mUseSound = null;

        [Tooltip("Sound when this skill hit the game object.")]
        [SerializeField]
        protected AudioClip mHitSound = null;

        /* Setter & Getter */

        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        /* Functions */

        protected virtual void Awake()
        {
            if (mSoundPlayer == null)
                mSoundPlayer = GetComponent<JCS_SoundPlayer>();
        }

        protected virtual void Update()
        {
            ProcessInput();
        }


        /// <summary>
        /// Call this function to active the skill u want.
        /// </summary>
        public abstract void ActiveSkill();

        /// <summary>
        /// Handle skill prevent the skill go to fast.
        /// </summary>
        private void ProcessInput()
        {
            if (!mAction)
            {
                if (JCS_Input.GetKey(mKeyCode))
                    mAction = true;
            }

            float dt = JCS_Time.ItTime(mTimeType);

            if (mAfterDelay)
            {
                mActionTimer += dt;

                if (mActiveTime < mActionTimer)
                {
                    // reset timer
                    mActionTimer = 0;

                    // can do the next shoot
                    mAction = false;

                    // exit delay process
                    mAfterDelay = false;
                }
            }

            if (mAction && !mAfterDelay)
            {
                mActionTimer += dt;

                if (mTimeToActive < mActionTimer)
                {
                    // Do shooting effect
                    ActiveSkill();

                    // start after delay timer.
                    mAfterDelay = true;

                    // reset timer for "mAfterDelay" Trigger.
                    mActionTimer = 0;
                }
            }
        }

        /// <summary>
        /// Spawn the main animattion 1.
        /// </summary>
        protected void SpawnMainAnim1()
        {
            if (mMainAnim1 == null)
            {
                Debug.Log(
                    "Assigning active skill action without animation is not allowed.");

                return;
            }

            GameObject obj = JCS_Util.SpawnAnimateObject(mMainAnim1, mOrderLayer);

            if (mSamePosition)
                obj.transform.position = transform.position + mAnimPosOffset1;
            if (mSameRotation)
                obj.transform.rotation = transform.rotation;
            if (mSameScale)
                obj.transform.localScale = transform.localScale;

            // if stay with player, simple set the position to
            // same position and set to child so it will follows
            // the active target!
            if (mStayWithActiveTarget)
                obj.transform.SetParent(transform);


            // add anim death event,
            // so when animation ends destroy itself.
            obj.AddComponent<JCS_DestroyAnimEndEvent>();
        }

        /// <summary>
        /// Spawn the main animattion 2.
        /// </summary>
        protected void SpawnMainAnim2()
        {
            if (mMainAnim2 == null)
            {
                Debug.Log(
                    "Assigning active skill action without animation is not allowed.");

                return;
            }

            GameObject obj = JCS_Util.SpawnAnimateObject(mMainAnim2, mOrderLayer + 1);

            if (mSamePosition)
                obj.transform.position = transform.position + mAnimPosOffset2;
            if (mSameRotation)
                obj.transform.rotation = transform.rotation;
            if (mSameScale)
                obj.transform.localScale = transform.localScale;

            // if stay with player, simple set the position to
            // same position and set to child so it will follows
            // the active target!
            if (mStayWithActiveTarget)
                obj.transform.SetParent(transform);


            // add anim death event,
            // so when animation ends destroy itself.
            obj.AddComponent<JCS_DestroyAnimEndEvent>();
        }
    }
}
