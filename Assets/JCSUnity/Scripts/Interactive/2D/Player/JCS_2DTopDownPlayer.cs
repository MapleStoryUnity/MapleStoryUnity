/**
 * $File: JCS_2DTopDownPlayer.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Example Player with topdown control.
    /// </summary>
    [RequireComponent(typeof(JCS_2DTopDownPlayerAudioController))]
    public class JCS_2DTopDownPlayer : JCS_Player
    {
        /* Variables */

        public int mIndex = 0;

        private JCS_2DTopDownPlayerAudioController mTopDownAudioController = null;


        /* Setter & Getter */

        protected override void Awake()
        {
            base.Awake();
            this.mTopDownAudioController = this.GetComponent<JCS_2DTopDownPlayerAudioController>();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (JCS_Input.GetKeyDown(KeyCode.H))
                JCS_SceneManager.instance.LoadScene("JCS_SideScrollerDemo");


            if (mIndex == 0)
            {
                if (JCS_Input.GetKey(KeyCode.UpArrow))
                    GoUp();
                else if (JCS_Input.GetKey(KeyCode.DownArrow))
                    GoDown();

                else if (JCS_Input.GetKey(KeyCode.RightArrow))
                    GoRight();
                else if (JCS_Input.GetKey(KeyCode.LeftArrow))
                    GoLeft();
                else
                    Idle();
            }
            
            if (mIndex == 1)
            {
                if (JCS_Input.GetKey(KeyCode.W))
                    GoUp();
                else if (JCS_Input.GetKey(KeyCode.S))
                    GoDown();

                else if (JCS_Input.GetKey(KeyCode.D))
                    GoRight();
                else if (JCS_Input.GetKey(KeyCode.A))
                    GoLeft();
                else
                    Idle();
            }
        }


        /* Functions */

        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        public override void ControlEnable(bool act)
        {
            if (mTopDownAudioController.GetAudioListener() == null)
                return;

            // enable all the component here
            if (act)
            {
                if (JCS_GameSettings.instance.CAMERA_TYPE != JCS_CameraType.MULTI_TARGET)
                    mTopDownAudioController.GetAudioListener().enabled = true;
                mIsControllable = true;
            }
            // diable all the component here
            else
            {
                mTopDownAudioController.GetAudioListener().enabled = false;
                mIsControllable = false;
            }
        }
        public void GoUp()
        {
            if (!mIsControllable)
                return;

            this.mVelocity.y = MoveSpeed;
        }
        public void GoDown()
        {
            if (!mIsControllable)
                return;

            this.mVelocity.y = -MoveSpeed;
        }
        public void GoRight()
        {
            if (!mIsControllable)
                return;

            this.mVelocity.x = MoveSpeed;
        }
        public void GoLeft()
        {
            if (!mIsControllable)
                return;

            this.mVelocity.x = -MoveSpeed;
        }
        public void Idle()
        {
            this.mVelocity = Vector3.zero;
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
    }
}
