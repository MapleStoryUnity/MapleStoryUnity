/**
 * $File: JCS_EmptyHandler.cs $
 * $Date: 2017-09-04 15:34:13 $
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
    /// Empty Handler, could be any place holder or for signal handler.
    /// </summary>
    public class JCS_EmptyHandler
        : JCS_PacketHandler
    {
        /// <summary>
        /// Handle packet.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="client"></param>
        public override void handlePacket(JCS_BinaryReader br, JCS_Client client)
        {
            // empty..
        }

        /// <summary>
        /// This method validates some general state constrains.
        /// For example that the Client has to be logged in for this
        /// packet.When the method returns false the Client should
        /// be disconnected.Further validation based on the content
        /// of the packet and disconnecting the client if it's invalid
        /// in handlePacket is recommended.
        /// </summary>
        /// <param name="client"> the client </param>
        /// <returns> validate state? </returns>
        public override bool validateState(JCS_Client client)
        {
            // Check if the valid client.
            return true;
        }
    }
}
