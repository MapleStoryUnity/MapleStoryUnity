/**
 * $File: JCS_3DPlayer.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// 3D base player.
    /// </summary>
    public class JCS_3DPlayer : JCS_Player
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_3DPlayer)")]

        [Tooltip("How fast it rotates.")]
        [SerializeField]
        [Range(0.0f, 3000.0f)]
        protected float mRotateSpeed = 25.0f;

        /* Setter & Getter */

        public float RotateSpeed { get { return this.mRotateSpeed; } }

        /* Functions */

        protected override void Update()
        {
            base.Update();

            PlayerInput();
        }

        protected override void FixedUpdate()
        {
            if (!mCharacterController.isGrounded)
                mVelocity.y += (JCS_Physics.GRAVITY * JCS_Time.ItTime(mTimeType) * JCS_GameSettings.FirstInstance().GRAVITY_PRODUCT);

            base.FixedUpdate();
        }

        /// <summary>
        /// Control this player or not base on boolean pass in.
        /// </summary>
        /// <param name="act"> 
        /// true : control. 
        /// false : not control. 
        /// </param>
        public override void ControlEnable(bool act)
        {
            // enable all the component here
            if (act)
            {
                mIsControllable = true;
            }
            // diable all the component here
            else
            {
                mIsControllable = false;
            }
        }

        /// <summary>
        /// Play input's design.
        /// </summary>
        protected virtual void PlayerInput()
        {
            if (JCS_Input.GetKey(KeyCode.RightArrow))
            {
                mVelocity.x = mMoveSpeed;
            }
            else if (JCS_Input.GetKey(KeyCode.LeftArrow))
            {
                mVelocity.x = -mMoveSpeed;
            }
            else
            {
                mVelocity.x = 0;
            }

            if (JCS_Input.GetKey(KeyCode.UpArrow))
            {
                mVelocity.z = mMoveSpeed;
            }
            else if (JCS_Input.GetKey(KeyCode.DownArrow))
            {
                mVelocity.z = -mMoveSpeed;
            }
            else
            {
                mVelocity.z = 0;
            }

            float dt = JCS_Time.ItTime(mTimeType);

            if (JCS_Input.GetKey(KeyCode.L))
                this.transform.Rotate(Vector3.up * mRotateSpeed * dt);
            else if (JCS_Input.GetKey(KeyCode.J))
                this.transform.Rotate(Vector3.up * -mRotateSpeed * dt);
        }

        public override void Stand()
        {
            throw new NotImplementedException();
        }
        public override void Attack()
        {
            throw new NotImplementedException();
        }
        public override void Jump()
        {
            throw new NotImplementedException();
        }
        public override void Prone()
        {
            throw new NotImplementedException();
        }
        public override void Alert()
        {
            throw new NotImplementedException();
        }
        public override void Ladder()
        {
            throw new NotImplementedException();
        }
        public override void Rope()
        {
            throw new NotImplementedException();
        }
        public override void Die()
        {
            throw new NotImplementedException();
        }
        public override void Dance()
        {
            throw new NotImplementedException();
        }
        public override void Swim()
        {
            throw new NotImplementedException();
        }
        public override void Fly()
        {
            throw new NotImplementedException();
        }
        public override void Sit()
        {
            throw new NotImplementedException();
        }
        public override void Hit()
        {
            throw new NotImplementedException();
        }
        public override void Ghost()
        {
            throw new NotImplementedException();
        }

        private void RotateRelativeToCamera()
        {
            // empty.
        }
    }
}
