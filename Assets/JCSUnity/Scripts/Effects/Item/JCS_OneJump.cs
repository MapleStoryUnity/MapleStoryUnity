/**
 * $File: JCS_OneJump.cs $
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
    /// Effect makes the item jumps and spreads.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class JCS_OneJump : MonoBehaviour
    {
        /* Variables */

        private bool mEffect = false;

#if UNITY_EDITOR
        [Separator("🧪 Helper Variables (JCS_OneJump)")]

        [Tooltip("Name of the collider that blocks the jump.")]
        [SerializeField]
        private string mColliderName = null;
#endif

        [Separator("⚡️ Runtime Variables (JCS_OneJump)")]

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

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
        [Range(JCS_Constants.FRICTION_MIN, 5.0f)]
        private float mBounceFriction = 0.2f;

        /* Setter & Getter */

        public bool fffect { get { return mEffect; } set { mEffect = value; } }
        public Vector3 GetVelocity() { return mVelocity; }

        public Rigidbody GetRigidbody() { return mRigidbody; }
        public Collider GetFixCollider() { return mFixCollider; }

        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        public bool bounceBackfromWall { get { return mBounceBackfromWall; } set { mBounceBackfromWall = value; } }
        public float bounceFriction { get { return mBounceFriction; } set { mBounceFriction = value; } }

        /* Functions */

        private void Awake()
        {
            mBoxCollider = GetComponent<BoxCollider>();
            mRigidbody = GetComponent<Rigidbody>();
        }
        private void Start()
        {
            var pm = JCS_PlayerManager.FirstInstance();
            var gm2d = JCS_2DGameManager.FirstInstance();

            if (pm != null)
                pm.IgnorePhysicsToAllPlayer(mBoxCollider);

            if (gm2d != null)
                gm2d.IgnoreAllPlatformTrigger(mBoxCollider);
        }

        private void FixedUpdate()
        {
            if (!mEffect)
                return;

            float dt = JCS_Time.ItTime(mTimeType);

            transform.position += mVelocity * dt;
            mVelocity.y += JCS_Physics.GRAVITY * dt * mItemGravity;
        }

        private void OnTriggerEnter(Collider other)
        {
            var iw = other.GetComponent<JCS_ItemWall>();
            if (iw != null)
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
            mMoveForce = moveForce;
            mJumpForce = jumpForce;
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

            var otherItem = GetComponent<JCS_Item>();
            // if itself is a item, we check other is a item or not.
            if (otherItem != null)
            {
                otherItem = other.GetComponent<JCS_Item>();
                // if both are item then we dont bother each other action.
                if (otherItem != null)
                    return;
            }

#if UNITY_EDITOR
            // if is debug mode print this out.
            // in order to know what does item touched and 
            // stop this movement.
            if (JCS_GameSettings.FirstInstance().debugMode)
                Debug.Log(other.transform.name);

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
            JCS_PlayerManager.FirstInstance().IgnorePhysicsToAllPlayer(mBoxCollider, false);
        }
    }
}
