/**
 * $File: JCS_3DShakeEffect.cs $
 * $Date: 2020-05-12 12:42:40 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright ?2020 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Effect that shake the transform in 3D.
    /// </summary>
    public class JCS_3DShakeEffect : JCS_UnityObject
    {
        /* Variables*/

        // Callback executed before the shake effect.
        public Action onBeforeShake = null;

        // Callback executed while doing the shake effect.
        public Action onShake = null;

        // Callback executed after the shake effect. 
        public Action onAfterShake = null;

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_3DShakeEffect)")]

        [Tooltip("Test this component with key.")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Do wave effect key.")]
        [SerializeField]
        private KeyCode mDoEffectKey = KeyCode.Y;
#endif

        [Separator("Check Variables (JCS_3DShakeEffect)")]

        [Tooltip("Flag to check if currently the effect active.")]
        [SerializeField]
        [ReadOnly]
        private bool mEffect = false;

        [Tooltip("Shake delta changes on transform properties.")]
        [SerializeField]
        [ReadOnly]
        private Vector3 mDelta = Vector3.zero;

        [Separator("Runtime Variables (JCS_3DShakeEffect)")]

        [Tooltip("Shake on this transform properties.")]
        [SerializeField]
        private JCS_TransformType mTransformType = JCS_TransformType.POSITION;

        [Tooltip("Force the effect even when its already in the motion.")]
        [SerializeField]
        private bool mForce = false;

        [Tooltip("How long it shakes.")]
        [SerializeField]
        [Range(0.001f, 360.0f)]
        private float mTime = 1.0f;

        [Tooltip("How intense it shakes.")]
        [SerializeField]
        private float mMargin = 3.0f;

        [Tooltip("Shake for each steps.")]
        [SerializeField]
        private float mSteps = 5.0f;

        // Support
        private float mTimer = 0.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("- Axis")]

        [Tooltip("Do shake on z axis.")]
        [SerializeField]
        private bool mOnX = true;

        [Tooltip("Do shake on y axis.")]
        [SerializeField]
        private bool mOnY = true;

        [Tooltip("Do shake on z axis.")]
        [SerializeField]
        private bool mOnZ = true;

        [Header("- Sound")]

        [Tooltip("Sound player for 3D sounds calculation.")]
        [SerializeField]
        private JCS_SoundPlayer mSoundPlayer = null;

        [Tooltip("Sound played when effect occurs.")]
        [SerializeField]
        private AudioClip mClip = null;

        /* Setter & Getter */

        public bool Effect { get { return this.mEffect; } set { this.mEffect = value; } }

        public JCS_TransformType TransformType { get { return this.mTransformType; } set { this.mTransformType = value; } }
        public bool Force { get { return this.mForce; } set { this.mForce = value; } }
        public float Time { get { return this.mTime; } set { this.mTime = value; } }
        public float Margin { get { return this.mMargin; } set { this.mMargin = value; } }
        public float Steps { get { return this.mSteps; } set { this.mSteps = value; } }

        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        public bool OnX { get { return this.mOnX; } set { this.mOnX = value; } }
        public bool OnY { get { return this.mOnY; } set { this.mOnY = value; } }
        public bool OnZ { get { return this.mOnZ; } set { this.mOnZ = value; } }

        public JCS_SoundPlayer SoundPlayer { get { return this.mSoundPlayer; } set { this.mSoundPlayer = value; } }
        public AudioClip Clip { get { return this.mClip; } set { this.mClip = value; } }

        /* Functions */

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif

            DoEffect();
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(mDoEffectKey))
            {
                DoShake();
            }
        }
#endif

        /// <summary>
        /// Do the shake effect with time and margin.
        /// </summary>
        /// <param name="time"> time to do the shake. </param>
        /// <param name="margin"> margin to do the shake. </param>
        public void DoShake()
        {
            DoShake(mTime, mMargin, mForce);
        }
        public void DoShake(float time, float margin, bool force)
        {
            if (!force)
            {
                // if is doing the effect
                if (mEffect)
                {
                    // is effecting.
                    return;
                }
            }

            this.mTime = time;
            this.mTimer = 0.0f;
            this.mMargin = margin;

            mDelta = Vector3.zero;

            mEffect = true;

            PlayeSound();

            onBeforeShake?.Invoke();
        }

        /// <summary>
        /// Do the actual shake job.
        /// </summary>
        private void DoEffect()
        {
            if (!mEffect)
                return;

            RevertShakeByTransformType(mDelta);

            mDelta = Vector3.zero;

            float dt = JCS_Time.ItTime(mTimeType);

            // Handle pause situation.
            {
                var pm = JCS_PauseManager.instance;

                if (pm != null && pm.Paused)
                    return;
            }

            mTimer += dt;

            if (mTimer < mTime)
            {
                // shake randomly
                if (mOnX)
                    mDelta.x = (JCS_Random.RangeInclude(-1.0f, 1.0f)) * mMargin * (mTime / mTimer) / mSteps;
                if (mOnY)
                    mDelta.y = (JCS_Random.RangeInclude(-1.0f, 1.0f)) * mMargin * (mTime / mTimer) / mSteps;
                if (mOnZ)
                    mDelta.z = (JCS_Random.RangeInclude(-1.0f, 1.0f)) * mMargin * (mTime / mTimer) / mSteps;

                ApplyShakeByTransformType(mDelta);

                onShake?.Invoke();
            }
            else
            {
                mTimer = 0.0f;
                mEffect = false;

                onAfterShake?.Invoke();
            }
        }

        /// <summary>
        /// Play shake sound.
        /// </summary>
        private void PlayeSound()
        {
            if (mClip == null)
                return;

            JCS_SoundPlayer.PlayByAttachment(mSoundPlayer, mClip);
        }

        /// <summary>
        /// Apply shake delta by transform type.
        /// </summary>
        /// <param name="shakeDelta"> Shake delta value. </param>
        private void ApplyShakeByTransformType(Vector3 shakeDelta)
        {
            switch (this.mTransformType)
            {
                /* Transform */
                case JCS_TransformType.POSITION:
                    this.transform.position += shakeDelta;
                    break;
                case JCS_TransformType.ROTATION:
                    this.transform.eulerAngles += shakeDelta;
                    break;
                case JCS_TransformType.SCALE:
                    this.transform.localScale += shakeDelta;
                    break;
                /* RectTransform */
                case JCS_TransformType.ANCHOR_MIN:
                    mRectTransform.anchorMin += (Vector2)shakeDelta;
                    break;
                case JCS_TransformType.ANCHOR_MAX:
                    mRectTransform.anchorMax += (Vector2)shakeDelta;
                    break;
                case JCS_TransformType.SIZE_DELTA:
                    mRectTransform.sizeDelta += (Vector2)shakeDelta;
                    break;
                case JCS_TransformType.PIVOT:
                    mRectTransform.pivot += (Vector2)shakeDelta;
                    break;
                case JCS_TransformType.ANCHOR_POSITION:
                    mRectTransform.anchoredPosition += (Vector2)shakeDelta;
                    break;
                case JCS_TransformType.ANCHOR_POSITION_3D:
                    mRectTransform.anchoredPosition3D += shakeDelta;
                    break;
                case JCS_TransformType.OFFSET_MIN:
                    mRectTransform.offsetMin += (Vector2)shakeDelta;
                    break;
                case JCS_TransformType.OFFSET_MAX:
                    mRectTransform.offsetMax += (Vector2)shakeDelta;
                    break;
            }
        }

        /// <summary>
        /// Revert shake delta by transform type.
        /// </summary>
        /// <param name="shakeDelta"> Shake delta value. </param>
        private void RevertShakeByTransformType(Vector3 shakeDelta)
        {
            switch (this.mTransformType)
            {
                /* Transform */
                case JCS_TransformType.POSITION:
                    this.transform.position -= shakeDelta;
                    break;
                case JCS_TransformType.ROTATION:
                    this.transform.eulerAngles -= shakeDelta;
                    break;
                case JCS_TransformType.SCALE:
                    this.transform.localScale -= shakeDelta;
                    break;
                /* RectTransform */
                case JCS_TransformType.ANCHOR_MIN:
                    mRectTransform.anchorMin -= (Vector2)shakeDelta;
                    break;
                case JCS_TransformType.ANCHOR_MAX:
                    mRectTransform.anchorMax -= (Vector2)shakeDelta;
                    break;
                case JCS_TransformType.SIZE_DELTA:
                    mRectTransform.sizeDelta -= (Vector2)shakeDelta;
                    break;
                case JCS_TransformType.PIVOT:
                    mRectTransform.pivot -= (Vector2)shakeDelta;
                    break;
                case JCS_TransformType.ANCHOR_POSITION:
                    mRectTransform.anchoredPosition -= (Vector2)shakeDelta;
                    break;
                case JCS_TransformType.ANCHOR_POSITION_3D:
                    mRectTransform.anchoredPosition3D -= shakeDelta;
                    break;
                case JCS_TransformType.OFFSET_MIN:
                    mRectTransform.offsetMin -= (Vector2)shakeDelta;
                    break;
                case JCS_TransformType.OFFSET_MAX:
                    mRectTransform.offsetMax -= (Vector2)shakeDelta;
                    break;
            }
        }
    }
}
