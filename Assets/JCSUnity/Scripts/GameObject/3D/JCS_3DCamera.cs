/**
 * $File: JCS_3DCamera.cs $
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
    /// Basic camera in 3d game.
    /// </summary>
    [RequireComponent(typeof(AudioListener))]
    public class JCS_3DCamera : JCS_Camera
    {
        /* Variables */

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_3DCamera) **")]

        [Tooltip("Test this component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;
#endif

        [Header("** Check Variables (JCS_3DCamera) **")]

        [Tooltip("Current tracking position.")]
        [SerializeField]
        private Vector3 mTrackPosition = Vector3.zero;

        [Tooltip("Current revolution that do the revolution.")]
        [SerializeField]
        private float mCurrentRevolution = 0.0f;

        [Header("** Runtime Variables (JCS_3DCamera) **")]

        [Tooltip("Target we want to look at.")]
        [SerializeField]
        private Transform mTargetTransform = null;

        [Tooltip("Targeting revolution.")]
        [SerializeField]
        private float mTargetRevolution = 0.0f;

        [Header("- Speed / Friction")]

        [Tooltip("How fast the camera track in each axis?")]
        [SerializeField]
        [Range(0.01f, 10.0f)]
        private float mSmoothTrackFriction = 0.2f;

        [Header("- Rotate Camera")]

        [Tooltip("How fast this camera rotates.")]
        [SerializeField]
        [Range(0.001f, 30.0f)]
        private float mRotateFriction = 0.2f;

        [Tooltip("Angle that rotate once.")]
        [SerializeField]
        [Range(0.0f, 360.0f)]
        private float mRotateAngle = 10.0f;

#if (UNITY_EDITOR || UNITY_STANDALONE)
        [Tooltip("Key to rotate around the left side.")]
        [SerializeField]
        private KeyCode mRotateAroundLeft = KeyCode.None;

        [Tooltip("Key to rotate around the right side.")]
        [SerializeField]
        private KeyCode mRotateAroundRight = KeyCode.None;

        [Header("** Reset Camera Settings (JCS_3DCamera) **")]

        [Tooltip("Key to reset the camera.")]
        [SerializeField]
        private KeyCode mResetKeyCode = KeyCode.None;

#endif
        [Tooltip("Angle when reset the camera.")]
        [SerializeField]
        [Range(0.0f, 180.0f)]
        private float mResetTargetAngle = 0.0f;

        [Header("- Up Down")]

        [Tooltip("Up and Down movement enable?")]
        [SerializeField]
        private bool mUpDownMovement = true;

#if (UNITY_EDITOR || UNITY_STANDALONE)
        [Tooltip("Key to go up.")]
        [SerializeField]
        private KeyCode mUpKey = KeyCode.None;

        [Tooltip("Key to go down.")]
        [SerializeField]
        private KeyCode mDownKey = KeyCode.None;
#endif

        [Tooltip("Space between each up and down movement.")]
        [SerializeField]
        private float mUpDownSpacing = 10;

        [Header("- Min / Max")]

        [Tooltip("How high the camera can reach.")]
        [SerializeField]
        private float mMaxHeight = 10;

        [Tooltip("How low the camera can reach.")]
        [SerializeField]
        private float mMinHeight = 0;

        private float mTargetHeight = 0;

        [Header("- Speed / Friction")]

        [Tooltip("How fast it change the view up and down?")]
        [SerializeField]
        private float mUpDownFriction = 0.2f;

        [Header("- Scroll / Zoom Settings")]

        [Tooltip("Do the zoom effect?")]
        [SerializeField]
        private bool mZoomEffect = true;

        [Tooltip("Distance once you scroll with mouse.")]
        [SerializeField]
        [Range(0.001f, 500.0f)]
        private float mScrollRange_Mouse = 120.0f;

        [Tooltip("Distance once you scroll with multi-touch.")]
        [SerializeField]
        [Range(0.001f, 500.0f)]
        private float mScrollRange_Touch = 1.0f;

        [Tooltip("How fast it scroll speed get reduce?")]
        [SerializeField]
        [Range(0.1f, 5.0f)]
        private float mScrollSpeedFriction = 0.4f;

        // the real target of the speed
        private float mTargetScrollSpeed = 0.0f;

        // Record the position down for zoom effect.
        private Vector3 mRecordPosition = Vector3.zero;

        // scroll distance.
        private float mWheelDegree = 0.0f;

        [Header("- Min / Max")]

        [Tooltip("Mininum distance camera can approach to?")]
        [SerializeField]
        [Range(0.01f, 500.0f)]
        private float mMinDistance = 2.0f;

        [Tooltip("Maxinum distance camera can far away from?")]
        [SerializeField]
        [Range(10.0f, 1000.0f)]
        private float mMaxDistance = 200.0f;

        /* Setter & Getter */

        public Transform FollowTarget { get { return this.mTargetTransform; } set { this.mTargetTransform = value; } }
        public override void SetFollowTarget(Transform targ) { this.mTargetTransform = targ; }
        public override Transform GetFollowTarget() { return this.mTargetTransform; }

        public bool ZoomEffect { get { return this.mZoomEffect; } set { this.mZoomEffect = value; } }

        public float ScrollRange_Mouse { get { return this.mScrollRange_Mouse; } set { this.mScrollRange_Mouse = value; } }
        public float ScrollRange_Touch { get { return this.mScrollRange_Touch; } set { this.mScrollRange_Touch = value; } }

        public float RotateAngle { get { return this.mRotateAngle; } set { this.mRotateAngle = value; } }
        public float ResetTargetAngle { get { return this.mResetTargetAngle; } set { this.mResetTargetAngle = value; } }

        public float MinDistance { get { return this.mMinDistance; } set { this.mMinDistance = value; } }
        public float MaxDistance { get { return this.mMaxDistance; } set { this.mMaxDistance = value; } }

        /* Functions */

        protected override void Start()
        {
            base.Start();

            if (mTargetTransform != null)
                mLastFramePos = mTargetTransform.position;

            // record down the height
            mTargetHeight = this.transform.position.y;

            // record down the transform position.
            mTrackPosition = this.transform.position;
        }

        protected override void LateUpdate()
        {
#if (UNITY_EDITOR)
            Test();
#endif
            base.LateUpdate();

            if (mTargetTransform == null)
                return;

            InputCamera();

            DoFollowing();

            UpDownMovement();


            this.mRecordPosition = this.transform.position;

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
                JCS_SlideInput slideInput = JCS_InputManager.instance.GetGlobalSlideInput();
                mWheelDegree = slideInput.TouchDistanceDelta;
#endif
                ZoomCamera(mWheelDegree);
            }

            Vector3 newPos = Vector3.forward * mTargetScrollSpeed * Time.deltaTime;

            // if is valid, do action.
            if (!JCS_Mathf.IsNaN(newPos))
            {
                // record down the position before calling the
                // translate function
                Vector3 currentPos = this.transform.position;

                // do translate base on the scrolling distance
                // we get from the input buffer.
                this.transform.Translate(newPos);

                // get the updated position!
                Vector3 afterTransPos = this.transform.position;

                // update the track position
                mTrackPosition += afterTransPos - currentPos;
            }

            // Fix the speed if reach the distance!
            FixedMinMaxDistance();

            // asymptotic back to zero
            mTargetScrollSpeed += (0.0f - mTargetScrollSpeed) / mScrollSpeedFriction * Time.deltaTime;

            // asymptotic revolution
            {
                float revoDelta = (mTargetRevolution - mCurrentRevolution) / mRotateFriction * Time.deltaTime;

                mCurrentRevolution += revoDelta;

                this.transform.RotateAround(mTargetTransform.position, Vector3.up, revoDelta);
            }
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(KeyCode.N))
            {
                SmoothTrack = !SmoothTrack;
            }
        }
#endif

        /// <summary>
        /// Rotate around the target toward right.
        /// </summary>
        public void RotateAroundRight()
        {
            DeltaRevolution(-mRotateAngle);
        }

        /// <summary>
        /// Rotate around the target toward left.
        /// </summary>
        public void RotateAroundLeft()
        {
            DeltaRevolution(mRotateAngle);
        }

        /// <summary>
        /// Reset the camera.
        /// </summary>
        public void ResetCamera()
        {
            if (mTargetTransform == null)
            {
                JCS_Debug.LogError("There is no target to reset the camera!");
                return;
            }

            SetToRevoluationAngle(mResetTargetAngle);
        }

        /// <summary>
        /// Move the camera upward.
        /// </summary>
        public void UpCamera()
        {
            mTargetHeight += mUpDownSpacing;

            if (mMaxHeight < mTargetHeight)
                mTargetHeight = mMaxHeight;
        }

        /// <summary>
        /// Move the caemra downward.
        /// </summary>
        public void DownCamera()
        {
            mTargetHeight -= mUpDownSpacing;

            if (mMinHeight > mTargetHeight)
                mTargetHeight = mMinHeight;
        }

        /// <summary>
        /// Do the follow algorithm.
        /// </summary>
        private void DoFollowing()
        {
            if (!mFollowing)
                return;

            // Smooth track.
            if (mSmoothTrack)
            {
                // follow the object with frame distance.
                // distance = current frame position - last frame position
                // , but in smooth track, we set the track position instead 
                // of the real position.
                Vector3 currentFramePos = mTargetTransform.position + mPositionOffset;
                mTrackPosition += currentFramePos - mLastFramePos;
                mLastFramePos = currentFramePos;

                // update the transform position
                this.transform.position += (this.mTrackPosition - this.transform.position) / mSmoothTrackFriction * Time.deltaTime;
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
        }

        /// <summary>
        /// Set the current revolution angle.
        /// </summary>
        /// <param name="angle"></param>
        private void SetToRevoluationAngle(float angle)
        {
            float targetAngle = angle;

            // Make it respect to the target transform.
            if (mTargetTransform)
                targetAngle += mTargetTransform.eulerAngles.y;

            // Calculate in order to rotate revolution to the nearest distance.
            float delta = targetAngle - mTargetRevolution;
            if (delta < -180.0f)
                delta += 360.0f;
            else if (delta > 180.0f)
                delta -= 360.0f;

            DeltaRevolution(delta);
        }

        /// <summary>
        /// Up down movement algorithm.
        /// </summary>
        private void UpDownMovement()
        {
            // Check if the effect enabled.
            if (!mUpDownMovement)
                return;

            // get current position.
            Vector3 newPos = this.transform.position;

            {
                // do the delta position formula
                float deltaHeight = (mTargetHeight - newPos.y) / mUpDownFriction * Time.deltaTime;

                // apply to new position, so get ready to 
                // apply on the current position.
                newPos.y += deltaHeight;

                // NOTE(jenchieh): also apply to the smooth track.
                mTrackPosition.y += deltaHeight;
            }

            // apply new position
            this.transform.position = newPos;
        }

        /// <summary>
        /// Process the input of this camera.
        /// </summary>
        private void InputCamera()
        {
#if (UNITY_EDITOR || UNITY_STANDALONE)
            if (JCS_Input.GetKeyDown(mResetKeyCode))
                ResetCamera();

            if (JCS_Input.GetKeyDown(mUpKey))
                UpCamera();
            else if (JCS_Input.GetKeyDown(mDownKey))
                DownCamera();

            if (JCS_Input.GetKey(mRotateAroundLeft))
                RotateAroundLeft();
            else if (JCS_Input.GetKey(mRotateAroundRight))
                RotateAroundRight();
#endif
        }

        /// <summary>
        /// Do the scrolling algorithm.
        /// </summary>
        private void ZoomCamera(float depthDistance)
        {
            // Check the trigger of the scrolling effect.
            if (!mZoomEffect)
                return;

            if (depthDistance == 0.0f)
                return;

#if (UNITY_EDITOR || UNITY_STANDALONE)
            this.mTargetScrollSpeed = this.mWheelDegree * this.mScrollRange_Mouse;
#elif (UNITY_ANDROID || UNITY_IPHIONE || UNITY_IOS)
            this.mTargetScrollSpeed = this.mWheelDegree * this.mScrollRange_Touch;
#endif
        }

        /// <summary>
        /// Fix the speed/range if reach the min/max distance.
        /// </summary>
        private void FixedMinMaxDistance()
        {
            float currentDistance = Vector3.Distance(this.transform.position, mTargetTransform.position);
            if (JCS_Mathf.IsPositive(mTargetScrollSpeed))
            {
                if (currentDistance <= mMinDistance)
                {
                    mTargetScrollSpeed = 0.0f;

                    Vector3 newPos = this.transform.position;
                    newPos.z = mRecordPosition.z;
                    this.transform.position = newPos;
                }
            }
            else
            {
                if (currentDistance >= mMaxDistance)
                {
                    mTargetScrollSpeed = 0.0f;

                    Vector3 newPos = this.transform.position;
                    newPos.z = mRecordPosition.z;
                    this.transform.position = newPos;
                }
            }
        }

        /// <summary>
        /// Safely delta the current revolution by ANGLE.
        /// </summary>
        /// <param name="angle"> Angle value to delta. </param>
        private void DeltaRevolution(float angle)
        {
            mTargetRevolution += angle;

            if (mTargetRevolution < 0.0f)
            {
                mTargetRevolution += 360.0f;
                mCurrentRevolution += 360.0f;
            }
            else if (mTargetRevolution > 360.0f)
            {
                mTargetRevolution -= 360.0f;
                mCurrentRevolution -= 360.0f;
            }
        }
    }
}

