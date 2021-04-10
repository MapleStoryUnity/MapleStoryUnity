/**
 * $File: JCS_PacketLostPreventer.cs $
 * $Date: 2017-09-02 16:57:26 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JCSUnity
{
    /// <summary>
    /// Prevent the packet have been lost. If the packet is lost we
    /// keep tracking the packet and resend the responed until the 
    /// server's response had arrived. This usually deal with the 
    /// UDP type communication.
    /// </summary>
    public class JCS_PacketLostPreventer
        : JCS_Settings<JCS_PacketLostPreventer>
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_PacketLostPreventer) **")]

        [Tooltip("How fast the packet resend?")]
        [SerializeField] [Range(0.0f, 1.0f)]
        private float mResendTime = 0.001f;

        private float mResendTimer = 0.0f;

        [Tooltip("Packet's ID that are still being process.")]
        [SerializeField]
        private List<short> mWaitingPacketIds = new List<short>();

        private List<JCS_Packet> mWaitingPackets = new List<JCS_Packet>();

        private HashSet<short> mRespondPacketIds = new HashSet<short>();

        /* Setter & Getter */

        public List<JCS_Packet> Packets { get { return this.mWaitingPackets; } set { this.mWaitingPackets = value; } }
        public List<short> PacketIds { get { return this.mWaitingPacketIds; } set { this.mWaitingPacketIds = value; } }
        public HashSet<short> RespondPacketIds { get { return this.mRespondPacketIds; } set { this.mRespondPacketIds = value; } }

        /* Functions */

        private void Awake()
        {
            instance = CheckSingleton(instance, this);
        }

        private void LateUpdate()
        {
            TrackPacket();
        }

        /// <summary>
        /// Track this packet. If the packet does not responed.
        /// keep sending the packet until the packet responed by the 
        /// server.
        /// 
        /// ATTENTION(jenchieh): Use this function carefully, if you
        /// pass in the wrong respond packet id. Packet will be track
        /// forever until the program terminated.
        /// </summary>
        /// <param name="packet"> Packet to send over time in order to track. </param>
        /// <param name="respondPacketId"> Packet id to check to stop the responsed. </param>
        public void AddTrack(JCS_Packet packet, short respondPacketId)
        {
            mWaitingPackets.Add(packet);
            mWaitingPacketIds.Add(respondPacketId);
        }

        /// <summary>
        /// Track this packet. If the packet does not responed.
        /// keep sending the packet until the packet responed by the 
        /// server.
        /// 
        /// ATTENTION(jenchieh): Use this function carefully, if you
        /// pass in the wrong respond packet id. Packet will be track
        /// forever until the program terminated.
        /// </summary>
        /// <param name="packet"> Packet to send over time in order to track. </param>
        /// <param name="respondPacketId"> Packet id to check to stop the responsed. </param>
        public void AddTrack(JCS_Packet packet, JCS_RecvPacketType respondPacketId)
        {
            AddTrack(packet, (short)respondPacketId);
        }

        /// <summary>
        /// Track this packet. If the packet does not responed.
        /// keep sending the packet until the packet responed by the 
        /// server.
        /// 
        /// ATTENTION(jenchieh): Use this function carefully, if you
        /// pass in the wrong respond packet id. Packet will be track
        /// forever until the program terminated.
        /// </summary>
        /// <param name="packet"> Packet to send over time in order to track. </param>
        /// <param name="respondPacketId"> Packet id to check to stop the responsed. </param>
        public void AddTrack<K>(JCS_Packet packet, K respondPacketId)
            where K : struct, IComparable, IFormattable, IConvertible
        {
            AddTrack(packet, Convert.ToInt16(respondPacketId));
        }

        /// <summary>
        /// If the packet have respond from the server, call this 
        /// function in order to stop tracking the packet.
        /// </summary>
        /// <param name="respondPacketId"></param>
        public void AddRespondPacketId(short respondPacketId)
        {
            mRespondPacketIds.Add(respondPacketId);
        }

        /// <summary>
        /// Check if this preventer still preventing any data
        /// packet lost issue.
        /// </summary>
        public bool IsPreventing()
        {
            return (mWaitingPackets.Count != 0);
        }

        /// <summary>
        /// Clear all tracking packets.
        /// </summary>
        public void ClearTracking()
        {
            mWaitingPacketIds.Clear();
            mWaitingPackets.Clear();
            mRespondPacketIds.Clear();
        }

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected override void TransferData(JCS_PacketLostPreventer _old, JCS_PacketLostPreventer _new)
        {
            _new.Packets = _old.Packets;
            _new.PacketIds = _old.PacketIds;
            _new.RespondPacketIds = _old.RespondPacketIds;

        }

        /// <summary>
        /// Track each packet, if need to resend packet. Send it.
        /// </summary>
        private void TrackPacket()
        {
            if (JCS_NetworkSettings.GetGameSocket() != null &&
                !JCS_NetworkSettings.GetGameSocket().IsConnected())
                return;

            mResendTimer += Time.deltaTime;

            if (mResendTimer < mResendTime)
                return;

            // reset timer.
            mResendTimer = 0;


            /* First check if the packet responded? Remove it if 
             already responded. */
            {
                for (int index = 0; index < mWaitingPacketIds.Count; ++index)
                {
                    JCS_Packet packet = mWaitingPackets[index];
                    short packetId = mWaitingPacketIds[index];

                    if (mRespondPacketIds.Contains(packetId))
                    {
                        // responded, so remove it.
                        mWaitingPacketIds.Remove(packetId);
                        mWaitingPackets.Remove(packet);
                    }
                }
            }

            // cleanup the respond packet id.
            mRespondPacketIds.Clear();

            /* Resend the packet if necessary. */
            for (int index = 0; index < mWaitingPackets.Count; ++index)
            {
                JCS_Packet packet = mWaitingPackets[index];

                // re-send the packet
                JCS_NetworkSettings.GetGameSocket().SendPacket(packet.GetBytes());
            }
        }
        
    }
}
