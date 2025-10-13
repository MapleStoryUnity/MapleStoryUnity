/**
 * $File: JCS_TransformTweener.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using PeterVuorela.Tweener;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Transform tweener.
    /// </summary>
    public class JCS_TransformTweener : JCS_UnityObject
    {
        /* Variables */

        // Callback to execute when start tweening.
        public Action onStart = null;

        // Callback to execute when done tweening.
        public Action onDone = null;

        // Callback to execute when done tweening but only with that
        // specific function call.
        private Action mOnDone = null;

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_TransformTweener)")]

        [Tooltip("Test component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key to active tween to point A.")]
        [SerializeField]
        private KeyCode mTweenToAKey = KeyCode.A;

        [Tooltip("Point A to tween to.")]
        [SerializeField]
        private Vector3 mTweenToA = new Vector3(0.0f, 0.0f, 0.0f);

        [Tooltip("Key to active tween to point B.")]
        [SerializeField]
        private KeyCode mTweenToBKey = KeyCode.B;

        [Tooltip("Point B to tween to.")]
        [SerializeField]
        private Vector3 mTweenToB = new Vector3(5.0f, 5.0f, 5.0f);

        [Tooltip("Key to tween continue.")]
        [SerializeField]
        private KeyCode mContinueKeyTween = KeyCode.C;
#endif

        private Tweener mTweenerX = new();
        private Tweener mTweenerY = new();
        private Tweener mTweenerZ = new();

        [Separator("Check Variables (JCS_TransformTweener)")]

        [SerializeField]
        [ReadOnly]
        private bool mContinueTween = false;

        [Tooltip("Whats the target we tween to?")]
        [SerializeField]
        [ReadOnly]
        private JCS_UnityObject mTarget = null;

        // use to check if the target transform move or not.
        private Vector3 mRecordTargetTransformValue = Vector3.zero;

        private JCS_UnityObject mRecordTarget = null;

        [Tooltip("Flag to check if done tweening on x-axis.")]
        [SerializeField]
        [ReadOnly]
        private bool mDoneTweenX = true;

        [Tooltip("Flag to check if done tweening on y-axis.")]
        [SerializeField]
        [ReadOnly]
        private bool mDoneTweenY = true;

        [Tooltip("Flag to check if done tweening on z-axis.")]
        [SerializeField]
        [ReadOnly]
        private bool mDoneTweenZ = true;

        [Tooltip("Is done tweening/animating?")]
        [SerializeField]
        [ReadOnly]
        private bool mIsDoneTweening = true;

        [Separator("Runtime Variables (JCS_TransformTweener)")]

        [Tooltip("Do the tween effect?")]
        [SerializeField]
        private bool mTween = true;

        [Tooltip("Value offset.")]
        [SerializeField]
        private Vector3 mValueOffset = Vector3.zero;

        [Tooltip("How fast it moves on x axis.")]
        [SerializeField]
        [Range(0.01f, 1000.0f)]
        private float mDurationX = 1.0f;

        [Tooltip("How fast it moves on y axis.")]
        [SerializeField]
        [Range(0.01f, 1000.0f)]
        private float mDurationY = 1.0f;

        [Tooltip("How fast it moves on z axis.")]
        [SerializeField]
        [Range(0.01f, 1000.0f)]
        private float mDurationZ = 1.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("Destroy")]

        [Tooltip("Destroy this object when done tweening?")]
        [SerializeField]
        private bool mDestroyWhenDoneTweening = false;

        [Tooltip("How many times of done tweening destroy will active?")]
        [SerializeField]
        [Range(1, 10)]
        private int mDestroyDoneTweeningCount = 1;

        [Header("Randomize Duration")]

        [Tooltip("Randomize the durations with all axis at start. (x, y, z)")]
        [SerializeField]
        [Range(0.0f, 1000.0f)]
        private float mRandomizeDuration = 0.0f;

        [Header("Tweener Effect Transform")]

        [Tooltip("Which transform's properties to tween.")]
        [SerializeField]
        private JCS_TransformType mTweenType = JCS_TransformType.POSITION;

        [Tooltip("Change the self position as local position.")]
        [SerializeField]
        private bool mTrackAsLocalSelf = false;

        [Tooltip("Track the target as local position.")]
        [SerializeField]
        private bool mTrackAsLocalTarget = false;

        [Header("Tweener Formula Type")]

        [Tooltip("Tweener formula on x axis.")]
        [SerializeField]
        private JCS_TweenType mEasingX = JCS_TweenType.LINEAR;

        [Tooltip("Tweener formula on y axis.")]
        [SerializeField]
        private JCS_TweenType mEasingY = JCS_TweenType.LINEAR;

        [Tooltip("Tweener formula on z axis.")]
        [SerializeField]
        private JCS_TweenType mEasingZ = JCS_TweenType.LINEAR;

        [Header("Continuous Tween (JCS_TransformTweener) ")]

        [Tooltip("While continue tween when did the tweener algorithm stop?")]
        [SerializeField]
        [Range(0.0f, 1000.0f)]
        private float mStopTweenDistance = 1;

        /* Setter & Getter */

        public Tweener tweenerX { get { return mTweenerX; } }
        public Tweener tweenerY { get { return mTweenerY; } }
        public Tweener tweenerZ { get { return mTweenerZ; } }

        public bool isDoneTweening { get { return mIsDoneTweening; } }
        public bool doneTweenX { get { return mDoneTweenX; } }
        public bool doneTweenY { get { return mDoneTweenY; } }
        public bool doneTweenZ { get { return mDoneTweenZ; } }

        public bool tween { get { return mTween; } set { mTween = value; } }
        public bool trackAsLocalSelf { get { return mTrackAsLocalSelf; } set { mTrackAsLocalSelf = value; } }
        public bool trackAsLocalTarget { get { return mTrackAsLocalTarget; } set { mTrackAsLocalTarget = value; } }
        public float stopTweenDistance { get { return mStopTweenDistance; } set { mStopTweenDistance = value; } }

        public float durationX { get { return mDurationX; } set { mDurationX = value; } }
        public float durationY { get { return mDurationY; } set { mDurationY = value; } }
        public float durationZ { get { return mDurationZ; } set { mDurationZ = value; } }

        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        public JCS_TweenType easingX { get { return mEasingX; } set { mEasingX = value; } }
        public JCS_TweenType easingY { get { return mEasingY; } set { mEasingY = value; } }
        public JCS_TweenType easingZ { get { return mEasingZ; } set { mEasingZ = value; } }

        public void SetTarget(JCS_UnityObject trans) { mTarget = trans; }
        public JCS_UnityObject recordTarget { get { return mRecordTarget; } }
        public bool destroyWhenDoneTweening { get { return mDestroyWhenDoneTweening; } set { mDestroyWhenDoneTweening = value; } }
        public JCS_TransformType tweenType { get { return mTweenType; } set { mTweenType = value; } }

        public Vector3 valueOffset { get { return mValueOffset; } set { mValueOffset = value; } }
        public float valueOffsetX { get { return mValueOffset.x; } set { mValueOffset.x = value; } }
        public float valueOffsetY { get { return mValueOffset.y; } set { mValueOffset.y = value; } }
        public float valueOffsetZ { get { return mValueOffset.z; } set { mValueOffset.z = value; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            RandomizeDuration();

            mTweenerX.onDone = DoneTweeningX;
            mTweenerY.onDone = DoneTweeningY;
            mTweenerZ.onDone = DoneTweeningZ;
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            Test();
#endif

            // check the effect enable?
            if (!tween)
                return;

            // check if do continue tweening
            ContinueTween();

            // Updates the Tweener
            {
                bool animating = mTweenerX.animating || mTweenerY.animating || mTweenerZ.animating;

                if (mTweenerX.animating) mTweenerX.update();
                if (mTweenerY.animating) mTweenerY.update();
                if (mTweenerZ.animating) mTweenerZ.update();

                if (animating)
                {
                    Vector3 newVal = new Vector3(
                        mTweenerX.progression,
                        mTweenerY.progression,
                        mTweenerZ.progression);

                    // Set value to one of the transform properties.
                    SetSelfTransformTypeVector3(newVal);

                    mIsDoneTweening = false;
                }
                else
                {
                    mIsDoneTweening = true;
                }
            }
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (Input.GetKey(mTweenToAKey))
                DoTween(mTweenToA);
            if (Input.GetKey(mTweenToBKey))
                DoTween(mTweenToB);

            if (Input.GetKey(mContinueKeyTween))
                DoTweenContinue(mTarget);
        }
#endif

        /// <summary>
        /// Reset tweener effect setting.
        /// </summary>
        public void ResetTweener()
        {
            mDoneTweenX = false;
            mDoneTweenY = false;
            mDoneTweenZ = false;

            mTweenerX.ResetTweener();
            mTweenerY.ResetTweener();
            mTweenerZ.ResetTweener();
        }

        /// <summary>
        /// Tween to this vector either position, scale, rotation.
        /// </summary>
        /// <param name="to"> target vector 3 </param>
        /// <param name="callback"> callback function pointer. </param>
        public void DoTween(Vector3 to, Action callback = null)
        {
            DoTween(to, true, mEasingX, mEasingY, mEasingZ, callback);
        }

        /// <summary>
        /// Tween to this vector either position, scale, rotation.
        /// </summary>
        /// <param name="to"> target vector 3 </param>
        /// <param name="resetElapsedTime"> reset elapsed time? (default : true) </param>
        /// <param name="callback"> callback function pointer. </param>
        public void DoTween(Vector3 to, bool resetElapsedTime, Action callback = null)
        {
            DoTween(to, resetElapsedTime, mEasingX, mEasingY, mEasingZ, callback);
        }

        /// <summary>
        /// Tween to this vector either position, scale, rotation.
        /// </summary>
        /// <param name="to"> target vector 3 </param>
        /// <param name="resetElapsedTime"> reset elapsed time? (default : true) </param>
        /// <param name="typeX"> easing type for x axis. </param>
        /// <param name="typeY"> easing type for y axis. </param>
        /// <param name="typeZ"> easing type for z axis. </param>
        /// <param name="callback"> callback function pointer. </param>
        public void DoTween(
            Vector3 to,
            bool resetElapsedTime,
            JCS_TweenType typeX,
            JCS_TweenType typeY,
            JCS_TweenType typeZ,
            Action callback = null)
        {
            DoTween(to, resetElapsedTime, mDurationX, mDurationY, mDurationZ, typeX, typeY, typeZ, callback);
        }

        /// <summary>
        /// Tween to this vector either position, scale, rotation.
        /// </summary>
        /// <param name="to"> target vector 3 </param>
        /// <param name="resetElapsedTime"> reset elapsed time? (default : true) </param>
        /// <param name="durationX"> how fast it tween on x axis. </param>
        /// <param name="durationY"> how fast it tween on y axis. </param>
        /// <param name="durationZ"> how fast it tween on z axis. </param>
        /// <param name="typeX"> easing type for x axis. </param>
        /// <param name="typeY"> easing type for y axis. </param>
        /// <param name="typeZ"> easing type for z axis. </param>
        /// <param name="callback"> callback function pointer. </param>
        public void DoTween(
            Vector3 to,
            bool resetElapsedTime,
            float durationX,
            float durationY,
            float durationZ,
            JCS_TweenType typeX,
            JCS_TweenType typeY,
            JCS_TweenType typeZ,
            Action callback = null)
        {
            Vector3 from = GetSelfTransformTypeVector3();

            DoTween(from, to, resetElapsedTime, durationX, durationY, durationZ, typeX, typeY, typeZ, callback);
        }

        /// <summary>
        /// Tween to this vector either position, scale, rotation.
        /// </summary>
        /// <param name="from"> starting vector 3 </param>
        /// <param name="to"> target vector 3 </param>
        /// <param name="resetElapsedTime"> reset elapsed time? (default : true) </param>
        /// <param name="durationX"> how fast it tween on x axis. </param>
        /// <param name="durationY"> how fast it tween on y axis. </param>
        /// <param name="durationZ"> how fast it tween on z axis. </param>
        /// <param name="typeX"> easing type for x axis. </param>
        /// <param name="typeY"> easing type for y axis. </param>
        /// <param name="typeZ"> easing type for z axis. </param>
        /// <param name="callback"> callback function pointer. </param>
        public void DoTween(
            Vector3 from,
            Vector3 to,
            bool resetElapsedTime,
            float durationX,
            float durationY,
            float durationZ,
            JCS_TweenType typeX,
            JCS_TweenType typeY,
            JCS_TweenType typeZ,
            Action callback = null)
        {
            TweenDelegate easingX = JCS_Util.GetEasing(typeX);
            TweenDelegate easingY = JCS_Util.GetEasing(typeY);
            TweenDelegate easingZ = JCS_Util.GetEasing(typeZ);

            StartTween(from, to, resetElapsedTime, durationX, durationY, durationZ, easingX, easingY, easingZ, callback);
        }

        /// <summary>
        /// Continue Tween to this target's position.
        /// </summary>
        /// <param name="target"> target's transform </param>
        public void DoTweenContinue(JCS_UnityObject target)
        {
            SetTarget(target);

            mRecordTarget = target;

            // reset record
            mRecordTargetTransformValue = Vector3.zero;

            mContinueTween = true;
        }

        /// <summary>
        /// Get itself transform type's vector3 value.
        /// </summary>
        /// <returns> Vector3 value base on transform type selected. </returns>
        public Vector3 GetSelfTransformTypeVector3()
        {
            Vector3 val = Vector3.zero;

            switch (mTweenType)
            {
                /* Transform */
                case JCS_TransformType.POSITION:
                    {
                        if (mTrackAsLocalSelf)
                            val = localPosition;
                        else
                            val = position;
                    }
                    break;
                case JCS_TransformType.ROTATION:
                    {
                        if (mTrackAsLocalSelf)
                            val = localEulerAngles;
                        else
                            val = eulerAngles;
                    }
                    break;
                case JCS_TransformType.SCALE:
                    val = localScale;
                    break;
                /* RectTransform */
                case JCS_TransformType.ANCHOR_MIN:
                    val = mRectTransform.anchorMin;
                    break;
                case JCS_TransformType.ANCHOR_MAX:
                    val = mRectTransform.anchorMax;
                    break;
                case JCS_TransformType.SIZE_DELTA:
                    val = mRectTransform.sizeDelta;
                    break;
                case JCS_TransformType.PIVOT:
                    val = mRectTransform.pivot;
                    break;
                case JCS_TransformType.ANCHOR_POSITION:
                    val = mRectTransform.anchoredPosition;
                    break;
                case JCS_TransformType.ANCHOR_POSITION_3D:
                    val = mRectTransform.anchoredPosition3D;
                    break;
                case JCS_TransformType.OFFSET_MIN:
                    val = mRectTransform.offsetMin;
                    break;
                case JCS_TransformType.OFFSET_MAX:
                    val = mRectTransform.offsetMax;
                    break;
            }
            return val;
        }

        /// <summary>
        /// Set self transform value.
        /// </summary>
        /// <param name="newVal"></param>
        public void SetSelfTransformTypeVector3(Vector3 newVal)
        {
            switch (mTweenType)
            {
                /* Transform */
                case JCS_TransformType.POSITION:
                    {
                        if (mTrackAsLocalSelf)
                            localPosition = newVal;
                        else
                            position = newVal;
                    }
                    break;
                case JCS_TransformType.ROTATION:
                    {
                        if (mTrackAsLocalSelf)
                            localEulerAngles = newVal;
                        else
                            eulerAngles = newVal;
                    }
                    break;
                case JCS_TransformType.SCALE:
                    localScale = newVal;
                    break;
                /* RectTransform */
                case JCS_TransformType.ANCHOR_MIN:
                    mRectTransform.anchorMin = newVal;
                    break;
                case JCS_TransformType.ANCHOR_MAX:
                    mRectTransform.anchorMax = newVal;
                    break;
                case JCS_TransformType.SIZE_DELTA:
                    mRectTransform.sizeDelta = newVal;
                    break;
                case JCS_TransformType.PIVOT:
                    mRectTransform.pivot = newVal;
                    break;
                case JCS_TransformType.ANCHOR_POSITION:
                    mRectTransform.anchoredPosition = newVal;
                    break;
                case JCS_TransformType.ANCHOR_POSITION_3D:
                    mRectTransform.anchoredPosition3D = newVal;
                    break;
                case JCS_TransformType.OFFSET_MIN:
                    mRectTransform.offsetMin = newVal;
                    break;
                case JCS_TransformType.OFFSET_MAX:
                    mRectTransform.offsetMax = newVal;
                    break;
            }
        }

        /// <summary>
        /// Get target transform type's vector3 value.
        /// </summary>
        /// <returns> Target transform's vector3 value base on transform
        /// type selected. </returns>
        public Vector3 GetTargetTransformTypeVector3()
        {
            Vector3 val = Vector3.zero;

            RectTransform rt = mTarget.GetRectTransform();

            switch (mTweenType)
            {
                /* Transform */
                case JCS_TransformType.POSITION:
                    {
                        if (mTrackAsLocalTarget)
                            val = mTarget.transform.localPosition;
                        else
                            val = mTarget.transform.position;
                    }
                    break;
                case JCS_TransformType.ROTATION:
                    {
                        if (mTrackAsLocalTarget)
                            val = mTarget.transform.localEulerAngles;
                        else
                            val = mTarget.transform.eulerAngles;
                    }
                    break;
                case JCS_TransformType.SCALE:
                    val = mTarget.transform.localScale;
                    break;
                /* RectTransform */
                case JCS_TransformType.ANCHOR_MIN:
                    val = rt.anchorMin;
                    break;
                case JCS_TransformType.ANCHOR_MAX:
                    val = rt.anchorMax;
                    break;
                case JCS_TransformType.SIZE_DELTA:
                    val = rt.sizeDelta;
                    break;
                case JCS_TransformType.PIVOT:
                    val = rt.pivot;
                    break;
                case JCS_TransformType.ANCHOR_POSITION:
                    val = rt.anchoredPosition;
                    break;
                case JCS_TransformType.ANCHOR_POSITION_3D:
                    val = rt.anchoredPosition3D;
                    break;
                case JCS_TransformType.OFFSET_MIN:
                    val = rt.offsetMin;
                    break;
                case JCS_TransformType.OFFSET_MAX:
                    val = rt.offsetMax;
                    break;
            }
            return val;
        }

        /// <summary>
        /// Default callback when done tweening for destroying.
        /// </summary>
        protected void DoneTweeningDestroy()
        {
            if (!mDestroyWhenDoneTweening)
                return;

            --mDestroyDoneTweeningCount;

            if (mDestroyDoneTweeningCount <= 0)
                Destroy(gameObject);
        }

        /// <summary>
        /// Do the callback.
        /// </summary>
        private void DoCallback()
        {
            mTweenerX.DoCallback();
            mTweenerY.DoCallback();
            mTweenerZ.DoCallback();

            SafeDoCallback();
        }

        private void SafeDoCallback()
        {
            if (!mDoneTweenX || !mDoneTweenY || !mDoneTweenZ)
                return;

            mOnDone?.Invoke();
            onDone?.Invoke();
        }

        /// <summary>
        /// Callback for each tweener.
        /// </summary>
        private void DoneTweeningX()
        {
            mDoneTweenX = true;
            SafeDoCallback();
        }
        private void DoneTweeningY()
        {
            mDoneTweenY = true;
            SafeDoCallback();
        }
        private void DoneTweeningZ()
        {
            mDoneTweenZ = true;
            SafeDoCallback();
        }

        /// <summary>
        /// Prepare for tweening.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="durationX"></param>
        /// <param name="durationY"></param>
        /// <param name="durationZ"></param>
        /// <param name="easingX"></param>
        /// <param name="easingY"></param>
        /// <param name="easingZ"></param>
        /// <param name="callback"></param>
        private void StartTween(
            Vector3 from,
            Vector3 to,
            bool resetElapsedTime = true,
            float durationX = 1f,
            float durationY = 1f,
            float durationZ = 1f,
            TweenDelegate easingX = null,
            TweenDelegate easingY = null,
            TweenDelegate easingZ = null,
            Action callback = null)
        {
            onStart?.Invoke();
            mOnDone = callback;

            mIsDoneTweening = false;
            mDoneTweenX = false;
            mDoneTweenY = false;
            mDoneTweenZ = false;

            mContinueTween = false;

            // Sets The Position From -> To
            mTweenerX.easeFromTo(
                from.x,
                to.x + mValueOffset.x,  // add offset to final value.
                resetElapsedTime,
                durationX,
                easingX,
                DoneTweeningX,
                mTimeType);

            // Sets The Position From -> To
            mTweenerY.easeFromTo(
                from.y,
                to.y + mValueOffset.y,  // add offset to final value.
                resetElapsedTime,
                durationY,
                easingY,
                DoneTweeningY,
                mTimeType);

            // Sets The Position From -> To
            mTweenerZ.easeFromTo(
                from.z,
                to.z + mValueOffset.z,  // add offset to final value.
                resetElapsedTime,
                durationZ,
                easingZ,
                DoneTweeningZ,
                mTimeType);
        }

        /// <summary>
        /// Continue tween to the target's position in update.
        /// </summary>
        private void ContinueTween()
        {
            if (!mContinueTween)
                return;

            if (mTarget == null)
            {
#if UNITY_EDITOR
                // log string to console cost alost of performance.
                // so do it only when is debug mode.
                if (JCS_GameSettings.FirstInstance().debugMode)
                {
                    Debug.LogError("Start the tween but the target is null");
                }
#endif

                mContinueTween = false;
                return;
            }

            float distance = 0;

            Vector3 selfVal = GetSelfTransformTypeVector3();
            Vector3 targetVal = GetTargetTransformTypeVector3();

            // no need to tween again if the position has not change.
            if (mRecordTargetTransformValue == targetVal)
                return;

            DoTween(targetVal, false);

            // Everytime we do tween will make 'mContinueTween' to false,
            // so make sure we enable this back here after do tween.
            mContinueTween = true;

            // record down the target position
            mRecordTargetTransformValue = targetVal;

            // Check if close enough to the distance we target.
            distance = Vector3.Distance(selfVal, targetVal);

            if (distance <= mStopTweenDistance)
            {
                // call the call back.
                ResetTweener();
                DoCallback();

                mContinueTween = false;
            }
        }

        /// <summary>
        /// Randomize the duration with all axis. (x, y, z)
        /// </summary>
        private void RandomizeDuration()
        {
            if (mRandomizeDuration != 0)
            {
                float randomizeDuration = JCS_Random.Range(-mRandomizeDuration, mRandomizeDuration);

                mDurationX += randomizeDuration;
                mDurationY += randomizeDuration;
                mDurationZ += randomizeDuration;
            }
        }
    }
}
