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

namespace JCSUnity
{
    /// <summary>
    /// Disable behaviours after a certain time.
    /// </summary>
    public class JCS_DisableWithTimeEvent : MonoBehaviour
    {
        /* Variables */

        private float mTimer = 0.0f;

        [Header("** Runtime Variables (JCS_DisableWithTimeEvent) **")]

        [Tooltip("Behaviours that take effect.")]
        [SerializeField]
        private List<Behaviour> mBehaviours = null;

        [Tooltip("Time before disable.")]
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

                // disable all components
                foreach (var comp in mBehaviours)
                    comp.enabled = false;
            }
        }
    }
}
