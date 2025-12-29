/**
 * $File: JCS_SliderTextDisplay.cs $
 * $Date: 2022-09-04 01:10:40 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright � 2022 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Text that display slider's value.
    /// </summary>
    public class JCS_SliderTextDisplay : JCS_TextObject
    {
        /* Variables */

        // The execution when the text data is refreshed.
        public Action onRefresh = null;

        // Record for last min value.
        private float mMinVal = 0;

        // Record for last max value.
        private float mMaxVal = 0;

        [Separator("⚡️ Runtime Variables (JCS_TextSliderDisplay)")]

        [Tooltip("To update the text along with this slider's value.")]
        [SerializeField]
        private Slider mSlider = null;

        [Tooltip("Format text to display.")]
        [SerializeField]
        private string mFormat = "{0} / {1}";

        [Tooltip("Place you want to round the decimal.")]
        [SerializeField]
        [Range(0, 15)]
        private int mRoundPlace = 2;

        /* Setter & Getter */

        public Slider slider { get { return mSlider; } set { mSlider = value; } }
        public string format { get { return mFormat; } set { mFormat = value; } }
        public int roundPlace { get { return mRoundPlace; } set { mRoundPlace = value; } }

        /* Functions */

        private void OnEnable()
        {
            AddListener();
        }

        private void OnDisable()
        {
            RemoveListener();
        }

        private void Update()
        {
            if (mSlider == null)
                return;

            HandleMinMaxValueChanged();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Refresh();
        }
#endif

        private void AddListener()
        {
            mSlider?.onValueChanged.AddListener(OnValueChanged);

            // Call once.
            Refresh();
        }

        private void RemoveListener()
        {
            mSlider?.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(float val)
        {
            Refresh();
        }

        /// <summary>
        /// Refresh the text once.
        /// </summary>
        public void Refresh()
        {
            if (mSlider == null)
                return;

            text = string.Format(mFormat,
                    Math.Round(mSlider.value, mRoundPlace),
                    Math.Round(mSlider.maxValue, mRoundPlace),
                    Math.Round(mSlider.minValue, mRoundPlace));

            onRefresh?.Invoke();
        }

        /// <summary>
        /// Handle min/max value changed.
        /// </summary>
        private void HandleMinMaxValueChanged()
        {
            if (mMinVal != mSlider.minValue ||
                mMaxVal != mSlider.maxValue)
            {
                mMinVal = mSlider.minValue;
                mMaxVal = mSlider.maxValue;

                // Handle min/max value change.
                Refresh();
            }
        }
    }
}
