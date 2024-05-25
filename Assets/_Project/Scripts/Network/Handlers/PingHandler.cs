/*
 This file is part of the MapleStory Unity

 Copyright (C) 2021-2024 Shen, Jen-Chieh <jcs090218@gmail.com> 

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
    public class PingHandler : JCS_PacketHandler
    {
        public override void handlePacket(JCS_BinaryReader br, JCS_Client client)
        {
            throw new System.NotImplementedException();
        }

        public override bool validateState(JCS_Client client)
        {
            throw new System.NotImplementedException();
        }
    }
}
