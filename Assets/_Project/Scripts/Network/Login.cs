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
using System;
using System.Collections.Generic;

namespace MSU
{
    public class Account
    {
        string name;
        int accid;
        bool female;
        bool muted;
        short pin;
        sbyte gmLevel;
    }

    public class World
    {
        string name;
        string message;
        List<int> chloads;
        byte channelcount;
        byte flag;
        sbyte wid;
    }

    public class StatsEntry
    {
        string name;
        List<int> petids;
        Dictionary<Maplestat.Id, ushort> stats;
        int exp;
        int mapid;
        byte portal;
        Tuple<int, sbyte> rank;
        Tuple<int, sbyte> jobrank;
    }

    public class LookEntry
    {
        bool female;
        byte skin;
        int faceid;
        int hairid;
        Dictionary<int, int> equips;
        Dictionary<int, int> maskedequips;
        List<int> petids;
    }

    public class CharEntry
    {
        StatsEntry stats;
        LookEntry look;
        int cid;
    }
}
