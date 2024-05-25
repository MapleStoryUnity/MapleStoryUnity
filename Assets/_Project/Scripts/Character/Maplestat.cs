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
using UnityEngine;

namespace MSU
{
    public class Maplestat
    {
        public enum Id
        {
            SKIN = 0x1, 
            FACE = 0x2, 
            HAIR = 0x4, 
            LEVEL = 0x10, 
            JOB = 0x20,

            STR = 0x40, 
            DEX = 0x80, 
            INT = 0x100, 
            LUK = 0x200,

            HP = 0x400, 
            MAXHP = 0x800, 
            MP = 0x1000, 
            MAXMP = 0x2000,

            AP = 0x4000, 
            SP = 0x8000, 
            EXP = 0x10000,
            FAME = 0x20000,
            MESO = 0x40000,

            PET = 0x180008,
            GACHAEXP = 0x200000,

            LENGTH,
        };

        public Id by_id(uint id)
        {
            if (id >= (uint)Id.LENGTH)
            {
                Debug.LogError("Invalid Maplestat id: " + id);
            }
            return (Id)id;
        }
    }
}
