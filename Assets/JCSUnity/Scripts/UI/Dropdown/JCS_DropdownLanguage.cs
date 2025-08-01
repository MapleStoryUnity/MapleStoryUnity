/**
 * $File: JCS_DropdownLanguage.cs $
 * $Date: 2025-04-15 01:14:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2025 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// A dropdown menu let you choose the language.
    /// </summary>
    public class JCS_DropdownLanguage : JCS_DropdownObject
    {
        /* Variables */

        [Separator("Initialize Variables (JCS_DropdownLanguage)")]

        [Tooltip("List of supported languages.")]
        [SerializeField]
        private List<SystemLanguage> mOptions = new()
        {
            SystemLanguage.English,
            SystemLanguage.ChineseTraditional,
        };

        [Tooltip("If true, remove all other options at the beginning.")]
        [SerializeField]
        private bool mRemoveAllOptions = true;

        /* Setter & Getter */

        public List<SystemLanguage> Options { get { return mOptions; } set { this.mOptions = value; } }
        public bool RemoveAllOptions { get { return mRemoveAllOptions; } set { this.mRemoveAllOptions = value; } }

        /* Functions */

        private void Start()
        {
            Refresh();

            AddListener();
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

            foreach (SystemLanguage option in mOptions)
            {
                string text = JCS_Locale.SystemLangToString(option);

                JCS_UIUtil.Dropdown_AddOption(this, text);
            }

            // Default to the current windowed mode.
            {
                string text = JCS_Locale.SystemLangToString(JCS_AppManager.FirstInstance().systemLanguage);

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
            SystemLanguage selected = JCS_Locale.StringToSystemLang(text);

            JCS_AppManager.FirstInstance().systemLanguage = selected;
        }
    }
}
