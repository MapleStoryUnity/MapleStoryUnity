/**
 * $File: JCS_SliderInputField.cs $
 * $Date: 2022-09-12 01:45:45 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2022 by Shen, Jen-Chieh $
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace JCSUnity
{
    /// <summary>
    /// Input field that controls sliders.
    /// </summary>
    [RequireComponent(typeof(InputField))]
    public class JCS_SliderInputField : MonoBehaviour
    {
        /* Variables */

        private InputField mInputField = null;

        [Header("** Initialize Variables (JCS_SliderInputField) **")]

        [Tooltip("Update the content type once on start.")]
        [SerializeField]
        private bool mAutoContentType = true;

        [Header("** Runtime Variables (JCS_SliderInputField) **")]

        [Tooltip("List of sliders you want this input field to control.")]
        public List<Slider> sliders = null;

        [Tooltip("Place you want to round the decimal.")]
        [SerializeField]
        [Range(0, 15)]
        private int mRoundPlace = 2;

        /* Setter & Getter */

        public bool AutoContentType { get { return mAutoContentType; } set { this.mAutoContentType = value; } }
        public int RoundPlace { get { return this.mRoundPlace; } set { this.mRoundPlace = value; } }

        /* Functions */

        private void Awake()
        {
            mInputField = GetComponent<InputField>();

            mInputField.onValueChanged.AddListener(OnValueChanged);
        }

        private void Start()
        {
            if (mAutoContentType)
                UpdateContentType();
        }

        private void Update()
        {
            if (mInputField.isFocused)
                return;

            Slider slider = MasterSlider();

            if (slider == null)
                return;

            double val = Math.Round(slider.value, mRoundPlace);

            mInputField.text = val.ToString();
        }

        private void OnValueChanged(string text)
        {
            float val;

            try
            {
                val = float.Parse(text.ToString());
            }
            catch (FormatException)
            {
                return;
            }

            UpdateSliders(val);
        }

        private void UpdateSliders(float val)
        {
            foreach (Slider slider in sliders)
            {
                slider.value = val;
            }
        }

        /// <summary>
        /// Update content type according to the master slider.
        /// </summary>
        public void UpdateContentType()
        {
            Slider slider = MasterSlider();

            if (slider == null)
                return;

            InputField.ContentType type = InputField.ContentType.DecimalNumber;

            if (slider.wholeNumbers)
                type = InputField.ContentType.IntegerNumber;

            mInputField.contentType = type;
        }

        /// <summary>
        /// Return a slider.
        /// </summary>
        private Slider MasterSlider()
        {
            if (sliders.Count == 0)
                return null;

            return sliders[0];
        }
    }
}
