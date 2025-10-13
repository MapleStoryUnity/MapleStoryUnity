/**
 * $File: JCS_Dropdown.cs $
 * $Date: 2018-08-14 20:36:27 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;
using TMPro;
using NUnit.Framework.Interfaces;
using System;

namespace JCSUnity
{
    /// <summary>
    /// A better version of dropdown handle for uGUI.
    /// </summary>
    public class JCS_Dropdown : JCS_DropdownObject
    {
        /* Variables */

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_Dropdown)")]

        [Tooltip("Test this component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Update the dropdown options' text value.")]
        [SerializeField]
        private KeyCode mUpdateDropdown = KeyCode.U;
#endif

        [Separator("Check Variables (JCS_Dropdown)")]

        [Tooltip("Store all the real dropdown texts, not the shortcut version.")]
        [SerializeField]
        [ReadOnly]
        private List<string> mDropdownRealTexts = null;

        [Tooltip("Store all the dropdown texts copy.")]
        [SerializeField]
        [ReadOnly]
        private List<string> mDropdownBackupTexts = null;

        [Separator("Runtime Variables (JCS_Dropdown)")]

        [Tooltip("Max letters shown in the options. Default is 14.")]
        [SerializeField] [Range(7, 200)]
        private int mMaxLetters = 14;

        [Tooltip("How many dot shown to replace the letter.")]
        [SerializeField] [Range(1, 5)]
        private int mDotCount = 3;

        [Tooltip("Dot is more approach to the second half of the text value.")]
        [SerializeField]
        private bool mApproachSec = false;

        /* Setter & Getter */

        public int maxLetters { get { return mMaxLetters; } set { mMaxLetters = value; } }
        public int dotCount { get { return mDotCount; } set { mDotCount = value; } }
        public List<string> dropdownRealTexts { get { return mDropdownRealTexts; } }
        public List<string> dropdownBackupTexts { get { return mDropdownBackupTexts; } }

        /* Functions */

#if UNITY_EDITOR
        private void Update()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(mUpdateDropdown))
            {
                UpdateDropdownData();
            }
        }
#endif

        /// <summary>
        /// Update the dropdown data.
        /// 
        /// Call this when the option data from the dropdown object
        /// has been updated.
        /// </summary>
        public void UpdateDropdownData()
        {
            // Clear it before we refresh.
            mDropdownRealTexts.Clear();

            int centerLetterPos = mMaxLetters / 2;

            if (JCS_Mathf.IsOdd(mMaxLetters))
                centerLetterPos += 1;

            int halfDotCount = mDotCount / 2;

            string dot = ".";

            /* First get the dot string. */
            {
                for (int count = 1; count < mDotCount; ++count)
                {
                    dot += ".";
                }
            }

            UpdateDropdownDataLegacy(centerLetterPos, halfDotCount, dot);
            UpdateDropdownDataTMP(centerLetterPos, halfDotCount, dot);


            // Refresh the selection.
            JCS_UIUtil.Dropdown_RefreshSelection(mDropdownLegacy);
            JCS_UIUtil.Dropdown_RefreshSelection(mDropdownTMP);
        }

        private void UpdateDropdownDataLegacy(int centerLetterPos, int halfDotCount, string dot)
        {
            int index = -1;

            foreach (var od in mDropdownLegacy.options)
            {
                ++index;

                // Check if we need to shortcut 
                string textData = od.text;

                string newTextData = ReplacementString(centerLetterPos, halfDotCount, dot, index, textData);

                if (newTextData == null)
                    continue;

                // Apply new text data.
                od.text = newTextData;
            }
        }

        private void UpdateDropdownDataTMP(int centerLetterPos, int halfDotCount, string dot)
        {
            int index = -1;

            foreach (var od in mDropdownTMP.options)
            {
                ++index;

                // Check if we need to shortcut 
                string textData = od.text;

                string newTextData = ReplacementString(centerLetterPos, halfDotCount, dot, index, textData);

                if (newTextData == null)
                    continue;

                // Apply new text data.
                od.text = newTextData;
            }
        }

        /// <summary>
        /// Return the truncated string.
        /// </summary>
        private string ReplacementString(int centerLetterPos, int halfDotCount, string dot, int index, string textData)
        {
            if (IsShortcutText(textData, dot))
            {
                // Add the value from backup then.
                mDropdownRealTexts.Add(mDropdownBackupTexts[index]);
                return null;
            }

            // add the text.
            mDropdownRealTexts.Add(textData);

            // make another backup.
            mDropdownBackupTexts.Add(textData);

            int textDataLen = textData.Length;

            // Check if we need the fold the option text.
            if (textDataLen <= mMaxLetters)
                return null;

            int firstReplacePos = centerLetterPos - halfDotCount;

            int secondReplacePos = firstReplacePos;

            if (JCS_Mathf.IsOdd(mDotCount))
            {
                if (mApproachSec)
                    secondReplacePos += 1;
                else
                    firstReplacePos += 1;
            }

            // We start counting from the text data length.
            // So we simply just minus it.
            secondReplacePos = textDataLen - secondReplacePos;

            string newTextData =
                // First half of the text data.
                textData.Substring(0, firstReplacePos)
                // Add replace dots.
                + dot
                // Add the second half of the text data.
                + textData.Substring(secondReplacePos, textDataLen - secondReplacePos);

            return newTextData;
        }

        /// <summary>
        /// Is the value of dropdown's option already been shortcut.
        /// </summary>
        /// <param name="inStr"> current dropdown option text. </param>
        /// <param name="dot"> dot string </param>
        /// <returns>
        /// true, is already been shortcut by this component.
        /// false, not a shortcut value. Meaning the value is raw 
        /// and have not take effect by this component.
        /// </returns>
        private bool IsShortcutText(string inStr, string dot)
        {
            // TODO(jenchieh): This isn't flawless, this could
            // cause any option value with '.' will cause issue.
            if (inStr.Contains(dot))
                return true;
            return false;
        }
    }
}
