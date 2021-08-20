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
    /// Enable behaviours after a certain time.
    /// </summary>
    public class JCS_EnableWithTimeEvent : MonoBehaviour
    {
        /* Variables */

        private float mTimer = 0.0f;

        [Header("** Runtime Variables (JCS_EnableWithTimeEvent) **")]

        [Tooltip("Behaviours that take effect.")]
        [SerializeField]
        private List<Behaviour> mBehaviours = null;

        [Tooltip("Time before enable.")]
        [SerializeField]
        [Range(0.0f, 3600.0f)]
        private float mTime = 2.0f;

        /* Setter & Getter */

        public List<Behaviour> Behaviours { get { return this.mBehaviours; } set { this.mBehaviours = value; } }
        public float time { get { return this.mTime; } set { this.mTime = value; } }

        /* Functions */

        private void Update()
        {
            mTimer += Time.deltaTime;

            if (mTime < mTimer)
            {
                // reset timer.
                mTimer = 0.0f;

                // enable all components
                foreach (var comp in mBehaviours)
                    comp.enabled = true;
            }
        }
    }
}
