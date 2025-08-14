/**
 * $File: JCS_DefaultPacketDecoder.cs $
 * $Date: $
 * $Reveision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Decoder, decode the packet before being use by local end.
    /// </summary>
    public class JCS_DefaultPacketDecoder : JCS_PacketDecoder
    {
        /// <summary>
        /// Decode the buffer by the public key.
        /// </summary>
        /// <param name="message"> buffer to decode. </param>
        /// <returns> decoded message. </returns>
        public System.Object Decode(System.Object message)
        {
            byte[] undecrypted = (byte[])message;

            int packetLength = undecrypted.Length - JCS_NetworkConstant.DECODE_BUFFER_LEN;

            // Check packet length
            if (undecrypted.Length < 0 || undecrypted.Length > JCS_NetworkConstant.INBUFSIZE)
            {
                // TODO(JenChieh): split the packet system
                Debug.LogError("Packet recieved is too big!!!");
                return null;
            }

            // decrypt packet and check if damaged / wrong packet
            for (int index = 0; index < JCS_NetworkConstant.DECODE_BUFFER_LEN; ++index)
            {
                if ((char)undecrypted[index] != (char)JCS_NetworkConstant.DECODE_BUFFER[index])
                {
                    Debug.LogError("Wrong Packet Header!!!");
                    return null;
                }
            }

            // Get the real message
            byte[] decryptedBuffer = new byte[packetLength];
            for (int index = 0; index < packetLength; ++index)
            {
                decryptedBuffer[index] = undecrypted[index + JCS_NetworkConstant.DECODE_BUFFER_LEN];
            }

            return decryptedBuffer;
        }

    }
}
