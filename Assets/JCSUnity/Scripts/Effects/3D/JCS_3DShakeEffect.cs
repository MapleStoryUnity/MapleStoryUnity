/**
 * $File: JCS_3DShakeEffect.cs $
 * $Date: 2020-05-12 12:42:40 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright ?2020 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Effect that shake the transform in 3D.
    /// </summary>
    public class JCS_3DShakeEffect : MonoBehaviour
    {
        /* Variables*/

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_3DShakeEffect) **")]

        [Tooltip("Test this component with key.")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Do wave effect key.")]
        [SerializeField]
        private KeyCode mDoShakeEffectKey = KeyCode.Y;
#endif

        [Header("** Check Variables (JCS_3DShakeEffect) **")]

        [Tooltip("Flag to check if currently the effect active.")]
        [SerializeField]
        private bool mEffect = false;

        [Tooltip("Shake delta changes on transform properties.")]
        [SerializeField]
        private Vector3 mShakeDelta = Vector3.zero;

        [Header("** Runtime Variables (JCS_3DShakeEffect) **")]

        [Tooltip("Shake on this transform properties.")]
        [SerializeField]
        private JCS_TransformType mTransformType = JCS_TransformType.POSITION;

        [Tooltip("Override the effect even the the effect is enabled already.")]
        [SerializeField]
        private bool mRepeatOverride = false;

        [Tooltip("How long it shakes.")]
        [SerializeField]
        [Range(0.001f, 360.0f)]
        private float mShakeTime = 1.0f;

        [Tooltip("How intense it shakes.")]
        [SerializeField]
        private float mShakeMargin = 3.0f;

        [Tooltip("Shake for each steps.")]
        [SerializeField]
        private float mShakeSteps = 5.0f;

        // Support
        private float mShakeTimer = 0.0f;

        [Header("- Axis")]

        [Tooltip("Do shake on z axis.")]
        [SerializeField]
        private bool mShakeOnX = true;

        [Tooltip("Do shake on y axis.")]
        [SerializeField]
        private bool mShakeOnY = true;

        [Tooltip("Do shake on z axis.")]
        [SerializeField]
        private bool mShakeOnZ = true;

        [Header("- Sound")]

        [Tooltip("Sound player for 3D sounds calculation.")]
        [SerializeField]
        private JCS_SoundPlayer mSoundPlayer = null;

        [Tooltip("Sound played when effect occurs.")]
        [SerializeField]
        private AudioClip mShakeSound = null;

        /* Setter & Getter */

        public bool Effect { get { return this.mEffect; } set { this.mEffect = value; } }

        public JCS_TransformType TransformType { get { return this.mTransformType; } set { this.mTransformType = value; } }
        public bool RepeatOverride { get { return this.mRepeatOverride; } set { this.mRepeatOverride = value; } }
        public float ShakeTime { get { return this.mShakeTime; } set { this.mShakeTime = value; } }
        public float ShakeMargin { get { return this.mShakeMargin; } }
        public float ShakeSteps { get { return this.mShakeSteps; } set { this.mShakeSteps = value; } }

        public bool ShakeOnX { get { return this.mShakeOnX; } set { this.mShakeOnX = value; } }
        public bool ShakeOnY { get { return this.mShakeOnY; } set { this.mShakeOnY = value; } }
        public bool ShakeOnZ { get { return this.mShakeOnZ; } set { this.mShakeOnZ = value; } }

        public JCS_SoundPlayer SoundPlayer { get { return this.mSoundPlayer; } set { this.mSoundPlayer = value; } }
        public AudioClip ShakeSound { get { return this.mShakeSound; } set { this.mShakeSound = value; } }

        /* Functions */

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            DoEffect();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKey(mDoShakeEffectKey))
            {
                DoShake();
            }
        }
#endif

        /// <summary>
        /// Do the shake with default shake time and shake margin.
        /// </summary>
        public void DoShake()
        {
            DoShake(mShakeTime, mShakeMargin);
        }

        /// <summary>
        /// Do the shake effect with time and margin.
        /// </summary>
        /// <param name="time"> time to do the shake. </param>
        /// <param name="margin"> margin to do the shake. </param>
        public void DoShake(float time, float margin)
        {
            if (!mRepeatOverride)
            {
                // if is doing the effect
                if (mEffect)
                {
                    // is effecting.
                    return;
                }
            }

            this.mShakeTime = time;
            this.mShakeTimer = 0;
            this.mShakeMargin = margin;

            mShakeDelta = Vector3.zero;

            mEffect = true;

            PlayeSound();
        }

        /// <summary>
        /// Do the actual shake job.
        /// </summary>
        private void DoEffect()
        {
            if (!mEffect)
                return;

            RevertShakeByTransformType(mShakeDelta);

            mShakeDelta = Vector3.zero;

            mShakeTimer += Time.deltaTime;

            if (mShakeTimer < mShakeTime)
            {
                // shake randomly
                if (mShakeOnX)
                    mShakeDelta.x = (JCS_Random.RangeInclude(-1.0f, 1.0f)) * mShakeMargin * (mShakeTime / mShakeTimer) / mShakeSteps;
                if (mShakeOnY)
                    mShakeDelta.y = (JCS_Random.RangeInclude(-1.0f, 1.0f)) * mShakeMargin * (mShakeTime / mShakeTimer) / mShakeSteps;
                if (mShakeOnZ)
                    mShakeDelta.z = (JCS_Random.RangeInclude(-1.0f, 1.0f)) * mShakeMargin * (mShakeTime / mShakeTimer) / mShakeSteps;

                ApplyShakeByTransformType(mShakeDelta);
            }
            else
            {
                mShakeTimer = 0.0f;
                mEffect = false;
            }
        }

        /// <summary>
        /// Play shake sound.
        /// </summary>
        private void PlayeSound()
        {
            if (mShakeSound == null)
                return;

            JCS_SoundPlayer.PlayByAttachment(mSoundPlayer, mShakeSound);
        }

        /// <summary>
        /// Apply shake delta by transform type.
        /// </summary>
        /// <param name="shakeDelta"> Shake delta value. </param>
        private void ApplyShakeByTransformType(Vector3 shakeDelta)
        {
            switch (this.mTransformType)
            {
                case JCS_TransformType.POSITION:
                    this.transform.position += shakeDelta;
                    break;
                case JCS_TransformType.ROTATION:
                    this.transform.eulerAngles += shakeDelta;
                    break;
                case JCS_TransformType.SCALE:
                    this.transform.localScale += shakeDelta;
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
                case JCS_TransformType.POSITION:
                    this.transform.position -= shakeDelta;
                    break;
                case JCS_TransformType.ROTATION:
                    this.transform.eulerAngles -= shakeDelta;
                    break;
                case JCS_TransformType.SCALE:
                    this.transform.localScale -= shakeDelta;
                    break;
            }
        }
    }
}
