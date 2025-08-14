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
        [Range(JCS_Constants.FRICTION_MIN, 10.0f)]
        private float mMoveFriction = 1.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public Transform targetTransform { get { return mTargetTransform; } set { mTargetTransform = value; } }
        public float moveFriction { get { return mMoveFriction; } set { mMoveFriction = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

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
            Vector3 newPos = transform.position;

            newPos += (targetPos - newPos) / mMoveFriction * JCS_Time.ItTime(mTimeType);

            transform.position = newPos;
        }
    }
}
