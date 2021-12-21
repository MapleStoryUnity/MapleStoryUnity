/**
 * $File: JCS_EchoGamePadButton.cs $
 * $Date: 2017-10-23 15:23:06 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Echo out with game pad button
    /// </summary>
    public class JCS_EchoGamePadButton : JCS_GamePadButton
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_EchoGamePadButton) **")]

        [Tooltip("String to echo out on the console window.")]
        public string echoString = "echo Hello World!~";

        /* Setter & Getter */

        /* Functions */

        public override void OnClick()
        {
            JCS_Debug.Log(echoString);
        }
    }
}
