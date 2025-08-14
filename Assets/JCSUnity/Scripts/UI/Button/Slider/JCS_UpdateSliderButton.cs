/**
 * $File: JCS_UpdateSliderButton.cs $
 * $Date: 2022-09-03 22:51:49 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2022 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Update the slider value.
    /// </summary>
    public class JCS_UpdateSliderButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_UpdateSliderButton)")]

        [Tooltip("Target value to assign to slider list.")]
        [SerializeField]
        private float mTargetValue = 1f;

        [Tooltip("List of effected sliders.")]
        public List<Slider> sliders = null;

        /* Setter & Getter */

        public float targetValue { get { return mTargetValue; } set { mTargetValue = value; } }

        /* Functions */

        public override void OnClick()
        {
            UpdateSlidesrValue();
        }

        /// <summary>
        /// Update all sliders' value once.
        /// </summary>
        public void UpdateSlidesrValue()
        {
            foreach (var slider in sliders)
            {
                if (slider == null)
                    continue;

                slider.value = mTargetValue;
            }
        }
    }
}
