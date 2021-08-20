/**
 * $File: JCS_InactiveWithTime.cs $
 * $Date: 2021-08-20 22:37:15 $
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
    /// Inactive the gameobject after a certain time.
    /// </summary>
    public class JCS_InactiveWithTime : MonoBehaviour
    {
        /* Variables */

        private float mTimer = 0.0f;

        [Header("** Runtime Variables (JCS_InactiveWithTime) **")]

        [Tooltip("GameObjects that take effect.")]
        [SerializeField]
        private List<GameObject> mGameObjects = null;

        [Tooltip("Take effect for this gameobject.")]
        [SerializeField]
        private bool mEffectSelf = true;

        [Tooltip("Time before inactive the gameobject.")]
        [SerializeField]
        [Range(0.0f, 3600.0f)]
        private float mTime = 2.0f;

        /* Setter & Getter */

        public List<GameObject> GameObjects { get { return this.mGameObjects; } set { this.mGameObjects = value; } }
        public bool EffectSelf { get { return this.mEffectSelf; } set { this.mEffectSelf = value; } }
        public float time { get { return this.mTime; } set { this.mTime = value; } }

        /* Functions */

        private void Update()
        {
            mTimer += Time.deltaTime;

            if (mTime < mTimer)
            {
                // reset timer.
                mTimer = 0.0f;

                // inactive all
                foreach (var comp in mGameObjects)
                    comp.SetActive(false);

                // inactive this
                if (mEffectSelf)
                    this.gameObject.SetActive(false);
            }
        }
    }
}
