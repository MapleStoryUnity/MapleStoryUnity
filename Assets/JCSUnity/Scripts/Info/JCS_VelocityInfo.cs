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

        [Separator("📋 Check Variabless (JCS_VelocityInfo)")]

        [Tooltip("Velocity for this character controller.")]
        [SerializeField]
        [ReadOnly]
        private Vector3 mVelocity = Vector3.zero;

        [Separator("⚡️ Runtime Variables (JCS_VelocityInfo)")]

        [Tooltip("Do this character controll applies gravity.")]
        [SerializeField]
        private bool mApplyGravity = true;

        [Tooltip("How fast it get back to original speed?")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 10.0f)]
        private float mSpeedFriction = 0.2f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

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

        public CharacterController GetCharacterController() { return mCharacterController; }
        public Vector3 velocity { get { return mVelocity; } set { mVelocity = value; } }
        public float velX { get { return mVelocity.x; } set { mVelocity.x = value; } }
        public float velY { get { return mVelocity.y; } set { mVelocity.y = value; } }
        public float velZ { get { return mVelocity.z; } set { mVelocity.z = value; } }
        public Vector3 moveSpeed { get { return mMoveSpeed; } set { mMoveSpeed = value; } }
        public float moveSpeedX { get { return mMoveSpeed.x; } set { mMoveSpeed.x = value; } }
        public float moveSpeedY { get { return mMoveSpeed.y; } set { mMoveSpeed.y = value; } }
        public float moveSpeedZ { get { return mMoveSpeed.z; } set { mMoveSpeed.z = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }
        public Vector3 recordSpeed { get { return mRecordSpeed; } set { mRecordSpeed = value; } }
        public float recordSpeedX { get { return mRecordSpeed.x; } set { mRecordSpeed.x = value; } }
        public float recordSpeedY { get { return mRecordSpeed.y; } set { mRecordSpeed.y = value; } }
        public float recordSpeedZ { get { return mRecordSpeed.z; } set { mRecordSpeed.z = value; } }
        public bool applyGravity { get { return mApplyGravity; } set { mApplyGravity = value; } }
        public bool isGrounded { get { return mCharacterController.isGrounded; } }
        public bool freezeX { get { return mFreezeX; } set { mFreezeX = value; } }
        public bool freezeY { get { return mFreezeY; } set { mFreezeY = value; } }
        public bool freezeZ { get { return mFreezeZ; } set { mFreezeZ = value; } }

        /* Functions */

        private void Awake()
        {
            mCharacterController = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            // do freezing before move the character.
            DoFreeze();

            float dt = JCS_Time.ItTime(mTimeType);

            if (mCharacterController.enabled)
            {
                // apply force (moving the character)
                /**
                 * NOTE(jenchieh): Move function cost so much 
                 * performance.
                 */
                mCharacterController.Move(mVelocity * dt);
            }

            // getting back speed
            mMoveSpeed += (mRecordSpeed - mMoveSpeed) / mSpeedFriction * Time.deltaTime;
            velX = mMoveSpeed.x;
            velZ = mMoveSpeed.z;

            // apply gravit
            if (mApplyGravity)
            {
                if (!isGrounded)
                {
                    velY +=
                        JCS_Physics.GRAVITY *
                        dt *
                        JCS_GameSettings.FirstInstance().gravityProduct;
                }
                // if touches the ground set to negative one.
                else
                {
                    velY = -1;
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
            if (mFreezeX) velX = 0;
            if (mFreezeY) velY = 0;
            if (mFreezeZ) velZ = 0;
        }

    }
}
