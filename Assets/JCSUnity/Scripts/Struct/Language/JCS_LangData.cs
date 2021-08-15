/**
 * $File: JCS_LangData.cs $
 * $Date: 2021-02-15 23:30:52 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2021 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Structure for specific language data.
    ///
    /// This data structure contains the following information.
    ///   - Language label for this data structure.
    ///   - String data representation for this language.
    /// </summary>
    [System.Serializable]
    public class JCS_LangData
    {
        /* Variables */

        [Tooltip("Label of this language.")]
        [SerializeField]
        private SystemLanguage mLanguage = SystemLanguage.Unknown;

        [Tooltip("String data for this specific language.")]
        [SerializeField]
        [TextArea]
        private string mData = "";

        /* Setter & Getter */

        public SystemLanguage Language { get { return this.mLanguage; } }
        public string Data { get { return this.mData; } }

        /* Functions */

    }
}
