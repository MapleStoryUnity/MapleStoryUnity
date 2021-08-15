/**
 * $File: JCS_LangText.cs $
 * $Date: 2021-02-15 23:25:57 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2021 by Shen, Jen-Chieh $
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
    /// Text for multiple languages support.
    ///
    /// This component should contains the following functionalities.
    ///
    ///   1. Multiple text object support including `Text Mesh Pro`.
    ///
    ///   2. A list of language definition for correctly displays the target
    ///      system lanauge.
    ///
    ///   3. Register to `JCS_ApplicationManager`, its' component will take
    ///      case of the rest.
    /// </summary>
    public class JCS_LangText : MonoBehaviour
    {
        /* Variables */

        [Header("** Initialize Variables (JCS_LangText) **")]

        [Tooltip("Text to display lang data.")]
        [SerializeField]
        private Text mText = null;

#if TMP_PRO
        [Tooltip("Text to display lang data.")]
        [SerializeField]
        private TextMeshPro mTextMesh = null;
#endif

        [Tooltip("List of languages with translation data.")]
        [SerializeField]
        private JCS_LangDataList mLangData = null;

        /* Setter & Getter */

        public Text text { get { return this.mText; } }
#if TMP_PRO
        public TextMeshPro TextMesh { get { return this.mTextMesh; } }
#endif
        public JCS_LangDataList LangData { get { return this.mLangData; } }

        /* Functions */

        private void Start()
        {
            JCS_ApplicationManager.instance.AddLangText(this);
            Refresh();
        }

        /// <summary>
        /// Refresh language text once.
        /// </summary>
        public void Refresh()
        {
            JCS_GUIUtil.SetLangText(this.mLangData, this.mText);
#if TMP_PRO
            JCS_GUIUtil.SetLangText(this.mLangData, this.mTextMesh);
#endif
        }
    }
}
