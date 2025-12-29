/**
 * $File: JCS_EnableWithTimeEvent.cs $
 * $Date: 2021-08-20 22:31:11 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Enable components after a certain time.
    /// </summary>
    public class JCS_EnableWithTimeEvent : MonoBehaviour
    {
        /* Variables */

        private float mTimer = 0.0f;

        [Separator("📋 Check Variabless (JCS_EnableWithTimeEvent)")]

        [Tooltip("Turn it to true when the task is completed.")]
        [SerializeField]
        [ReadOnly]
        private bool mDone = false;

        [Separator("⚡️ Runtime Variables (JCS_EnableWithTimeEvent)")]

        [Tooltip("Components that take effect.")]
        [SerializeField]
        private List<Component> mComponents = null;

        [Tooltip("Time before enable.")]
        [SerializeField]
        [Range(0.0f, 3600.0f)]
        private float mTime = 2.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public bool done { get { return mDone; } }
        public List<Component> components { get { return mComponents; } set { mComponents = value; } }
        public float time { get { return mTime; } set { mTime = value; } }
        public float timer { get { return mTimer; } set { mTimer = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        /* Functions */

        private void Update()
        {
            if (mDone)
                return;

            mTimer += JCS_Time.ItTime(mTimeType);

            if (mTime < mTimer)
            {
                // reset timer.
                mTimer = 0.0f;

                // enable all components
                foreach (var comp in mComponents)
                    JCS_Util.EnableComponent(comp, true);

                mDone = true;
            }
        }
    }
}
