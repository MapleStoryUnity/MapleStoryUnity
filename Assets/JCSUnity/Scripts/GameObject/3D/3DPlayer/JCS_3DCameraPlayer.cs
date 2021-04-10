/**
 * $File: JCS_3DCameraPlayer.cs $
 * $Date: 2020-04-07 16:57:50 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Player control that works with camera relatively.
    /// 
    /// Reference game => `Monster Hunter`.
    /// </summary>
    public class JCS_3DCameraPlayer
        : JCS_3DPlayer
    {
        /* Variables */

        private JCS_3DLookAtAction mLookAtAction = null;

        [Header("** Initialize Variables (JCS_3DCameraPlayer) **")]

        [Tooltip("Object the character want to look at.")]
        [SerializeField]
        private Transform mLookPoint = null;

        [Tooltip("Distance to place look point object.")]
        [SerializeField]
        [Range(0.0f, 50.0f)]
        private float mLookDistance = 5.0f;

        [Header("** Runtime Variables (JCS_3DCameraPlayer) **")]

        [Tooltip("How hard this player jumps.")]
        [SerializeField]
        [Range(0.0f, 300.0f)]
        private float mJumpForce = 10.0f;

        [Tooltip("Keys that goes forward.")]
        [SerializeField]
        private KeyCode mUpKey = KeyCode.UpArrow;

        [Tooltip("Keys that goes backward.")]
        [SerializeField]
        private KeyCode mDownKey = KeyCode.DownArrow;

        [Tooltip("Keys that goes right.")]
        [SerializeField]
        private KeyCode mRightKey = KeyCode.RightArrow;

        [Tooltip("Keys that goes left.")]
        [SerializeField]
        private KeyCode mLeftKey = KeyCode.LeftArrow;

        /* Setter & Getter */

        public Transform LookPoint { get { return this.mLookPoint; } set { this.mLookPoint = value; } }
        public float LookDistance { get { return this.mLookDistance; } set { this.mLookDistance = value; } }

        public float JumpForce { get { return this.mJumpForce; } set { this.mJumpForce = value; } }

        public KeyCode UpKey { get { return this.mUpKey; } set { this.mUpKey = value; } }
        public KeyCode DownKey { get { return this.mDownKey; } set { this.mDownKey = value; } }
        public KeyCode RightKey { get { return this.mRightKey; } set { this.mRightKey = value; } }
        public KeyCode LeftKey { get { return this.mLeftKey; } set { this.mLeftKey = value; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            this.mLookAtAction = this.GetComponent<JCS_3DLookAtAction>();
            mLookAtAction.SetTargetTransform(mLookPoint);
        }

        protected override void Update()
        {
            base.Update();

            HandleFacing();
        }

        protected override void FixedUpdate()
        {
            if (mCharacterController.enabled)
            {
                // apply force
                mCharacterController.Move(transform.forward * mVelocity.z * Time.deltaTime);
            }
        }

        protected override void PlayerInput()
        {
            if (JCS_Input.GetKey(mUpKey) || JCS_Input.GetKey(mDownKey) ||
                JCS_Input.GetKey(mLeftKey) || JCS_Input.GetKey(mRightKey))
            {
                mVelocity.z = mMoveSpeed;
            }
            else
            {
                mVelocity.z = 0;
            }

            if (JCS_Input.GetKeyDown(KeyCode.Space))
                mVelocity.y = mJumpForce;
        }

        private void HandleFacing()
        {
            if (mLookPoint == null)
                return;

            JCS_Camera cam = JCS_Camera.main;

            Vector3 newPos = this.transform.localPosition;
            Vector3 direction = Vector3.zero;

            if (JCS_Input.GetKey(mUpKey))
                direction = cam.transform.forward;
            else if (JCS_Input.GetKey(mDownKey))
                direction = -cam.transform.forward;

            if (JCS_Input.GetKey(mRightKey))
                direction += cam.transform.right;
            else if (JCS_Input.GetKey(mLeftKey))
                direction += -cam.transform.right;

            mLookPoint.localPosition = newPos + direction * mLookDistance;
        }
    }
}
