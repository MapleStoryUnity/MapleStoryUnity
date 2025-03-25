/**
 * $File: JCS_DisableWithTimeEvent.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Disable components after a certain time.
    /// </summary>
    public class JCS_DisableWithTimeEvent : MonoBehaviour
    {
        /* Variables */

        private float mTimer = 0.0f;

        [Separator("Check Variables (JCS_DisableWithTimeEvent)")]

        [Tooltip("Turn it to true when the task is completed.")]
        [SerializeField]
        [ReadOnly]
        private bool mDone = false;

        [Separator("Runtime Variables (JCS_DisableWithTimeEvent)")]

        [Tooltip("Components that take effect.")]
        [SerializeField]
        private List<Component> mComponents = null;

        [Tooltip("Time before disable.")]
        [SerializeField]
        [Range(0.0f, 3600.0f)]
        private float mTime = 2.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public bool Done { get { return this.mDone; } }
        public List<Component> Components { get { return this.mComponents; } set { this.mComponents = value; } }
        public float time { get { return this.mTime; } set { this.mTime = value; } }
        public float timer { get { return this.mTimer; } set { this.mTimer = value; } }
        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

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

                // disable all components
                foreach (var comp in mComponents)
                    JCS_Util.EnableComponent(comp, false);

                mDone = true;
            }
        }
    }
}
