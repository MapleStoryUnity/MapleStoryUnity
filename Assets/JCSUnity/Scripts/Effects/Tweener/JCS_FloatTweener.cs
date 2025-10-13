/**
 * $File: JCS_FloatTweener.cs $
 * $Date: 2020-04-18 20:45:08 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Tweener thats tweens one specific variable value.
    /// 
    /// Can only use for `float`.
    /// </summary>
    public class JCS_FloatTweener : MonoBehaviour
    {
        public delegate void SetFloat(float newVal);
        public delegate float GetFloat();

        /* Variables */

        public SetFloat onValueChange = null;
        public GetFloat onValueReturn = null;

        // Callback to execute when start tweening.
        public Action onStart = null;

        // Callback to execute when done tweening.
        public Action onDone = null;

        // Callback to execute when done tweening but only with that
        // specific function call.
        private Action mOnDone = null;

        private TweenDelegate mEasingFunc = null;

        private float mTimeElapsed = 0.0f;

        private float mProgression = 0.0f;

        private float mProgressPct = 0.0f;

        private float mRealDuration = 1.0f;

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_FloatTweener)")]

        [Tooltip("Test component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("The current value used for the test.")]
        [SerializeField]
        private float mTestCurrentValue = 0.0f;

        [Tooltip("Key to active tween to point A.")]
        [SerializeField]
        private KeyCode mTweenToAKey = KeyCode.A;

        [Tooltip("Value A to tween to.")]
        [SerializeField]
        private float mTweenToA = 0.0f;

        [Tooltip("Key to active tween to point B.")]
        [SerializeField]
        private KeyCode mTweenToBKey = KeyCode.B;

        [Tooltip("Value B to tween to.")]
        [SerializeField]
        private float mTweenToB = 5.0f;
#endif

        [Separator("Check Variables (JCS_FloatTweener)")]

        [SerializeField]
        [ReadOnly]
        private float mStartingValue = 0.0f;

        [SerializeField]
        [ReadOnly]
        private float mTargetValue = 0.0f;

        [Tooltip("Check if is done animation.")]
        [SerializeField]
        [ReadOnly]
        private bool mAnimating = false;

        [Separator("Runtime Variables (JCS_FloatTweener)")]

        [Tooltip("Do the tween effect?")]
        [SerializeField]
        private bool mTween = true;

        [Tooltip("Value offset.")]
        [SerializeField]
        private float mValueOffset = 0.0f;

        [Tooltip("How fast it moves on value.")]
        [SerializeField]
        [Range(0.01f, 1000.0f)]
        private float mDuration = 1.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("Tweener Formula Type")]

        [Tooltip("Tweener formula on value.")]
        [SerializeField]
        private JCS_TweenType mEasing = JCS_TweenType.LINEAR;

        [Header("Callback")]

        [Tooltip("Callback after easing.")]
        [SerializeField]
        private UnityEvent mUnityCallback = null;

        /* Setter & Getter */

        public bool animating { get { return mAnimating; } }
        public bool tween { get { return mTween; } set { mTween = value; } }
        public float valueOffset { get { return mValueOffset; } set { mValueOffset = value; } }
        public float duration { get { return mDuration; } set { mDuration = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }
        public JCS_TweenType easing
        {
            get { return mEasing; }
            set
            {
                mEasing = value;
                mEasingFunc = JCS_Util.GetEasing(mEasing);
            }
        }
        public UnityEvent unityCallback { get { return mUnityCallback; } set { mUnityCallback = value; } }

        /* Functions */

#if UNITY_EDITOR
        private void Awake()
        {
            if (!mTestWithKey)
                return;

            onValueChange += (val) =>
            {
                mTestCurrentValue = val;
            };

            onValueReturn += () =>
            {
                return mTestCurrentValue;
            };
        }
#endif

        private void LateUpdate()
        {
#if UNITY_EDITOR
            Test();
#endif

            UpdateValue();
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
        }
#endif

        /// <summary>
        /// Tween the value.
        /// </summary>
        public void DoTween(float to)
        {
            DoTween(onValueReturn.Invoke(), to, mDuration, mEasing);
        }
        public void DoTween(float from, float to)
        {
            DoTween(from, to, mDuration, mEasing);
        }
        public void DoTween(float from, float to, float duration)
        {
            DoTween(from, to, duration, mEasing);
        }
        public void DoTween(float from, float to, float duration, JCS_TweenType type)
        {
            mStartingValue = from;
            mTargetValue = to + mValueOffset;

            mEasing = type;

            mRealDuration = mDuration;

            mAnimating = true;
        }

        /// <summary>
        /// Check if all variables are available.
        /// </summary>
        /// <returns></returns>
        private bool CheckValid()
        {
            return (onValueChange != null && onValueReturn != null);
        }

        /// <summary>
        /// Check weather if the easing are done.
        /// </summary>
        private void DoDoneEasing()
        {
            mOnDone?.Invoke();

            mUnityCallback?.Invoke();

            onDone?.Invoke();
        }

        private void UpdateValue()
        {
            if (!mAnimating)
                return;

            if (!CheckValid())
            {
                mAnimating = false;
                return;
            }

            if (mTimeElapsed < mRealDuration)
            {
                mProgression = mEasingFunc.Invoke(
                    mTimeElapsed,
                    mStartingValue,
                    (mTargetValue - mStartingValue),
                    mRealDuration);

                mProgressPct = mTimeElapsed / mRealDuration;

                onValueChange.Invoke(mProgression);

                mTimeElapsed += JCS_Time.ItTime(mTimeType);
            }
            else
            {
                mProgression = mTargetValue;
                onValueChange.Invoke(mProgression);

                mAnimating = false;
                mTimeElapsed = 0.0f;
                mProgressPct = 1.0f;

                DoDoneEasing();
            }
        }
    }
}
