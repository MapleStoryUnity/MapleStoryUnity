/**
 * $File: JCS_CodecFactory.cs $
 * $Date: $
 * $Reveision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Factory generate the packet decodder and encoder.
    /// </summary>
    public class JCS_CodecFactory
    {
        /* Variables */

        private static JCS_CodecFactory instance = null;

        private JCS_PacketDecoder mDecoder = null;
        private JCS_PacketEncoder mEncoder = null;

        /* Setter & Getter */

        /* Functions */

        private JCS_CodecFactory()
        {
            // Create Decoder and Encoder
            this.mDecoder = new JCS_DefaultPacketDecoder();
            this.mEncoder = new JCS_DefaultPacketEncoder();
        }

        public static JCS_CodecFactory GetInstance()
        {
            // singleton.
            if (instance == null)
                instance = new JCS_CodecFactory();
            return instance;
        }

        public JCS_PacketEncoder GetEncoder() { return this.mEncoder; }
        public JCS_PacketDecoder GetDecoder() { return this.mDecoder; }

    }
}
