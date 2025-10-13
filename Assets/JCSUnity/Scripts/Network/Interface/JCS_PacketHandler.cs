/**
 * $File: JCS_PacketHandler.cs $
 * $Date: 2017-08-21 17:47:52 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Base packet handler.
    /// </summary>
    public abstract class JCS_PacketHandler
    {
        private JCS_BinaryReader mPacketData = null;
        private JCS_Client mClient = null;

        public JCS_Client Client { get { return mClient; } set { mClient = value; } }
        public JCS_BinaryReader PacketData { get { return mPacketData; } set { mPacketData = value; } }

        /// <summary>
        /// Handle packet.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="client"></param>
        public abstract void HandlePacket(JCS_BinaryReader br, JCS_Client client);

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
        public abstract bool ValidateState(JCS_Client client);
    }
}
