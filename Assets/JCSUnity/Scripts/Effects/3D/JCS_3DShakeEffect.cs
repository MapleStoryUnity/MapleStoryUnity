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

        [Tooltip("Key to shake once.")]
        [SerializeField]
        private KeyCode mKeyShake = KeyCode.Y;

        [Tooltip("Key to shake continously.")]
        [SerializeField]
        private KeyCode mKeyShakeForce = KeyCode.U;
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

        [Header("Axis")]

        [Tooltip("Do shake on z axis.")]
        [SerializeField]
        private bool mOnX = true;

        [Tooltip("Do shake on y axis.")]
        [SerializeField]
        private bool mOnY = true;

        [Tooltip("Do shake on z axis.")]
        [SerializeField]
        private bool mOnZ = true;

        [Header("Sound")]

        [Tooltip("Sound player for 3D sounds calculation.")]
        [SerializeField]
        private JCS_SoundPlayer mSoundPlayer = null;

        [Tooltip("Sound played when effect occurs.")]
        [SerializeField]
        private AudioClip mClip = null;

        /* Setter & Getter */

        public bool effect { get { return mEffect; } set { mEffect = value; } }

        public JCS_TransformType transformType { get { return mTransformType; } set { mTransformType = value; } }
        public bool force { get { return mForce; } set { mForce = value; } }
        public float time { get { return mTime; } set { mTime = value; } }
        public float margin { get { return mMargin; } set { mMargin = value; } }
        public float steps { get { return mSteps; } set { mSteps = value; } }

        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        public bool onX { get { return mOnX; } set { mOnX = value; } }
        public bool onY { get { return mOnY; } set { mOnY = value; } }
        public bool onZ { get { return mOnZ; } set { mOnZ = value; } }

        public JCS_SoundPlayer soundPlayer { get { return mSoundPlayer; } set { mSoundPlayer = value; } }
        public AudioClip audioClip { get { return mClip; } set { mClip = value; } }

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

            if (JCS_Input.GetKeyDown(mKeyShake))
            {
                DoShake();
            }

            if (JCS_Input.GetKey(mKeyShakeForce))
            {
                DoShake(true);
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
            DoShake(mForce);
        }
        public void DoShake(bool force)
        {
            DoShake(mTime, mMargin, mSteps, force);
        }
        public void DoShake(float time, float margin, float steps, bool force)
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

            mTime = time;
            mTimer = 0.0f;
            mMargin = margin;
            mSteps = steps;

            RevertShakeByTransformType(mDelta);

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

            float dt = JCS_Time.ItTime(mTimeType);

            mTimer += dt;

            if (mTimer < mTime)
            {
                if (mTimer <= 0.0f)
                    return;

                float timeStep = mTime / mTimer;

                float amount = (mMargin * timeStep / mSteps) * dt;

                // shake randomly
                if (mOnX)
                    mDelta.x = (JCS_Random.RangeInclude(-1.0f, 1.0f)) * amount;
                if (mOnY)
                    mDelta.y = (JCS_Random.RangeInclude(-1.0f, 1.0f)) * amount;
                if (mOnZ)
                    mDelta.z = (JCS_Random.RangeInclude(-1.0f, 1.0f)) * amount;

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
        private void ApplyShakeByTransformType(Vector3 delta)
        {
            if (JCS_Mathf.IsNaNOrInfinity(delta))
                return;

            switch (mTransformType)
            {
                /* Transform */
                case JCS_TransformType.POSITION:
                    transform.position += delta;
                    break;
                case JCS_TransformType.ROTATION:
                    transform.eulerAngles += delta;
                    break;
                case JCS_TransformType.SCALE:
                    transform.localScale += delta;
                    break;
                /* RectTransform */
                case JCS_TransformType.ANCHOR_MIN:
                    mRectTransform.anchorMin += (Vector2)delta;
                    break;
                case JCS_TransformType.ANCHOR_MAX:
                    mRectTransform.anchorMax += (Vector2)delta;
                    break;
                case JCS_TransformType.SIZE_DELTA:
                    mRectTransform.sizeDelta += (Vector2)delta;
                    break;
                case JCS_TransformType.PIVOT:
                    mRectTransform.pivot += (Vector2)delta;
                    break;
                case JCS_TransformType.ANCHOR_POSITION:
                    mRectTransform.anchoredPosition += (Vector2)delta;
                    break;
                case JCS_TransformType.ANCHOR_POSITION_3D:
                    mRectTransform.anchoredPosition3D += delta;
                    break;
                case JCS_TransformType.OFFSET_MIN:
                    mRectTransform.offsetMin += (Vector2)delta;
                    break;
                case JCS_TransformType.OFFSET_MAX:
                    mRectTransform.offsetMax += (Vector2)delta;
                    break;
            }
        }

        /// <summary>
        /// Revert shake delta by transform type.
        /// </summary>
        /// <param name="shakeDelta"> Shake delta value. </param>
        private void RevertShakeByTransformType(Vector3 delta)
        {
            if (JCS_Mathf.IsNaNOrInfinity(delta))
                return;

            switch (mTransformType)
            {
                /* Transform */
                case JCS_TransformType.POSITION:
                    transform.position -= delta;
                    break;
                case JCS_TransformType.ROTATION:
                    transform.eulerAngles -= delta;
                    break;
                case JCS_TransformType.SCALE:
                    transform.localScale -= delta;
                    break;
                /* RectTransform */
                case JCS_TransformType.ANCHOR_MIN:
                    mRectTransform.anchorMin -= (Vector2)delta;
                    break;
                case JCS_TransformType.ANCHOR_MAX:
                    mRectTransform.anchorMax -= (Vector2)delta;
                    break;
                case JCS_TransformType.SIZE_DELTA:
                    mRectTransform.sizeDelta -= (Vector2)delta;
                    break;
                case JCS_TransformType.PIVOT:
                    mRectTransform.pivot -= (Vector2)delta;
                    break;
                case JCS_TransformType.ANCHOR_POSITION:
                    mRectTransform.anchoredPosition -= (Vector2)delta;
                    break;
                case JCS_TransformType.ANCHOR_POSITION_3D:
                    mRectTransform.anchoredPosition3D -= delta;
                    break;
                case JCS_TransformType.OFFSET_MIN:
                    mRectTransform.offsetMin -= (Vector2)delta;
                    break;
                case JCS_TransformType.OFFSET_MAX:
                    mRectTransform.offsetMax -= (Vector2)delta;
                    break;
            }

            // Reset.
            mDelta = Vector3.zero;
        }
    }
}
