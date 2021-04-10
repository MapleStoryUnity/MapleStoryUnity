/**
 * $File: JCS_SequenceShootAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Shoot in Sequence act like damage text.(sequence)
    /// </summary>
    [RequireComponent(typeof(JCS_ShootAction))]
    public class JCS_SequenceShootAction
        : MonoBehaviour
        , JCS_Action
    {
        /* Variables */

        private JCS_ShootAction mShootAction = null;


        [Header("** Runtime Variables (JCS_SequenceShootAction) **")]

        [Tooltip("How many shoot in sequence?")]
        [SerializeField] [Range(1, 30)]
        private int mHit = 8;

        [Tooltip("Time to a shoot in sequence.")]
        [SerializeField] [Range(0.01f, 0.5f)]
        private float mTimePerShoot = 0.1f;

        [Tooltip("Make the action do in sequence.")]
        [SerializeField]
        private bool mInSequenceEffect = false;

        [Tooltip("Make the bullet shoots at the position that starts.")]
        [SerializeField]
        private bool mSequenceStay = true;


        [Header("** Optional Variables (JCS_SequenceShootAction) **")]

        [Tooltip("Ability format to use.")]
        [SerializeField]
        private JCS_AbilityFormat mAbilityFormat = null;


        [Header("** Action Settings (JCS_SequenceShootAction) **")]

        [Tooltip("Time delay before shoot.")]
        [SerializeField]
        private float mTimeBeforeShoot = 0.0f;

        private bool mAction = false;

        [Tooltip("Time delay after a shoot.")]
        [SerializeField]
        private float mTimeDelayAfterShoot = 0.0f;

        private bool mAfterDelay = false;

        private float mActionTimer = 0.0f;


        [Header("** Shoot Gap Effect (JCS_SequenceShootAction) **")]

        [Tooltip("Shoot with gap?")]
        [SerializeField]
        private bool mShootGapEffect = false;

        [Tooltip("Gap distance.")]
        [SerializeField]
        private float mShootGap = 0.1f;

        private int[] mDamageApplying = null;

        private JCS_DetectAreaObject mDetectedObject = null;


        //** Sequence Data **
        private JCS_Vector<int> mThread = null;        // main thread
        private JCS_Vector<float> mTimers = null;           // timer per thread
        private JCS_Vector<int> mShootCount = null;         // how many shoot should process per thread
        private JCS_Vector<int> mShootCounter = null;         // counter per thread
        private JCS_Vector<Vector3> mShootPos = null;
        private JCS_Vector<Transform> mTargetsPerSequence = null;
        private JCS_Vector<bool> mShootDirection = null;


        /* Setter & Getter */

        public int Hit { get { return this.mHit; } set { this.mHit = value; } }
        public float TimePerShoot { get { return this.mTimePerShoot; } set { this.mTimePerShoot = value; } }
        public bool InSequenceEffect { get { return this.mInSequenceEffect; } set { this.mInSequenceEffect = value; } }
        public bool SequenceStay { get { return this.mSequenceStay; } set { this.mSequenceStay = value; } }
        public void SetShootCallback(ShootCallback func) { this.mShootAction.SetShootCallback(func); }
        public JCS_AbilityFormat AbilityFormat { get { return this.mAbilityFormat; } set { this.mAbilityFormat = value; } }
        public float TimeBeforeShoot { get { return this.mTimeBeforeShoot; } set { this.mTimeBeforeShoot = value; } }
        public float TimeDelayAfterShoot { get { return this.mTimeDelayAfterShoot; } set { this.mTimeDelayAfterShoot = value; } }
        public bool ShootGapEffect { get { return this.mShootGapEffect; } set { this.mShootGapEffect = value; } }
        public float ShootGap { get { return this.mShootGap; } set { this.mShootGap = value; } }


        /* Functions */

        private void Awake()
        {
            mShootAction = this.GetComponent<JCS_ShootAction>();

            // override the shoot effect in the base one.
            mShootAction.OverrideShoot = true;

            mThread = new JCS_Vector<int>();
            mTimers = new JCS_Vector<float>();
            mShootCount = new JCS_Vector<int>();
            mShootCounter = new JCS_Vector<int>();
            mShootPos = new JCS_Vector<Vector3>();
            mTargetsPerSequence = new JCS_Vector<Transform>();
            mShootDirection = new JCS_Vector<bool>();

            // try to get the ability format.
            if (mAbilityFormat == null)
                mAbilityFormat = this.GetComponent<JCS_AbilityFormat>();
        }

        private void Update()
        {
            ProcessInput();

            // process sequence of threads.
            ProccessSequences();
        }

        /// <summary>
        /// Shoot bullets multiple times in times. (not in per frame)
        /// </summary>
        public void Shoots(int hit, Vector3 pos)
        {
            if (mShootAction.Bullet == null)
            {
                JCS_Debug.LogReminder(
                    "There is no bullet assign to \"JCS_ShootAction\", so we cannot shoot a sequence...");

                return;
            }

            if (hit <= 0)
            {
                JCS_Debug.LogReminder(
                    "Cannot shoot sequence of bullet with lower than 0 hit...");

                return;
            }

            // after finding once is still null,
            // try it agian!
            if (mDetectedObject == null)
            {
                // find the target
                switch (mShootAction.GetTrackType())
                {
                    case JCS_ShootAction.TrackType.CLOSEST:
                        mDetectedObject = mShootAction.GetDetectAreaAction().FindTheClosest();
                        break;
                    case JCS_ShootAction.TrackType.FURTHEST:
                        mDetectedObject = mShootAction.GetDetectAreaAction().FindTheFurthest();
                        break;
                }

            }

            // does not found the target to damage
            if (mDetectedObject == null)
                mTargetsPerSequence.push(null);
            else {
                // found target to damage, add in to data segment
                mTargetsPerSequence.push(mDetectedObject.transform);

                JCS_2DLiveObject liveObj = mDetectedObject.GetComponent<JCS_2DLiveObject>();
                if (liveObj != null)
                {
                    int defenseVal = 0;

                    // get the targeting defense value
                    if (liveObj.AbilityFormat != null)
                        defenseVal = liveObj.AbilityFormat.GetDefenseValue();

                    // calculate the damage we are going to apply to
                    // the target object.
                    mDamageApplying = PreCalculateSequenceDamage(
                        mAbilityFormat.GetMinDamage(),
                        mAbilityFormat.GetMaxDamage(),
                        hit,
                        defenseVal);

                    // pre calculate the damage before the
                    // actual bullet hit the object,
                    // so it could decide what object are dead already.
                    // and other object or this object wont target the
                    // object is going die.
                    liveObj.ReceivePreCalDamage(mDamageApplying);

                    // start targeting object to hit
                    liveObj.BeenTarget = true;
                }
            }


            // thread itself
            mThread.push(mThread.length);

            // needed data
            mTimers.push(0);                // timer to calculate between each shoot.
            mShootCount.push(hit);          // hit per sequence.
            mShootCounter.push(0);          // counter to count how many shoot left?
            mShootPos.push(pos);            // position to spawn the bullet implements the position stay effect!


            bool isLeft = true;
            if (this.transform.localScale.x < 0)
                isLeft = false;

            // shoot direction
            mShootDirection.push(isLeft);   // decide which direction should the bullet goes? (right/left)
        }

        /// <summary>
        /// Process the sequence of threads queue.
        /// </summary>
        private void ProccessSequences()
        {
            for (int processIndex = 0;
                processIndex < mThread.length;
                ++processIndex)
            {
                // process all the thread
                Sequence(processIndex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="processIndex"> thread id. </param>
        private void Sequence(int processIndex)
        {
            // get the timer from the thread
            float newTimer = mTimers.at(processIndex);

            // add time to timer
            newTimer += Time.deltaTime;

            // check if we can shoot or not
            if (mTimePerShoot < newTimer)
            {
                int totalShootCount = mShootCount.at(processIndex);
                int currentShootCount = mShootCounter.at(processIndex);
                if (currentShootCount == totalShootCount)
                {
                    // Remove Thread.
                    EndProcessSequence(processIndex);
                    return;
                }

                Vector3 spawnPos = this.transform.position;

                // if stay in the same position
                if (mSequenceStay)
                    spawnPos = mShootPos.at(processIndex);

                if (mShootGapEffect)
                {
                    spawnPos.y += currentShootCount * mShootGap;
                }

                // direction.
                bool direction = mShootDirection.at(processIndex);

                // shoot a bullet
                if (mInSequenceEffect)
                {
                    bool theSub = true;

                    // meaning the head of the bullet in sequence.
                    if (currentShootCount == 0)
                    {
                        theSub = false;
                        if (mDamageApplying != null)
                            mShootAction.Shoot(spawnPos, direction, mDamageApplying, currentShootCount, theSub, mTargetsPerSequence.at(processIndex));
                        else
                            mShootAction.Shoot(spawnPos, direction, mHit, currentShootCount, theSub, mTargetsPerSequence.at(processIndex));

                        // after set the damage set it back to null
                        mDamageApplying = null;

                    }
                    // process the sub bullet in sequence
                    else {
                        mShootAction.Shoot(spawnPos, direction, mHit, currentShootCount, theSub, mTargetsPerSequence.at(processIndex));
                    }
                }
                else
                    mShootAction.Shoot(spawnPos, direction, 1, currentShootCount, false);


                ++currentShootCount;

                // update new count, in order
                // to spawn next bullet
                mShootCounter.set(processIndex, currentShootCount);
                newTimer = 0;
            }

            // update timer
            mTimers.set(processIndex, newTimer);
        }

        /// <summary>
        /// End the thread, kill the process.
        /// </summary>
        /// <param name="processIndex"> thread id to kill. </param>
        private void EndProcessSequence(int processIndex)
        {
            mThread.slice(processIndex);

            mTimers.slice(processIndex);
            mShootCount.slice(processIndex);
            mShootCounter.slice(processIndex);
            mShootPos.slice(processIndex);
            mTargetsPerSequence.slice(processIndex);
            mShootDirection.slice(processIndex);
        }

        /// <summary>
        /// Process/Handle the inputs from the user.
        /// </summary>
        private void ProcessInput()
        {
            if (!mAction)
            {
                if (JCS_Input.GetKey(mShootAction.ShootKeyCode) ||
                    JCS_Input.GetMouseByAction(mShootAction.KeyAct, mShootAction.MouseButton))
                {

                    // find the target
                    switch (mShootAction.GetTrackType())
                    {
                        case JCS_ShootAction.TrackType.CLOSEST:
                            mDetectedObject = mShootAction.GetDetectAreaAction().FindTheClosest();
                            break;
                        case JCS_ShootAction.TrackType.FURTHEST:
                            mDetectedObject = mShootAction.GetDetectAreaAction().FindTheFurthest();
                            break;
                    }

                    mAction = true;
                }
            }

            if (mAfterDelay)
            {
                mActionTimer += Time.deltaTime;

                if (mTimeDelayAfterShoot < mActionTimer)
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
                mActionTimer += Time.deltaTime;

                if (mTimeBeforeShoot < mActionTimer)
                {
                    if (mShootAction.GetShootCallback() != null)
                    {
                        // do call back
                        mShootAction.GetShootCallback().Invoke();
                    }

                    // Do shooting effect
                    Shoots(mHit, this.transform.position);

                    // start after delay timer.
                    mAfterDelay = true;

                    // reset timer for "mAfterDelay" Trigger.
                    mActionTimer = 0;
                }
            }
        }

        /// <summary>
        /// Calculate the damage before we apply to the live object.
        /// </summary>
        /// <param name="minDamage"></param>
        /// <param name="maxDamage"></param>
        /// <param name="hit"></param>
        /// <param name="defenseValue"></param>
        /// <returns></returns>
        public int[] PreCalculateSequenceDamage(int minDamage, int maxDamage, int hit, int defenseValue)
        {
            if (minDamage > maxDamage)
            {
                JCS_Debug.LogError("min damage cannot be higher or equal to the max damage!");
                return null;
            }

            if (minDamage < 0 || maxDamage < 0)
            {
                JCS_Debug.LogError("Min or Max damage cannot be lower than 0!");
                return null;
            }

            if (hit <= 0)
            {
                JCS_Debug.LogError("Hit count should not be equal or lower than 0!");
                return null;
            }


            // get the game setting first
            JCS_GameSettings jcsGm = JCS_GameSettings.instance;

            int[] damages = new int[hit];

            for (int index = 0;
                index < hit;
                ++index)
            {
                int dm = Random.Range(minDamage, maxDamage);

                // 受到的傷害 = 傷害 - 防禦力
                damages[index] = dm - defenseValue;

                // Check min max
                {
                    // 如果小於最下限得值, 就設定為最下限的值
                    if (damages[index] < jcsGm.MIN_DAMAGE)
                        damages[index] = jcsGm.MIN_DAMAGE;

                    // 如果大於最上限得值, 就設定為最上限的值
                    if (damages[index] > jcsGm.MAX_DAMAGE)
                        damages[index] = jcsGm.MAX_DAMAGE;
                }
            }


            // return the damages we just create!
            return damages;
        }
    }
}
