/**
 * $File: JCS_3DCameraPlayer.cs $
 * $Date: 2020-04-07 16:57:50 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Player control that works with camera relatively.
    /// 
    /// Reference game => `Monster Hunter`.
    /// </summary>
    public class JCS_3DCameraPlayer : JCS_3DPlayer
    {
        /* Variables */

        private JCS_3DLookAtAction mLookAtAction = null;

        [Separator("Initialize Variables (JCS_3DCameraPlayer)")]

        [Tooltip("Object the character want to look at.")]
        [SerializeField]
        private Transform mLookPoint = null;

        [Tooltip("Distance to place look point object.")]
        [SerializeField]
        [Range(0.0f, 50.0f)]
        private float mLookDistance = 5.0f;

        [Separator("Runtime Variables (JCS_3DCameraPlayer)")]

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

        public Transform lookPoint { get { return mLookPoint; } set { mLookPoint = value; } }
        public float lookDistance { get { return mLookDistance; } set { mLookDistance = value; } }

        public float jumpForce { get { return mJumpForce; } set { mJumpForce = value; } }

        public KeyCode upKey { get { return mUpKey; } set { mUpKey = value; } }
        public KeyCode downKey { get { return mDownKey; } set { mDownKey = value; } }
        public KeyCode rightKey { get { return mRightKey; } set { mRightKey = value; } }
        public KeyCode leftKey { get { return mLeftKey; } set { mLeftKey = value; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            mLookAtAction = GetComponent<JCS_3DLookAtAction>();
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
                mCharacterController.Move(transform.forward * mVelocity.z * JCS_Time.ItTime(mTimeType));
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

            var cam = JCS_Camera.main;

            Vector3 newPos = transform.localPosition;
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
