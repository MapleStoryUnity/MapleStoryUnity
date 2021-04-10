/**
 * $File: JCS_SwitchServerButton.cs $
 * $Date: 2017-09-04 08:00:43 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Button when switching the server. For example, switch 
    /// channel server.
    /// </summary>
    public class JCS_SwitchServerButton
        : JCS_Button
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_SwtichServerButton) **")]

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

        public int Port { get { return this.mPort; } set { this.mPort = value; } }
        public string Host { get { return this.mHost; } set { this.mHost = value; } }
        public bool Force { get { return this.mForce; } set { this.mForce = value; } }


        /* Functions */

        public override void JCS_OnClickCallback()
        {
            // Switch the server.
            JCS_NetworkSettings.instance.SwitchServer(mHost, mPort, mForce);
        }
    }
}
