/**
 * $File: JCS_SliderTextDisplay.cs $
 * $Date: 2022-09-04 01:10:40 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2022 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using UnityEngine.UI;

namespace JCSUnity
{
    /// <summary>
    /// Text that display slider's value.
    /// </summary>
    public class JCS_SliderTextDisplay : JCS_TextObject
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_TextSliderDisplay) **")]

        [Tooltip("To update the text along with this slider's value.")]
        [SerializeField]
        private Slider mSlider = null;

        [Tooltip("Place you want to round the decimal.")]
        [SerializeField]
        [Range(0, 15)]
        private int mRoundPlace = 2;

        /* Setter & Getter */

        public Slider slider { get { return this.mSlider; } set { this.mSlider = value; } }
        public int RoundPlace { get { return this.mRoundPlace; } set { this.mRoundPlace = value; } }

        /* Functions */

        private void Update()
        {
            if (mSlider == null)
                return;

            double val = Math.Round(mSlider.value, mRoundPlace);

            this.text = val.ToString();
        }
    }
}
