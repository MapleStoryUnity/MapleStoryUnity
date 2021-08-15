/**
 * $File: JCS_Packet.cs $
 * $Date: 2017-08-23 12:45:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System;

namespace JCSUnity
{
    /// <summary>
    /// JCSUnity base packet. Instead of making 'byte[]' everywhere use
    /// this class instead make more readable and meaningful.
    /// </summary>
    [Serializable]
    public class JCS_Packet
    {
        private byte[] mData = null;

        public JCS_Packet(byte[] data)
        {
            this.mData = data;
        }

        /// <summary>
        /// Get byte array.
        /// </summary>
        /// <returns> byte array. </returns>
        public byte[] GetBytes()
        {
            return this.mData;
        }
    }
}
