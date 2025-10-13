/**
 * $File: JCS_SoundSlider.cs $
 * $Date: 2022-08-24 00:01:40 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2022 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.Audio;
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

        [Tooltip("The mixer to control; if null assign the default mixer.")]
        [SerializeField]
        private AudioMixer mMixer = null;

        [Tooltip("The target volume parameter.")]
        [SerializeField]
        private string mParameter = "";

        /* Setter & Getter */

        public Slider slider { get { return mSlider; } }

        /* Functions */

        private void Awake()
        {
            mSlider = GetComponent<Slider>();

            // The minimum value can't be 0.
            if (mSlider.minValue == 0.0f)
                mSlider.minValue = 0.0001f;
        }

        private void Start()
        {
            // Assign default mixer.
            if (mMixer == null)
                mMixer = JCS_SoundSettings.FirstInstance().mixer;

            JCS_GameManager.FirstInstance().RegisterOnAfterInit(AddListener);
        }

        private void OnValidate()
        {
            AddListener();
        }

        private void AddListener()
        {
            if (mSlider == null)
                return;

            // First, initialize to keep the settings untouched.
            mSlider.value = JCS_Audio.GetVolume(mMixer, mParameter);

            mSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });

            // Call once.
            OnValueChanged();
        }

        private void OnValueChanged()
        {
            var sm = JCS_SceneManager.FirstInstance();

            if (sm.IsSwitchingScene())
                return;

            float total = mSlider.maxValue - mSlider.minValue;  // Find total.
            float val = mSlider.value / total;                  // Convert to 0 to 1 scale.

            JCS_Audio.SetVolume(mMixer, mParameter, val);
        }
    }
}
