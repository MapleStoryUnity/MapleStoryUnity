/**
 * $File: JCS_SendPacketType.cs $
 * $Date: 2017-08-25 11:23:36 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Packet handle for sending.
    /// </summary>
    public enum JCS_SendPacketType
    {
        // GENERAL
        PONG = 0x11,

        // LOGIN
        LOGIN_STATUS = 0x00,

        // CHANNEL
        MOVE_PLAYER = 0x21,
    }
}
