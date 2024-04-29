/**
 * $File: JCS_TimeScaleSlider.cs $
 * $Date: 2023-09-03 14:19:04 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2023 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;

namespace JCSUnity
{
    /// <summary>
    /// Slider component to control time scale value.
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class JCS_TimeScaleSlider : MonoBehaviour
    {
        /* Variables */

        private Slider mSlider = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            this.mSlider = this.GetComponent<Slider>();
        }

        private void Update()
        {
            Time.timeScale = mSlider.value;
        }
    }
}
