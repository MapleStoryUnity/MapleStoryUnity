/**
 * $File: JCS_VersionText.cs $
 * $Date: 2023-08-02 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2023 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Display version number.
    /// </summary>
    public class JCS_VersionText : JCS_TextObject
    {
        /* Variables */

        [Separator("🌱 Initialize Variables (JCS_VersionText)")]

        [Tooltip("Format string to show version text.")]
        [SerializeField]
        public string mFormat = "v{0}";

        /* Setter & Getter */

        public string format { get { return mFormat; } set { mFormat = value; } }

        /* Functions */

        private void Awake()
        {
            UpdateVersionNo();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateVersionNo();
        }
#endif

        /// <summary>
        /// Update the version number once.
        /// </summary>
        public void UpdateVersionNo()
        {
            text = string.Format(mFormat, Application.version);
        }
    }
}
