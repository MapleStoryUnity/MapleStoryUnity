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

namespace MSU
{
    /// <summary>
    /// A list of packets to be sent to the server.
    /// </summary>
    public enum SendPacketType
    {
        // GENERIC
        PONG = 24,

        // Login
        LOGIN = 1,
        SERVERLIST_REREQUEST = 4,
        CHARLIST_REQUEST = 5,
        STATUS_REQUEST = 6,
        ACCEPT_TOS = 7,
        SET_GENDER = 8,
        AFTER_LOGIN = 9,
        REGISTER_PIN = 10,
        SERVERLIST_REQUEST = 11,
        SELECT_CHAR = 19,
        PLAYER_LOGIN = 20,
        NAME_CHAR = 21,
        CREATE_CHAR = 22,
        DELETE_CHAR = 23,
        REGISTER_PIC = 29,
        SELECT_CHAR_PIC = 30,

        // Gameplay 1
        CHANGEMAP = 38,
        MOVE_PLAYER = 41,
        CLOSE_ATTACK = 44,
        RANGED_ATTACK = 45,
        MAGIC_ATTACK = 46,
        TAKE_DAMAGE = 48,

        // Messaging
        GENERAL_CHAT = 49,

        // Npc Interaction
        TALK_TO_NPC = 58,
        NPC_TALK_MORE = 60,
        NPC_SHOP_ACTION = 61,

        // Inventory
        GATHER_ITEMS = 69,
        SORT_ITEMS = 70,
        MOVE_ITEM = 71,
        USE_ITEM = 72,
        SCROLL_EQUIP = 86,

        // Player
        SPEND_AP = 87,
        SPEND_SP = 90,

        // Skill
        USE_SKILL = 91,

        // Gameplay 2
        PARTY_OPERATION = 124,
        MOVE_MONSTER = 188,
        PICKUP_ITEM = 202,

        // Custom
        HASH_CHECK = 30000,
    }
}
