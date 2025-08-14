/**
 * $File: JCS_SwitchServerButton.cs $
 * $Date: 2017-09-04 08:00:43 $
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
    /// Button when switching the server. For example, switch 
    /// channel server.
    /// </summary>
    public class JCS_SwitchServerButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_SwtichServerButton)")]

        [Tooltip("Target host name to switch.")]
        [SerializeField]
        private string mHost = "127.0.0.1";

        [Tooltip("Target Port to switch.")]
        [SerializeField]
        private int mPort = 5656;

        [Tooltip("Force to switch server.")]
        [SerializeField]
        private bool mForce = false;

        /* Setter & Getter */

        public int port { get { return mPort; } set { mPort = value; } }
        public string host { get { return mHost; } set { mHost = value; } }
        public bool force { get { return mForce; } set { mForce = value; } }

        /* Functions */

        public override void OnClick()
        {
            // Switch the server.
            JCS_NetworkSettings.FirstInstance().SwitchServer(mHost, mPort, mForce);
        }
    }
}
