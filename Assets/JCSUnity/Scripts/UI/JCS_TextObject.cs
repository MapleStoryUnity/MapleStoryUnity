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

        [Header("** Initialize Variables (JCS_TextObject) **")]

        [Tooltip("Target text renderer.")]
        [SerializeField]
        protected Text mTextContainer = null;

#if TMP_PRO
        [Tooltip("Target text renderer. (TMP)")]
        [SerializeField]
        protected TextMeshPro mTextMesh = null;
#endif

        /* Setter & Getter */

        public Text TextContainer { get { return this.mTextContainer; } set { this.mTextContainer = value; } }
#if TMP_PRO
        public TextMeshPro TextMesh { get { return this.mTextMesh; } set { this.mTextMesh = value; } }
#endif

        /* Functions */

        public string text
        {
            get { return this.mTextContainer.text; }  // just return one of them
            set
            {
                if (this.mTextContainer) this.mTextContainer.text = value;
#if TMP_PRO
                if (this.mTextMesh) this.mTextMesh.text = value;
#endif
            }
        }
    }
}
