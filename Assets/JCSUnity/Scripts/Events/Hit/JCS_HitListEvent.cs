/**
 * $File: JCS_HitListEvent.cs $
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
    /// Check if the object hit something on the list.
    /// </summary>
    public class JCS_HitListEvent : MonoBehaviour
    {
        /* Variables */

        [Separator("📋 Check Variabless (JCS_HitListEvent)")]

        [Tooltip("trigger if this even occurs")]
        [SerializeField]
        [ReadOnly]
        private bool mIsHit = false;

        [Separator("⚡️ Runtime Variables (JCS_HitListEvent)")]

        [Tooltip("List of object will lower 1 hit count.")]
        [SerializeField]
        private string[] mHitObjectList = null;

        [Tooltip("Base on Hit Object List will automatically add clone object.")]
        [SerializeField]
        private bool mIncludeClone = true;

        [Tooltip("When is hit, destroy this object.")]
        [SerializeField]
        private bool mDestroyWhenOccurs = false;

        /* Setter & Getter */

        public bool isHit { get { return mIsHit; } set { mIsHit = value; } }

        /* Functions */

        private void OnTriggerEnter(Collider other)
        {
            foreach (string obj in mHitObjectList)
            {
                if (other.gameObject.name == obj)
                {
                    mIsHit = true;
                    continue;
                }

                // Check clone
                if (mIncludeClone)
                {
                    if (JCS_Util.IsClone(other.gameObject))
                    {
                        mIsHit = true;
                        continue;
                    }
                }
            }

            if (mDestroyWhenOccurs && mIsHit)
            {
                Destroy(gameObject);
            }
        }
    }
}
