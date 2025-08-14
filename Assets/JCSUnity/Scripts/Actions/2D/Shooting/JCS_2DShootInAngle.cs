/**
 * $File: JCS_2DShootInAngle.cs $
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
    /// Do the shoot action in specific angle.
    /// </summary>
    [RequireComponent(typeof(JCS_ShootAction))]
    public class JCS_2DShootInAngle : MonoBehaviour , JCS_IAction
    {
        /* Variables */

        private JCS_ShootAction mShootAction = null;

        [Separator("Runtime Variables (JCS_2DShootInAngle)")]

        [Tooltip("Degree per bullet shoot.")]
        [SerializeField]
        private float mDegreePerShoot = 10.0f;

        [Tooltip("Minimum degree in shoot.")]
        [SerializeField]
        private float mMinDegree = 120.0f;

        [Tooltip("Maximum degree in shoot.")]
        [SerializeField]
        private float mMaxDegree = 240.0f;

        /* Setter & Getter */

        public JCS_ShootAction GetShootAction() { return mShootAction; }
        public float degreePerShoot { get { return mDegreePerShoot; } }
        public float minDegree { get { return mMinDegree; } }
        public float maxDegree { get { return mMaxDegree; } }
        
        /* Functions */

        private void Awake()
        {
            mShootAction = GetComponent<JCS_ShootAction>();
        }
    }
}
