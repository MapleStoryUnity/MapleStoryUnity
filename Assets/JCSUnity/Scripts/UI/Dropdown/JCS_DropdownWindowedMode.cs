/**
 * $File: JCS_DropdownWindowedMode.cs $
 * $Date: 2025-04-15 01:14:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright � 2025 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// A dropdown menu let you choose the windowed mode.
    /// </summary>
    [RequireComponent(typeof(TMP_Dropdown))]
    public class JCS_DropdownWindowedMode : JCS_DropdownObject
    {
        /* Variables */

        private List<string> mOptions = new()
        {
            "Full Screen",
            "Windowed",
        };

        [Separator("🌱 Initialize Variables (JCS_DropdownWindowedMode)")]

        [Tooltip("If true, remove all other options at the beginning.")]
        [SerializeField]
        private bool mRemoveAllOptions = true;

        /* Setter & Getter */

        public List<string> options { get { return mOptions; } set { mOptions = value; } }
        public bool removeAllOptions { get { return mRemoveAllOptions; } set { mRemoveAllOptions = value; } }

        /* Functions */

        private void Awake()
        {
            Refresh();

            AddListener();
        }

        private void Start()
        {
            var screens = JCS_ScreenSettings.FirstInstance();

            screens.onChangedMode += Refresh;
        }

        private void AddListener()
        {
            DropdownLegacy?.onValueChanged.AddListener(delegate
            {
                OnValueChanged_Legacy(DropdownLegacy);
            });

            mDropdownTMP?.onValueChanged.AddListener(delegate
            {
                OnValueChanged_TMP(mDropdownTMP);
            });
        }

        /// <summary>
        /// Refresh all options once.
        /// </summary>
        public void Refresh()
        {
            if (mRemoveAllOptions)
                ClearOptions();

            foreach (string option in mOptions)
            {
                JCS_UIUtil.Dropdown_AddOption(this, option);
            }

            // Default to the current windowed mode.
            {
                string text = ModeToString(Screen.fullScreenMode);

                JCS_UIUtil.Dropdown_SetSelection(this, text);
            }

            JCS_UIUtil.Dropdown_RefreshSelection(this);
        }

        private void OnValueChanged_Legacy(Dropdown dropdown)
        {
            if (dropdown == null)
                return;

            string text = JCS_UIUtil.Dropdown_GetSelectedValue(dropdown);

            OnValueChanged(text);
        }
        private void OnValueChanged_TMP(TMP_Dropdown dropdown)
        {
            if (dropdown == null)
                return;

            string text = JCS_UIUtil.Dropdown_GetSelectedValue(dropdown);

            OnValueChanged(text);
        }
        private void OnValueChanged(string text)
        {
            FullScreenMode mode = StringToMode(text);

            Resolution res = Screen.currentResolution;

            int width = res.width;
            int height = res.height;

            Screen.SetResolution(width, height, mode);
        }

        private string ModeToString(FullScreenMode mode)
        {
            switch (mode)
            {
                case FullScreenMode.FullScreenWindow:
                    return "Full Screen";
                default:
                    return "Windowed";
            }
        }

        private FullScreenMode StringToMode(string text)
        {
            switch (text)
            {
                case "Full Screen":
                    return FullScreenMode.FullScreenWindow;
                case "Windowed":
                    return FullScreenMode.Windowed;
            }

            return Screen.fullScreenMode;
        }
    }
}
