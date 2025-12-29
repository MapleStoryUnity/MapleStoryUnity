/**
 * $File: JCS_TextObject.cs $
 * $Date: 2022-09-04 01:14:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright � 2022 by Shen, Jen-Chieh $
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

        [Separator("🌱 Initialize Variables (JCS_TextObject)")]

        [Tooltip("Target text renderer.")]
        [SerializeField]
        protected Text mTextLegacy = null;

#if TMP_PRO
        [Tooltip("Target text renderer. (TMP)")]
        [SerializeField]
        protected TMP_Text mTextTMP = null;
#endif

        /* Setter & Getter */

        public Text textLegacy { get { return mTextLegacy; } set { mTextLegacy = value; } }
#if TMP_PRO
        public TMP_Text textTMP { get { return mTextTMP; } set { mTextTMP = value; } }
#endif

        /* Functions */

        public string text
        {
            get 
            { 
                if (mTextLegacy)
                    return mTextLegacy.text;

                return mTextTMP.text;
            }

            set
            {
                if (mTextLegacy) 
                    mTextLegacy.text = value;
#if TMP_PRO
                if (mTextTMP) 
                    mTextTMP.text = value;
#endif
            }
        }
    }
}
