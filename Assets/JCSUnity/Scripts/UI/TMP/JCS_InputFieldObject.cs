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

        public InputField InputFieldLegacy { get { return this.mInputFieldLegacy; } set { this.mInputFieldLegacy = value; } }
#if TMP_PRO
        public TMP_InputField InputFieldTMP { get { return this.mInputFieldTMP; } set { this.mInputFieldTMP = value; } }
#endif

        public string text
        {
            get
            {
                if (this.mInputFieldLegacy)
                    return this.mInputFieldLegacy.text;
#if TMP_PRO
                return this.InputFieldTMP.text;
#endif
            }

            set
            {
                if (this.mInputFieldLegacy)
                    this.mInputFieldLegacy.text = value;
#if TMP_PRO
                if (this.InputFieldTMP)
                    this.InputFieldTMP.text = value;
#endif
            }
        }
        public bool isFocused
        {
            get
            {
                if (this.mInputFieldLegacy)
                    return this.mInputFieldLegacy.isFocused;
#if TMP_PRO
                return this.InputFieldTMP.isFocused;
#endif
            }
        }

        /* Functions */

    }
}
