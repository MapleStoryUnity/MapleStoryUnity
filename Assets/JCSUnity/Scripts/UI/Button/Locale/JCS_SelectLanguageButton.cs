/**
 * $File: JCS_SelectLanguageButton.cs $
 * $Date: 2025-04-15 01:14:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright � 2025 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Button to select the system langauge.
    /// </summary>
    public class JCS_SelectLanguageButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_SelectLanguageButton)")]

        [Tooltip("The newly selected system language.")]
        [SerializeField]
        private SystemLanguage mSystemLanguage = SystemLanguage.Unknown;

        /* Setter & Getter */

        public SystemLanguage systemLanguage { get { return mSystemLanguage; } set { mSystemLanguage = value; } }

        /* Functions */

        public override void OnClick()
        {
            JCS_AppManager.FirstInstance().systemLanguage = mSystemLanguage;
        }
    }
}
