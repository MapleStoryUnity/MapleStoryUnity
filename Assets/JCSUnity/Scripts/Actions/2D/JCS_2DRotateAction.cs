/**
 * $File: JCS_2DRotateAction.cs $
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
    /// Action that make game object rotates.
    /// </summary>
    public class JCS_2DRotateAction : MonoBehaviour, JCS_IAction
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_2DRotateAction)")]

        [Tooltip("Do the action?")]
        [SerializeField]
        private bool mAction = true;

        [Tooltip("How fast it rotates?")]
        [SerializeField]
        [Range(-1000.0f, 1000.0f)]
        private float mTurnSpeed = 1000.0f;

        [Tooltip("Rotate direction.")]
        [SerializeField]
        private JCS_2DFaceType mRotateDirection = JCS_2DFaceType.FACE_LEFT;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        /* Setter & Getter */

        public bool Action { get { return this.mAction; } set { this.mAction = value; } }
        public float TurnSpeed { get { return this.mTurnSpeed; } set { this.mTurnSpeed = value; } }
        public JCS_2DFaceType RotateDirection { get { return this.mRotateDirection; } set { this.mRotateDirection = value; } }
        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }

        /* Functions */

        private void Update()
        {
            if (!mAction)
                return;

            this.transform.Rotate(Vector3.forward * mTurnSpeed * -((int)mRotateDirection) * JCS_Time.DeltaTime(mDeltaTimeType));
        }
    }
}
