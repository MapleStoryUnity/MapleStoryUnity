/**
 * $File: JCS_SwingAttackAction.cs $
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
    /// Action to do swing attack.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_SwingAttackAction : MonoBehaviour , JCS_IAction
    {
        /* Variables */

        private JCS_SoundPlayer mSoundPlayer = null;

        [Header("** Runtime Variables (JCS_SwingAttackAction) **")]

        [Tooltip("Collider to detect weather the enemy get hit or not.")]
        [SerializeField]
        private Collider mAttackRange = null;

        // if this is true, meaning there are other action going on.
        private bool mOverrideAction = false;

        [Tooltip("Ability format in order to apply damage.")]
        [SerializeField]
        private JCS_AbilityFormat mAbilityFormat = null;

        [Tooltip("Animation while this action is active.")]
        [SerializeField]
        private RuntimeAnimatorController mAtkAnim = null;

        [Tooltip("Transform attack animation will spawn, default will be the collider attah above.")]
        [SerializeField]
        private Transform mAtkAnimSpawnTrans = null;

        [Tooltip("What sorting layer you want the skill to be render?")]
        [SerializeField]
        private int mOrderLayer = 15;

        [Tooltip("How many time you want the animation to loops?")]
        [SerializeField]
        [Range(1, 15)]
        private int mLoopTimes = 1;

        [Tooltip("The same position as the spawn transform's position.")]
        [SerializeField]
        private bool mAsSamePosition = true;

        [Tooltip("The same rotation as the spawn transform's rotation.")]
        [SerializeField]
        private bool mAsSameRotation = false;

        [Tooltip("The same scale as the spawn transform's scale.")]
        [SerializeField]
        private bool mAsSameScale = false;

        [Tooltip("Animation offset position value.")]
        [SerializeField]
        private Vector3 mAnimOffsetPosition = Vector3.zero;

        [Tooltip("Animation offset scale value.")]
        [SerializeField]
        private Vector3 mAnimOffsetScale = Vector3.zero;

        [Tooltip("Sound to play for this action.")]
        [SerializeField]
        private AudioClip mAudioClip = null;

        [Tooltip("Sound settings type.")]
        [SerializeField]
        private JCS_SoundSettingType mSoundType = JCS_SoundSettingType.NONE;

        [Header("- Damage Text")]

        [Tooltip("If you want the action apply damage text add apply this.")]
        [SerializeField]
        private JCS_ApplyDamageTextToLiveObjectAction mApplyDamageTextAction = null;

        [Header("- Other")]

        [Tooltip("Key to active attack.")]
        [SerializeField]
        private KeyCode mKeyCode = KeyCode.None;

        [Tooltip("Speed layer.")]
        [SerializeField]
        private JCS_SpeedLayer mSpeedLayer = JCS_SpeedLayer.NORMAL;

        private float mAnimSpeed = 1.0f;

        [SerializeField]
        private float mLayerLevel = 1.0f;

        private bool mAction = false;
        private float mActionTimer = 0;

        [Tooltip("Time to active attack.")]
        [SerializeField]
        private float mTimeToActTime = 0.5f;

        // boolean check if is time before attack, 
        // or after attack.
        private bool mAfterDelay = false;

        [Tooltip("Time after attack, delay a while.")]
        [SerializeField]
        private float mActTime = 0.25f;

        private float mRecordTimeToAttackTime = 0;
        private float mRecordActTime = 0;

        /* Setter & Getter */

        public void SetSpeedLayer(JCS_SpeedLayer sl)
        {
            this.mSpeedLayer = sl;

            // update speed of this action.
            ProcessSpeedLayer();
        }
        public bool AsSamePosition { get { return this.mAsSamePosition; } set { this.mAsSamePosition = value; } }
        public bool AsSameRotation { get { return this.mAsSameRotation; } set { this.mAsSameRotation = value; } }
        public bool AsSameScale { get { return this.mAsSameScale; } set { this.mAsSameScale = value; } }

        /* Functions */

        private void Awake()
        {
            // try to get it own transform's componenet.
            if (mAbilityFormat == null)
                mAbilityFormat = this.GetComponent<JCS_AbilityFormat>();

            if (mAttackRange != null)
            {
                // range must be trigger
                mAttackRange.isTrigger = true;

                // disable the trigger
                EndAttack();

                // set defualt attack animation spawn transform.
                if (mAtkAnimSpawnTrans == null)
                {
                    // defualt as the collider's transform.
                    mAtkAnimSpawnTrans = mAttackRange.transform;
                }
            }

            // try to get the action from the attack range.
            if (mApplyDamageTextAction == null)
                this.mApplyDamageTextAction = this.GetComponentInChildren<JCS_ApplyDamageTextToLiveObjectAction>();


            if (mSoundPlayer == null)
                this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            // record down the time
            mRecordTimeToAttackTime = mTimeToActTime;
            mRecordActTime = mActTime;
        }

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif
            ProcessInput();
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (JCS_Input.GetKeyDown(KeyCode.Alpha1))
                SetSpeedLayer(JCS_SpeedLayer.VERY_FAST);

            if (JCS_Input.GetKeyDown(KeyCode.Alpha2))
                SetSpeedLayer(JCS_SpeedLayer.FAST);

            if (JCS_Input.GetKeyDown(KeyCode.Alpha3))
                SetSpeedLayer(JCS_SpeedLayer.NORMAL);

            if (JCS_Input.GetKeyDown(KeyCode.Alpha4))
                SetSpeedLayer(JCS_SpeedLayer.SLOW);

            if (JCS_Input.GetKeyDown(KeyCode.Alpha5))
                SetSpeedLayer(JCS_SpeedLayer.VERY_SLOW);
        }
#endif

        /// <summary>
        /// Start the action.
        /// </summary>
        public void Attack()
        {
            // active the object
            mAttackRange.gameObject.SetActive(true);
        }

        /// <summary>
        /// End the action.
        /// </summary>
        public void EndAttack()
        {
            // de-active the object
            mAttackRange.gameObject.SetActive(false);
        }

        /// <summary>
        /// Receive input.
        /// </summary>
        private void ProcessInput()
        {
            if (!mOverrideAction)
            {

                if (!mAction && 
                    JCS_Input.GetKey(mKeyCode))
                {
                    // display animation
                    SpawnAttackAnimation();

                    // display sound
                    SpawnSwingSound();

                    mAction = true;
                }

                if (mAfterDelay)
                {
                    mActionTimer += Time.deltaTime;

                    if (mActTime < mActionTimer)
                    {
                        // close trigger
                        EndAttack();

                        // reset timer
                        mActionTimer = 0.0f;

                        // can do the next shoot
                        mAction = false;

                        // exit delay process
                        mAfterDelay = false;
                    }
                }

                if (mAction && !mAfterDelay)
                {
                    mActionTimer += Time.deltaTime;

                    if (mTimeToActTime < mActionTimer)
                    {
                        // active trigger
                        Attack();

                        // start after delay timer.
                        mAfterDelay = true;

                        // reset timer for "mAfterDelay" Trigger.
                        mActionTimer = 0.0f;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SpawnAttackAnimation()
        {
            if (mAtkAnim == null)
            {
                JCS_Debug.LogReminder("No animation assign but you still want to spawn a animation");
                return;
            }


            GameObject gm = new GameObject();
#if UNITY_EDITOR
            gm.name = "JCS_SwingAttackAction";
#endif
            // set the effect transform.
            if (mAsSamePosition)
                gm.transform.position = this.mAtkAnimSpawnTrans.position;
            if (mAsSameRotation)
                gm.transform.rotation = this.mAtkAnimSpawnTrans.rotation;
            if (mAsSameScale)
                gm.transform.localScale = this.mAtkAnimSpawnTrans.localScale;

            gm.transform.position += mAnimOffsetPosition;
            gm.transform.localScale += mAnimOffsetScale;

            if (this.transform.localScale.x < 0.0f)
            {
                Vector3 newScale = gm.transform.localScale;
                newScale.x = JCS_Mathf.ToNegative(newScale.x);
                gm.transform.localScale = newScale;
            }

            SpriteRenderer sr = gm.AddComponent<SpriteRenderer>();
            sr.sortingOrder = mOrderLayer;
            Animator animator = gm.AddComponent<Animator>();
            animator.runtimeAnimatorController = mAtkAnim;
            animator.speed = animator.speed * JCS_Mathf.Reciprocal(mAnimSpeed);


            JCS_DestroyAnimEndEvent dae = gm.AddComponent<JCS_DestroyAnimEndEvent>();
            dae.LoopTimes = mLoopTimes;
        }

        /// <summary>
        /// Play the swing sound.
        /// </summary>
        private void SpawnSwingSound()
        {
            if (mAudioClip == null)
                return;

            mSoundPlayer.PlayOneShot(mAudioClip, mSoundType);
        }

        /// <summary>
        /// Update the speed of this action.
        /// </summary>
        private void ProcessSpeedLayer()
        {
            float timeToAct = mRecordTimeToAttackTime;
            float timeActing = mRecordActTime;

            switch (mSpeedLayer)
            {
                case JCS_SpeedLayer.VERY_FAST:
                    {
                        mAnimSpeed = 1.0f / 3.0f * mLayerLevel;
                    }
                    break;
                case JCS_SpeedLayer.FAST:
                    {
                        mAnimSpeed = 1.0f / 2.0f * mLayerLevel;
                    }
                    break;
                case JCS_SpeedLayer.NORMAL:
                    {
                        // nothing here
                        mAnimSpeed = 1 * mLayerLevel;
                    }
                    break;
                case JCS_SpeedLayer.SLOW:
                    {
                        mAnimSpeed = 2 * mLayerLevel;
                    }
                    break;
                case JCS_SpeedLayer.VERY_SLOW:
                    {
                        mAnimSpeed = 3 * mLayerLevel;
                    }
                    break;
            }

            timeToAct = timeToAct * mAnimSpeed;
            timeActing = timeActing * mAnimSpeed;

            // apply new speed
            mTimeToActTime = timeToAct;
            mActTime = timeActing;
        }
    }
}
