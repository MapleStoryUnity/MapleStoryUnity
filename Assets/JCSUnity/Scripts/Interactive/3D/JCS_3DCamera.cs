/**
 * $File: JCS_3DCamera.cs $
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
    /// Basic camera in 3d game.
    /// </summary>
    [RequireComponent(typeof(AudioListener))]
    public class JCS_3DCamera : JCS_Camera
    {
        /* Variables */

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_3DCamera)")]

        [Tooltip("Test this component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;
#endif

        [Separator("Check Variables (JCS_3DCamera)")]

        [Tooltip("Current tracking position.")]
        [SerializeField]
        [ReadOnly]
        private Vector3 mTrackPosition = Vector3.zero;

        [Tooltip("Current revolution that do the revolution.")]
        [SerializeField]
        [ReadOnly]
        private float mCurrentRevolution = 0.0f;

        [Separator("Runtime Variables (JCS_3DCamera)")]

        [Tooltip("Targeting revolution.")]
        [SerializeField]
        private float mTargetRevolution = 0.0f;

        [Header("Speed / Friction")]

        [Tooltip("How fast the camera track in each axis?")]
        [SerializeField]
        [Range(0.01f, 10.0f)]
        private float mSmoothTrackFriction = 0.2f;

        [Header("Rotate Camera")]

        [Tooltip("How fast this camera rotates.")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 30.0f)]
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

        [Separator("Reset Camera Settings (JCS_3DCamera)")]

        [Tooltip("Key to reset the camera.")]
        [SerializeField]
        private KeyCode mResetKeyCode = KeyCode.None;

#endif
        [Tooltip("Angle when reset the camera.")]
        [SerializeField]
        [Range(0.0f, 180.0f)]
        private float mResetTargetAngle = 0.0f;

        [Header("Up Down")]

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

        [Header("Min / Max")]

        [Tooltip("How high the camera can reach.")]
        [SerializeField]
        private float mMaxHeight = 10;

        [Tooltip("How low the camera can reach.")]
        [SerializeField]
        private float mMinHeight = 0;

        private float mTargetHeight = 0;

        [Header("Speed / Friction")]

        [Tooltip("How fast it change the view up and down?")]
        [SerializeField]
        private float mUpDownFriction = 0.2f;

        [Header("Scroll / Zoom Settings")]

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

        [Header("Min / Max")]

        [Tooltip("Mininum distance camera can approach to?")]
        [SerializeField]
        [Range(0.01f, 500.0f)]
        private float mMinDistance = 2.0f;

        [Tooltip("Maxinum distance camera can far away from?")]
        [SerializeField]
        [Range(10.0f, 1000.0f)]
        private float mMaxDistance = 200.0f;

        /* Setter & Getter */

        public Transform followTarget { get { return mTargetTransform; } set { mTargetTransform = value; } }
        public override void SetFollowTarget(Transform targ) { mTargetTransform = targ; }
        public override Transform GetFollowTarget() { return mTargetTransform; }

        public bool zoomEffect { get { return mZoomEffect; } set { mZoomEffect = value; } }

        public float scrollRange_Mouse { get { return mScrollRange_Mouse; } set { mScrollRange_Mouse = value; } }
        public float scrollRange_Touch { get { return mScrollRange_Touch; } set { mScrollRange_Touch = value; } }

        public float rotateAngle { get { return mRotateAngle; } set { mRotateAngle = value; } }
        public float resetTargetAngle { get { return mResetTargetAngle; } set { mResetTargetAngle = value; } }

        public float minDistance { get { return mMinDistance; } set { mMinDistance = value; } }
        public float maxDistance { get { return mMaxDistance; } set { mMaxDistance = value; } }

        /* Functions */

        protected override void Start()
        {
            base.Start();

            if (mTargetTransform != null)
                mLastFramePos = mTargetTransform.position;

            // record down the height
            mTargetHeight = transform.position.y;

            // record down the transform position.
            mTrackPosition = transform.position;
        }

        protected override void LateUpdate()
        {
#if UNITY_EDITOR
            Test();
#endif
            base.LateUpdate();

            if (mTargetTransform == null)
                return;

            InputCamera();

            DoFollowing();

            UpDownMovement();

            mRecordPosition = transform.position;

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

            Vector3 newPos = Vector3.forward * mTargetScrollSpeed * JCS_Time.ItTime(mTimeType);

            // if is valid, do action.
            if (!JCS_Mathf.IsNaN(newPos))
            {
                // record down the position before calling the
                // translate function
                Vector3 currentPos = transform.position;

                // do translate base on the scrolling distance
                // we get from the input buffer.
                transform.Translate(newPos);

                // get the updated position!
                Vector3 afterTransPos = transform.position;

                // update the track position
                mTrackPosition += afterTransPos - currentPos;
            }

            // Fix the speed if reach the distance!
            FixedMinMaxDistance();

            float dt = JCS_Time.ItTime(mTimeType);

            // asymptotic back to zero
            mTargetScrollSpeed += (0.0f - mTargetScrollSpeed) / mScrollSpeedFriction * dt;

            // asymptotic revolution
            {
                float revoDelta = (mTargetRevolution - mCurrentRevolution) / mRotateFriction * dt;

                mCurrentRevolution += revoDelta;

                transform.RotateAround(mTargetTransform.position, Vector3.up, revoDelta);
            }
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(KeyCode.N))
            {
                smoothTrack = !smoothTrack;
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
                Debug.LogError("There is no target to reset the camera!");
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
                transform.position += (mTrackPosition - transform.position) / mSmoothTrackFriction * JCS_Time.ItTime(mTimeType);
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
            Vector3 newPos = transform.position;

            {
                // do the delta position formula
                float deltaHeight = (mTargetHeight - newPos.y) / mUpDownFriction * JCS_Time.ItTime(mTimeType);

                // apply to new position, so get ready to 
                // apply on the current position.
                newPos.y += deltaHeight;

                // NOTE(jenchieh): also apply to the smooth track.
                mTrackPosition.y += deltaHeight;
            }

            // apply new position
            transform.position = newPos;
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
            mTargetScrollSpeed = mWheelDegree * mScrollRange_Mouse;
#elif (UNITY_ANDROID || UNITY_IPHIONE || UNITY_IOS)
            mTargetScrollSpeed = mWheelDegree * mScrollRange_Touch;
#endif
        }

        /// <summary>
        /// Fix the speed/range if reach the min/max distance.
        /// </summary>
        private void FixedMinMaxDistance()
        {
            float currentDistance = Vector3.Distance(transform.position, mTargetTransform.position);
            if (JCS_Mathf.IsPositive(mTargetScrollSpeed))
            {
                if (currentDistance <= mMinDistance)
                {
                    mTargetScrollSpeed = 0.0f;

                    Vector3 newPos = transform.position;
                    newPos.z = mRecordPosition.z;
                    transform.position = newPos;
                }
            }
            else
            {
                if (currentDistance >= mMaxDistance)
                {
                    mTargetScrollSpeed = 0.0f;

                    Vector3 newPos = transform.position;
                    newPos.z = mRecordPosition.z;
                    transform.position = newPos;
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

