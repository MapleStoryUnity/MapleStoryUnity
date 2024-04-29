/**
 * $File: JCS_HopEffect.cs $
 * $Date: 2019-07-29 16:32:44 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2019 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Make object hop for a force.
    /// </summary>
    public class JCS_HopEffect : JCS_UnityObject
    {
        /* Variables */

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_HopEffect)")]

        [Tooltip("Test the module with key.")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key that record starting position and hops.")]
        [SerializeField]
        private KeyCode mHopRecordKey = KeyCode.S;

        [Tooltip("Key that does normal hop effect.")]
        [SerializeField]
        private KeyCode mHopKey = KeyCode.A;
#endif

        [Separator("Check Variables (JCS_HopEffect)")]

        [Tooltip("Flag for the effect activation.")]
        [SerializeField]
        [ReadOnly]
        private bool mActive = false;

        [Tooltip("Position before hopping.")]
        [SerializeField]
        [ReadOnly]
        private Vector3 mStartingPosition = Vector3.zero;

        private float mRealGravity = 0.0f;

        [Separator("Runtime Variables (JCS_HopEffect)")]

        [Tooltip("How much force to jump away from current position.")]
        [SerializeField]
        [Range(-300.01f, 300.0f)]
        private float mForce = 10.0f;

        [Tooltip("How fast the object goes back to original position.")]
        [SerializeField]
        [Range(0.01f, 1000.0f)]
        private float mGravity = 10.0f;

        [Tooltip("Hop axis.")]
        [SerializeField]
        private JCS_Axis mAxis = JCS_Axis.AXIS_Y;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        private Vector3 mVelocity = Vector3.zero;

        /* Setter & Getter */

        public float Force { get { return this.mForce; } set { this.mForce = value; } }
        public float Gravity { get { return this.mGravity; } set { this.mGravity = value; } }
        public JCS_Axis Axis { get { return this.mAxis; } set { this.mAxis = value; } }
        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }

        /* Functions */

        private void Update()
        {
#if UNITY_EDITOR
            TestInput();
#endif
            DoHop();
        }

#if UNITY_EDITOR
        private void TestInput()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(mHopRecordKey))
            {
                StartHop(true);
            }
            else if (JCS_Input.GetKeyDown(mHopKey))
            {
                StartHop(false);
            }
        }
#endif

        /// <summary>
        /// Start the hopping process.
        /// </summary>
        public void StartHop(bool recordStarting = true)
        {
            StartHop(this.mForce, this.mGravity, recordStarting);
        }

        /// <summary>
        /// Start the hopping process.
        /// </summary>
        public void StartHop(float force, float gravity, bool recordStarting = true)
        {
            mActive = true;

            if (recordStarting)
                mStartingPosition = this.LocalPosition;

            switch (mAxis)
            {
                case JCS_Axis.AXIS_X:
                    mVelocity.x = this.mForce;
                    break;
                case JCS_Axis.AXIS_Y:
                    mVelocity.y = this.mForce;
                    break;
                case JCS_Axis.AXIS_Z:
                    mVelocity.z = this.mForce;
                    break;
            }
        }

        /// <summary>
        /// Do the hopping process.
        /// </summary>
        private void DoHop()
        {
            if (!mActive)
                return;

            float dt = JCS_Time.DeltaTime(mDeltaTimeType);

            float prevPos = GetLocalPositionByAxis();
            this.LocalPosition += mVelocity * dt;
            float currPos = GetLocalPositionByAxis();

            float startingPos = GetStartingPositionByAxis();

            if ((prevPos < startingPos && currPos > startingPos) ||
                (prevPos > startingPos && currPos < startingPos))
            {
                SetLocalPositionByAxis(startingPos);
                mActive = false;
            }

            switch (mAxis)
            {
                case JCS_Axis.AXIS_X:
                    {
                        if (this.LocalPosition.x > mStartingPosition.x)
                            mRealGravity = JCS_Mathf.ToNegative(mGravity);
                        else
                            mRealGravity = JCS_Mathf.ToPositive(mGravity);

                        mVelocity.x += mRealGravity * dt;
                    }
                    break;
                case JCS_Axis.AXIS_Y:
                    {
                        if (this.LocalPosition.y > mStartingPosition.y)
                            mRealGravity = JCS_Mathf.ToNegative(mGravity);
                        else
                            mRealGravity = JCS_Mathf.ToPositive(mGravity);

                        mVelocity.y += mRealGravity * dt;
                    }
                    break;
                case JCS_Axis.AXIS_Z:
                    {
                        if (this.LocalPosition.z > mStartingPosition.z)
                            mRealGravity = JCS_Mathf.ToNegative(mGravity);
                        else
                            mRealGravity = JCS_Mathf.ToPositive(mGravity);

                        mVelocity.z += mRealGravity * dt;
                    }
                    break;
            }
        }

        private void SetLocalPositionByAxis(float val)
        {
            Vector3 newPos = this.LocalPosition;
            switch (mAxis)
            {
                case JCS_Axis.AXIS_X: newPos.x = val; break;
                case JCS_Axis.AXIS_Y: newPos.y = val; break;
                case JCS_Axis.AXIS_Z: newPos.z = val; break;

            }
            this.LocalPosition = newPos;
        }

        private float GetLocalPositionByAxis()
        {
            float val = 0.0f;

            switch (mAxis)
            {
                case JCS_Axis.AXIS_X: val = LocalPosition.x; break;
                case JCS_Axis.AXIS_Y: val = LocalPosition.y; break;
                case JCS_Axis.AXIS_Z: val = LocalPosition.z; break;

            }
            return val;
        }

        private float GetStartingPositionByAxis()
        {
            float val = 0.0f;

            switch (mAxis)
            {
                case JCS_Axis.AXIS_X: val = mStartingPosition.x; break;
                case JCS_Axis.AXIS_Y: val = mStartingPosition.y; break;
                case JCS_Axis.AXIS_Z: val = mStartingPosition.z; break;

            }
            return val;
        }
    }
}
