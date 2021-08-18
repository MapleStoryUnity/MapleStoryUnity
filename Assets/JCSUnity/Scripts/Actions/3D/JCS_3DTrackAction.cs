/**
 * $File: JCS_3DTrackAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Track a gameobject in 3D space.
    /// </summary>
    public class JCS_3DTrackAction : MonoBehaviour , JCS_Action
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_3DTrackAction) **")]

        [Tooltip("Transform we want to target.")]
        [SerializeField]
        private Transform mTargetTransform = null;

        [Tooltip("Invers of speed.")]
        [SerializeField]
        private float mMoveFriction = 1.0f;

        /* Setter & Getter */

        public Transform TargetTransform { get { return this.mTargetTransform; } set { this.mTargetTransform = value; } }
        public float MoveFriction { get { return this.mMoveFriction; } set { this.mMoveFriction = value; } }

        /* Functions */

        private void Update()
        {
            FollowObject();
        }

        /// <summary>
        /// Do the following gameobject action.
        /// </summary>
        private void FollowObject()
        {
            if (mTargetTransform == null)
                return;

            Vector3 targetPos = mTargetTransform.position;
            Vector3 newPos = this.transform.position;

            newPos += (targetPos - newPos) / mMoveFriction * Time.deltaTime;

            this.transform.position = newPos;
        }
    }
}
