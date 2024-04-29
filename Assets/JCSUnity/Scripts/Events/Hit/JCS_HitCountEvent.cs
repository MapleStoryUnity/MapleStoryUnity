/**
 * $File: JCS_HitCountEvent.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Event that count the hit count and check if the gameobject
    /// needed to by destroyed.
    /// </summary>
    [RequireComponent(typeof(JCS_HitListEvent))]
    public class JCS_HitCountEvent : MonoBehaviour
    {
        /* Variables */

        private JCS_HitListEvent mHitList = null;

        [Separator("Runtime Variables (JCS_HitCountEvent)")]

        [Tooltip(@"How many hits needed to destroy this gameobject. 
(If this is 0 than wont active hit animation event.)")]
        [SerializeField]
        [Range(0, 300)]
        private uint mHitCount = 1;

        /* Setter & Getter */

        public uint HitCount { get { return this.mHitCount; } set { this.mHitCount = value; } }

        /* Functions */

        private void Awake()
        {
            mHitList = this.GetComponent<JCS_HitListEvent>();
        }

        private void FixedUpdate()
        {
            if (mHitList.IsHit)
            {
                --mHitCount;
                CheckDestroy();
            }
        }

        /// <summary>
        /// Check if the object should destroy or not.
        /// </summary>
        private void CheckDestroy()
        {
            if (mHitCount <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                mHitList.IsHit = false;
            }
        }
    }
}
