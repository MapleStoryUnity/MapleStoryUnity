/**
 * $File: JCS_2DCircleAction.cs $
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
    /// Do the circle action.
    /// </summary>
    public class JCS_2DCircleAction : MonoBehaviour , JCS_IAction
    {
        /* Variables */

        [Separator("Initialize Variables (JCS_2DCircleAction)")]

        [Tooltip("Starting rotation degree.")]
        [SerializeField]
        private float mStartingDegree = 0.0f;

        [Tooltip("Full rotate degree.")]
        [SerializeField]
        private int mRotateDegree = 360;

        private float mRotateDegreeTimer = 0.0f;

        [Separator("Runtime Variables (JCS_2DCircleAction)")]

        [Tooltip("Do the movement.")]
        [SerializeField]
        private bool mDoMovement = false;

        [Tooltip("Do the rotation.")]
        [SerializeField]
        private bool mDoRotate = false;

        [Tooltip("How fast the force apply.")]
        [SerializeField]
        private float mMoveSpeed = 10.0f;

        [Tooltip("How fast it rotate.")]
        [SerializeField]
        private float mTurnSpeed = 150.0f;

        [Tooltip("Rotate direction.")]
        [SerializeField]
        private JCS_2DFaceType mRotateDirection = JCS_2DFaceType.FACE_LEFT;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public float StartingPosition { get { return this.mStartingDegree; } set { this.mStartingDegree = value; } }
        public int RotateDegree { get { return this.mRotateDegree; } set { this.mRotateDegree = value; } }
        public float RotateDegreeTimer { get { return this.mRotateDegreeTimer; } set { this.mRotateDegreeTimer = value; } }
        public bool DoMovement { get { return this.mDoMovement; } set { this.mDoMovement = value; } }
        public bool DoRotate { get { return this.mDoRotate; } set { this.mDoRotate = value; } }
        public float MoveSpeed { get { return this.mMoveSpeed; } set { this.mMoveSpeed = value; } }
        public float TurnSpeed { get { return this.mTurnSpeed; } set { this.mTurnSpeed = value; } }
        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        /* Functions */

        private void Awake()
        {
            // apply the starting position
            this.transform.Rotate(Vector3.forward, -mStartingDegree);
        }

        private void Update()
        {
            Movement();
            DoRotation();
        }

        /// <summary>
        /// Do the movement behavior.
        /// </summary>
        private void Movement()
        {
            if (!mDoMovement)
                return;

            // Do movement
            this.transform.Translate(Vector3.right * MoveSpeed * JCS_Time.ItTime(mTimeType));
        }

        /// <summary>
        /// Do the rotation behavior.
        /// </summary>
        private void DoRotation()
        {
            if (!mDoRotate)
                return;

            float dt = JCS_Time.ItTime(mTimeType);

            mRotateDegreeTimer += mTurnSpeed * dt;

            if (mRotateDegreeTimer > mRotateDegree)
            {
                mRotateDegreeTimer = 0;
                mDoRotate = false;
                return;
            }

            // Do rotation!
            this.transform.Rotate(Vector3.forward * mTurnSpeed * -((int)mRotateDirection) * dt);
        }
    }
}
