/**
 * $File: JCS_DefaultPacketEncoder.cs $
 * $Date: $
 * $Reveision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System;

namespace JCSUnity
{
    /// <summary>
    /// Encoder, just hold the alorigthm to encode the packet before 
    /// being sent to client/server.
    /// </summary>
    public class JCS_DefaultPacketEncoder
        : JCS_PacketEncoder
    {

        /// <summary>
        /// Encode the message before being sent to the other end.
        /// </summary>
        /// <param name="message"> buffer to encode. </param>
        /// <returns> encoded message. </returns>
        public System.Object Encode(System.Object message)
        {
            byte[] unencrypted = (byte[])message;

            int packetLength = JCS_NetworkConstant.ENCODE_BUFFER_LEN + unencrypted.Length;

            // Check packet length
            if (packetLength < 0 || packetLength > JCS_NetworkConstant.OUTBUFSIZE)
            {
                JCS_Debug.LogError("Packet you are sending is too big!");
                return null;
            }

            byte[] encryptedBuffer = new byte[packetLength];

            // encrypt the packet for security usage
            for (int index = 0; index < JCS_NetworkConstant.ENCODE_BUFFER_LEN; ++index)
            {
                encryptedBuffer[index] = JCS_NetworkConstant.ENCODE_BUFFER[index];
            }

            // apply message
            for (int index = JCS_NetworkConstant.ENCODE_BUFFER_LEN; index < packetLength; ++index)
            {
                encryptedBuffer[index] = unencrypted[index - JCS_NetworkConstant.ENCODE_BUFFER_LEN];
            }

            return encryptedBuffer;
        }

    }
}
