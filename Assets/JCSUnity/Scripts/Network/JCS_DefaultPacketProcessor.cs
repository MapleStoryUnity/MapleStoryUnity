/**
 * $File: JCS_DefaultPacketProcessor.cs $
 * $Date: 2017-09-01 18:03:49 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Default packet processor. (Demo)
    /// </summary>
    public class JCS_DefaultPacketProcessor : JCS_PacketProcessor
    {
        private JCS_DefaultPacketProcessor(JCS_ClientMode mode)
            : base(mode)
        {
            // empty..
        }

        // singleton
        public static JCS_PacketProcessor GetProcessor(JCS_ClientMode mode)
        {
            if (JCS_ClientMode.LOGIN_SERVER == mode)
            {
                if (LOGIN_INSTANCE == null)
                    LOGIN_INSTANCE = new JCS_DefaultPacketProcessor(mode);
                return LOGIN_INSTANCE;
            }
            else if (JCS_ClientMode.CHANNEL_SERVER == mode)
            {
                if (CHANNEL_INSTANCE == null)
                    CHANNEL_INSTANCE = new JCS_DefaultPacketProcessor(mode);
                return CHANNEL_INSTANCE;
            }
            return null;
        }

        /// <summary>
        /// Reset all the packet handler list by mode.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public override void Reset(JCS_ClientMode mode)
        {
            mHandlers = new JCS_PacketHandler[mHandlers.Length];

            // General
            RegisterHandler(JCS_RecvPacketType.HELLO, new JCS_HelloHandler());

            if (mode == JCS_ClientMode.LOGIN_SERVER)
            {
                // Design here...
            }
            else if (mode == JCS_ClientMode.CHANNEL_SERVER)
            {
                // Design here...
            }
        }

    }
}
