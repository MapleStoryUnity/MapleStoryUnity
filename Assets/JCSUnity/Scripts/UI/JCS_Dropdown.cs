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

namespace JCSUnity
{
    /// <summary>
    /// A better version of dropdown handle for uGUI.
    /// </summary>
    [RequireComponent(typeof(Dropdown))]
    public class JCS_Dropdown : MonoBehaviour
    {
        /* Variables */

        private Dropdown mDropdown = null;


#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_Dropdown) **")]

        [Tooltip("Test this component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Update the dropdown options' text value.")]
        [SerializeField]
        private KeyCode mUpdateDropdown = KeyCode.U;
#endif

        [Header("** Check Variables (JCS_Dropdown) **")]

        [Tooltip("Store all the real dropdown texts, not the shortcut version.")]
        [SerializeField]
        private List<string> mDropdownRealTexts = null;

        [Tooltip("Store all the dropdown texts copy.")]
        [SerializeField]
        private List<string> mDropdownBackupTexts = null;


        [Header("** Runtime Variables (JCS_Dropdown) **")]

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

        public Dropdown dropdown { get { return this.mDropdown; } }
        public int MaxLetters { get { return this.mMaxLetters; } set { this.mMaxLetters = value; } }
        public int DotCount { get { return this.mDotCount; } set { this.mDotCount = value; } }
        public List<string> DropdownRealTexts { get { return this.mDropdownRealTexts; } }
        public List<string> DropdownBackupTexts { get { return this.mDropdownBackupTexts; } }


        /* Functions */

        private void Awake()
        {
            this.mDropdown = this.GetComponent<Dropdown>();
        }

#if (UNITY_EDITOR)
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
                for (int count = 1;
                    count < mDotCount;
                    ++count)
                {
                    dot += ".";
                }
            }

            int index = -1;

            foreach (Dropdown.OptionData od in mDropdown.options)
            {
                ++index;

                // Check if we need to shortcut this.
                string textData = od.text;

                if (IsShortcutText(textData, dot))
                {
                    // Add the value from backup then.
                    mDropdownRealTexts.Add(mDropdownBackupTexts[index]);
                    continue;
                }

                // add the text.
                mDropdownRealTexts.Add(textData);

                // make another backup.
                mDropdownBackupTexts.Add(textData);

                int textDataLen = textData.Length;

                // Check if we need the fold the option text.
                if (textDataLen <= mMaxLetters)
                    continue;

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

                // Apply new text data.
                od.text = newTextData;
            }

            // Refresh the selection.
            JCS_UIUtil.Dropdown_RefreshSelection(mDropdown);
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
