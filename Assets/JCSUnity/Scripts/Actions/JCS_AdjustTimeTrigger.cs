/**
 * $File: JCS_AdjustTimeTrigger.cs $
 * $Date: 2017-05-10 17:30:23 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
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

        // action to trigger if the time is reached.
        public EmptyFunction onAction = null;

        [Separator("Check Variables (JCS_AdjustTimeTrigger)")]

        [Tooltip(@"Time to record down the real time to do one action after 
we calculate the real time.")]
        [SerializeField]
        [ReadOnly]
        private float mRealTimeZone = 0.0f;

        [Tooltip("Timer to check if reach the real time zone.")]
        [SerializeField]
        [ReadOnly]
        private float mTimer = 0.0f;

        [Tooltip("check if the action trigger.")]
        [SerializeField]
        [ReadOnly]
        private bool mDidAction = false;

        [Separator("Runtime Variables (JCS_AdjustTimeTrigger)")]

        [Tooltip("Is this component active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Time to trigger the event.")]
        [SerializeField]
        [Range(0.0f, 30.0f)]
        private float mTimeZone = 2.0f;

        [Tooltip("Time that will randomly affect the time.")]
        [SerializeField]
        [Range(0.0f, 20.0f)]
        private float mAdjustTimeZone = 1.5f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        [Tooltip("Event that will be triggered.")]
        [SerializeField]
        private UnityEvent mOnAction = null;

        /* Setter & Getter */

        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public float TimeZone { get { return this.mTimeZone; } set { this.mTimeZone = value; } }
        public float AdjustTimeZone { get { return this.mAdjustTimeZone; } set { this.mAdjustTimeZone = value; } }
        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }
        public UnityEvent OnAction { get { return this.mOnAction; } set { this.mOnAction = value; } }

        /* Functions */

        private void Update()
        {
            if (!mActive)
                return;

            DoAction();
        }

        /// <summary>
        /// Calculate the time to do event once.
        /// </summary>
        public void ResetTimeZone()
        {
            float adjustTime = JCS_Random.Range(-mAdjustTimeZone, mAdjustTimeZone);
            mRealTimeZone = mTimeZone + adjustTime;

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
                ResetTimeZone();

            mTimer += JCS_Time.DeltaTime(mDeltaTimeType);

            if (mRealTimeZone > mTimer)
                return;

            // active actions.
            if (onAction != null)
                onAction.Invoke();

            if (mOnAction != null)
                mOnAction.Invoke();

            mDidAction = true;
        }
    }
}
