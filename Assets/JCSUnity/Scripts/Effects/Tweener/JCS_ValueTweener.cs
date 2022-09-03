/**
 * $File: JCS_ValueTweener.cs $
 * $Date: 2020-04-18 20:45:08 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.Events;

namespace JCSUnity
{
    /// <summary>
    /// Tweener thats tweens one specific variable value.
    /// 
    /// Can only use for `float`.
    /// </summary>
    public class JCS_ValueTweener : MonoBehaviour
    {
        public delegate void SetValFloat(float newVal);
        public delegate float GetValFloat();

        /* Variables */

        public SetValFloat set_float = null;
        public GetValFloat get_float = null;

        private EmptyFunction mValueCallback = null;

        private TweenDelegate mEasingFunc = null;

        private float mTimeElapsed = 0.0f;

        private float mProgression = 0.0f;

        private float mProgressPct = 0.0f;

        private float mRealDuration = 1.0f;

#if UNITY_EDITOR
        [Header("** Helper Variables (JCS_ValueTweener) **")]

        [Tooltip("Test component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;

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

        [Header("** Check Variables (JCS_ValueTweener) **")]

        [SerializeField]
        private float mStartingValue = -0.0f;

        [SerializeField]
        private float mTargetValue = -0.0f;

        [Tooltip("Check if is done animation.")]
        [SerializeField]
        private bool mAnimating = false;

        [Header("** Runtime Variables (JCS_ValueTweener) **")]

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

        [Header("- Tweener Formula Type")]

        [Tooltip("Tweener formula on value.")]
        [SerializeField]
        private JCS_TweenType mEasing = JCS_TweenType.LINEAR;

        [Header("- Callback")]

        [Tooltip("Callback after easing.")]
        [SerializeField]
        private UnityEvent mUnityCallback = null;

        /* Setter & Getter */

        public bool Animating { get { return this.mAnimating; } }
        public bool Tween { get { return this.mTween; } set { this.mTween = value; } }
        public float ValueOffset { get { return this.mValueOffset; } set { this.mValueOffset = value; } }
        public float Duration { get { return this.mDuration; } set { this.mDuration = value; } }
        public JCS_TweenType Easing
        {
            get { return this.mEasing; }
            set
            {
                this.mEasing = value;
                this.mEasingFunc = JCS_Util.GetEasing(this.mEasing);
            }
        }
        public UnityEvent UnityCallback { get { return this.mUnityCallback; } set { this.mUnityCallback = value; } }

        /* Functions */

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
            DoTween(this.get_float.Invoke(), to, this.mDuration, this.mEasing);
        }
        public void DoTwenn(float from, float to)
        {
            DoTween(from, to, this.mDuration, this.mEasing);
        }
        public void DoTwenn(float from, float to, float duration)
        {
            DoTween(from, to, duration, this.mEasing);
        }
        public void DoTween(float from, float to, float duration, JCS_TweenType type)
        {
            this.mStartingValue = from;
            this.mTargetValue = to + mValueOffset;

            this.Easing = type;

            this.mRealDuration = this.mDuration;

            this.mAnimating = true;
        }

        /// <summary>
        /// Check if all variables are available.
        /// </summary>
        /// <returns></returns>
        private bool CheckValid()
        {
            return (set_float != null && get_float != null);
        }

        /// <summary>
        /// Check weather if the easing are done.
        /// </summary>
        private void DoDoneEasing()
        {
            // trigger callback.
            if (mValueCallback != null)
                mValueCallback.Invoke();

            // trigger unity callback.
            if (mUnityCallback != null)
                mUnityCallback.Invoke();
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
                this.mProgression = mEasingFunc.Invoke(
                    mTimeElapsed,
                    mStartingValue,
                    (mTargetValue - mStartingValue),
                    mRealDuration);

                this.mProgressPct = mTimeElapsed / mRealDuration;

                this.set_float.Invoke(this.mProgression);

                this.mTimeElapsed += Time.deltaTime;
            }
            else
            {
                this.mProgression = this.mTargetValue;
                this.set_float.Invoke(this.mProgression);

                this.mAnimating = false;
                this.mTimeElapsed = 0.0f;
                this.mProgressPct = 1.0f;

                DoDoneEasing();
            }
        }
    }
}
