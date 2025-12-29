/**
 * $File: JCS_2DSequenceShootActionNoDetection.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Shoot bullets toward the cursor position in sequence.
    /// </summary>
    [RequireComponent(typeof(JCS_ShootAction))]
    [RequireComponent(typeof(JCS_2DCursorShootAction))]
    public class JCS_2DSequenceShootActionNoDetection : MonoBehaviour, JCS_IAction
    {
        /* Variables */

        [Separator("📋 Check Variabless (JCS_2DCursorShootAction)")]

        [SerializeField]
        [ReadOnly]
        private JCS_ShootAction mShootAction = null;

        [SerializeField]
        [ReadOnly]
        private JCS_2DCursorShootAction mCursorShootAction = null;

        [Separator("⚡️ Runtime Variables (JCS_2DCursorShootAction)")]

        [Tooltip("How many shots in sequence?")]
        [SerializeField]
        [Range(1, 30)]
        private int mHit = 8;

        [Tooltip("Time to a shoot in sequence.")]
        [SerializeField]
        [Range(0.01f, 0.5f)]
        private float mTimePerShoot = 0.1f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Tooltip("Make the bullet shoots at the position that starts.")]
        [SerializeField]
        private bool mSequenceStay = true;

        [Tooltip("Keep all bullets in the same angle per sequence.")]
        [SerializeField]
        private bool mKeepShootAngle = true;

        [Header("🔍 Shoot Gap")]

        [Tooltip("Shoot with gap?")]
        [SerializeField]
        private bool mShootGapEffect = false;

        [Tooltip("Gap distance.")]
        [SerializeField]
        private float mShootGap = 0.1f;

        //** Sequence Data **
        private JCS_Vec<int> mThread = null;           // main thread
        private JCS_Vec<float> mTimers = null;         // timer per thread
        private JCS_Vec<int> mShootCount = null;       // how many shoot should process per thread
        private JCS_Vec<int> mShootCounter = null;     // counter per thread
        private JCS_Vec<Vector3> mShootPos = null;
        private JCS_Vec<Vector3> mShootAngles = null;

        /* Setter & Getter */

        public int hit { get { return mHit; } set { mHit = value; } }
        public float timePerShoot { get { return mTimePerShoot; } set { mTimePerShoot = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }
        public bool sequenceStay { get { return mSequenceStay; } set { mSequenceStay = value; } }
        public bool keepShootAngle { get { return mKeepShootAngle; } set { mKeepShootAngle = value; } }
        public bool shootGapEffect { get { return mShootGapEffect; } set { mShootGapEffect = value; } }
        public float shootGap { get { return mShootGap; } set { mShootGap = value; } }

        /// <summary>
        /// Call back during shooting a bullet.
        /// </summary>
        /// <param name="func"> function to set. </param>
        public void SetShootCallback(Action func)
        {
            mShootAction.SetShootCallback(func);
        }

        public void SetCheckAbleToShootFunction(Func<bool> func)
        {
            mShootAction.SetCheckAbleToShootFunction(func);
        }

        /* Functions */

        private void Awake()
        {
            mShootAction = GetComponent<JCS_ShootAction>();
            mCursorShootAction = GetComponent<JCS_2DCursorShootAction>();

            // overwrite both shoot action,
            // so they won't process the input.
            mShootAction.overrideShoot = true;
            mCursorShootAction.overrideShoot = true;

            mThread = new JCS_Vec<int>();
            mTimers = new JCS_Vec<float>();
            mShootCount = new JCS_Vec<int>();
            mShootCounter = new JCS_Vec<int>();
            mShootPos = new JCS_Vec<Vector3>();
            mShootAngles = new JCS_Vec<Vector3>();
        }

        private void Update()
        {
            ProcessInput();

            ProccessSequences();
        }

        /// <summary>
        /// Shoot bullets in sequence queue.
        /// </summary>
        public void ShootsInSequence()
        {
            ShootsInSequence(mHit, transform.position);
        }
        /// <summary>
        /// Shoot bullets in sequence queue.
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="pos"></param>
        public void ShootsInSequence(int hit, Vector3 pos)
        {
            if (mShootAction.bullet == null)
            {
                Debug.Log("There is no bullet assign to `JCS_ShootAction`, so we cannot shoot a sequence...");
                return;
            }

            if (hit <= 0)
            {
                Debug.Log("Can't shoot sequence of bullet with lower than 0 hit...");
                return;
            }

            // thread itself
            mThread.push(mThread.length);

            // needed data
            mTimers.push(0);                // timer to calculate between each shoot.
            mShootCount.push(hit);          // hit per sequence.
            mShootCounter.push(0);          // counter to count how many shoot left?
            mShootPos.push(pos);            // position to spawn the bullet implements the position stay effect!
            mShootAngles.push(mShootAction.spawnPoint.eulerAngles);

        }

        /// <summary>
        /// Process/Handle the inputs from the user.
        /// </summary>
        private void ProcessInput()
        {
            if (JCS_Input.GetMouseByAction(mShootAction.keyAct, mShootAction.mouseButton) ||
                JCS_Input.GetKeyByAction(mShootAction.keyAct, mShootAction.shootKeyCode))
            {
                // check we are able to shoot.
                // like enough mana to cast? or something like 
                if (mShootAction.GetCheckAbleToShootFunction().Invoke())
                {
                    // do callback
                    mShootAction.GetShootCallback().Invoke();

                    ShootsInSequence();
                }
            }
        }

        /// <summary>
        /// Process the sequence of threads queue.
        /// </summary>
        private void ProccessSequences()
        {
            for (int processIndex = 0; processIndex < mThread.length; ++processIndex)
            {
                // process all the thread
                Sequence(processIndex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="processIndex"></param>
        private void Sequence(int processIndex)
        {
            // get the timer from the thread
            float newTimer = mTimers.at(processIndex);

            // add time to timer
            newTimer += JCS_Time.ItTime(mTimeType);

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

                Vector3 spawnPos = transform.position;
                Vector3 shootAngle = mShootAction.spawnPoint.eulerAngles;

                // if stay in the same position
                if (mSequenceStay)
                    spawnPos = mShootPos.at(processIndex);

                if (mShootGapEffect)
                    spawnPos.y += currentShootCount * mShootGap;

                if (mKeepShootAngle)
                    shootAngle = mShootAngles.at(processIndex);

                // do shooting
                mShootAction.ShootWithShootCount(spawnPos, shootAngle);

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
            mShootAngles.slice(processIndex);
        }
    }
}
