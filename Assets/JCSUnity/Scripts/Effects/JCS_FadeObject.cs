/**
 * $File: JCS_FadeObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Fade object to a particular alpha channel.
    /// </summary>
    public class JCS_FadeObject : JCS_UnityObject
    {
        /* Variables */

        // Execution after its done fading.
        public Action onAfterFade = null;
        public Action onAfterFadeOut = null;
        public Action onAfterFadeIn = null;

        // Exectuion before start fading.
        public Action onBeforeFade = null;
        public Action onBeforeFadeIn = null;
        public Action onBeforeFadeOut = null;

        // Execution while fading.
        public Action<float> onFading = null;

        // The current fade type.
        private JCS_FadeType mFadeType = JCS_FadeType.IN;  // defaul as visible

        // Hold the alpha value.
        private float mAlpha = 1.0f;

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_FadeObject)")]

        [Tooltip("Test Fade in/out with key.")]
        [SerializeField]
        private bool mTestWithKey = false;

        [SerializeField]
        private KeyCode mFadeInKey = KeyCode.M;
        [SerializeField]
        private KeyCode mFadeOutKey = KeyCode.N;
#endif

        [Separator("Check Variables (JCS_FadeObject)")]

        [Tooltip("Is current fade object doing the effect? (fade in/out)")]
        [SerializeField]
        [ReadOnly]
        private bool mEffect = false;

        [Tooltip("Is current fade object visible?")]
        [SerializeField]
        [ReadOnly]
        private bool mVisible = true;

        [Separator("Runtime Variables (JCS_FadeObject)")]

        [Tooltip("How long it fades.")]
        [SerializeField]
        [Range(0.0f, 60.0f)]
        private float mFadeTime = 1.0f;

        [Tooltip("Override the action before it complete the action.")]
        [SerializeField]
        private bool mOverrideFade = false;

        [Tooltip("Maxinum of fade value.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mFadeInAmount = 1.0f;

        [Tooltip("Mininum of fade value.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mFadeOutAmount = 0.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public bool Effect { get { return this.mEffect; } set { this.mEffect = value; } }
        public bool Visible { get { return this.mVisible; } set { this.mVisible = value; } }
        public float FadeTime { get { return this.mFadeTime; } set { this.mFadeTime = value; } }
        public bool OverrideFade { get { return this.mOverrideFade; } set { this.mOverrideFade = value; } }
        public float Alpha { get { return this.mAlpha; } set { this.mAlpha = value; } }
        public float FadeInAmount { get { return this.mFadeInAmount; } set { this.mFadeInAmount = value; } }
        public float FadeOutAmount { get { return this.mFadeOutAmount; } set { this.mFadeOutAmount = value; } }
        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        /* Functions */

        private void Start()
        {
            if (LocalColor.a <= 0.0f)
                mVisible = false;
            else
                mVisible = true;
        }

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif
            DoFade();
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (Input.GetKeyDown(mFadeOutKey))
                FadeOut();
            if (Input.GetKeyDown(mFadeInKey))
                FadeIn();
        }
#endif

        /// <summary>
        /// Return true if the object is fade in.
        /// </summary>
        public bool IsFadeIn()
        {
            return (this.mAlpha >= mFadeInAmount);
        }

        /// <summary>
        /// Return true if the object is fade out.
        /// </summary>
        public bool IsFadeOut()
        {
            return (this.mAlpha <= mFadeOutAmount);
        }

        /// <summary>
        /// Fade out.
        /// </summary>
        public void FadeOut()
        {
            FadeOut(mFadeTime);
        }
        public void FadeOut(float time)
        {
            FadeEffect(JCS_FadeType.OUT, time);
        }

        /// <summary>
        /// Fade in.
        /// </summary>
        public void FadeIn()
        {
            FadeIn(mFadeTime);
        }
        public void FadeIn(float time)
        {
            FadeEffect(JCS_FadeType.IN, time);
        }

        /// <summary>
        /// Default function to point to, prevent null 
        /// reference exception.
        /// 
        /// To save check null pointer performance.
        /// </summary>
        public static void DefaultFadeCallback()
        {
            // empty.
        }

        /// <summary>
        /// Do the fade effect.
        /// </summary>
        /// <param name="type"> Type to fade </param>
        /// <param name="time"> time to fade in/out, in seconds </param>
        private void FadeEffect(JCS_FadeType type, float time)
        {
            if (!mOverrideFade)
            {
                // Check is already fade out or fade in!
                if ((mVisible && type == JCS_FadeType.IN) ||
                    (!mVisible && type == JCS_FadeType.OUT))
                    return;
            }

            // enable the effect component
            this.LocalEnabled = true;

            switch (type)
            {
                case JCS_FadeType.OUT:
                    {
                        mAlpha = mFadeInAmount;
                        this.mVisible = false;

                        onBeforeFadeOut?.Invoke();
                    }
                    break;
                case JCS_FadeType.IN:
                    {
                        mAlpha = mFadeOutAmount;
                        this.mVisible = true;

                        onBeforeFadeIn?.Invoke();
                    }
                    break;
            }

            this.mFadeTime = time;
            this.mFadeType = type;
            this.mEffect = true;

            onBeforeFade?.Invoke();
        }

        /// <summary>
        /// Do the core fade effect.
        /// </summary>
        private void DoFade()
        {
            if (GetObjectType() == JCS_UnityObjectType.GAME_OBJECT &&
                JCS_GameManager.FirstInstance().GAME_PAUSE)
                return;

            if (!mEffect)
                return;

            switch (mFadeType)
            {
                case JCS_FadeType.OUT:
                    {
                        // Fade out effect complete
                        if (mAlpha < mFadeOutAmount)
                        {
                            this.LocalEnabled = false;

                            mEffect = false;

                            // do callback
                            {
                                onAfterFadeOut?.Invoke();
                                onAfterFade?.Invoke();
                            }

                            return;
                        }

                        mAlpha -= JCS_Time.ItTime(mTimeType) / mFadeTime;
                    }
                    break;

                case JCS_FadeType.IN:
                    {
                        // Fade in effect complete
                        if (mAlpha > mFadeInAmount)
                        {
                            mEffect = false;

                            // do callback
                            {
                                onAfterFadeIn?.Invoke();
                                onAfterFade?.Invoke();
                            }

                            return;
                        }

                        mAlpha += JCS_Time.ItTime(mTimeType) / mFadeTime;
                    }
                    break;
            }

            Color screenColor = this.LocalColor;
            screenColor.a = mAlpha;
            this.LocalColor = screenColor;

            onFading?.Invoke(mAlpha);
        }
    }
}
