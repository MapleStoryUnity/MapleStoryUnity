/**
 * $File: JCS_EchoGamepadButton.cs $
 * $Date: 2017-10-23 15:23:06 $
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
    /// Echo out with game pad button
    /// </summary>
    public class JCS_EchoGamepadButton : JCS_GamepadButton
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_EchoGamepadButton)")]

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
