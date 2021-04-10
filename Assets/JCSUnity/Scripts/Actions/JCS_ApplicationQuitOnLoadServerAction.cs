/**
 * $File: JCS_ApplicationQuitOnLoadServerAction.cs $
 * $Date: 2017-09-03 14:18:53 $
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
    /// Quit the application by confirming the packet from the server.
    /// 
    /// Please use this with UDP, or any other unreliable 
    /// network protocol.
    /// 
    /// USAGE(jenchieh): please use 'JCS_PacketLostPreventer' and 
    /// send the 'client close' packet before the this scene loaded or
    /// else the connection would not be clean for this client. Server
    /// could have the fatal processing performance.
    /// </summary>
    public class JCS_ApplicationQuitOnLoadServerAction
        : MonoBehaviour
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        private void Update()
        {
            // If there are no packet waiting for confirm then meaning
            // the packet has already complete the task for the 'client
            // close' request.
            // 
            // NOTE(jenchieh): normally the length of the packet id from
            // 'JCS_PacketLostPreventer' should always be zero, so this
            // script is the same as the 'JCS_ApplicationQuitOnLoadAction'.
            // But just a simple check before quiting the application 
            // layer.
            if (JCS_PacketLostPreventer.instance.PacketIds.Count != 0)
                return;

            Application.Quit();
        }
    }
}
