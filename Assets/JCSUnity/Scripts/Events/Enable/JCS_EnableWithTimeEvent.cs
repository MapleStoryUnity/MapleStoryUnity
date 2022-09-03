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

namespace JCSUnity
{
    /// <summary>
    /// Enable components after a certain time.
    /// </summary>
    public class JCS_EnableWithTimeEvent : MonoBehaviour
    {
        /* Variables */

        private float mTimer = 0.0f;

        [Header("** Check Variables (JCS_EnableWithTimeEvent) **")]

        [Tooltip("Turn it to true when the task is completed.")]
        [SerializeField]
        private bool mDone = false;

        [Header("** Runtime Variables (JCS_EnableWithTimeEvent) **")]

        [Tooltip("Components that take effect.")]
        [SerializeField]
        private List<Component> mComponents = null;

        [Tooltip("Time before enable.")]
        [SerializeField]
        [Range(0.0f, 3600.0f)]
        private float mTime = 2.0f;

        /* Setter & Getter */

        public bool Done { get { return this.mDone; } }
        public List<Component> Components { get { return this.mComponents; } set { this.mComponents = value; } }
        public float time { get { return this.mTime; } set { this.mTime = value; } }
        public float timer { get { return this.mTimer; } set { this.mTimer = value; } }

        /* Functions */

        private void Update()
        {
            if (mDone)
                return;

            mTimer += Time.deltaTime;

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
