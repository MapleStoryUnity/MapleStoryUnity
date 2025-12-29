/**
 * $File: JCS_DropdownQuality.cs $
 * $Date: 2025-07-14 01:14:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright � 2025 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// A dropdown menu let you choose the quality level.
    /// </summary>
    public class JCS_DropdownQuality : JCS_DropdownObject
    {
        /* Variables */

        [Separator("🌱 Initialize Variables (JCS_DropdownQuality)")]

        [Tooltip("If true, remove all other options at the beginning.")]
        [SerializeField]
        private bool mRemoveAllOptions = true;

        [Separator("⚡️ Runtime Variables (JCS_DropdownQuality)")]

        [Tooltip("Should expensive changes be applied.")]
        [SerializeField]
        private bool mApplyExpensiveChanges = false;

        /* Setter & Getter */

        public bool removeAllOptions { get { return mRemoveAllOptions; } set { mRemoveAllOptions = value; } }
        public bool applyExpensiveChanges { get { return mApplyExpensiveChanges; } set { mApplyExpensiveChanges = value; } }

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

            foreach (string option in QualitySettings.names)
            {
                JCS_UIUtil.Dropdown_AddOption(this, option);
            }

            // Default to the current quality level.
            {
                string text = JCS_Quality.ItName();

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
            JCS_Quality.SetLevel(text);
        }
    }
}
