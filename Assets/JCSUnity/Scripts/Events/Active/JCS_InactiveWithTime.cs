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
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Inactive the game object after a certain time.
    /// </summary>
    public class JCS_InactiveWithTime : MonoBehaviour
    {
        /* Variables */

        private float mTimer = 0.0f;

        [Separator("Runtime Variables (JCS_InactiveWithTime)")]

        [Tooltip("GameObjects that take effect.")]
        [SerializeField]
        private List<GameObject> mGameObjects = null;

        [Tooltip("Take effect for this game object.")]
        [SerializeField]
        private bool mEffectSelf = true;

        [Tooltip("Time before inactive the game object.")]
        [SerializeField]
        [Range(0.0f, 3600.0f)]
        private float mTime = 2.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        /* Setter & Getter */

        public List<GameObject> GameObjects { get { return this.mGameObjects; } set { this.mGameObjects = value; } }
        public bool EffectSelf { get { return this.mEffectSelf; } set { this.mEffectSelf = value; } }
        public float time { get { return this.mTime; } set { this.mTime = value; } }
        public float timer { get { return this.mTimer; } set { this.mTimer = value; } }
        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }

        /* Functions */

        private void Update()
        {
            mTimer += JCS_Time.DeltaTime(mDeltaTimeType);

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
