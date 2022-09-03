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
    /// Disable components after a certain time.
    /// </summary>
    public class JCS_DisableWithTimeEvent : MonoBehaviour
    {
        /* Variables */

        private float mTimer = 0.0f;

        [Header("** Check Variables (JCS_DisableWithTimeEvent) **")]

        [Tooltip("Turn it to true when the task is completed.")]
        [SerializeField]
        private bool mDone = false;

        [Header("** Runtime Variables (JCS_DisableWithTimeEvent) **")]

        [Tooltip("Components that take effect.")]
        [SerializeField]
        private List<Component> mComponents = null;

        [Tooltip("Time before disable.")]
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

                // disable all components
                foreach (var comp in mComponents)
                    JCS_Util.EnableComponent(comp, false);

                mDone = true;
            }
        }
    }
}
