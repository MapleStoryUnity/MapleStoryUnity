/**
 * $File: JCS_3DTrackAction.cs $
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
    /// Track a game object in 3D space.
    /// </summary>
    public class JCS_3DTrackAction : MonoBehaviour, JCS_IAction
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_3DTrackAction)")]

        [Tooltip("Transform we want to target.")]
        [SerializeField]
        private Transform mTargetTransform = null;

        [Tooltip("Invers of speed.")]
        [SerializeField]
        private float mMoveFriction = 1.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        /* Setter & Getter */

        public Transform TargetTransform { get { return this.mTargetTransform; } set { this.mTargetTransform = value; } }
        public float MoveFriction { get { return this.mMoveFriction; } set { this.mMoveFriction = value; } }
        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }

        /* Functions */

        private void Update()
        {
            FollowObject();
        }

        /// <summary>
        /// Do the following game object action.
        /// </summary>
        private void FollowObject()
        {
            if (mTargetTransform == null)
                return;

            Vector3 targetPos = mTargetTransform.position;
            Vector3 newPos = this.transform.position;

            newPos += (targetPos - newPos) / mMoveFriction * JCS_Time.DeltaTime(mDeltaTimeType);

            this.transform.position = newPos;
        }
    }
}
