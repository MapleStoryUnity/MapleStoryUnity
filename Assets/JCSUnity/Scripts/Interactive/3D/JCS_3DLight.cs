/**
 * $File: JCS_3DLight.cs $
 * $Date: 2020-04-18 20:30:23 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Control the 3D light.
    /// </summary>
    [RequireComponent(typeof(Light))]
    [RequireComponent(typeof(JCS_AdjustTimeTrigger))]
    [RequireComponent(typeof(JCS_FloatTweener))]
    public class JCS_3DLight : MonoBehaviour
    {
        /* Variables */

        private Light mLight = null;

        private JCS_AdjustTimeTrigger mAdjustTimerTrigger = null;

        private JCS_FloatTweener mFloatTweener = null;

        [Separator("Runtime Variables (JCS_3DLight)")]

        [Tooltip("Flag for active this component.")]
        [SerializeField]
        private bool mActive = true;

        [Header("Min/Max")]

        [Tooltip("Mininum value of the light's range.")]
        [SerializeField]
        private float mMinRangeValue = 10.0f;

        [Tooltip("Maxinum value of the light's range.")]
        [SerializeField]
        private float mMaxRangeValue = 300.0f;

        /* Setter & Getter */

        public bool active { get { return mActive; } set { mActive = value; } }

        public float minRangeValue { get { return mMinRangeValue; } set { mMinRangeValue = value; } }
        public float maxRangeValue { get { return mMaxRangeValue; } set { mMaxRangeValue = value; } }

        /* Functions */

        private void Awake()
        {
            mLight = GetComponent<Light>();
            mAdjustTimerTrigger = GetComponent<JCS_AdjustTimeTrigger>();
            mFloatTweener = GetComponent<JCS_FloatTweener>();

            mFloatTweener.onValueChange = SetLight_Range;
            mFloatTweener.onValueReturn = GetLight_Range;

            mAdjustTimerTrigger.onAction = DoRange;
        }

        private void SetLight_Range(float newVal) { mLight.range = newVal; }
        private float GetLight_Range() { return mLight.range; }

        /// <summary>
        /// Do the fade algorithm.
        /// </summary>
        private void DoRange()
        {
            float newRange = JCS_Random.Range(mMinRangeValue, mMaxRangeValue);

            mFloatTweener.DoTween(newRange);
        }
    }
}
