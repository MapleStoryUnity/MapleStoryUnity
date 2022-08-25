/**
 * $File: JCS_OneJump.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Effect makes the item jumps and spreads.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class JCS_OneJump : MonoBehaviour
    {
        /* Variables */

        private bool mEffect = false;

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_OneJump) **")]

        [Tooltip("Name of the collider that blocks the jump.")]
        [SerializeField]
        private string mColliderName = null;
#endif

        [Header("** Runtime Variables (JCS_OneJump) **")]

        [Tooltip("How many force to apply on jump?")]
        [SerializeField]
        private float mJumpForce = 10.0f;

        [Tooltip("How fast this item moves?")]
        [SerializeField]
        private float mMoveForce = 10.0f;

        [Tooltip("Item gravity.")]
        [SerializeField]
        private float mItemGravity = 2.0f;

        private Vector3 mVelocity = Vector3.zero;
        private BoxCollider mBoxCollider = null;

        private Rigidbody mRigidbody = null;
        private RaycastHit mRaycastHit;
        private Collider mFixCollider = null;

        [Tooltip(@"Do the item bounce back from the wall after hit the wall or 
just stop there.")]
        [SerializeField]
        private bool mBounceBackfromWall = true;

        [Tooltip("Deacceleration after bouncing from the wall.")]
        [SerializeField]
        private float mBounceFriction = 0.2f;

        /* Setter & Getter */

        public bool Effect { get { return this.mEffect; } set { this.mEffect = value; } }
        public Vector3 GetVelocity() { return this.mVelocity; }

        public Rigidbody GetRigidbody() { return this.mRigidbody; }
        public Collider GetFixCollider() { return this.mFixCollider; }

        public bool BounceBackfromWall { get { return this.mBounceBackfromWall; } set { this.mBounceBackfromWall = value; } }
        public float BounceFriction { get { return this.mBounceFriction; } set { this.mBounceFriction = value; } }

        /* Functions */

        private void Awake()
        {
            mBoxCollider = this.GetComponent<BoxCollider>();
            mRigidbody = this.GetComponent<Rigidbody>();
        }
        private void Start()
        {
            var pm = JCS_PlayerManager.instance;
            var gm2d = JCS_2DGameManager.instance;

            if (pm != null)
                pm.IgnorePhysicsToAllPlayer(mBoxCollider);

            if (gm2d != null)
                gm2d.IgnoreAllPlatformTrigger(mBoxCollider);
        }

        private void FixedUpdate()
        {
            if (!mEffect)
                return;

            this.transform.position += mVelocity * Time.deltaTime;
            mVelocity.y += -JCS_GameConstant.GRAVITY * Time.deltaTime * mItemGravity;
        }

        private void OnTriggerEnter(Collider other)
        {
            var jcsiw = other.GetComponent<JCS_ItemWall>();
            if (jcsiw != null)
            {
                // no longer moving along x-axis or z-axis.
                {
                    // bounce it back!
                    if (mBounceBackfromWall)
                    {
                        mVelocity.x = -mVelocity.x * mBounceFriction;
                        mVelocity.z = -mVelocity.z * mBounceFriction;
                    }
                    else
                    {
                        mVelocity.x = 0;
                        mVelocity.z = 0;
                    }
                }

                return;
            }

            /* Only check when the item start dropping. */
            TriggerDropping(other);
        }

        /// <summary>
        /// Apply force in order to do hop effect.
        /// </summary>
        /// <param name="depth"> include depth? </param>
        public void DoForce(bool depth = false)
        {
            DoForce(mMoveForce, mJumpForce, depth);
        }
        /// <summary>
        /// Apply force in order to do hop effect.
        /// </summary>
        /// <param name="moveForce"> force to move in x axis </param>
        /// <param name="jumpForce"> force to move in y axis </param>
        /// /// <param name="depth"> include depth? </param>
        public void DoForce(float moveForce, float jumpForce, bool depth = false)
        {
            mVelocity.y = jumpForce;
            mVelocity.x = moveForce;
            this.mMoveForce = moveForce;
            this.mJumpForce = jumpForce;
            mEffect = true;

            // including depth!
            if (depth)
            {
                float tempMoveForce = moveForce / 2.0f;

                // override x
                mVelocity.x = JCS_Random.Range(-tempMoveForce, tempMoveForce);

                // apply depth
                mVelocity.z = JCS_Random.Range(-tempMoveForce, tempMoveForce);
            }
        }

        /// <summary>
        /// Only check when the item start dropping.
        /// </summary>
        /// <param name="other"> collider detected. </param>
        private void TriggerDropping(Collider other)
        {
            if (mVelocity.y > 0)
                return;

            // meet ignore object
            var ignore = other.GetComponent<JCS_ItemIgnore>();
            if (ignore != null)
                return;

            var otherItem = this.GetComponent<JCS_Item>();
            // if itself is a item, we check other is a item or not.
            if (otherItem != null)
            {
                otherItem = other.GetComponent<JCS_Item>();
                // if both are item then we dont bother each other action.
                if (otherItem != null)
                    return;
            }

#if (UNITY_EDITOR)
            // if is debug mode print this out.
            // in order to know what does item touched and 
            // stop this movement.
            if (JCS_GameSettings.instance.DEBUG_MODE)
                JCS_Debug.PrintName(other.transform);

            mColliderName = other.name;
#endif

            mVelocity.y = 0;
            mEffect = false;

            mFixCollider = other;

            // TODO(jenchieh): not all the object we get set are box collider only.
            var beSetBox = other.GetComponent<BoxCollider>();

            // set this ontop of the other box(ground)
            if (beSetBox != null)
                JCS_Physics.SetOnTopOfBoxWithSlope(mBoxCollider, beSetBox);

            // enable the physic once on the ground
            JCS_PlayerManager.instance.IgnorePhysicsToAllPlayer(this.mBoxCollider, false);
        }
    }
}
