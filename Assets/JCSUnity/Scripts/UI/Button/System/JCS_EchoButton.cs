/**
 * $File: JCS_EchoButton.cs $
 * $Date: 2017-10-07 15:24:48 $
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
    /// Echo out a string on the console window when this
    /// button triggered.
    /// </summary>
    public class JCS_EchoButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_EchoButton)")]

        [Tooltip("String to echo out on the console window.")]
        public string echoString = "echo Hello World!~";

        /* Setter & Getter */

        /* Functions */

        public override void OnClick()
        {
            Debug.Log(echoString);
        }
    }
}
