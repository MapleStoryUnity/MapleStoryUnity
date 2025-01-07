/**
 * $File: JCS_Rotation.cs $
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
    /// Do the rotate action depends on axis.
    /// </summary>
    public class JCS_Rotation : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_Rotation)")]

        [Tooltip("Do the effect?")]
        [SerializeField]
        protected bool mEffect = false;

        [Tooltip("How fast it rotates?")]
        [SerializeField]
        [Range(-1000, 1000)]
        protected float mRotateSpeed = 10;

        [Tooltip("Direction it rotates.")]
        [SerializeField]
        protected JCS_Vector3Direction mRotateDirection = JCS_Vector3Direction.FORWARD;

        [Tooltip("Rotate by its transform.")]
        [SerializeField]
        protected bool mBySelf = false;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        protected JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        [Header("- Random Effect")]

        [Tooltip("Randomize the rotation speed a bit at start.")]
        [SerializeField]
        protected bool mRotateRandomizeAtStart = false;

        [Tooltip("Value to randomize.")]
        [SerializeField]
        [Range(0.0f, 1000.0f)]
        protected float mRotateRandomizeAtStartValue = 0.0f;

        [Tooltip("Randomize the rotate direction on start.")]
        [SerializeField]
        protected bool mRandomRotateDirectonAtStart = false;

        /* Setter & Getter */

        public bool Effect { get { return this.mEffect; } set { this.mEffect = value; } }
        public float RotateSpeed { get { return this.mRotateSpeed; } set { this.mRotateSpeed = value; } }
        public JCS_Vector3Direction RotateDirection { get { return this.mRotateDirection; } set { this.mRotateDirection = value; } }
        public bool BySelf { get { return this.mBySelf; } set { this.mBySelf = value; } }
        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }

        /* Functions */

        protected virtual void Awake()
        {
            // randomize the rotate speed at start?
            if (mRotateRandomizeAtStart)
                RandomizeTheRotateSpeed();

            // randomize the rotate direction.
            if (mRandomRotateDirectonAtStart)
                RandomizeTheRotateDirection();
        }

        protected virtual void Update()
        {
            if (!Effect)
                return;

            DoRotation();
        }

        /// <summary>
        /// Stop doing the effect.
        /// </summary>
        public void Stop()
        {
            Effect = false;
            this.transform.localEulerAngles = Vector3.zero;
        }

        /// <summary>
        /// Algorithm do the rotate effect.
        /// </summary>
        private void DoRotation()
        {
            Vector3 rotateDirection;

            if (mBySelf)
                rotateDirection = JCS_Vector.Direction(mRotateDirection, this.transform);
            else
                rotateDirection = JCS_Vector.Direction(mRotateDirection);

            transform.Rotate(rotateDirection * mRotateSpeed * JCS_Time.DeltaTime(mDeltaTimeType));
        }

        /// <summary>
        /// Randomize the rotate speed.
        /// </summary>
        private void RandomizeTheRotateSpeed()
        {
            RandomizeTheRotateSpeed(mRotateRandomizeAtStartValue);
        }
        /// <summary>
        /// Randomize the value.
        /// </summary>
        /// <param name="value"> how much it randomize. </param>
        private void RandomizeTheRotateSpeed(float value)
        {
            // NOTE(jenchieh): this make sure the min/max are fit.
            float val = JCS_Mathf.ToPositive(value);

            // add up he randomize value.
            mRotateSpeed += JCS_Random.Range(-val, val);
        }

        /// <summary>
        /// Randomize the rotate direction.
        /// </summary>
        private void RandomizeTheRotateDirection()
        {
            // NOTE(jenchieh): see JCS_Vector3Direction.cs enum.
            // there are 0 ~ 26 options.
            int directionSize = JCS_Random.Range(0, 26);

            mRotateDirection = (JCS_Vector3Direction)directionSize;
        }
    }
}
