/**
 * $File: JCS_DropdownObject.cs $
 * $Date: 2025-04-15 01:14:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2025 by Shen, Jen-Chieh $
 */

/* NOTE: If you are using `TextMesh Pro` uncomment this line.
 */
#define TMP_PRO

using UnityEngine;
using UnityEngine.UI;
using MyBox;

#if TMP_PRO
using TMPro;
#endif

namespace JCSUnity
{
    /// <summary>
    /// Holds all type of UI input field objects.
    /// </summary>
    public class JCS_InputFieldObject : MonoBehaviour
    {
        /* Variables */

        [Separator("Initialize Variables (JCS_InputFieldObject)")]

        [Tooltip("Target input field renderer.")]
        [SerializeField]
        protected InputField mInputFieldLegacy = null;

#if TMP_PRO
        [Tooltip("Target input field renderer. (TMP)")]
        [SerializeField]
        protected TMP_InputField mInputFieldTMP = null;
#endif

        /* Setter & Getter */

        public InputField InputFieldLegacy { get { return mInputFieldLegacy; } set { mInputFieldLegacy = value; } }
#if TMP_PRO
        public TMP_InputField InputFieldTMP { get { return mInputFieldTMP; } set { mInputFieldTMP = value; } }
#endif

        public string text
        {
            get
            {
                if (mInputFieldLegacy)
                    return mInputFieldLegacy.text;
#if TMP_PRO
                return mInputFieldTMP.text;
#endif
            }

            set
            {
                if (mInputFieldLegacy)
                    mInputFieldLegacy.text = value;
#if TMP_PRO
                if (mInputFieldTMP)
                    mInputFieldTMP.text = value;
#endif
            }
        }
        public bool isFocused
        {
            get
            {
                if (mInputFieldLegacy)
                    return mInputFieldLegacy.isFocused;
#if TMP_PRO
                return mInputFieldTMP.isFocused;
#endif
            }
        }

        /* Functions */

    }
}
