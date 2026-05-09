/**
 * $File: JCS_AttackerRecorder.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Record down the attacker that attacked
    /// this transform object.
    /// </summary>
    public class JCS_AttackerRecorder : MonoBehaviour
    {
        /* Variables */

        [Separator("📋 Check Variables (JCS_AttackerRecorder)")]

        // Every time this object get damage will 
        // record down the last attacker.
        [Tooltip("Last attacker that attacked this object.")]
        [SerializeField]
        private Transform mLastAttacker= null;

        // Record all the attacker attack this enemy!
        private List<Transform> mAttackers = null;

        /* Setter & Getter */

        public Transform lastAttacker { get { return mLastAttacker; } set { mLastAttacker = value; } }
        public List<Transform> GetAttackers() { return mAttackers; }

        /* Functions */

        private void Awake()
        {
            mAttackers = new List<Transform>();
        }
    }
}
