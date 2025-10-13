/**
 * $File: JCS_ApplyDamageTextToLiveObjectAction.cs $
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
    /// Apply the damage to live object by automatically.
    /// </summary>
    [RequireComponent(typeof(JCS_AttackerInfo))]
    public class JCS_ApplyDamageTextToLiveObjectAction : MonoBehaviour
    {
        /* Variables */

        [Separator("Check Variables (JCS_ApplyDamageTextToLiveObjectAction)")]

        [SerializeField]
        [ReadOnly]
        private bool mInSequence = false;

        [SerializeField]
        [ReadOnly]
        private int mHit = 1;

        [SerializeField]
        [ReadOnly]
        private int mMinDamage = 1;

        [SerializeField]
        [ReadOnly]
        private int mMaxDamage = 5;

        [SerializeField]
        [ReadOnly]
        private int mCriticalChance = 10;

        [Separator("Runtime Variables (JCS_ApplyDamageTextToLiveObjectAction)")]

        // Ability Format
        [Tooltip("Ability format for calculation.")]
        [SerializeField]
        private JCS_AbilityFormat mAbilityFormat = null;

        // Offset
        [Tooltip("Position offset where damage text spawns.")]
        [SerializeField]
        private Vector3 mDamageTextPositionOffset = Vector3.zero;

        [Header("Pre Calculate")]

        [Tooltip("Attack will be calculate before hit the object.")]
        [SerializeField]
        private bool mPreCalculateEffect = false;

        [Tooltip("Damages that store in here ready to apply to the target.")]
        [SerializeField]
        private int[] mDamageApplying = null;

        [Header("Lock Effect")]

        [Tooltip("Lock on the target?")]
        [SerializeField]
        private bool mOnlyWithTarget = false;

        [Tooltip("Target we lock on!")]
        [SerializeField]
        private Transform mTargetTransform = null;

        [Header("Random Effect")]

        [Tooltip("Random position effect.")]
        [SerializeField]
        private bool mRandPos = false;

        [Tooltip("Random position limit to within this range.")]
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mRandPosRange = 0.0f;

        [Header("Destroy")]

        [Tooltip("Destroy live object by this object.")]
        [SerializeField]
        private bool mDestroyByThisAction = true;

        [Header("AOE")]

        [Tooltip("Make object un-destroyable, count down by AOECount below.")]
        [SerializeField]
        private bool mIsAOE = false;

        [Tooltip("Once the object hit a object count one down.")]
        [SerializeField]
        [Range(1, 15)]
        private int mAOECount = 7;

        // record down the aoe cound!
        private int mRecordAOECount = 0;

        private JCS_AttackerInfo mAttackerInfo = null;

        // every time in a frame, one bullet can shoot 
        // multiple target. To prevent this happen make
        // a boolean handler the detection.
        private bool mIsDestroyed = false;

        [Header("Sound")]

        [Tooltip("Play this hit sound, while is action happens.")]
        [SerializeField]
        private AudioClip mHitSound = null;

        /* Setter & Getter */

        public Transform targetTransform { get { return mTargetTransform; } set { mTargetTransform = value; } }
        public int hit { get { return mHit; } set { mHit = value; } }
        public JCS_AbilityFormat abilityFormat { get { return mAbilityFormat; } set { mAbilityFormat = value; } }
        public bool inSequence { get { return mInSequence; } set { mInSequence = value; } }

        public int[] damageApplying { get { return mDamageApplying; } set { mDamageApplying = value; } }
        public JCS_AttackerInfo attackerInfo { get { return mAttackerInfo; } set { mAttackerInfo = value; } }

        /* Functions */

        private void Awake()
        {
            mAttackerInfo = GetComponent<JCS_AttackerInfo>();

            mRecordAOECount = mAOECount;
        }

        private void OnEnable()
        {
            // relief the aoe count
            mAOECount = mRecordAOECount;

            mIsDestroyed = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (mIsDestroyed)
                return;

            // do not target itself.
            if (other.transform == mAttackerInfo.attacker)
                return;


            if (mInSequence)
            {
                if (mDestroyByThisAction)
                {
                    if (mTargetTransform == other.transform)
                        Destroy(gameObject);
                }

                return;
            }

            JCS_2DLiveObject liveObject = other.GetComponent<JCS_2DLiveObject>();

            // doing the lock on effect
            if (mOnlyWithTarget)
            {
                // if the target isn't what we want ignore than.
                if (mTargetTransform != other.transform)
                    return;
                else
                {

                    if (liveObject != null)
                    {
                        var tact = GetComponent<JCS_2DTrackAction>();

                        // only the last bullet in sequence will check dead.
                        if (tact != null)
                        {
                            if (tact.orderIndex == hit)
                            {
                                liveObject.beenTarget = false;
                                liveObject.CheckDie();
                            }
                        }
                    }

                    DestroyWithAction();
                }
            }
            // if not in sequence
            else
            {
                if (liveObject == null)
                    return;

                if (!liveObject.canDamage)
                {
                    return;
                }

                // liveObject is already dead.
                if (liveObject.IsDead())
                {
                    //DestroyWithAction();
                    return;
                }
            }

            Transform attacker = mAttackerInfo.attacker;
            if (attacker != null)
            {
                JCS_2DLiveObject owenerLiveObject = mAttackerInfo.attacker.GetComponent<JCS_2DLiveObject>();

                if (owenerLiveObject != null)
                {
                    if (!JCS_GameSettings.FirstInstance().tribeDamageEachOther)
                    {
                        // if both player does not need to add in to list.
                        // or if both enemy does not need to add in to list.
                        if (liveObject.isPlayer == owenerLiveObject.isPlayer)
                            return;
                    }
                }
            }

            if (mAbilityFormat != null)
            {
                mMinDamage = mAbilityFormat.GetMinDamage();
                mMaxDamage = mAbilityFormat.GetMaxDamage();
                mCriticalChance = mAbilityFormat.GetCriticalChance();
            }
            else
            {
                Debug.Log("You sure to not using any `JCS_AbilityFormat`?");
            }

            Vector3 currentPos = liveObject.transform.position + mDamageTextPositionOffset;

            if (mRandPos)
                AddRandomPosition(ref currentPos);

            if (mPreCalculateEffect)
            {
                liveObject.ApplyDamageText(
                    mAttackerInfo.attacker,
                    mDamageApplying,
                    currentPos,
                    mCriticalChance,
                    mHitSound);

                mHit = mDamageApplying.Length;
            }
            else
            {
                liveObject.ApplyDamageText(
                    mAttackerInfo.attacker,
                    mMinDamage,
                    mMaxDamage,
                    currentPos,
                    mHit,
                    mCriticalChance,
                    mHitSound);
            }

            // only if there is only one hit need to do 
            if (mHit == 1)
                liveObject.beenTarget = false;

            liveObject.CheckDie();

            DestroyWithAction();
        }

        /// <summary>
        /// Copy some information from the other.
        /// </summary>
        public void CopyToThis(JCS_ApplyDamageTextToLiveObjectAction copy)
        {
            abilityFormat = copy.abilityFormat;
            hit = copy.hit;
            attackerInfo = copy.attackerInfo;
        }

        /// <summary>
        /// Randomize the position.
        /// </summary>
        /// <param name="currentPos"> current position to be randomize. </param>
        private void AddRandomPosition(ref Vector3 currentPos)
        {
            float addPos;
            Vector3 newPos = currentPos;

            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.x += addPos;

            addPos = Random.Range(-mRandPosRange, mRandPosRange);
            newPos.y += addPos;

            currentPos = newPos;
        }

        /// <summary>
        /// Check aoe and decide to destroy 
        /// the game object now or later.
        /// </summary>
        private void DestroyWithAction()
        {
            // return if is aoe continue.
            if (CheckAOE())
                return;

            if (mDestroyByThisAction)
            {
                Destroy(gameObject);
            }

            // once is destroy, this action will 
            // not be active anymore.
            mIsDestroyed = true;
        }

        /// <summary>
        /// Check is the attack aoe?
        /// </summary>
        /// <returns> TRUE: is aoe, dont destroy the object
        /// FALSE: is not aoe or aoe count is less/equal to zero, 
        /// destroy the object. </returns>
        private bool CheckAOE()
        {
            // if we have aoe count left.
            if (0 < mAOECount)
            {
                // if aoe effect is on.
                if (mIsAOE)
                {
                    // count down the aoe count, 
                    --mAOECount;

                    // then return, dont destory the object, 
                    // before the aoe count down meet to less 
                    // or equal to zero.
                    return true;
                }
            }

            return false;
        }
    }
}
