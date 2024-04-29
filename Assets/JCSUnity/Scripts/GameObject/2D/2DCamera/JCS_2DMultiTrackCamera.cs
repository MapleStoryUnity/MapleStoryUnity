/**
 * $File: JCS_2DMultiTrackCamera.cs $
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
    /// Game Scene Camera.
    /// For game that we need to track multiple objects at
    /// the same time.
    /// 
    /// Using JCS_2DCamera for additional help!
    /// </summary>
    [RequireComponent(typeof(AudioListener))]
    public class JCS_2DMultiTrackCamera : MonoBehaviour
    {
        /* Variables */

        private JCS_Vector<JCS_Player> mTargetList = null;

        private AudioListener mAudioListener = null;

        [Separator("Runtime Variables (JCS_2DMultiTrackCamera)")]

        [Tooltip("Camera use to do the action.")]
        [SerializeField]
        private JCS_2DCamera mCamera = null;

        private float mLastDiffDistanceX = 0;
        private float mLastDiffDistanceY = 0;

        private float mTargetFieldOfView = 0;

        [Tooltip("How fast the camera moves.")]
        [SerializeField]
        private float mCamerSpeed = 8;

        [Tooltip("How fast the caemra zoom in/out.")]
        [SerializeField]
        private float mCameraFriction = 0.7f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        // Range under this will not do the scale effect,
        // otherwise do the scale effect
        [Tooltip("Width for view of field.")]
        [SerializeField]
        private float mViewWidth = 15;

        [Tooltip("Height for view of field.")]
        [SerializeField]
        private float mViewHeight = 5;

        [Tooltip("Mininum field of view value.")]
        [SerializeField]
        private float mMinFieldOfView = 60;

        [Tooltip("Maxinum field of view value.")]
        [SerializeField]
        private float mMaxFieldOfView = 100;

        /* Setter & Getter */

        public AudioListener GetAudioListener() { return this.mAudioListener; }
        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }
        public float MinFieldOfView { get { return this.mMinFieldOfView; } set { this.mMinFieldOfView = value; } }
        public float MaxFieldOfView { get { return this.mMaxFieldOfView; } set { this.mMaxFieldOfView = value; } }

        /* Functions */

        protected void Awake()
        {
            mTargetList = new JCS_Vector<JCS_Player>();

            mAudioListener = this.GetComponent<AudioListener>();

            // find the camera in the scene first
            mCamera = JCS_Util.FindObjectByType(typeof(JCS_2DCamera)) as JCS_2DCamera;

            if (mCamera == null)
                return;

            mCamera.SetFollowTarget(this.transform);

            // record down the fild of view
            mTargetFieldOfView = mCamera.fieldOfView;
        }

        private void Start()
        {
            JCS_SoundManager.instance.SetAudioListener(GetAudioListener());
        }

        private void Update()
        {
            if (mCamera == null)
                return;

            this.transform.position = CalculateTheCameraPosition();

            mCamera.fieldOfView += (mTargetFieldOfView - mCamera.fieldOfView) / mCameraFriction * JCS_Time.DeltaTime(mDeltaTimeType);

            if (mCamera.fieldOfView < mMinFieldOfView)
                mCamera.fieldOfView = mMinFieldOfView;
            else if (mCamera.fieldOfView > mMaxFieldOfView)
                mCamera.fieldOfView = mMaxFieldOfView;
        }

        /// <summary>
        /// Add a target to track list.
        /// </summary>
        /// <param name="p"></param>
        public void AddTargetToTrackList(JCS_Player p)
        {
            mTargetList.push(p);
        }

        /// <summary>
        /// Remove a target from track list.
        /// </summary>
        /// <param name="p"></param>
        public void RemoveTargetFromTrackList(JCS_Player p)
        {
            mTargetList.slice(p);
        }

        /// <summary>
        /// Calcualte the camera position, in order to get the 
        /// correct movement.
        /// </summary>
        /// <returns></returns>
        private Vector3 CalculateTheCameraPosition()
        {
            // no target trackable
            if (mTargetList.length == 0)
                return transform.position;

            float minHeight = 0,
                maxHeight = 0,
                minWidth = 0,
                maxWidth = 0;

            bool firstAssign = false;

            JCS_Player p = null;

            for (int index = 0; index < mTargetList.length; ++index)
            {
                p = mTargetList.at(index);

                if (p == null)
                    continue;

                Transform trans = p.transform;

                // found the first object
                if (!firstAssign)
                {
                    minWidth = trans.position.x;
                    maxWidth = trans.position.x;
                    minHeight = trans.position.y;
                    maxHeight = trans.position.y;
                    firstAssign = true;
                    continue;
                }
                else
                {
                    // if other object is height than the other
                    // override the min/max value
                    if (trans.position.x < minWidth)
                        minWidth = trans.position.x;
                    if (trans.position.x > maxWidth)
                        maxWidth = trans.position.x;

                    if (trans.position.y < minHeight)
                        minHeight = trans.position.y;
                    if (trans.position.y > maxHeight)
                        maxHeight = trans.position.y;
                }
            }

            // 找出兩個物體最遠的距離
            float currentDiffDistanceX = maxWidth - minWidth;
            float currentDiffDistanceY = maxHeight - minHeight;

            // 如果超出一定距離, 則把鏡頭往後拉
            // 這樣才能 keep track of all the object
            {
                // X-axis
                if (currentDiffDistanceX > mViewWidth)
                {
                    float temp = (currentDiffDistanceX - mLastDiffDistanceX) * mCamerSpeed;
                    //mJCS_2DCamera.DeltaFieldOfView(temp);
                    mTargetFieldOfView += temp;
                }
                // Y-axis
                if (currentDiffDistanceY > mViewHeight)
                {
                    float temp = (currentDiffDistanceY - mLastDiffDistanceY) * mCamerSpeed;
                    //mJCS_2DCamera.DeltaFieldOfView(temp);
                    mTargetFieldOfView += temp;
                }
            }

            // record down the last distance
            mLastDiffDistanceX = currentDiffDistanceX;
            mLastDiffDistanceY = currentDiffDistanceY;


            // position
            float finalPosX = ((maxWidth - minWidth) / 2) + minWidth;
            float finalPosY = ((maxHeight - minHeight) / 2) + minHeight;

            return new Vector3(finalPosX, finalPosY, transform.position.z);
        }
    }
}
