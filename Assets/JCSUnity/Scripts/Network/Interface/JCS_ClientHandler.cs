/**
 * $File: JCS_ClientHandler.cs $
 * $Date: 2017-08-21 16:33:53 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Client Hanlder for receiving the packet.
    /// </summary>
    public interface JCS_ClientHandler
    {
        /// <summary>
        /// Callback when message is sent.
        /// </summary>
        /// <param name="buffer"> buffer we sent. </param>
        void MessageSent(byte[] buffer);

        /// <summary>
        /// Callback when message received.
        /// </summary>
        /// <param name="buffer"> buffer we received. </param>
        void MessageReceived(byte[] buffer);
    }
}
