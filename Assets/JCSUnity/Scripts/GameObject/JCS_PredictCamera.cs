/**
 * $File: JCS_PredictCamera.cs $
 * $Date: 2017-11-12 14:10:01 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Camera will predict the movement and move toward the certain
    /// direction.
    /// </summary>
    public class JCS_PredictCamera : MonoBehaviour
    {
        /* Variables */

#if UNITY_EDITOR
        [Header("** Helper Variables (JCS_PredictCamera) **")]

        public bool testWithKey = false;

        public KeyCode targetRightKey = KeyCode.None;
        public KeyCode targetLeftKey = KeyCode.None;
        public KeyCode targetRightUpKey = KeyCode.None;
        public KeyCode targetRightDownKey = KeyCode.None;
        public KeyCode targetLeftUpKey = KeyCode.None;
        public KeyCode targetLeftDownKey = KeyCode.None;

        public KeyCode targetUpKey = KeyCode.None;
        public KeyCode targetDownKey = KeyCode.None;
        public KeyCode targetUpForwardKey = KeyCode.None;
        public KeyCode targetUpBackwardKey = KeyCode.None;
        public KeyCode targetDownForwardKey = KeyCode.None;
        public KeyCode targetDownBackwardKey = KeyCode.None;

        public KeyCode targetForwardKey = KeyCode.None;
        public KeyCode targetBakcwardKey = KeyCode.None;
        public KeyCode targetForwardRightKey = KeyCode.None;
        public KeyCode targetForwardLeftKey = KeyCode.None;
        public KeyCode targetBackwardRightKey = KeyCode.None;
        public KeyCode targetBackwardLeftKey = KeyCode.None;

        public KeyCode targetRightUpForwardKey = KeyCode.None;
        public KeyCode targetRightUpBackwardKey = KeyCode.None;
        public KeyCode targetRightDownForwardKey = KeyCode.None;
        public KeyCode targetRightDownBackwardKey = KeyCode.None;
        public KeyCode targetLeftUpForwardKey = KeyCode.None;
        public KeyCode targetLeftUpBackwardKey = KeyCode.None;
        public KeyCode targetLeftDownForwardKey = KeyCode.None;
        public KeyCode targetLeftDownBackwardKey = KeyCode.None;

        public KeyCode targetRecordKey = KeyCode.None;
#endif

        [Header("** Check Variables (JCS_PredictCamera) **")]

        [SerializeField]
        private Transform mRecordFollowTarget = null;

        [SerializeField]
        private bool mTargetingRecord = true;

        [Header("** Runtime Variables (JCS_PredictCamera) **")]

        [Tooltip("Camera to apply the effect.")]
        [SerializeField]
        private JCS_Camera mCamera = null;

        [Header("- X Axis")]

        [Tooltip("Point to move toward right.")]
        [SerializeField]
        private Transform mRightPoint = null;

        [Tooltip("Point to move toward left.")]
        [SerializeField]
        private Transform mLeftPoint = null;

        [Tooltip("Point to move toward right and up.")]
        [SerializeField]
        private Transform mRightUpPoint = null;

        [Tooltip("Point to move toward right and down.")]
        [SerializeField]
        private Transform mRightDownPoint = null;

        [Tooltip("Point to move toward left and up.")]
        [SerializeField]
        private Transform mLefttUpPoint = null;

        [Tooltip("Point to move toward left and down.")]
        [SerializeField]
        private Transform mLeftDownPoint = null;

        [Header("- Y Axis")]

        [Tooltip("Point to move toward up.")]
        [SerializeField]
        private Transform mUpPoint = null;

        [Tooltip("Point to move toward down.")]
        [SerializeField]
        private Transform mDownPoint = null;

        [Tooltip("Point to move toward up and forward.")]
        [SerializeField]
        private Transform mUpForwardPoint = null;

        [Tooltip("Point to move toward up and backward.")]
        [SerializeField]
        private Transform mUpBackwardPoint = null;

        [Tooltip("Point to move toward down and forward.")]
        [SerializeField]
        private Transform mDownForwardPoint = null;

        [Tooltip("Point to move toward down and backward.")]
        [SerializeField]
        private Transform mDownBackwardPoint = null;

        [Header("- Z Axis")]

        [Tooltip("Point to move toward foward.")]
        [SerializeField]
        private Transform mForwardPoint = null;

        [Tooltip("Point to move toward backward.")]
        [SerializeField]
        private Transform mBackwardPoint = null;

        [Tooltip("Point to move toward forward and right.")]
        [SerializeField]
        private Transform mForwardRightPoint = null;

        [Tooltip("Point to move toward forward and left.")]
        [SerializeField]
        private Transform mForwardLeftPoint = null;

        [Tooltip("Point to move toward backward and right.")]
        [SerializeField]
        private Transform mBackwardRightPoint = null;

        [Tooltip("Point to move toward backward and left.")]
        [SerializeField]
        private Transform mBackwardLeftPoint = null;

        [Header("- 8 Corners")]

        [Tooltip("Point to move toward right, up and forward.")]
        [SerializeField]
        private Transform mRightUpForwardPoint = null;

        [Tooltip("Point to move toward right, up and backward.")]
        [SerializeField]
        private Transform mRightUpBackwardPoint = null;

        [Tooltip("Point to move toward right, down and forward.")]
        [SerializeField]
        private Transform mRightDownForwardPoint = null;

        [Tooltip("Point to move toward right, down and backward.")]
        [SerializeField]
        private Transform mRightDownBackwardPoint = null;

        [Tooltip("Point to move toward left, up and forward.")]
        [SerializeField]
        private Transform mLeftUpForwardPoint = null;

        [Tooltip("Point to move toward left, up and backward.")]
        [SerializeField]
        private Transform mLeftUpBackwardPoint = null;

        [Tooltip("Point to move toward left, down, and forward.")]
        [SerializeField]
        private Transform mLeftDownForwardPoint = null;

        [Tooltip("Point to move toward left, down and backward.")]
        [SerializeField]
        private Transform mLeftDownBackwardPoint = null;

        /* Setter & Getter */

        public bool TargetingRecord { get { return this.mTargetingRecord; } }

        /* X axis */
        public Transform RightPoint { get { return this.mRightPoint; } set { this.mRightPoint = value; } }
        public Transform LeftPoint { get { return this.mLeftPoint; } set { this.mLeftPoint = value; } }
        public Transform RightUpPoint { get { return this.mRightUpPoint; } set { this.mRightUpPoint = value; } }
        public Transform RightDownPoint { get { return this.mRightDownPoint; } set { this.mRightDownPoint = value; } }
        public Transform LefttUpPoint { get { return this.mLefttUpPoint; } set { this.mLefttUpPoint = value; } }
        public Transform LeftDownPoint { get { return this.mLeftDownPoint; } set { this.mLeftDownPoint = value; } }

        /* Y axis */
        public Transform UpPoint { get { return this.mUpPoint; } set { this.mUpPoint = value; } }
        public Transform DownPoint { get { return this.mDownPoint; } set { this.mDownPoint = value; } }
        public Transform UpForwardPoint { get { return this.mUpForwardPoint; } set { this.mUpForwardPoint = value; } }
        public Transform UpBackwardPoint { get { return this.mUpBackwardPoint; } set { this.mUpBackwardPoint = value; } }
        public Transform DownForwardPoint { get { return this.mDownForwardPoint; } set { this.mDownForwardPoint = value; } }
        public Transform DownBackwardPoint { get { return this.mDownBackwardPoint; } set { this.mDownBackwardPoint = value; } }

        /* Z axis */
        public Transform ForwardPoint { get { return this.mForwardPoint; } set { this.mForwardPoint = value; } }
        public Transform BackwardPoint { get { return this.mBackwardPoint; } set { this.mBackwardPoint = value; } }
        public Transform ForwardRightPoint { get { return this.mForwardRightPoint; } set { this.mForwardRightPoint = value; } }
        public Transform ForwardLeftPoint { get { return this.mForwardLeftPoint; } set { this.mForwardLeftPoint = value; } }
        public Transform BackwardRightPoint { get { return this.mBackwardRightPoint; } set { this.mBackwardRightPoint = value; } }
        public Transform BackwardLeftPoint { get { return this.mBackwardLeftPoint; } set { this.mBackwardLeftPoint = value; } }

        /* 8 Corners */
        public Transform RightUpForwardPoint { get { return this.mRightUpForwardPoint; } set { this.mRightUpForwardPoint = value; } }
        public Transform RightUpBackwardPoint { get { return this.mRightUpBackwardPoint; } set { this.mRightUpBackwardPoint = value; } }
        public Transform RightDownForwardPoint { get { return this.mRightDownForwardPoint; } set { this.mRightDownForwardPoint = value; } }
        public Transform RightDownBackwardPoint { get { return this.mRightDownBackwardPoint; } set { this.mRightDownBackwardPoint = value; } }
        public Transform LeftUpForwardPoint { get { return this.mLeftUpForwardPoint; } set { this.mLeftUpForwardPoint = value; } }
        public Transform LeftUpBackwardPoint { get { return this.mLeftUpBackwardPoint; } set { this.mLeftUpBackwardPoint = value; } }
        public Transform LeftDownForwardPoint { get { return this.mLeftDownForwardPoint; } set { this.mLeftDownForwardPoint = value; } }
        public Transform LeftDownBackwardPoint { get { return this.mLeftDownBackwardPoint; } set { this.mLeftDownBackwardPoint = value; } }

        /* Functions */

        private void Awake()
        {
            // try to get camera from it transform.
            if (this.mCamera == null)
                this.mCamera = this.GetComponent<JCS_Camera>();

            SetEffectCamera(this.mCamera);
        }

#if UNITY_EDITOR
        private void Update()
        {
            Test();
        }

        private void Test()
        {
            if (!testWithKey)
                return;

            /* X axis */
            if (JCS_Input.GetKeyDown(targetRightKey))
                TargetRight();
            if (JCS_Input.GetKeyDown(targetLeftKey))
                TargetLeft();
            if (JCS_Input.GetKeyDown(targetRightUpKey))
                TargetRightUp();
            if (JCS_Input.GetKeyDown(targetRightDownKey))
                TargetRightDown();
            if (JCS_Input.GetKeyDown(targetLeftUpKey))
                TargetLeftUp();
            if (JCS_Input.GetKeyDown(targetLeftDownKey))
                TargetLeftDown();

            /* Y axis */
            if (JCS_Input.GetKeyDown(targetUpKey))
                TargetUp();
            if (JCS_Input.GetKeyDown(targetDownKey))
                TargetDown();
            if (JCS_Input.GetKeyDown(targetUpForwardKey))
                TargetUpForward();
            if (JCS_Input.GetKeyDown(targetUpBackwardKey))
                TargetUpBackward();
            if (JCS_Input.GetKeyDown(targetDownForwardKey))
                TargetDownForward();
            if (JCS_Input.GetKeyDown(targetDownBackwardKey))
                TargetDownBackward();

            /* Z axis */
            if (JCS_Input.GetKeyDown(targetForwardKey))
                TargetForward();
            if (JCS_Input.GetKeyDown(targetBakcwardKey))
                TargetBackward();
            if (JCS_Input.GetKeyDown(targetForwardRightKey))
                TargetForwardRight();
            if (JCS_Input.GetKeyDown(targetForwardLeftKey))
                TargetForwardLeft();
            if (JCS_Input.GetKeyDown(targetBackwardRightKey))
                TargetBackwardRight();
            if (JCS_Input.GetKeyDown(targetBackwardLeftKey))
                TargetBackwardLeft();


            /* 8 corners */
            if (JCS_Input.GetKeyDown(targetRightUpForwardKey))
                TargetRightUpForward();
            if (JCS_Input.GetKeyDown(targetRightUpBackwardKey))
                TargetRightUpBackward();
            if (JCS_Input.GetKeyDown(targetRightDownForwardKey))
                TargetRightDownForward();
            if (JCS_Input.GetKeyDown(targetRightDownForwardKey))
                TargetRightDownBackward();
            if (JCS_Input.GetKeyDown(targetLeftUpForwardKey))
                TargetLeftUpForward();
            if (JCS_Input.GetKeyDown(targetLeftUpBackwardKey))
                TargetLeftUpBackward();
            if (JCS_Input.GetKeyDown(targetLeftDownForwardKey))
                TargetLeftDownForward();
            if (JCS_Input.GetKeyDown(targetLeftDownBackwardKey))
                TargetLeftDownBackward();

            /* Record */
            if (JCS_Input.GetKeyDown(targetRecordKey))
                TargetRecord();
        }
#endif

        /// <summary>
        /// Apply the effect of the camera.
        /// </summary>
        public void SetEffectCamera(JCS_Camera cam)
        {
            this.mCamera = cam;
        }

        /// <summary>
        /// Target what every it was recorded.
        /// </summary>
        public void TargetRecord()
        {
            // target original transform we were targeting.
            this.mCamera.SetFollowTarget(this.mRecordFollowTarget);

            mTargetingRecord = true;
        }

        /** 
         * X axis. 
         **/

        /// <summary>
        /// Target point right.
        /// </summary>
        public void TargetRight()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mRightPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point left.
        /// </summary>
        public void TargetLeft()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mLeftPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point right up.
        /// </summary>
        public void TargetRightUp()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mRightUpPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point right down.
        /// </summary>
        public void TargetRightDown()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mRightDownPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point left up.
        /// </summary>
        public void TargetLeftUp()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mLefttUpPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point left down.
        /// </summary>
        public void TargetLeftDown()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mLeftDownPoint);

            mTargetingRecord = false;
        }

        /** 
         * Y axis. 
         **/

        /// <summary>
        /// Target point up.
        /// </summary>
        public void TargetUp()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mUpPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point down.
        /// </summary>
        public void TargetDown()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mDownPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point up forward.
        /// </summary>
        public void TargetUpForward()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mUpForwardPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point up backward.
        /// </summary>
        public void TargetUpBackward()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mUpBackwardPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point down forward.
        /// </summary>
        public void TargetDownForward()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mDownForwardPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point down backward.
        /// </summary>
        public void TargetDownBackward()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mDownBackwardPoint);

            mTargetingRecord = false;
        }

        /** 
         * Z axis. 
         **/

        /// <summary>
        /// Target point forward.
        /// </summary>
        public void TargetForward()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mForwardPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point backward.
        /// </summary>
        public void TargetBackward()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mBackwardPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point forward right.
        /// </summary>
        public void TargetForwardRight()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mForwardRightPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point forward left.
        /// </summary>
        public void TargetForwardLeft()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mForwardLeftPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point backward right.
        /// </summary>
        public void TargetBackwardRight()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mBackwardRightPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point backward left.
        /// </summary>
        public void TargetBackwardLeft()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mBackwardLeftPoint);

            mTargetingRecord = false;
        }

        /** 
         * 8 Corners.
         **/

        /// <summary>
        /// Target point right up forward.
        /// </summary>
        public void TargetRightUpForward()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mRightUpForwardPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point right up backward.
        /// </summary>
        public void TargetRightUpBackward()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mRightUpBackwardPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point right down forward.
        /// </summary>
        public void TargetRightDownForward()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mRightDownForwardPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point right down backward.
        /// </summary>
        public void TargetRightDownBackward()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mRightDownBackwardPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point left up forward.
        /// </summary>
        public void TargetLeftUpForward()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mLeftUpForwardPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point left up backward.
        /// </summary>
        public void TargetLeftUpBackward()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mLeftUpBackwardPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point left down forward.
        /// </summary>
        public void TargetLeftDownForward()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mLeftDownForwardPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Target point left down backward.
        /// </summary>
        public void TargetLeftDownBackward()
        {
            if (mCamera == null)
                return;

            // Only record if we are target the original transform 
            // right now.
            if (mTargetingRecord)
            {
                // record before moving.
                RecordFollowTargetFromCamera();
            }

            // target the target point.
            this.mCamera.SetFollowTarget(mLeftDownBackwardPoint);

            mTargetingRecord = false;
        }

        /// <summary>
        /// Recrod down the follow target.
        /// </summary>
        /// <param name="trans"> follow target's transform to record. </param>
        private void RecordFollowTargetFromCamera()
        {
            this.mRecordFollowTarget = this.mCamera.GetFollowTarget();
        }
    }
}
