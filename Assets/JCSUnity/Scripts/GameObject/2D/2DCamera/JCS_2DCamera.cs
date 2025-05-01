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

        [Separator("Check Varialbes (JCS_2DCamera)")]

        [SerializeField]
        [ReadOnly]
        private Vector3 mTargetPosition = Vector3.zero;

        [Separator("Initialize Variables (JCS_2DCamera)")]

        [Tooltip("Set the camera's position to the player when the game starts?")]
        [SerializeField]
        private bool mSetToPlayerPositionAtStart = true;

        [Separator("Runtime Variables (JCS_2DCamera)")]

        [Tooltip("Set velocity to zero while the follow not active.")]
        [SerializeField]
        private bool mResetVelocityToZeroWhileNotActive = false;

        // effect: camera will do the smooth movement
        //[SerializeField] private bool mSmoothMoveX = true;
        //[SerializeField] private bool mSmoothMoveY = true;

        [Header("- Speed / Friction")]

        [Tooltip("How fast this camera move toward the target. (x-axis)")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 10.0f)]
        private float mFrictionX = 0.6f;

        [Tooltip("How fast this camera move toward the target. (y-axis)")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 10.0f)]
        private float mFrictionY = 0.6f;

        [Header("- Freeze")]

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

        [Header("- Scroll")]

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

        [Header("- Scene")]

        [Tooltip("Maxinum this camera can go in x-axis.")]
        [SerializeField]
        private float mMax_X_PositionInScene = float.PositiveInfinity;
        [Tooltip("Mininum this camera can go in x-axis.")]
        [SerializeField]
        private float mMin_X_PositionInScene = float.NegativeInfinity;
        [Tooltip("Maxinum this camera can go in y-axis.")]
        [SerializeField]
        private float mMax_Y_PositionInScene = float.PositiveInfinity;
        [Tooltip("Mininum this camera can go in y-axis.")]
        [SerializeField]
        private float mMin_Y_PositionInScene = float.NegativeInfinity;

        /* Setter & Getter */

        public bool FreezeX { get { return this.mFreezeX; } set { this.mFreezeX = value; } }
        public bool FreezeY { get { return this.mFreezeY; } set { this.mFreezeY = value; } }
        public bool FreezeZ { get { return this.mFreezeZ; } set { this.mFreezeZ = value; } }

        public Transform FollowTarget { get { return this.mTargetTransform; } set { this.mTargetTransform = value; } }
        public override void SetFollowTarget(Transform trans) { this.mTargetTransform = trans; }
        public override Transform GetFollowTarget() { return this.mTargetTransform; }

        public bool GetFollowing() { return this.mFollowing; }
        public void SetFollowing(bool val) { this.mFollowing = val; }
        public Transform GetTargetTransform() { return this.mTargetTransform; }
        public bool ResetVelocityToZeroWhileNotActive { get { return this.mResetVelocityToZeroWhileNotActive; } set { this.mResetVelocityToZeroWhileNotActive = value; } }
        public bool ZoomEffect { get { return this.mZoomEffect; } set { this.mZoomEffect = value; } }
        public bool ZoomWithMouseOrTouch { get { return this.mZoomWithMouseOrTouch; } set { this.mZoomWithMouseOrTouch = value; } }
        public float ScrollRange_Mouse { get { return this.mScrollRange_Mouse; } set { this.mScrollRange_Mouse = value; } }
        public float ScrollRange_Touch { get { return this.mScrollRange_Touch; } set { this.mScrollRange_Touch = value; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            this.mFreezeRecord = this.transform.position;
        }

        protected override void Start()
        {
            base.Start();

            // Use player from "JCS_GameManager" as default
            if (this.mTargetTransform == null)
            {
                JCS_Player player = JCS_GameManager.instance.Player;
                if (player != null)
                    SetFollowTarget(player.transform);
            }

            if (mTargetTransform != null)
            {
                // first assign the target transform's position to target position.
                mTargetPosition = this.mTargetTransform.position;

                // record the target position
                mLastFramePos = this.mTargetTransform.position;
            }

            if (mSetToPlayerPositionAtStart)
            {
                JCS_Player player = JCS_PlayerManager.instance.GetActivePlayer();

                if (player != null)
                {
                    // set the camera position
                    main.SetPosition(player.transform.position.x, player.transform.position.y);
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (this.mTargetTransform == null)
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
                this.mFreezeRecord = this.transform.position;

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
                var slideInput = JCS_SlideInput.instance;
                mWheelDegree = slideInput.TouchDistanceDelta;
#endif
                ZoomCamera(mWheelDegree);
            }

            // Smooth track.
            if (mSmoothTrack)
            {
                // Try to approach to the target position.
                mVelocity.x = ((this.mTargetTransform.position.x + mPositionOffset.x) -
                    this.transform.position.x) / mFrictionX;
                mVelocity.y = ((this.mTargetTransform.position.y + mPositionOffset.y) -
                    this.transform.position.y) / mFrictionY;
                mVelocity.z = ((this.mTargetPosition.z + mPositionOffset.z) -
                    this.transform.position.z) / mScrollFriction;

                // Update self position
                this.transform.position += this.mVelocity * JCS_Time.ItTime(mTimeType);
            }
            // Hard track
            else
            {
                // follow the object with frame distance.
                // distance = current frame position - last frame position
                Vector3 currentFramePos = mTargetTransform.position + mPositionOffset;
                this.transform.position += currentFramePos - mLastFramePos;
                mLastFramePos = currentFramePos;
            }

            // do freezing the last
            DoFreezing();

            // Do the camera boundaries check!!
            CameraBoundaries();
        }

        /// <summary>
        /// Set to the target's position in frame.
        /// </summary>
        public void SetToTargetImmediate()
        {
            if (GetTargetTransform() == null)
                return;

            Vector3 targetPos = GetTargetTransform().position;

            Vector3 newPos = this.transform.position;

            newPos.x = targetPos.x;
            newPos.y = targetPos.y;

            this.transform.position = newPos;
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

            this.mTargetPosition.z += cameraDepth;
        }

        /// <summary>
        /// 4 boundaries (top, bottom, right, left) that camera should not 
        /// go through.
        /// 
        /// check the boundries and do the trick!
        /// </summary>
        private void CameraBoundaries()
        {
            Vector3 camPos = this.transform.position;

            if (camPos.x > mMax_X_PositionInScene)
            {
                camPos.x = mMax_X_PositionInScene;
                mVelocity.x = 0;
            }
            else if (camPos.x < mMin_X_PositionInScene)
            {
                camPos.x = mMin_X_PositionInScene;
                mVelocity.x = 0;
            }

            if (camPos.y > mMax_Y_PositionInScene)
            {
                camPos.y = mMax_Y_PositionInScene;
                mVelocity.y = 0;
            }
            else if (camPos.y < mMin_Y_PositionInScene)
            {
                camPos.y = mMin_Y_PositionInScene;
                mVelocity.y = 0;
            }

            this.transform.position = camPos;
        }

        /// <summary>
        /// Do freezing.
        /// </summary>
        private void DoFreezing()
        {
            Vector3 newPos = this.transform.position;
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

            this.transform.position = newPos;
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
