/*
 This file is part of the MapleStory Unity

 Copyright (C) 2021-2022 Shen, Jen-Chieh <jcs090218@gmail.com> 

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Affero General Public License version 3
 as published by the Free Software Foundation. You may not use, modify
 or distribute this program under any other version of the
 GNU Affero General Public License.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Affero General Public License for more details.

 You should have received a copy of the GNU Affero General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using JCSUnity;

namespace MSU
{
    public class PacketProcessor : JCS_PacketProcessor
    {
        private PacketProcessor(JCS_ClientMode mode)
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
                    LOGIN_INSTANCE = new PacketProcessor(mode);
                return LOGIN_INSTANCE;
            }
            else if (JCS_ClientMode.CHANNEL_SERVER == mode)
            {
                if (CHANNEL_INSTANCE == null)
                    CHANNEL_INSTANCE = new PacketProcessor(mode);
                return CHANNEL_INSTANCE;
            }
            return null;
        }

        /// <summary>
        /// Register the handler to the array list.
        /// </summary>
        /// <param name="code"> code byte/packet id </param>
        /// <param name="handler"> handler want to register. </param>
        public void RegisterHandler(RecvPacketType code, JCS_PacketHandler handler)
        {
            mHandlers[(int)code] = handler;
        }

        public override void Reset(JCS_ClientMode mode)
        {
            mHandlers = new JCS_PacketHandler[mHandlers.Length];

            // General
            RegisterHandler(RecvPacketType.PING, new PingHandler());

            if (mode == JCS_ClientMode.LOGIN_SERVER)
            {
                // TODO: ..
            }
            else if (mode == JCS_ClientMode.CHANNEL_SERVER)
            {
                // TODO: ..
            }
        }
    }
}
