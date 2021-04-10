/**
 * $File: JCS_DefaultClientHandler.cs $
 * $Date: 2017-08-21 16:38:46 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace JCSUnity
{
    /// <summary>
    /// Default message handler
    /// </summary>
    public class JCS_DefaultClientHandler
        : JCS_ClientHandler
    {
        /// <summary>
        /// Callback when message is sent.
        /// </summary>
        /// <param name="buffer"> buffer we sent. </param>
        public void MessageSent(byte[] buffer)
        {
            PrintSendPacket(buffer);
        }

        /// <summary>
        /// Callback when message received.
        /// </summary>
        /// <param name="buffer"> buffer we received. </param>
        public void MessageReceived(byte[] buffer)
        {
            // convert byte array to stream
            Stream stream = new MemoryStream(buffer);

            // using byte reader for the stream.
            BinaryReader br = new BinaryReader(stream);
            JCS_BinaryReader jcsbr = new JCS_BinaryReader(br);

            short packetId = jcsbr.ReadShort();
            /**
             * NOTE(jenchieh): packet responded does not need a handler 
             * to handle to response. I think is good if we have it inside 
             * check if the packet get handler, but I think is easier if 
             * just want to check if the packet responded or not. So I 
             * decide to have this function here instead inside the 
             * packet handler check under.
             */
            // packet responded!
            JCS_PacketLostPreventer.instance.AddRespondPacketId(packetId);

            JCS_Client client = JCS_ClientManager.LOCAL_CLIENT;

            if (IsPacketOutdated(jcsbr, client, packetId))
                return;

            // handler depends on the client/server mode.
            JCS_PacketHandler packetHandler = 
                JCS_DefaultPacketProcessor.GetProcessor(
                    JCS_NetworkSettings.instance.CLIENT_MODE
                    ).GetHandler(packetId);

            if (packetHandler != null && packetHandler.validateState(client))
            {
                // set the client and packet data buffer sequence.
                packetHandler.Client = client;
                packetHandler.PacketData = jcsbr;

                // register request.
                JCS_ServerRequestProcessor.instance.RegisterRequest(packetHandler.handlePacket, jcsbr, client);
            }
            else
            {
                if (packetHandler == null)
                    JCS_Debug.Log("Exception during processing packet: null");
                else
                    JCS_Debug.Log("Exception during processing packet: " + packetHandler);
            }
        }

        /// <summary>
        /// Is packet outdated by the packet number.
        /// </summary>
        /// <param name="jcsbr">binary read to read the packet number.</param>
        /// <param name="client">client to check if the packet is out-dated.</param>
        /// <param name="packetId">packet id use to get the packet number.</param>
        /// <returns>true, is out-dated. false, is newest packet.</returns>
        private bool IsPacketOutdated(JCS_BinaryReader jcsbr, JCS_Client client, short packetId)
        {
            // Check if the server need to check the packet No type of server.
            // UDP is one of the protocol type that does need to check
            // the packet by packet number. (order issue)
            if (client.IsOrderCheckServer())
            {
                long packetNumber = jcsbr.ReadLong();
                if (client.GetPacketNumber(packetId) > packetNumber)
                {
                    // No need to do any process, because the packet has been 
                    // to late or already update by another packet.
                    return true;
                }
                else
                {
                    // update packet number
                    client.SetPacketNumber(packetId, packetNumber);
                }
            }
            return false;
        }

        /// <summary>
        /// Print out the message we are going to send.
        /// </summary>
        /// <param name="message"> meesage sending to print. </param>
        private void PrintSendPacket(System.Object message)
        {
#if (UNITY_EDITOR)
            if (!JCS_GameSettings.instance.DEBUG_MODE)
                return;

            byte[] encryptedBuffer = (byte[])message;

            // Print out the buffer for test
            JCS_Logger.Info("HeapBuffer[" + BitConverter.ToString(encryptedBuffer) + "]");
#endif
        }

        /// <summary>
        /// Print out the message we received.
        /// </summary>
        /// <param name="message"> message received to print. </param>
        private void PrintRecievedPacket(System.Object message)
        {
#if (UNITY_EDITOR)
            if (!JCS_GameSettings.instance.DEBUG_MODE)
                return;

            byte[] decryptedBuffer = (byte[])message;

            // Print out the buffer for test
            JCS_Logger.Info("HeapBuffer[" + BitConverter.ToString(decryptedBuffer) + "]");
#endif
        }
    }
}
