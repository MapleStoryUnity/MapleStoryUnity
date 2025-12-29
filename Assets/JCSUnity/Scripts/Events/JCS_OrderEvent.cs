/**
 * $File: JCS_OrderEvent.cs $
 * $Date: 2020-06-20 20:57:33 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Execute an operation in order with interval of time.
    /// </summary>
    public class JCS_OrderEvent : MonoBehaviour
    {
        /* Variables */

        private Action mExecution = null;

        [Separator("📋 Check Variabless (JCS_OrderEvent)")]

        [Tooltip("Flag represent the activation of the event.")]
        [SerializeField]
        [ReadOnly]
        private bool mActive = false;

        [Tooltip("Timer to run through all executions.")]
        [SerializeField]
        [ReadOnly]
        private float mTimer = 0.0f;

        [Separator("⚡️ Runtime Variables (JCS_OrderEvent)")]

        [Tooltip("Time for each execution.")]
        [SerializeField]
        private float mIntervalTime = 0.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Tooltip("Unity execution event.")]
        [SerializeField]
        private UnityEvent mUnityExecution = null;

        /* Setter & Getter */

        public bool active { get { return mActive; } }
        public float intervalTime { get { return mIntervalTime; } set { mIntervalTime = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }
        public UnityEvent unityExecution { get { return mUnityExecution; } set { mUnityExecution = value; } }

        /* Functions */

        private void Update()
        {
            DoLoop();
        }

        /// <summary>
        /// Start a new event with interval of time.
        /// </summary>
        /// <param name="intervalTime"> Interval of time to execute each operation. </param>
        /// <param name="evt"> Operation to get execute. </param>
        public void StartEvent(float intervalTime, Action evt)
        {
            mIntervalTime = intervalTime;
            mExecution = evt;
            mTimer = 0.0f;
            mActive = true;
        }

        /// <summary>
        /// Terminate the current event loop.
        /// </summary>
        public void DoneEvent()
        {
            mActive = false;
        }

        /// <summary>
        /// Loop through the execution.
        /// </summary>
        private void DoLoop()
        {
            if (!mActive)
                return;

            mTimer += JCS_Time.ItTime(mTimeType);

            if (mTimer < mIntervalTime)
                return;

            mExecution?.Invoke();

            mUnityExecution?.Invoke();

            mTimer = 0.0f;
        }
    }
}
