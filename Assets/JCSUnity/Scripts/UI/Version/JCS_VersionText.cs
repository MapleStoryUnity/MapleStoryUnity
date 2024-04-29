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

        [Separator("Initialize Variables (JCS_VersionText)")]

        [Tooltip("Format string to show version text.")]
        [SerializeField]
        public string mFormat = "v{0}";

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            UpdateVersionNo();
        }

        /// <summary>
        /// Update the version number once.
        /// </summary>
        public void UpdateVersionNo()
        {
            this.text  = string.Format(mFormat, Application.version);
        }
    }
}
