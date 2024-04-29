/**
 * $File: JCS_VelocityInfo.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Velocity Object must have "CharacterController"
    /// componenet involved.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class JCS_VelocityInfo : MonoBehaviour
    {
        /* Variables */

        private CharacterController mCharacterController = null;

        [Separator("Check Variables (JCS_VelocityInfo)")]

        [Tooltip("Velocity for this character controller.")]
        [SerializeField]
        [ReadOnly]
        private Vector3 mVelocity = Vector3.zero;

        [Separator("Runtime Variables (JCS_VelocityInfo)")]

        [Tooltip("Do this character controll applies gravity.")]
        [SerializeField]
        private bool mApplyGravity = true;
        
        [Tooltip("How fast it get back to original speed?")]
        [SerializeField]
        private float mSpeedFriction = 0.2f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        // 原本速度的變化量
        private Vector3 mMoveSpeed = Vector3.zero;

        // 如果你要換他原本的速度, 就用這個變量
        private Vector3 mRecordSpeed = Vector3.zero;

        [Tooltip("Freeze Velocity Horizontal Direction. (X Axis)")]
        [SerializeField]
        private bool mFreezeX = false;

        [Tooltip("Freeze Velocity Verticl Direction. (Y Axis)")]
        [SerializeField]
        private bool mFreezeY = false;

        [Tooltip("Freeze Velocity Depth Direction. (Z Axis)")]
        [SerializeField]
        private bool mFreezeZ = false;

        /* Setter & Getter */

        public CharacterController GetCharacterController() { return this.mCharacterController; }
        public Vector3 Velocity { get { return this.mVelocity; } set { this.mVelocity = value; } }
        public float VelX { get { return this.mVelocity.x; } set { this.mVelocity.x = value; } }
        public float VelY { get { return this.mVelocity.y; } set { this.mVelocity.y = value; } }
        public float VelZ { get { return this.mVelocity.z; } set { this.mVelocity.z = value; } }
        public Vector3 MoveSpeed { get { return this.mMoveSpeed; } set { this.mMoveSpeed = value; } }
        public float MoveSpeedX { get { return this.mMoveSpeed.x; } set { this.mMoveSpeed.x = value; } }
        public float MoveSpeedY { get { return this.mMoveSpeed.y; } set { this.mMoveSpeed.y = value; } }
        public float MoveSpeedZ { get { return this.mMoveSpeed.z; } set { this.mMoveSpeed.z = value; } }
        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }
        public Vector3 RecordSpeed { get { return this.mRecordSpeed; } set { this.mRecordSpeed = value; } }
        public float RecordSpeedX { get { return this.mRecordSpeed.x; } set { this.mRecordSpeed.x = value; } }
        public float RecordSpeedY { get { return this.mRecordSpeed.y; } set { this.mRecordSpeed.y = value; } }
        public float RecordSpeedZ { get { return this.mRecordSpeed.z; } set { this.mRecordSpeed.z = value; } }
        public bool ApplyGravity { get { return this.mApplyGravity; } set { this.mApplyGravity = value; } }
        public bool isGrounded { get { return this.mCharacterController.isGrounded; } }
        public bool FreezeX { get { return this.mFreezeX; } set { this.mFreezeX = value; } }
        public bool FreezeY { get { return this.mFreezeY; } set { this.mFreezeY = value; } }
        public bool FreezeZ { get { return this.mFreezeZ; } set { this.mFreezeZ = value; } }

        /* Functions */

        private void Awake()
        {
            mCharacterController = this.GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            // do freezing before move the character.
            DoFreeze();

            float dt = JCS_Time.DeltaTime(mDeltaTimeType);

            if (mCharacterController.enabled)
            {
                // apply force (moving the character)
                /**
                 * NOTE(jenchieh): Move function cost so much 
                 * performance.
                 */
                mCharacterController.Move(Velocity * dt);
            }

            // getting back speed
            this.mMoveSpeed += (this.mRecordSpeed - this.mMoveSpeed) / mSpeedFriction * Time.deltaTime;
            this.VelX = mMoveSpeed.x;
            this.VelZ = mMoveSpeed.z;

            // apply gravit
            if (ApplyGravity)
            {
                if (!isGrounded)
                {
                    VelY -= 
                        JCS_GameConstant.GRAVITY *
                        dt *
                        JCS_GameSettings.instance.GRAVITY_PRODUCT;
                }
                // if touches the ground set to negative one.
                else {
                    VelY = -1;
                }
            }
        }

        /// <summary>
        /// Set all freeze axis to the act
        /// </summary>
        /// <param name="act"> default is true </param>
        public void Freeze(bool act = true)
        {
            mFreezeX = act;
            mFreezeY = act;
            mFreezeZ = act;
        }
        /// <summary>
        /// Set all freeze axis to false, 
        /// so it no longer freezing.
        /// </summary>
        public void UnFreeze()
        {
            Freeze(false);
        }

        private void DoFreeze()
        {
            if (mFreezeX) VelX = 0;
            if (mFreezeY) VelY = 0;
            if (mFreezeZ) VelZ = 0;
        }

    }
}
