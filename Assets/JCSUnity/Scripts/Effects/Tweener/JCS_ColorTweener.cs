/**
 * $File: JCS_ColorTweener.cs $
 * $Date: 2017-04-10 21:32:03 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JCSUnity
{
    public delegate float TweenDelegate(float t, float b, float c, float d);
    public delegate void CallBackDelegate();

    /// <summary>
    /// Color tweener.
    /// </summary>
    public class JCS_ColorTweener
        : JCS_UnityObject
    {
        /* Variables */

        private TweenDelegate mEasingRed = null;
        private TweenDelegate mEasingGreen = null;
        private TweenDelegate mEasingBlue = null;
        private TweenDelegate mEasingAlpha = null;

        private CallBackDelegate mColorCallback = null;

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

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_ColorTweener) **")]

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

        [Header("** Check Variables (JCS_ColorTweener) **")]

        // progress color
        [SerializeField]
        private Color mProgressionColor = Color.white;

        [SerializeField]
        private Color mProgressPctColor = Color.white;

        [SerializeField]
        private Color mTargetColor = Color.white;

        [SerializeField]
        private bool mEasingR = false;
        [SerializeField]
        private bool mEasingG = false;
        [SerializeField]
        private bool mEasingB = false;
        [SerializeField]
        private bool mEasingA = false;

        [Header("** Runtime Variables (JCS_ColorTweener) **")]

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

        public bool Animating { get { return (mEasingR || mEasingG || mEasingB || mEasingA); } }
        public JCS_TweenType EaseTypeR
        {
            get { return this.mEaseTypeR; }
            set
            {
                this.mEaseTypeR = value;

                // update easing function pointer / function formula.
                this.mEasingRed = JCS_Utility.GetEasing(mEaseTypeR);
            }
        }
        public JCS_TweenType EaseTypeG
        {
            get { return this.mEaseTypeG; }
            set
            {
                this.mEaseTypeG = value;

                // update easing function pointer / function formula.
                this.mEasingGreen = JCS_Utility.GetEasing(mEaseTypeG);
            }
        }
        public JCS_TweenType EaseTypeB
        {
            get { return this.mEaseTypeB; }
            set
            {
                this.mEaseTypeB = value;

                // update easing function pointer / function formula.
                this.mEasingBlue = JCS_Utility.GetEasing(mEaseTypeB);
            }
        }
        public JCS_TweenType EaseTypeA
        {
            get { return this.mEaseTypeA; }
            set
            {
                this.mEaseTypeA = value;

                // update easing function pointer / function formula.
                this.mEasingAlpha = JCS_Utility.GetEasing(mEaseTypeA);
            }
        }
        public float DurationRed { get { return this.mDurationRed; } set { this.mDurationRed = value; } }
        public float DurationGreen { get { return this.mDurationGreen; } set { this.mDurationGreen = value; } }
        public float DurationBlue { get { return this.mDurationBlue; } set { this.mDurationBlue = value; } }
        public float DurationAlpha { get { return this.mDurationAlpha; } set { this.mDurationAlpha = value; } }
        public bool IgnoreR { get { return this.mIgnoreR; } set { this.mIgnoreR = value; } }
        public bool IgnoreG { get { return this.mIgnoreG; } set { this.mIgnoreG = value; } }
        public bool IgnoreB { get { return this.mIgnoreB; } set { this.mIgnoreB = value; } }
        public bool IgnoreA { get { return this.mIgnoreA; } set { this.mIgnoreA = value; } }
        public void SetCallback(CallBackDelegate func)
        {
            this.mColorCallback = func;
        }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            // get all function pointer/formula.
            this.mEasingRed = JCS_Utility.GetEasing(mEaseTypeR);
            this.mEasingGreen = JCS_Utility.GetEasing(mEaseTypeG);
            this.mEasingBlue = JCS_Utility.GetEasing(mEaseTypeB);
            this.mEasingAlpha = JCS_Utility.GetEasing(mEaseTypeA);
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            DoTweening();
        }

#if (UNITY_EDITOR)
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
        public void DoTween(Color inToColor)
        {
            DoTween(
                inToColor,
                this.mEaseTypeR,
                this.mEaseTypeG,
                this.mEaseTypeB,
                this.mEaseTypeR);
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
            JCS_TweenType inTweenTypeA)
        {
            DoTween(
                this.LocalColor,
                inToColor,
                inTweenTypeR,
                inTweenTypeG,
                inTweenTypeB,
                inTweenTypeA,
                this.mDurationRed,
                this.mDurationGreen,
                this.mDurationBlue,
                this.mDurationAlpha);
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
            float inDurationA)
        {
            this.mFromColor = inFromColor;
            this.mTargetColor = inToColor;

            EaseTypeR = inTweenTypeR;
            EaseTypeG = inTweenTypeG;
            EaseTypeB = inTweenTypeB;
            EaseTypeA = inTweenTypeA;

            this.mRealDurationRed = inDurationR;
            this.mRealDurationGreen = inDurationG;
            this.mRealDurationBlue = inDurationB;
            this.mRealDurationAlpha = inDurationA;

            this.mEasingR = true;
            this.mEasingG = true;
            this.mEasingB = true;
            this.mEasingA = true;
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

                this.mTimeElapsed.x += Time.deltaTime;
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

                this.mTimeElapsed.y += Time.deltaTime;
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

                this.mTimeElapsed.z += Time.deltaTime;
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

                this.mTimeElapsed.w += Time.deltaTime;
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


            // trigger callback.
            if (mColorCallback != null)
                mColorCallback.Invoke();

            // trigger unity callback.
            if (mUnityCallback != null)
                mUnityCallback.Invoke();
        }

        /// <summary>
        /// Do the tweening.
        /// </summary>
        private void DoTweening()
        {
            if (!Animating)
                return;

            UpdateRed();
            UpdateGreen();
            UpdateBlue();
            UpdateAlpha();

            // update color.
            this.LocalColor = this.mProgressionColor;
        }
    }
}
