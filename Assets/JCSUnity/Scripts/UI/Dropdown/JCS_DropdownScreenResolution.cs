/**
 * $File: JCS_DropdownScreenResolution.cs $
 * $Date: 2025-04-15 01:14:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright � 2025 by Shen, Jen-Chieh $
 */
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JCSUnity
{
    /// <summary>
    /// A dropdown menu let you choose the screen resolution.
    /// </summary>
    public class JCS_DropdownScreenResolution : JCS_DropdownObject
    {
        /* Variables */

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_DropdownScreenResolution)")]

        [Tooltip("Change the game view editor's window.")]
        [SerializeField]
        private bool mChangeEditorWindow = false;
#endif

        [Separator("Initialize Variables (JCS_DropdownScreenResolution)")]

        [Tooltip("If true, remove all other options at the beginning.")]
        [SerializeField]
        private bool mRemoveAllOptions = true;

        /* Setter & Getter */

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

            screens.onChangedSize += Refresh;
        }

        private void AddListener()
        {
            mDropdownLegacy?.onValueChanged.AddListener(delegate
            {
                OnValueChanged_Legacy(mDropdownLegacy);
            });

            mDropdownTMP?.onValueChanged.AddListener(delegate
            {
                OnValueChanged_TMP(mDropdownTMP);
            });

            // Run once.
            OnValueChanged_Legacy(mDropdownLegacy);
            OnValueChanged_TMP(mDropdownTMP);
        }

        /// <summary>
        /// Refresh all options once.
        /// </summary>
        public void Refresh()
        {
            if (mRemoveAllOptions)
                ClearOptions();

            foreach (Resolution res in Screen.resolutions.Reverse())
            {
                string text = FormatName(res.width, res.height);

                int index = JCS_UIUtil.Dropdown_GetItemIndex(this, text);

                // Prevent adding the same options.
                if (index == -1)
                    JCS_UIUtil.Dropdown_AddOption(this, text);
            }

            // Default to the current screen resolution.
            {
                string res = FormatName(Screen.width, Screen.height);

                JCS_UIUtil.Dropdown_SetSelection(this, res);
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
            string[] data = text.Split("x");

            Resolution res = Screen.currentResolution;

            int width = JCS_Util.Parse(data[0], res.width);
            int height = JCS_Util.Parse(data[1], res.height);

            Screen.SetResolution(width, height, Screen.fullScreenMode);

#if UNITY_EDITOR
            if (mChangeEditorWindow)
            {
                GameViewUtils.AddAndSelectCustomSize(
                    GameViewUtils.GameViewSizeType.FixedResolution,
                    GameViewSizeGroupType.Standalone, width, height,
                    text);
            }
#endif
        }

        private string FormatName(int width, int height)
        {
            return width + "x" + height;
        }
    }
}
