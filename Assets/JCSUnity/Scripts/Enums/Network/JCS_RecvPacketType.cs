/**
 * $File: JCS_RecvPacketType.cs $
 * $Date: 2017-08-25 11:22:06 $
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
    /// Packet handler for receive.
    /// </summary>
    public enum JCS_RecvPacketType 
    {
        // GENERAL
        PING = 0x18,
        HELLO = 0x10,           // NOTE: this does not list it on the recv op.

        // LOGIN
        LOGIN_PASSWORD = 0x01,

        // CHANNEL
        PLAYER_LOGGEDIN = 0x20,
        MOVE_PLAYER = 0x21,
    }
}
