/**
 * $File: JCS_OpenURLGamePadButton.cs $
 * $Date: 2017-10-27 11:50:47 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Open a URL in the default browser. (Game Pad)
    /// </summary>
    public class JCS_OpenURLGamePadButton : JCS_GamePadButton
    {
        /* Variables*/

        [Header("** Runtime Variables (JCS_OpenURLGamePadButton) **")]

        [Tooltip("URL to open in the default browser.")]
        [SerializeField]
        private string mURL = "www.your-url.com";

        /* Setter & Getter */

        public string URL { get { return this.mURL; } set { this.mURL = value; } }

        /* Functions */

        public override void OnClick()
        {
            string url = JCS_Utility.EscapeURL(mURL);
            Application.OpenURL(url);
        }
    }
}
