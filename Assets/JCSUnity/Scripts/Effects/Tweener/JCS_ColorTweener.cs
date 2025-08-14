/**
 * $File: JCS_ColorTweener.cs $
 * $Date: 2017-04-10 21:32:03 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Color tweener.
    /// </summary>
    public class JCS_ColorTweener : JCS_UnityObject
    {
        /* Variables */

        private TweenDelegate mEasingRed = null;
        private TweenDelegate mEasingGreen = null;
        private TweenDelegate mEasingBlue = null;
        private TweenDelegate mEasingAlpha = null;


        // Callback to execute when start tweening.
        public Action onStart = null;

        // Callback to execute when done tweening.
        public Action onDone = null;

        // Callback to execute when done tweening but only with that
        // specific function call.
        private Action mOnDone = null;

        /**
         * time to calculate the progress.
         * 
         * Compare List:
         *      x = r
         *      y = g
         *      z = b
         *      w = a
         */
        private Vector4 mTimeElapsed = Vector4.zero;

        // starting color
        private Color mFromColor = Color.white;

        // these value are actually going to use.
        private float mRealDurationRed = 0.0f;
        private float mRealDurationGreen = 0.0f;
        private float mRealDurationBlue = 0.0f;
        private float mRealDurationAlpha = 0.0f;

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_ColorTweener)")]

        [Tooltip("Test component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Tween key to color A.")]
        [SerializeField]
        private KeyCode mTweenKeyA = KeyCode.Q;

        [Tooltip("Color to tween to.")]
        [SerializeField]
        private Color mTweenColorA = new Color(0.5f, 0, 0.5f, 0.5f);

        [Tooltip("Tween key to color B.")]
        [SerializeField]
        private KeyCode mTweenKeyB = KeyCode.W;

        [Tooltip("Color to tween to.")]
        [SerializeField]
        private Color mTweenColorB = new Color(1.0f, 1.0f, 1.0f, 1.0f);
#endif

        [Separator("Check Variables (JCS_ColorTweener)")]

        // progress color
        [SerializeField]
        [ReadOnly]
        private Color mProgressionColor = Color.white;

        [SerializeField]
        [ReadOnly]
        private Color mProgressPctColor = Color.white;

        [SerializeField]
        [ReadOnly]
        private Color mTargetColor = Color.white;

        [SerializeField]
        [ReadOnly]
        private bool mEasingR = false;
        [SerializeField]
        [ReadOnly]
        private bool mEasingG = false;
        [SerializeField]
        [ReadOnly]
        private bool mEasingB = false;
        [SerializeField]
        [ReadOnly]
        private bool mEasingA = false;

        [Separator("Runtime Variables (JCS_ColorTweener)")]

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Tooltip("Tween type for red channel.")]
        [SerializeField]
        private JCS_TweenType mEaseTypeR = JCS_TweenType.LINEAR;

        [Tooltip("Tween type for green channel.")]
        [SerializeField]
        private JCS_TweenType mEaseTypeG = JCS_TweenType.LINEAR;

        [Tooltip("Tween type for blue channel.")]
        [SerializeField]
        private JCS_TweenType mEaseTypeB = JCS_TweenType.LINEAR;

        [Tooltip("Tween type for alpha channel.")]
        [SerializeField]
        private JCS_TweenType mEaseTypeA = JCS_TweenType.LINEAR;

        [Tooltip("How fast it changes the red channel.")]
        [SerializeField]
        [Range(0.01f, 1000.0f)]
        private float mDurationRed = 0.2f;

        [Tooltip("How fast it changes the green channel.")]
        [SerializeField]
        [Range(0.01f, 1000.0f)]
        private float mDurationGreen = 0.2f;

        [Tooltip("How fast it changes the blue channel.")]
        [SerializeField]
        [Range(0.01f, 1000.0f)]
        private float mDurationBlue = 0.2f;

        [Tooltip("How fast it changes the alpha channel.")]
        [SerializeField]
        [Range(0.01f, 1000.0f)]
        private float mDurationAlpha = 0.2f;

        [Tooltip("Do not do anything with red channel.")]
        [SerializeField]
        private bool mIgnoreR = false;

        [Tooltip("Do not do anything with green channel.")]
        [SerializeField]
        private bool mIgnoreG = false;

        [Tooltip("Do not do anything with blue channel.")]
        [SerializeField]
        private bool mIgnoreB = false;

        [Tooltip("Do not do anything with alpha channel.")]
        [SerializeField]
        private bool mIgnoreA = false;

        [Header("- Callback")]

        [Tooltip("Callback after easing.")]
        [SerializeField]
        private UnityEvent mUnityCallback = null;

        /* Setter & Getter */

        public bool animating { get { return (mEasingR || mEasingG || mEasingB || mEasingA); } }
        public Color progressionColor { get { return mProgressionColor; } }
        public Color progressPctColor { get { return mProgressPctColor; } }
        public Color targetColor { get { return mTargetColor; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }
        public JCS_TweenType easeTypeR
        {
            get { return mEaseTypeR; }
            set
            {
                mEaseTypeR = value;

                // update easing function pointer / function formula.
                mEasingRed = JCS_Util.GetEasing(mEaseTypeR);
            }
        }
        public JCS_TweenType easeTypeG
        {
            get { return mEaseTypeG; }
            set
            {
                mEaseTypeG = value;

                // update easing function pointer / function formula.
                mEasingGreen = JCS_Util.GetEasing(mEaseTypeG);
            }
        }
        public JCS_TweenType easeTypeB
        {
            get { return mEaseTypeB; }
            set
            {
                mEaseTypeB = value;

                // update easing function pointer / function formula.
                mEasingBlue = JCS_Util.GetEasing(mEaseTypeB);
            }
        }
        public JCS_TweenType easeTypeA
        {
            get { return mEaseTypeA; }
            set
            {
                mEaseTypeA = value;

                // update easing function pointer / function formula.
                mEasingAlpha = JCS_Util.GetEasing(mEaseTypeA);
            }
        }
        public float durationRed { get { return mDurationRed; } set { mDurationRed = value; } }
        public float durationGreen { get { return mDurationGreen; } set { mDurationGreen = value; } }
        public float durationBlue { get { return mDurationBlue; } set { mDurationBlue = value; } }
        public float durationAlpha { get { return mDurationAlpha; } set { mDurationAlpha = value; } }
        public bool ignoreR { get { return mIgnoreR; } set { mIgnoreR = value; } }
        public bool ignoreG { get { return mIgnoreG; } set { mIgnoreG = value; } }
        public bool ignoreB { get { return mIgnoreB; } set { mIgnoreB = value; } }
        public bool ignoreA { get { return mIgnoreA; } set { mIgnoreA = value; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            // get all function pointer/formula.
            mEasingRed = JCS_Util.GetEasing(mEaseTypeR);
            mEasingGreen = JCS_Util.GetEasing(mEaseTypeG);
            mEasingBlue = JCS_Util.GetEasing(mEaseTypeB);
            mEasingAlpha = JCS_Util.GetEasing(mEaseTypeA);
        }

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif

            DoTweening();
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (Input.GetKey(mTweenKeyA))
                DoTween(mTweenColorA);
            if (Input.GetKey(mTweenKeyB))
                DoTween(mTweenColorB);
        }
#endif

        /// <summary>
        /// Reset the tweener progress.
        /// </summary>
        public void ResetTweener()
        {
            this.mProgressionColor = this.mTargetColor;

            this.mProgressPctColor.r = 1.0f;
            this.mProgressPctColor.g = 1.0f;
            this.mProgressPctColor.b = 1.0f;
            this.mProgressPctColor.a = 1.0f;

            this.mTimeElapsed = Vector4.zero;

            // reset trigger
            this.mEasingR = false;
            this.mEasingG = false;
            this.mEasingB = false;
            this.mEasingA = false;
        }

        /// <summary>
        /// Tween to the color.
        /// </summary>
        /// <param name="inToColor"> target color </param>
        public void DoTween(Color inToColor, Action func = null)
        {
            DoTween(
                inToColor,
                this.mEaseTypeR,
                this.mEaseTypeG,
                this.mEaseTypeB,
                this.mEaseTypeR,
                func);
        }

        /// <summary>
        /// Tween to the color.
        /// </summary>
        /// <param name="inToColor"> target color </param>
        /// <param name="inTweenTypeR"> tween type for red channel </param>
        /// <param name="inTweenTypeG"> tween type for green channel </param>
        /// <param name="inTweenTypeB"> tween type for blue channel </param>
        /// <param name="inTweenTypeA"> tween type for alpha channel </param>
        public void DoTween(
            Color inToColor,
            JCS_TweenType inTweenTypeR,
            JCS_TweenType inTweenTypeG,
            JCS_TweenType inTweenTypeB,
            JCS_TweenType inTweenTypeA,
            Action func = null)
        {
            DoTween(
                this.localColor,
                inToColor,
                inTweenTypeR,
                inTweenTypeG,
                inTweenTypeB,
                inTweenTypeA,
                this.mDurationRed,
                this.mDurationGreen,
                this.mDurationBlue,
                this.mDurationAlpha,
                func);
        }

        /// <summary>
        /// Tween to the color.
        /// </summary>
        /// <param name="inFromColor"> starting color </param>
        /// <param name="inToColor"> target color </param>
        /// <param name="inTweenTypeR"> tween type for red channel </param>
        /// <param name="inTweenTypeG"> tween type for green channel </param>
        /// <param name="inTweenTypeB"> tween type for blue channel </param>
        /// <param name="inTweenTypeA"> tween type for alpha channel </param>
        /// <param name="inDurationR"> how fast it tween in red channel </param>
        /// <param name="inDurationG"> how fast it tween in green channel </param>
        /// <param name="inDurationB"> how fast it tween in blue channel </param>
        /// <param name="inDurationA"> how fast it tween in alpha channel </param>
        public void DoTween(
            Color inFromColor,
            Color inToColor,
            JCS_TweenType inTweenTypeR,
            JCS_TweenType inTweenTypeG,
            JCS_TweenType inTweenTypeB,
            JCS_TweenType inTweenTypeA,
            float inDurationR,
            float inDurationG,
            float inDurationB,
            float inDurationA,
            Action func = null)
        {
            onStart?.Invoke();

            mFromColor = inFromColor;
            mTargetColor = inToColor;

            mEaseTypeR = inTweenTypeR;
            mEaseTypeG = inTweenTypeG;
            mEaseTypeB = inTweenTypeB;
            mEaseTypeA = inTweenTypeA;

            mRealDurationRed = inDurationR;
            mRealDurationGreen = inDurationG;
            mRealDurationBlue = inDurationB;
            mRealDurationAlpha = inDurationA;

            mEasingR = true;
            mEasingG = true;
            mEasingB = true;
            mEasingA = true;

            mOnDone = func;
        }

        /// <summary>
        /// Update red channel.
        /// </summary>
        private void UpdateRed()
        {
            if (mIgnoreR)
                mEasingR = false;

            if (!mEasingR)
                return;

            if (mTimeElapsed.x < mRealDurationRed)
            {
                this.mProgressionColor.r = mEasingRed.Invoke(
                    mTimeElapsed.x,
                    mFromColor.r,
                    (mTargetColor.r - mFromColor.r),
                    mRealDurationRed);

                this.mProgressPctColor.r = mTimeElapsed.x / mRealDurationRed;

                this.mTimeElapsed.x += JCS_Time.ItTime(mTimeType);
            }
            else
            {
                this.mProgressionColor.r = this.mTargetColor.r;

                this.mEasingR = false;
                this.mTimeElapsed.x = 0.0f;
                this.mProgressPctColor.r = 1.0f;

                CheckDoneEasing();
            }
        }

        /// <summary>
        /// Update green channel.
        /// </summary>
        private void UpdateGreen()
        {
            if (mIgnoreG)
                mEasingG = false;

            if (!mEasingG)
                return;

            if (mTimeElapsed.y < mRealDurationGreen)
            {
                this.mProgressionColor.g = mEasingGreen.Invoke(
                    mTimeElapsed.y,
                    mFromColor.g,
                    (mTargetColor.g - mFromColor.g),
                    mRealDurationGreen);

                this.mProgressPctColor.g = mTimeElapsed.y / mRealDurationGreen;

                this.mTimeElapsed.y += JCS_Time.ItTime(mTimeType);
            }
            else
            {
                this.mProgressionColor.g = this.mTargetColor.g;

                this.mEasingG = false;
                this.mTimeElapsed.y = 0.0f;
                this.mProgressPctColor.g = 1.0f;

                CheckDoneEasing();
            }
        }

        /// <summary>
        /// Update blue channel.
        /// </summary>
        private void UpdateBlue()
        {
            if (mIgnoreB)
                mEasingB = false;

            if (!mEasingB)
                return;

            if (mTimeElapsed.z < mRealDurationBlue)
            {
                this.mProgressionColor.b = mEasingBlue.Invoke(
                    mTimeElapsed.z,
                    mFromColor.b,
                    (mTargetColor.b - mFromColor.b),
                    mRealDurationBlue);

                this.mProgressPctColor.b = mTimeElapsed.z / mRealDurationBlue;

                this.mTimeElapsed.z += JCS_Time.ItTime(mTimeType);
            }
            else
            {
                this.mProgressionColor.b = this.mTargetColor.b;

                this.mEasingB = false;
                this.mTimeElapsed.z = 0.0f;
                this.mProgressPctColor.b = 1.0f;

                CheckDoneEasing();
            }
        }

        /// <summary>
        /// Update alpha channel.
        /// </summary>
        private void UpdateAlpha()
        {
            if (mIgnoreA)
                mEasingA = false;

            if (!mEasingA)
                return;

            if (mTimeElapsed.w < mRealDurationAlpha)
            {
                this.mProgressionColor.a = mEasingAlpha.Invoke(
                    mTimeElapsed.w,
                    mFromColor.a,
                    (mTargetColor.a - mFromColor.a),
                    mRealDurationAlpha);

                this.mProgressPctColor.a = mTimeElapsed.w / mRealDurationAlpha;

                this.mTimeElapsed.w += JCS_Time.ItTime(mTimeType);
            }
            else
            {
                this.mProgressionColor.a = this.mTargetColor.a;

                this.mEasingA = false;
                this.mTimeElapsed.w = 0.0f;
                this.mProgressPctColor.a = 1.0f;

                CheckDoneEasing();
            }
        }

        /// <summary>
        /// Check weather if the easing are done.
        /// </summary>
        private void CheckDoneEasing()
        {
            // check if any still easing.
            if (mEasingA || mEasingR || mEasingG || mEasingB)
                return;

            mOnDone?.Invoke();

            mUnityCallback?.Invoke();

            onDone?.Invoke();
        }

        /// <summary>
        /// Do the tweening.
        /// </summary>
        private void DoTweening()
        {
            if (!animating)
                return;

            UpdateRed();
            UpdateGreen();
            UpdateBlue();
            UpdateAlpha();

            // update color.
            localColor = mProgressionColor;
        }
    }
}
