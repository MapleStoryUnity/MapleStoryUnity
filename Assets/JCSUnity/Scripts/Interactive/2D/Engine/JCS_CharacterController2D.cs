/**
 * $File: JCS_CharacterController2D.cs $
 * $Date: 2016-09-20 06:17:03 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Character Controller using Unity 2D Engine.
    /// 
    /// TODO(jenchieh): Use this source to make the perfect character controller 2d.
    /// SOURCE(jenchieh): https://docs.unity3d.com/ScriptReference/GeometryUtility.TestPlanesAABB.html
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class JCS_CharacterController2D : MonoBehaviour
    {
        /* Variables */

        private Rigidbody2D mRigidbody2d = null;
        private BoxCollider2D mBoxCollider2d = null;
        private SpriteRenderer mSpriteRenderer = null;

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_CharacterController2D)")]

        [Tooltip("")]
        [SerializeField]
        private bool mControl = false;

        [SerializeField]
        private KeyCode mJumpKey = KeyCode.Space;
        [SerializeField]
        private KeyCode mLeftKey = KeyCode.LeftArrow;
        [SerializeField]
        private KeyCode mRightKey = KeyCode.RightArrow;
#endif

        [Separator("Check Variables (JCS_CharacterController2D)")]

        // STUDY(jenchieh): use this as the composition info?
        [SerializeField]
        [ReadOnly]
        private Vector3 mVelocity = Vector3.zero;

        [Tooltip("Record down the box info from the unity engine.")]
        [SerializeField]
        [ReadOnly]
        private Vector2 mBoxInfo = Vector2.zero;

        [Tooltip("Hit the bottom?")]
        [SerializeField]
        [ReadOnly]
        private bool mHitBottom = false;

        [Tooltip("Hit the top?")]
        [SerializeField]
        [ReadOnly]
        private bool mHitTop = false;

        [Tooltip("Hit the right?")]
        [SerializeField]
        [ReadOnly]
        private bool mHitRight = false;

        [Tooltip("Hit the left?")]
        [SerializeField]
        [ReadOnly]
        private bool mHitLeft = false;

        // holde the bottom collider
        [SerializeField]
        [ReadOnly]
        private List<Collider2D> mBottomColliders = null;

        [SerializeField]
        [ReadOnly]
        private List<Collider2D> mTopColliders = null;

        [SerializeField]
        [ReadOnly]
        private List<Collider2D> mRightColliders = null;

        [SerializeField]
        [ReadOnly]
        private List<Collider2D> mLeftColliders = null;

        private Vector3 mCurrentFrame = Vector3.zero;

        [Separator("Runtime Variables Variables (JCS_CharacterController2D)")]

        [Tooltip("Apply gravity?")]
        [SerializeField]
        private bool mApplyGravity = true;

        // NOTE(jenchieh): if the class get bigger, 
        // will switch to use composition instead 
        // of having all the feature here which is non-sense.
        [Tooltip("Impetus for this object to jump.")]
        [SerializeField]
        [Range(0.0f, 50.0f)]
        private float mJumpForce = 3;

        [Tooltip("How far the raycast is cast.")]
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float mDetectDistance = 10.0f;

        // record down the last frame collider's position.
        private Vector2 mLastFrameColliderPosition = Vector2.zero;

        // record down the last frame position.
        private Vector2 mLastFramePosition = Vector2.zero;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("Freeze")]

        [Tooltip("Freeze the object in x axis.")]
        [SerializeField]
        private bool mFreezeX = false;

        [Tooltip("Freeze the object in y axis.")]
        [SerializeField]
        private bool mFreezeY = false;

        [Header("Optional")]

        [Tooltip("Zero out the rotation when collider is trigger.")]
        [SerializeField]
        private bool mZeroRotationWhenIsTrigger = true;

        /* Setter & Getter */

        public Vector2 boxInfo { get { return mBoxInfo; } }
        public Vector3 velocity { get { return mVelocity; } }
        public float velX { get { return mVelocity.x; } set { mVelocity.x = value; } }
        public float velY { get { return mVelocity.y; } set { mVelocity.y = value; } }
        public float velZ { get { return mVelocity.z; } set { mVelocity.z = value; } }

        public SpriteRenderer GetSpriteRenderer() { return mSpriteRenderer; }
        public BoxCollider2D GetBoxCollider2D() { return mBoxCollider2d; }
        public Rigidbody2D GetRigidbody2D() { return mRigidbody2d; }

        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        public bool hitTop { get { return mHitTop; } }
        public bool hitBottom { get { return mHitBottom; } }
        public bool hitLeft { get { return mHitLeft; } }
        public bool hitRight { get { return mHitRight; } }

        public bool freezeX { get { return mFreezeX; } set { mFreezeX = value; } }
        public bool freezeY { get { return mFreezeY; } set { mFreezeY = value; } }


        /* Functions */

        private void Awake()
        {
            mBoxCollider2d = GetComponent<BoxCollider2D>();
            mRigidbody2d = GetComponent<Rigidbody2D>();
            mSpriteRenderer = GetComponent<SpriteRenderer>();

            // get the box info.
            mBoxInfo = JCS_Physics.GetColliderInfo(mBoxCollider2d);

            // set to never sleep
            mRigidbody2d.sleepMode = RigidbodySleepMode2D.NeverSleep;

            mBottomColliders = new List<Collider2D>();
            mTopColliders = new List<Collider2D>();
            mRightColliders = new List<Collider2D>();
            mLeftColliders = new List<Collider2D>();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!mControl)
                return;

            Test();
        }
#endif

        private void LateUpdate()
        {
            mLastFramePosition = transform.position;

            if (mHitLeft && velX < 0)
                velX = 0;
            else if (mHitRight && velX > 0)
                velX = 0;

#if UNITY_EDITOR
            // draw the collider each frame.
            JCS_Debug.DrawCollider(mBoxCollider2d, Color.cyan);
            JCS_Debug.DrawCollider(mBoxCollider2d, Color.blue, mLastFrameColliderPosition);
#endif

            // record down the last frame's collider's position
            mLastFrameColliderPosition = JCS_Physics.GetColliderPosition(mBoxCollider2d);
        }

        private void FixedUpdate()
        {
            // check if the bottom collider length to zero, 
            // which mean no collider detected.
            if (mBottomColliders.Count == 0)
                mHitBottom = false;

            if (mTopColliders.Count == 0)
                mHitTop = false;

            if (mLeftColliders.Count == 0)
                mHitLeft = false;

            if (mRightColliders.Count == 0)
                mHitRight = false;

            ApplyGravity();

            // apply force base on the velocity.
            transform.position += mVelocity * JCS_Time.ItTime(mTimeType);

            // lastly check the freezing.
            DoFreeze();
        }

        // OPTIMIZE(jenchieh): no idea why on trigger enter 
        // wont set on top of the collider.
        private void OnTriggerEnter2D(Collider2D other)
        {
            // check ray ignore.
            if (other.GetComponent<JCS_RayIgnore>() != null)
                return;

            if (mZeroRotationWhenIsTrigger)
                transform.eulerAngles = Vector3.zero;

            // Detect Right
            {
                Vector3 right = transform.TransformDirection(Vector3.right);
                RaycastHit2D[] hits = Physics2D.RaycastAll(
                    JCS_Physics.GetColliderPosition(mBoxCollider2d),
                    right,
                    mBoxInfo.x / 2 + mDetectDistance);

#if UNITY_EDITOR
                //Debug.DrawRay(transform.position, right, Color.green);
#endif

                foreach (RaycastHit2D hit in hits)
                {
                    BoxCollider2D bc2d = hit.transform.GetComponent<BoxCollider2D>();

                    // ignore the tag.
                    if (hit.transform.GetComponent<JCS_RayIgnore>() != null ||
                        bc2d == null ||
                        hit.transform == transform ||
                        hit.transform != other.transform)
                        continue;

                    mHitRight = true;

                    mVelocity.x = 0;

                    Vector3 newPos = transform.position;
                    newPos.x = mCurrentFrame.x;
                    transform.position = newPos;

                    // fix collision.
                    JCS_Physics.SetOnLeftOfBox(mBoxCollider2d, bc2d);

                    // check if colllider in array already
                    bool found = false;
                    foreach (Collider2D temp in mRightColliders)
                    {
                        if (temp.transform == other.transform)
                            found = true;
                    }

                    // if not found.
                    if (!found)
                        mRightColliders.Add(other);

                    return;
                }
            }

            // Detect Left
            {
                Vector3 left = transform.TransformDirection(Vector3.left);
                RaycastHit2D[] hits = Physics2D.RaycastAll(
                    JCS_Physics.GetColliderPosition(mBoxCollider2d),
                    left,
                    mBoxInfo.x / 2 + mDetectDistance);

#if UNITY_EDITOR
                //Debug.DrawRay(transform.position, left, Color.green);
#endif

                foreach (RaycastHit2D hit in hits)
                {
                    BoxCollider2D bc2d = hit.transform.GetComponent<BoxCollider2D>();

                    // ignore the tag.
                    if (hit.transform.GetComponent<JCS_RayIgnore>() != null ||
                        bc2d == null ||
                        hit.transform == transform ||
                        hit.transform != other.transform)
                        continue;

                    mHitLeft = true;

                    mVelocity.x = 0;

                    Vector3 newPos = transform.position;
                    newPos.x = mCurrentFrame.x;
                    transform.position = newPos;

                    // fix collision.
                    JCS_Physics.SetOnRightOfBox(mBoxCollider2d, bc2d);

                    // check if colllider in array already
                    bool found = false;
                    foreach (Collider2D temp in mLeftColliders)
                    {
                        if (temp.transform == other.transform)
                            found = true;
                    }

                    // if not found.
                    if (!found)
                        mLeftColliders.Add(other);

                    return;
                }
            }

            // Detect bottom
            {
                Vector3 down = transform.TransformDirection(Vector3.down);
                // NOTE(jenchieh): box info .x are the same as saying the width.
                //RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, down, mBoxInfo.y / 2 + mDetectDistance);
                RaycastHit2D[] hits = Physics2D.CircleCastAll(
                    JCS_Physics.GetColliderPosition(mBoxCollider2d),
                    mBoxInfo.x,
                    down,
                    mBoxInfo.y / 2 + mDetectDistance);

#if UNITY_EDITOR
                //Debug.DrawRay(transform.position, down, Color.green);
#endif

                foreach (RaycastHit2D hit in hits)
                {
                    BoxCollider2D bc2d = hit.transform.GetComponent<BoxCollider2D>();

                    // ignore the tag.
                    if (hit.transform.GetComponent<JCS_RayIgnore>() != null ||
                        bc2d == null ||
                        hit.transform == transform ||
                        hit.transform != other.transform)
                        continue;

                    mHitBottom = true;

                    Vector3 newPos = transform.position;
                    newPos.y = mCurrentFrame.y;
                    transform.position = newPos;

                    // fixed collision
                    JCS_Physics.SetOnTopOfBox(mBoxCollider2d, bc2d);


                    // check if colllider in array already
                    bool found = false;
                    foreach (Collider2D temp in mBottomColliders)
                    {
                        if (temp.transform == other.transform)
                            found = true;
                    }

                    // if not found.
                    if (!found)
                        mBottomColliders.Add(other);

                    break;
                }
            }

            // Detect top
            {
                Vector3 top = transform.TransformDirection(Vector3.up);
                // NOTE(jenchieh): box info .x are the same as saying the width.
                RaycastHit2D[] hits = Physics2D.RaycastAll(
                    JCS_Physics.GetColliderPosition(mBoxCollider2d),
                    top,
                    mBoxInfo.y / 2 + mDetectDistance);

#if UNITY_EDITOR
                //Debug.DrawRay(transform.position, top, Color.green);
#endif

                foreach (RaycastHit2D hit in hits)
                {
                    BoxCollider2D bc2d = hit.transform.GetComponent<BoxCollider2D>();

                    // ignore the tag.
                    if (hit.transform.GetComponent<JCS_RayIgnore>() != null ||
                        bc2d == null ||
                        hit.transform == transform)
                        continue;

                    mHitTop = true;

                    Vector3 newPos = transform.position;
                    newPos.y = mCurrentFrame.y;
                    transform.position = newPos;

                    // fixed collision
                    JCS_Physics.SetOnBottomOfBox(mBoxCollider2d, bc2d);

                    // check if colllider in array already
                    bool found = false;
                    foreach (Collider2D temp in mTopColliders)
                    {
                        if (temp.transform == other.transform)
                            found = true;
                    }

                    // if not found.
                    if (!found)
                        mTopColliders.Add(other);

                    break;
                }
            }


            if (!mHitLeft && !mHitRight)
            {
                // record down the frame.
                mCurrentFrame = transform.position;
            }
        }

        // 
        private void OnTriggerExit2D(Collider2D other)
        {
            // check ray ignore.
            if (other.GetComponent<JCS_RayIgnore>() != null)
                return;


            // Detect bottom
            {
                Vector3 down = transform.TransformDirection(Vector3.down);
                // NOTE(jenchieh): box info .x are the same as saying the width.
                //RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, down, mBoxInfo.y / 2 + mDetectDistance);
                RaycastHit2D[] hits = Physics2D.CircleCastAll(
                    mLastFrameColliderPosition,
                    mBoxInfo.x,
                    down,
                    mBoxInfo.y / 2 + mDetectDistance);

#if UNITY_EDITOR
                //Debug.DrawRay(transform.position, down, Color.green);
#endif

                foreach (RaycastHit2D hit in hits)
                {
                    BoxCollider2D bc2d = hit.transform.GetComponent<BoxCollider2D>();

                    // ignore the tag.
                    if (hit.transform.GetComponent<JCS_RayIgnore>() != null ||
                        bc2d == null ||
                        hit.transform == transform ||
                        hit.transform != other.transform)
                        continue;

                    mHitBottom = true;

                    for (int index = 0; index < mBottomColliders.Count; ++index)
                    {
                        Collider2D temp = mBottomColliders[index];

                        if (temp.transform == other.transform)
                        {
                            mBottomColliders.Remove(other);
                            break;
                        }
                    }

                    break;
                }
            }

            // Detect top
            {
                Vector3 top = transform.TransformDirection(Vector3.up);
                // NOTE(jenchieh): box info .x are the same as saying the width.
                RaycastHit2D[] hits = Physics2D.RaycastAll(
                    mLastFrameColliderPosition,
                    top,
                    mBoxInfo.y / 2 + mDetectDistance);

#if UNITY_EDITOR
                //Debug.DrawRay(transform.position, top, Color.green);
#endif

                foreach (RaycastHit2D hit in hits)
                {
                    BoxCollider2D bc2d = hit.transform.GetComponent<BoxCollider2D>();

                    // ignore the tag.
                    if (hit.transform.GetComponent<JCS_RayIgnore>() != null ||
                        bc2d == null ||
                        hit.transform == transform)
                        continue;

                    mHitTop = true;

                    for (int index = 0; index < mTopColliders.Count; ++index)
                    {
                        Collider2D temp = mTopColliders[index];

                        if (temp.transform == other.transform)
                        {
                            mTopColliders.Remove(other);
                            break;
                        }
                    }

                    break;
                }
            }

            // Detect Right
            {
                Vector3 right = transform.TransformDirection(Vector3.right);
                RaycastHit2D[] hits = Physics2D.RaycastAll(
                    mLastFrameColliderPosition,
                    right,
                    mBoxInfo.x / 2 + mDetectDistance);

#if UNITY_EDITOR
                //Debug.DrawRay(transform.position, right, Color.green);
#endif

                foreach (RaycastHit2D hit in hits)
                {
                    BoxCollider2D bc2d = hit.transform.GetComponent<BoxCollider2D>();

                    // ignore the tag.
                    if (hit.transform.GetComponent<JCS_RayIgnore>() != null ||
                        bc2d == null ||
                        hit.transform == transform ||
                        hit.transform != other.transform)
                        continue;

                    mHitRight = true;

                    for (int index = 0; index < mRightColliders.Count; ++index)
                    {
                        Collider2D temp = mRightColliders[index];

                        if (temp.transform == other.transform)
                        {
                            mRightColliders.Remove(other);
                            break;
                        }
                    }

                    break;
                }
            }

            // Detect Left
            {
                Vector3 left = transform.TransformDirection(Vector3.left);
                RaycastHit2D[] hits = Physics2D.RaycastAll(
                    mLastFrameColliderPosition,
                    left,
                    mBoxInfo.x / 2 + mDetectDistance);

#if UNITY_EDITOR
                //Debug.DrawRay(transform.position, left, Color.green);
#endif

                foreach (RaycastHit2D hit in hits)
                {
                    BoxCollider2D bc2d = hit.transform.GetComponent<BoxCollider2D>();

                    // ignore the tag.
                    if (hit.transform.GetComponent<JCS_RayIgnore>() != null ||
                        bc2d == null ||
                        hit.transform == transform ||
                        hit.transform != other.transform)
                        continue;

                    mHitLeft = true;

                    for (int index = 0; index < mLeftColliders.Count; ++index)
                    {
                        Collider2D temp = mLeftColliders[index];

                        if (temp.transform == other.transform)
                        {
                            mLeftColliders.Remove(other);
                            break;
                        }
                    }

                    break;
                }
            }

        }

#if UNITY_EDITOR
        private void Test()
        {
            float speed = 2;

            // Test..
            if (JCS_Input.GetKey(mRightKey))
                velX = speed;
            else if (JCS_Input.GetKey(mLeftKey))
                velX = -speed;
            else
                velX = 0;

            if (JCS_Input.GetKey(mJumpKey))
                Jump();
        }
#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Jump once.
        /// </summary>
        public void Jump()
        {
            Jump(mJumpForce);
        }
        /// <summary>
        /// Jump once.
        /// </summary>
        /// <param name="force"> force to jump </param>
        public void Jump(float force)
        {
            if (mVelocity.y != 0)
                return;

            mVelocity.y = force;
            mHitBottom = false;

            //mBottomColliders.Clear();
        }

        /// <summary>
        /// Check if is on the ground.
        /// </summary>
        /// <returns></returns>
        public bool isGrounded()
        {
            return mHitBottom;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Apply G
        /// </summary>
        private void ApplyGravity()
        {
            // check apply gravity?
            if (!mApplyGravity)
                return;

            // check grounded.
            if (!isGrounded())
            {
                // apply gravity
                mVelocity.y += JCS_Physics.GRAVITY * JCS_Time.ItTime(mTimeType);
            }
            else
            {
                // stop falling.
                mVelocity.y = 0;
            }
        }

        /// <summary>
        /// Do the freezing.
        /// </summary>
        private void DoFreeze()
        {
            Vector2 newPos = transform.position;

            if (mFreezeX)
            {
                newPos.x = mLastFramePosition.x;
            }

            if (mFreezeY)
            {
                newPos.y = mLastFramePosition.y;
            }

            transform.position = newPos;
        }
    }

}
