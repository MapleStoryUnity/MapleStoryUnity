/**
 * $File: JCS_TextObject.cs $
 * $Date: 2022-09-04 01:14:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2022 by Shen, Jen-Chieh $
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
    /// Holds all type of UI text objects.
    /// </summary>
    public class JCS_TextObject : MonoBehaviour
    {
        /* Variables */

        [Separator("Initialize Variables (JCS_TextObject)")]

        [Tooltip("Target text renderer.")]
        [SerializeField]
        protected Text mTextContainer = null;

#if TMP_PRO
        [Tooltip("Target text renderer. (TMP)")]
        [SerializeField]
        protected TMP_Text mTMP_Text = null;
#endif

        /* Setter & Getter */

        public Text TextContainer { get { return this.mTextContainer; } set { this.mTextContainer = value; } }
#if TMP_PRO
        public TMP_Text TMP_Text { get { return this.mTMP_Text; } set { this.mTMP_Text = value; } }
#endif

        /* Functions */

        public string text
        {
            get { return this.mTextContainer.text; }  // just return one of them
            set
            {
                if (this.mTextContainer) this.mTextContainer.text = value;
#if TMP_PRO
                if (this.mTMP_Text) this.mTMP_Text.text = value;
#endif
            }
        }
    }
}
