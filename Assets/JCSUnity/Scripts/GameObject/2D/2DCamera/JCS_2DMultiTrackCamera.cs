/**
 * $File: JCS_2DMultiTrackCamera.cs $
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

        [Header("** Runtime Variables (JCS_2DMultiTrackCamera) **")]

        [Tooltip("Camera use to do the action.")]
        [SerializeField]
        private JCS_2DCamera mJCS_2DCamera = null;
        
        private float mLastDiffDistanceX = 0;
        private float mLastDiffDistanceY = 0;

        private float mTargetFieldOfView = 0;

        [Tooltip("How fast the camera moves.")]
        [SerializeField]
        private float mCamerSpeed = 8;

        [Tooltip("How fast the caemra zoom in/out.")]
        [SerializeField]
        private float mCameraFriction = 0.7f;

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
        public float MinFieldOfView { get { return this.mMinFieldOfView; } set { this.mMinFieldOfView = value; } }
        public float MaxFieldOfView { get { return this.mMaxFieldOfView; } set { this.mMaxFieldOfView = value; } }


        /* Functions */

        protected void Awake()
        {

            mTargetList = new JCS_Vector<JCS_Player>();

            mAudioListener = this.GetComponent<AudioListener>();

            // find the camera in the scene first
            mJCS_2DCamera = (JCS_2DCamera)FindObjectOfType(typeof(JCS_2DCamera));

            // if still null spawn a default one!
            if (mJCS_2DCamera == null)
            {
                JCS_Debug.LogError("There is not JCS_2DCamera attach to, spawn a default one!");

                // Spawn a Default one!
                this.mJCS_2DCamera = JCS_Util.SpawnGameObject(
                    JCS_2DCamera.JCS_2DCAMERA_PATH,
                    transform.position,
                    transform.rotation).GetComponent<JCS_2DCamera>();
            }

            mJCS_2DCamera.SetFollowTarget(this.transform);

            // record down the fild of view
            mTargetFieldOfView = mJCS_2DCamera.fieldOfView;
        }

        private void Start()
        {
            JCS_SoundManager.instance.SetAudioListener(GetAudioListener());
        }

        private void Update()
        {
            this.transform.position = CalculateTheCameraPosition();

            mJCS_2DCamera.fieldOfView += (mTargetFieldOfView - mJCS_2DCamera.fieldOfView) / mCameraFriction * Time.deltaTime;

            if (mJCS_2DCamera.fieldOfView < mMinFieldOfView)
                mJCS_2DCamera.fieldOfView = mMinFieldOfView;
            else if (mJCS_2DCamera.fieldOfView > mMaxFieldOfView)
                mJCS_2DCamera.fieldOfView = mMaxFieldOfView;
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
