/**
 * $File: JCS_2DWaveEffect.cs $
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
    /// Effect that make wave effect on the gameobject.
    /// </summary>
    public class JCS_2DWaveEffect : JCS_2DEffect
    {
        /* Variables */

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_2DWaveEffect) **")]

        [Tooltip("Test this component with key.")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Do wave effect key.")]
        [SerializeField]
        private KeyCode mDoWaveEffectKey = KeyCode.T;
#endif

        [Header("** Runtime Variables (JCS_2DWaveEffect) **")]

        [Tooltip("Height offset.")]
        [SerializeField]
        private float mWaveRestPosition = 0.0f;

        [Tooltip("How intense the wave is.")]
        [SerializeField]
        private float mAmplitude = 1.0f;

        [Tooltip("How fast per period in wave.")]
        [SerializeField]
        private float mFrequency = 2.0f;

        private Vector3 mOrigin = Vector3.zero;

        [Tooltip("Effect Axis(Effect will do only on axis!, for random plz use the 3D version!)")]
        [SerializeField]
        private JCS_Axis mEffectAxis = JCS_Axis.AXIS_X;

        [Tooltip("How long the effect takes.")]
        [SerializeField]
        [Range(0.001f, 360.0f)]
        private float mEffectTime = 0.0f;

        private float mTime = 0.0f;

        [Header("NOTE: If the effect object is camera, plz fill the camera in here.")]

        [SerializeField]
        private JCS_2DCamera mJCS_2DCamera = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            if (mJCS_2DCamera == null)
                mJCS_2DCamera = this.GetComponent<JCS_2DCamera>();
        }

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

            if (JCS_Input.GetKeyDown(mDoWaveEffectKey))
                DoWaveEffect(1, mFrequency);
        }
#endif

        /// <summary>
        /// Active the wave effect.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="frequency"></param>
        public void DoWaveEffect(float time, float frequency)
        {
            // if is doing the effect
            if (mEffect)
                return;

            mTime = 0;
            mEffectTime = time;
            mFrequency = frequency;

            mOrigin = this.transform.position;

            // cps = cycle per second = wave speed

            mEffect = true;
        }

        private void DoEffect()
        {
            if (!mEffect)
                return;

            if (mJCS_2DCamera != null)
            {
                this.mOrigin.x = mJCS_2DCamera.GetTargetTransform().position.x;
                this.mOrigin.y = mJCS_2DCamera.GetTargetTransform().position.y;
            }

            mTime += Time.deltaTime;

            if (mEffectTime > mTime)
            {
                Vector3 newPos = mOrigin;


                float force =
                    (mAmplitude *
                    Mathf.Cos(mTime * Mathf.PI * mFrequency * 2.0f));

                switch (mEffectAxis)
                {
                    case JCS_Axis.AXIS_X:
                        newPos.x = force +
                            (mWaveRestPosition + mOrigin.x);
                        break;
                    case JCS_Axis.AXIS_Y:
                        newPos.y = force +
                            (mWaveRestPosition + mOrigin.y);
                        break;
                    case JCS_Axis.AXIS_Z:
                        newPos.z = force +
                            (mWaveRestPosition + mOrigin.z);
                        break;
                }

                this.transform.position = newPos;
            }
            else
            {
                this.transform.position = mOrigin;

                mEffect = false;
            }
        }
    }
}
