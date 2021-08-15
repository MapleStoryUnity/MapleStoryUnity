/**
 * $File: JCS_GameSocket.cs $
 * $Date: 2017-08-31 16:01:58 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Socket Interface.
    /// </summary>
    public interface JCS_GameSocket
    {
        /// <summary>
        /// Connect to the server.
        /// </summary>
        /// <param name="hostname"> Internet Protocol. </param>
        /// <param name="port"> Port Number. </param>
        void Connect(string hostname, int port);

        /// <summary>
        /// Close the socket the safe way.
        /// </summary>
        void Close();

        /// <summary>
        /// Send the packet to the other end.
        /// </summary>
        /// <param name="buffer"> buffer stream to send. </param>
        bool SendPacket(byte[] buffer);

        /// <summary>
        /// Check if the socket connected?
        /// </summary>
        bool IsConnected();
    }
}
