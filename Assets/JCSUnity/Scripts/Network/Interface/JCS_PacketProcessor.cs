/**
 * $File: JCS_PacketProcessor.cs $
 * $Date: 2017-08-25 11:18:45 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System;

namespace JCSUnity
{
    /// <summary>
    /// Packet Handler generator.
    /// </summary>
    public abstract class JCS_PacketProcessor
    {
        protected static JCS_PacketProcessor LOGIN_INSTANCE = null;
        protected static JCS_PacketProcessor CHANNEL_INSTANCE = null;

        protected JCS_PacketHandler[] mHandlers = null;
        protected JCS_ClientMode mMode = JCS_ClientMode.LOGIN_SERVER;


        protected JCS_PacketProcessor(JCS_ClientMode mode)
        {
            mMode = mode;
            Initialize();
        }

        /// <summary>
        /// Initialize the packet handlers array list.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        protected void InitPacketHandlersArray<K>()
            where K : struct, IComparable, IFormattable, IConvertible
        {
            if (typeof(K).BaseType != typeof(Enum))
            {
                throw new InvalidCastException();
            }

            int maxRecvOp = 0;
            foreach (K op in JCS_Util.GetValues<K>())
            {
                int opId = Convert.ToInt32(op);

                if (opId > maxRecvOp)
                    maxRecvOp = opId;
            }

            mHandlers = new JCS_PacketHandler[maxRecvOp + 1];

            Reset(mMode);
        }

        /// <summary>
        /// Initialize this packet processor.
        /// </summary>
        public virtual void Initialize()
        {
            InitPacketHandlersArray<JCS_RecvPacketType>();
        }

        /// <summary>
        /// Register the handler to the array list.
        /// </summary>
        /// <param name="code"> code byte/packet id </param>
        /// <param name="handler"> handler want to register. </param>
        public virtual void RegisterHandler(JCS_RecvPacketType code, JCS_PacketHandler handler)
        {
            mHandlers[(int)code] = handler;
        }

        /// <summary>
        /// Reset all the packet handler list by mode.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public abstract void Reset(JCS_ClientMode mode);

        /// <summary>
        /// Get the packet by the packet id.
        /// </summary>
        /// <param name="packetId"> packet id. </param>
        /// <returns> packet handller that handles the packet by the 
        /// packet id. </returns>
        public virtual JCS_PacketHandler GetHandler(short packetId)
        {
            if (packetId > mHandlers.Length)
                return null;

            JCS_PacketHandler handler = mHandlers[packetId];
            if (handler != null)
                return handler;
            return null;
        }

        /// <summary>
        /// Get the list of handlers.
        /// </summary>
        /// <returns> List of handlers </returns>
        public virtual JCS_PacketHandler[] GetHandlers()
        {
            return mHandlers;
        }
    }
}
