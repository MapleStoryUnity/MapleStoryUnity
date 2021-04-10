/**
 * $File: JCS_AttackerRecorder.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Record down the attacker that attacked
    /// this transform object.
    /// </summary>
    public class JCS_AttackerRecorder
        : MonoBehaviour
    {
        /* Variables */

        [Header("** Check Variables (JCS_AttackerRecorder) **")]

        // Every time this object get damage will 
        // record down the last attacker.
        [Tooltip("Last attacker that attacked this object.")]
        [SerializeField]
        private Transform mLastAttacker= null;

        // Record all the attacker attack this enemy!
        private JCS_Vector<Transform> mAttackers = null;

        /* Setter & Getter */

        public Transform LastAttacker { get { return this.mLastAttacker; } set { this.mLastAttacker = value; } }
        public JCS_Vector<Transform> GetAttackers() { return this.mAttackers; }

        /* Functions */

        private void Awake()
        {
            mAttackers = new JCS_Vector<Transform>();
        }
    }
}
