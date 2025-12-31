/*
 This file is part of the MapleStory Unity

 Copyright (C) 2021-2026 Shen, Jen-Chieh <jcs090218@gmail.com> 

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

namespace MSU
{
    /// <summary>
    /// 
    /// </summary>
    public static class NetworkConstant
    {
        /* Variables */

        public const int HEADER_LENGTH = 4;
        public const int OPCODE_LENGTH = 2;
        public const int MIN_PACKET_LENGTH = HEADER_LENGTH + OPCODE_LENGTH;
        public const int MAX_PACKET_LENGTH = 131072;

        /* Setter & Getter */

        /* Functions */

    }
}
