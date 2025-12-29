/**
 * $File: JCS_OpenURLButton.cs $
 * $Date: 2017-09-04 17:19:53 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Open a URL in the default browser.
    /// </summary>
    public class JCS_OpenURLButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_OpenURLButton)")]

        [Tooltip("URL to open in the default browser.")]
        [SerializeField]
        private string mURL = "www.your-url.com";

        /* Setter & Getter */

        public string url { get { return mURL; } set { mURL = value; } }

        /* Functions */

        public override void OnClick()
        {
            string url = JCS_Util.EscapeURL(mURL);
            Application.OpenURL(url);
        }
    }
}
