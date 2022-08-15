/**
 * $File: JCS_InputField.cs $
 * $Date: 2018-09-06 02:38:35 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;

namespace JCSUnity
{
    /// <summary>
    /// A better version of input field handle for uGUI.
    /// </summary>
    [RequireComponent(typeof(InputField))]
    public class JCS_InputField : MonoBehaviour
    {
        /* Variables */

        private InputField mInputField = null;

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_InputField) **")]

        [Tooltip("Test this component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Update the input field options' text value.")]
        [SerializeField]
        private KeyCode mUpdateInputField = KeyCode.U;
#endif

        [Header("** Check Variables (JCS_InputField) **")]

        [Tooltip("Real text data this input field hold.")]
        [SerializeField]
        private string mRealText = null;

        [Tooltip("Is the input field focused?")]
        [SerializeField]
        private bool mIsFocus = false;

        [Header("** Runtime Variables (JCS_InputField) **")]

        [Tooltip("Max letters shown in the options. Default is 14.")]
        [SerializeField]
        [Range(7, 200)]
        private int mMaxLetters = 14;

        [Tooltip("How many dot shown to replace the letter.")]
        [SerializeField]
        [Range(1, 5)]
        private int mDotCount = 3;

        [Tooltip("Dot is more approach to the second half of the text value.")]
        [SerializeField]
        private bool mApproachSec = false;

        /* Setter & Getter */

        public InputField inputField { get { return this.mInputField; } }
        public string RealText { get { return this.mRealText; } }

        /* Functions */

        private void Awake()
        {
            this.mInputField = this.GetComponent<InputField>();

            RegisterUpdateInputFieldDataEvent();
        }

        private void Start()
        {
            // Update once, if value is already applied.
            UpdateInputFieldData();
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            InputFieldFocusedEvent();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(mUpdateInputField))
            {
                UpdateInputFieldData();
            }
        }
#endif

        /// <summary>
        /// Update the input field data, and store the real 
        /// data to real text data variables.
        /// </summary>
        public void UpdateInputFieldData()
        {
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

            // Check if we need to shortcut this.
            string textData = mInputField.text;

            // If shortcut text already, return it.
            if (IsShortcutText(textData, dot))
                return;

            // Store the real text.
            mRealText = mInputField.text;

            int textDataLen = textData.Length;

            // Check if we need the fold the text.
            if (textDataLen <= mMaxLetters)
                return;

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
            mInputField.text = newTextData;
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

        /// <summary>
        /// Register the event that will automatically update 
        /// the input field data.
        /// </summary>
        private void RegisterUpdateInputFieldDataEvent()
        {
            mInputField.onEndEdit.AddListener(delegate
            {
                UpdateInputFieldData();

                mIsFocus = false;
            });
        }
        
        /// <summary>
        /// Input field focused event.
        /// </summary>
        private void InputFieldFocusedEvent()
        {
            if (!mInputField.isFocused || mIsFocus)
                return;

            mInputField.text = mRealText;

            // Make sure it only do once.
            mIsFocus = true;
        }
    }
}
