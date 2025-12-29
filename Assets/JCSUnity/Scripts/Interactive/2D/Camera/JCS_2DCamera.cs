/**
 * $File: JCS_2DCamera.cs $
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
    /// Basic camera for 2d Games.
    /// </summary>
    public class JCS_2DCamera : JCS_Camera
    {
        /* Variables */

        [Separator("📋 Check Varialbes (JCS_2DCamera)")]

        [SerializeField]
        [ReadOnly]
        private Vector3 mTargetPosition = Vector3.zero;

        [Separator("🌱 Initialize Variables (JCS_2DCamera)")]

        [Tooltip("Set the camera's position to the player when the game starts?")]
        [SerializeField]
        private bool mSetToPlayerPositionAtStart = true;

        [Separator("⚡️ Runtime Variables (JCS_2DCamera)")]

        [Tooltip("Set velocity to zero while the follow not active.")]
        [SerializeField]
        private bool mResetVelocityToZeroWhileNotActive = false;

        // effect: camera will do the smooth movement
        //[SerializeField] private bool mSmoothMoveX = true;
        //[SerializeField] private bool mSmoothMoveY = true;

        [Header("🔍 Speed / Friction")]

        [Tooltip("How fast this camera move toward the target. (x-axis)")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 10.0f)]
        private float mFrictionX = 0.6f;

        [Tooltip("How fast this camera move toward the target. (y-axis)")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 10.0f)]
        private float mFrictionY = 0.6f;

        [Header("🔍 Freeze")]

        [Tooltip("Do freeze in runtime?")]
        [SerializeField]
        private bool mFreezeInRuntime = false;

        [Tooltip("Freeze on X axis?")]
        [SerializeField]
        private bool mFreezeX = false;

        [Tooltip("Freeze on Y axis?")]
        [SerializeField]
        private bool mFreezeY = false;

        [Tooltip("Freeze on Z axis?")]
        [SerializeField]
        private bool mFreezeZ = false;

        private Vector3 mFreezeRecord = Vector3.zero;

        [Header("🔍 Scroll")]

        [Tooltip("Do the zoom effect.")]
        [SerializeField]
        private bool mZoomEffect = true;

        [Tooltip("Zoom with the mouse or touches.")]
        [SerializeField]
        private bool mZoomWithMouseOrTouch = true;

        [Tooltip("Distance once you scroll with mouse.")]
        [SerializeField]
        [Range(0.001f, 500.0f)]
        private float mScrollRange_Mouse = 2.0f;

        [Tooltip("Distance once you scroll with multi-touch.")]
        [SerializeField]
        [Range(0.001f, 500.0f)]
        private float mScrollRange_Touch = 1.0f;

        [Tooltip("How fast it scroll. (Zoom In/Out)")]
        [SerializeField]
        [Range(0.1f, 10.0f)]
        private float mScrollFriction = 0.4f;

        private float mWheelDegree = 0;

        /* Setter & Getter */

        public bool freezeX { get { return mFreezeX; } set { mFreezeX = value; } }
        public bool freezeY { get { return mFreezeY; } set { mFreezeY = value; } }
        public bool freezeZ { get { return mFreezeZ; } set { mFreezeZ = value; } }

        public Transform followTarget { get { return mTargetTransform; } set { mTargetTransform = value; } }
        public override void SetFollowTarget(Transform trans) { mTargetTransform = trans; }
        public override Transform GetFollowTarget() { return mTargetTransform; }

        public bool GetFollowing() { return mFollowing; }
        public void SetFollowing(bool val) { mFollowing = val; }
        public Transform GetTargetTransform() { return mTargetTransform; }
        public bool resetVelocityToZeroWhileNotActive { get { return mResetVelocityToZeroWhileNotActive; } set { mResetVelocityToZeroWhileNotActive = value; } }
        public bool zoomEffect { get { return mZoomEffect; } set { mZoomEffect = value; } }
        public bool zoomWithMouseOrTouch { get { return mZoomWithMouseOrTouch; } set { mZoomWithMouseOrTouch = value; } }
        public float scrollRange_Mouse { get { return mScrollRange_Mouse; } set { mScrollRange_Mouse = value; } }
        public float scrollRange_Touch { get { return mScrollRange_Touch; } set { mScrollRange_Touch = value; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            mFreezeRecord = transform.position;
        }

        protected override void Start()
        {
            base.Start();

            // Use player from "JCS_GameManager" as default
            if (mTargetTransform == null)
            {
                JCS_Player player = JCS_GameManager.FirstInstance().player;
                if (player != null)
                    SetFollowTarget(player.transform);
            }

            if (mTargetTransform != null)
            {
                // first assign the target transform's position to target position.
                mTargetPosition = mTargetTransform.position;

                // record the target position
                mLastFramePos = mTargetTransform.position;
            }

            if (mSetToPlayerPositionAtStart)
            {
                JCS_Player player = JCS_PlayerManager.FirstInstance().GetActivePlayer();

                if (player != null)
                {
                    // set the camera position
                    main.SetPosition(player.transform.position.x, player.transform.position.y);
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (mTargetTransform == null)
            {
                ResetVelocityWhileNotActive();
                return;
            }

            if (!mFollowing)
            {
                ResetVelocityWhileNotActive();
                return;
            }

            // if freezing effect in runtime, we have to record down this
            if (mFreezeInRuntime)
                mFreezeRecord = transform.position;

            if (mZoomWithMouseOrTouch)
            {
#if (UNITY_EDITOR || UNITY_STANDALONE)
                // Get the wheel value from the Unity API
                // 
                // physical layer [mouse wheel] ->
                // OS layer ->
                // application layer [Unity itself] ->
                // to here...
                mWheelDegree = Input.GetAxis("Mouse ScrollWheel");
#elif (UNITY_ANDROID || UNITY_IPHIONE || UNITY_IOS)
                var input = JCS_TouchInput.FirstInstance();
                mWheelDegree = input.touchDistanceDelta;
#endif
                ZoomCamera(mWheelDegree);
            }

            // Smooth track.
            if (mSmoothTrack)
            {
                // Try to approach to the target position.
                mVelocity.x = ((mTargetTransform.position.x + mPositionOffset.x) -
                    transform.position.x) / mFrictionX;
                mVelocity.y = ((mTargetTransform.position.y + mPositionOffset.y) -
                    transform.position.y) / mFrictionY;
                mVelocity.z = ((mTargetPosition.z + mPositionOffset.z) -
                    transform.position.z) / mScrollFriction;

                // Update self position
                transform.position += mVelocity * JCS_Time.ItTime(mTimeType);
            }
            // Hard track
            else
            {
                // follow the object with frame distance.
                // distance = current frame position - last frame position
                Vector3 currentFramePos = mTargetTransform.position + mPositionOffset;
                transform.position += currentFramePos - mLastFramePos;
                mLastFramePos = currentFramePos;
            }

            // do freezing the last
            DoFreezing();
        }

        /// <summary>
        /// Set to the target's position in frame.
        /// </summary>
        public void SetToTargetImmediate()
        {
            if (GetTargetTransform() == null)
                return;

            Vector3 targetPos = GetTargetTransform().position;

            Vector3 newPos = transform.position;

            newPos.x = targetPos.x;
            newPos.y = targetPos.y;

            transform.position = newPos;
        }

        /// <summary>
        /// Do the zooming on Z axis.
        /// </summary>
        /// <param name="depthDistance"></param>
        public void ZoomCamera(float depthDistance)
        {
            // Check the trigger of the scrolling effect.
            if (!mZoomEffect)
                return;

            if (depthDistance == 0)
                return;

#if (UNITY_ANDROID || UNITY_IPHIONE || UNITY_IOS)
            float cameraDepth = depthDistance * mScrollRange_Touch;
#else
            float cameraDepth = depthDistance * mScrollRange_Mouse;
#endif

            mTargetPosition.z += cameraDepth;
        }

        /// <summary>
        /// Do freezing.
        /// </summary>
        private void DoFreezing()
        {
            Vector3 newPos = transform.position;
            if (mFreezeX)
            {
                newPos.x = mFreezeRecord.x;
                mVelocity.x = 0;
            }
            if (mFreezeY)
            {
                newPos.y = mFreezeRecord.y;
                mVelocity.y = 0;
            }
            if (mFreezeZ)
            {
                newPos.z = mFreezeRecord.z;
                mVelocity.z = 0;
            }

            transform.position = newPos;
        }

        /// <summary>
        /// Set Velocity to zero while the follow effect is not active.
        /// </summary>
        private void ResetVelocityWhileNotActive()
        {
            if (!mResetVelocityToZeroWhileNotActive)
                return;

            mVelocity = Vector3.zero;
        }
    }
}
