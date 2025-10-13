/**
 * $File: JCS_Client.cs $
 * $Date: 2017-08-24 17:12:14 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Please inherent the client object to this interface.
    /// </summary>
    [Serializable]
    public abstract class JCS_Client
    {
        [Separator("Runtime Variables (JCS_Client)")]

        [Tooltip("Current Channel this player in.")]
        [SerializeField]
        protected int mChannel = -1;

        protected long[] mPacketNumbers = null;
        protected bool mLoggedIn = false;

        /// <summary>
        /// Check if the UDP client.
        /// </summary>
        public bool IsOrderCheckServer()
        {
            return (JCS_NetworkSettings.FirstInstance().protocolType == JCS_ProtocalType.UDP);
        }

        /// <summary>
        /// Check if the packet numbers is overflow.
        /// 
        /// Limit the range of the generic data type, prevent 
        /// overflow issue.
        /// </summary>
        /// <param name="handlersLength"></param>
        public void ResetPacketNumbers(int handlersLength)
        {
            this.mPacketNumbers = new long[handlersLength];
            for (int count = 0; count < mPacketNumbers.Length; ++count)
            {
                // all packet number start at -1.
                this.mPacketNumbers[count] = -1;
            }
        }

        /// <summary>
        /// Check if the packet numbers is overflow.
        /// </summary>
        public void ResetPacketNumbers()
        {
            ResetPacketNumbers(GetPacketProcessor().GetHandlers().Length);
        }

        public abstract JCS_PacketProcessor GetPacketProcessor();

        public void SetIsLoggedIn(bool loggedIn)
        {
            if (this.IsLoggedIn() != loggedIn)
            {
                // if not the same val then, meaning the whole packet switch
                // to the new packet processor.
                this.mLoggedIn = loggedIn;
                ResetPacketNumbers(GetPacketProcessor().GetHandlers().Length);
            }
            this.mLoggedIn = loggedIn;
        }

        public bool IsLoggedIn()
        {
            return this.mLoggedIn;
        }

        public long GetPacketNumber(short packetId)
        {
            return this.mPacketNumbers[packetId];
        }

        public void SetPacketNumber(short packetId, long packetNumber)
        {
            this.mPacketNumbers[packetId] = packetNumber;

            /*
             * Check if the packet numbers is overflow.
             * 
             * Limit the range of the generic data type, prevent 
             * overflow issue.
             */
            if (this.mPacketNumbers[packetId] > JCS_NetworkConstant.MAX_PACKET_NUMBER)
                this.mPacketNumbers[packetId] = -1;
        }

        public int Channel { get { return this.mChannel; } set { this.mChannel = value; } }
    }
}
