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
    ///   3. Register to `JCS_AppManager`, its' component will take
    ///      case of the rest.
    /// </summary>
    public class JCS_LangText : JCS_TextObject
    {
        /* Variables */

        [Tooltip("List of languages with translation data.")]
        [SerializeField]
        private JCS_LangDataList mLangData = null;

        /* Setter & Getter */

        public JCS_LangDataList LangData { get { return this.mLangData; } }

        /* Functions */

        private void Start()
        {
            JCS_AppManager.instance.AddLangText(this);
            Refresh();
        }

        /// <summary>
        /// Refresh language text once.
        /// </summary>
        public void Refresh()
        {
            JCS_UIUtil.SetLangText(this.mLangData, this.mTextLegacy);
#if TMP_PRO
            JCS_UIUtil.SetLangText(this.mLangData, this.mTextTMP);
#endif
        }
    }
}
