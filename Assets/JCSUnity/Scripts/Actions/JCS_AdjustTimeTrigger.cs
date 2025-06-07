/**
 * $File: JCS_AdjustTimeTrigger.cs $
 * $Date: 2017-05-10 17:30:23 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Trigger a event while the time is reached.
    /// </summary>
    public class JCS_AdjustTimeTrigger : MonoBehaviour
    {
        /* Variables */

        // Action to trigger if the time is reached.
        public Action onAction = null;

        // Execution before action is being executed.
        public Action onBeforeAction = null;

        // Execution after action is being executed.
        public Action onAfterAction = null;

        [Separator("Check Variables (JCS_AdjustTimeTrigger)")]

        [Tooltip(@"Time to record down the real time to do one action after 
we calculate the real time.")]
        [SerializeField]
        [ReadOnly]
        private float mRealTime = 0.0f;

        [Tooltip("Timer to check if reach the real time.")]
        [SerializeField]
        [ReadOnly]
        private float mTimer = 0.0f;

        [Tooltip("check if the action trigger.")]
        [SerializeField]
        [ReadOnly]
        private bool mDidAction = false;

        [Separator("Initialize Variables (JCS_AdjustTimeTrigger)")]

        [Tooltip("Run immediately on the first frame.")]
        [SerializeField]
        private bool mInvokeOnStart = true;

        [Separator("Runtime Variables (JCS_AdjustTimeTrigger)")]

        [Tooltip("Is this component active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Time to trigger the event.")]
        [SerializeField]
        [Range(0.0f, 30.0f)]
        private float mTime = 2.0f;

        [Tooltip("Time that will randomly affect the time.")]
        [SerializeField]
        [Range(0.0f, 20.0f)]
        private float mAdjustTime = 1.5f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Tooltip("Event that will be triggered.")]
        [SerializeField]
        private UnityEvent mOnAction = null;

        /* Setter & Getter */

        public bool InvokeOnStart { get { return this.mInvokeOnStart; } set { this.mInvokeOnStart = value; } }
        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public float Time { get { return this.mTime; } set { this.mTime = value; } }
        public float AdjustTime { get { return this.mAdjustTime; } set { this.mAdjustTime = value; } }
        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }
        public UnityEvent OnAction { get { return this.mOnAction; } set { this.mOnAction = value; } }

        /* Functions */

        private void Start()
        {
            // Run immediately on the first frame.
            if (mInvokeOnStart)
                Invoke(nameof(ExecuteAction), JCS_Constants.FIRST_FRAME_INVOKE_TIME);
        }

        private void Update()
        {
            if (!mActive)
                return;

            DoAction();
        }

        /// <summary>
        /// Reset time and timer, then run the action once.
        /// </summary>
        public void ResetAndRun()
        {
            RecalculateTimeAndResetTimer();

            ExecuteAction();
        }

        /// <summary>
        /// Recalculate the time and reset the timer.
        /// </summary>
        public void RecalculateTimeAndResetTimer()
        {
            RecalculateTime();

            ResetTimer();
        }

        /// <summary>
        /// Recalculate the real time.
        /// </summary>
        public void RecalculateTime()
        {
            float adjustTime = JCS_Random.Range(-mAdjustTime, mAdjustTime);
            mRealTime = mTime + adjustTime;
        }

        /// <summary>
        /// Reset the timer.
        /// </summary>
        public void ResetTimer()
        {
            mDidAction = false;
            mTimer = 0.0f;
        }

        /// <summary>
        /// Do the timer action, execute the function pointer if the 
        /// timer had reached repeatedly.
        /// </summary>
        private void DoAction()
        {
            if (mDidAction)
            {
                RecalculateTimeAndResetTimer();
            }

            mTimer += JCS_Time.ItTime(mTimeType);

            if (mRealTime > mTimer)
                return;

            ExecuteAction();

            mDidAction = true;
        }

        /// <summary>
        /// Execute the action.
        /// </summary>
        private void ExecuteAction()
        {
            onBeforeAction?.Invoke();

            onAction?.Invoke();

            mOnAction?.Invoke();

            onAfterAction?.Invoke();
        }
    }
}
