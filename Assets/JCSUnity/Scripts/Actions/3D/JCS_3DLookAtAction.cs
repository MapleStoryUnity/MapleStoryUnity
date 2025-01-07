/**
 * $File: JCS_3DLookAtAction.cs $
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
    /// Look at a transform in 3D space.
    /// </summary>
    public class JCS_3DLookAtAction : MonoBehaviour, JCS_IAction
    {
        /* Variables */

        public enum State
        {
            POSITIVE = 1,
            NEGATIVE = -1
        };

        [Separator("Runtime Variables (JCS_3DLookAtAction)")]

        [Tooltip("Do the action?")]
        [SerializeField]
        private bool mLookAction = true;

        [Tooltip("Target we are going to look at.")]
        [SerializeField]
        private Transform mTargetTransform = null;

        [Tooltip("Direction the object looking at.")]
        [SerializeField]
        private JCS_Vector3Direction mLookDirection = JCS_Vector3Direction.UP;

        [Tooltip("Is the direction positive or negative value?")]
        [SerializeField]
        private State mState = State.POSITIVE;

        [Tooltip("Some reason the formula isn't seem correct?")]
        [SerializeField]
        private bool mRotateBack90 = false;

        [Tooltip("Freeze x axis?")]
        [SerializeField]
        private bool mFreezeX = false;
        [Tooltip("Freeze y axis?")]
        [SerializeField]
        private bool mFreezeY = false;
        [Tooltip("Freeze z axis?")]
        [SerializeField]
        private bool mFreezeZ = false;

        [Tooltip("Offset each angle.")]
        [SerializeField]
        private Vector3 mAngleOffset = Vector3.zero;

        [Tooltip("Track as local euler angles.")]
        [SerializeField]
        private bool mLocalEulerAngles = false;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        [Header("- Asymptotic Look")]

        [Tooltip("Did the object gradually look at the target?")]
        [SerializeField]
        private bool mAsympLook = false;

        [Tooltip("How it look at the target?")]
        [SerializeField]
        [Range(0.01f, 10.0f)]
        private float mLookFriction = 0.4f;

        // record the current rotation every frame, in order to set it back
        // to original rotation which is the freezing effect.
        private Vector3 mCurrentEulerAngles = Vector3.zero;

        /* Setter & Getter */

        public Transform GetTargetTransform() { return this.mTargetTransform; }
        public void SetTargetTransform(Transform t) { this.mTargetTransform = t; }
        public JCS_Vector3Direction LookDirection { get { return this.mLookDirection; } set { this.mLookDirection = value; } }
        public State state { get { return this.mState; } set { this.mState = value; } }
        public bool RotateBack90 { get { return this.mRotateBack90; } set { this.mRotateBack90 = value; } }

        public bool LookAction { get { return this.mLookAction; } set { this.mLookAction = value; } }
        public bool LocalEulerAngles { get { return this.mLocalEulerAngles; } set { this.mLocalEulerAngles = value; } }
        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }

        public bool AsympLook { get { return this.mAsympLook; } set { this.mAsympLook = value; } }
        public float LookFriction { get { return this.mLookFriction; } set { this.mLookFriction = value; } }

        /* Functions */

        private void LateUpdate()
        {
            // check if the effect and target avaliable?
            if (mTargetTransform == null || !mLookAction)
                return;

            // record the current rotation.
            if (mLocalEulerAngles)
                mCurrentEulerAngles = this.transform.localEulerAngles;
            else
                mCurrentEulerAngles = this.transform.eulerAngles;

            DoLookAt();
            DoAsympLook();
            DoFreeze();
        }

        /// <summary>
        /// Do the look at algorithm here.
        /// </summary>
        private void DoLookAt()
        {
            if (mAsympLook)
                return;

            Vector3 lookPoint = mTargetTransform.position;
            // get direction according to the type.
            Vector3 direction = JCS_Vector.Direction(mLookDirection);

            transform.LookAt(lookPoint, direction * (int)mState);

            // apply offset angle
            if (mLocalEulerAngles)
                this.transform.localEulerAngles += mAngleOffset;
            else
                this.transform.eulerAngles += mAngleOffset;

            // TODO(jenchieh): study the rotation going on in Unity lower 
            // level archietecture.
            //
            // rotate back to X-axis.
            if (mRotateBack90)
                transform.Rotate(0, -90, 0);
        }

        /// <summary>
        /// Do the asymptotic look algorithm here.
        /// </summary>
        private void DoAsympLook()
        {
            // check if the effect active?
            if (!mAsympLook)
                return;

            Vector3 forward = mTargetTransform.position - transform.position;

            if (forward == Vector3.zero)
                return;

            Vector3 direction = JCS_Vector.Direction(mLookDirection);

            Quaternion dir = Quaternion.LookRotation(forward, direction * (int)mState);

            dir.eulerAngles += mAngleOffset;

            transform.rotation = Quaternion.Slerp(transform.rotation, dir, 1.0f / mLookFriction * JCS_Time.DeltaTime(mDeltaTimeType));
        }

        /// <summary>
        /// Do the freeze rotation?
        /// </summary>
        private void DoFreeze()
        {
            Vector3 newEulerAngles = this.transform.eulerAngles;
            if (mLocalEulerAngles)
                newEulerAngles = this.transform.localEulerAngles;

            //------------------------------------------------
            // if freeze, set to previous rotation.
            if (mFreezeX)
                newEulerAngles.x = mCurrentEulerAngles.x;

            if (mFreezeY)
                newEulerAngles.y = mCurrentEulerAngles.y;

            if (mFreezeZ)
                newEulerAngles.z = mCurrentEulerAngles.z;
            //------------------------------------------------

            if (mLocalEulerAngles)
                this.transform.localEulerAngles = newEulerAngles;
            else
                this.transform.eulerAngles = newEulerAngles;
        }
    }
}
