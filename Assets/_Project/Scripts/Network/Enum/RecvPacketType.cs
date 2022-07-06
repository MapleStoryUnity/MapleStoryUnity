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
    public enum RecvPacketType
    {
        // Generic
        PING = 17,

        // Login 1
        LOGIN_RESULT = 0,
        SERVERLIST = 10,
        CHARLIST = 11,
        SERVER_IP = 12,
        CHARNAME_RESPONSE = 13,
        ADD_NEWCHAR_ENTRY = 14,
        DELCHAR_RESPONSE = 15,

        // Player 1
        APPLY_BUFF = 20,

        // Login 2
        SELECT_WORLD = 26,
        RECOMMENDED_WORLDS = 27,

        // Inventory 1
        MODIFY_INVENTORY = 29,

        // Player 2
        CHANGE_STATS = 31,
        GIVE_BUFF = 32,
        CANCEL_BUFF = 33,
        RECALCULATE_STATS = 35,
        UPDATE_SKILL = 36,

        // Messaging 1
        SHOW_STATUS_INFO = 39,
        MEMO_RESULT = 41,
        ENABLE_REPORT = 47,

        //Inventory 2
        GATHER_RESULT = 52,
        SORT_RESULT = 53,

        // Player 3
        UPDATE_GENDER = 58,
        BUDDY_LIST = 63,
        GUILD_OPERATION = 65,

        // Messaging 2
        SERVER_MESSAGE = 68,
        WEEK_EVENT_MESSAGE = 77,

        FIELD_SET_VARIABLE = 92,
        FAMILY_PRIV_LIST = 100,
        CANCEL_RENAME_BY_OTHER = 120,
        SCRIPT_PROGRESS_MESSAGE = 122,
        RECEIVE_POLICE = 123,
        SKILL_MACROS = 124,
        SET_FIELD = 125,
        FIELD_EFFECT = 138,
        CLOCK = 147,

        // Mapobject
        SPAWN_CHAR = 160,
        REMOVE_CHAR = 161,

        // Messaging
        CHAT_RECEIVED = 162,
        SCROLL_RESULT = 167,

        // Mapobject
        SPAWN_PET = 168,
        CHAR_MOVED = 185,

        // Attack
        ATTACKED_CLOSE = 186,
        ATTACKED_RANGED = 187,
        ATTACKED_MAGIC = 188,

        SHOW_ITEM_EFFECT = 194,
        SHOW_CHAIR = 196,
        UPDATE_CHARLOOK = 197,
        SHOW_FOREIGN_EFFECT = 198,
        SHOW_ITEM_GAIN_INCHAT = 206, // this is terribly named
        LOCK_UI = 221,
        TOGGLE_UI = 222,

        // Player
        ADD_COOLDOWN = 234,

        // Mapobject
        SPAWN_MOB = 236,
        KILL_MOB = 237,
        SPAWN_MOB_C = 238,
        MOB_MOVED = 239,
        MOVE_MOB_RESPONSE = 240,
        SHOW_MOB_HP = 250,
        SPAWN_NPC = 257,
        SPAWN_NPC_C = 259,
        MAKE_NPC_SCRIPTED = 263,
        DROP_LOOT = 268,
        REMOVE_LOOT = 269,
        SPAWN_REACTOR = 279,
        REMOVE_REACTOR = 280,

        // NPC Interaction
        NPC_DIALOGUE = 304,
        OPEN_NPC_SHOP = 305,

        KEYMAP = 335,
    }
}
