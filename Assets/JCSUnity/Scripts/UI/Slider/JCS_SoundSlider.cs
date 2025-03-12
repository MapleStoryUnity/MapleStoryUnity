/**
 * $File: JCS_SoundSlider.cs $
 * $Date: 2022-08-24 00:01:40 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2022 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Slider component to control framework's sound control.
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class JCS_SoundSlider : MonoBehaviour
    {
        /* Variables */

        private Slider mSlider = null;

        [Separator("Initialize Variables (JCS_SoundSlider)")]

        [Tooltip("Sound type you would like the slider to control.")]
        [SerializeField]
        private JCS_SoundSettingType mSoundType = JCS_SoundSettingType.NONE;

        /* Setter & Getter */

        public Slider slider { get { return this.mSlider; } }
        public JCS_SoundSettingType SoundType { get { return this.mSoundType; } }

        /* Functions */

        private void Awake()
        {
            this.mSlider = this.GetComponent<Slider>();
        }

        private void LateUpdate()
        {
            var sm = JCS_SceneManager.instance;

            if (sm.IsSwitchingScene())
                return;

            float total = mSlider.maxValue - mSlider.minValue;  // Find total.
            float val = mSlider.value / total;                  // Convert to 0 to 1 scale.

            JCS_SoundSettings.instance.SetVolume(mSoundType, val);
        }
    }
}
