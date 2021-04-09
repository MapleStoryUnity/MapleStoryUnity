﻿/*  MapleLib - A general-purpose MapleStory library
 * Copyright (C) 2009, 2010, 2015 Snow and haha01haha01
   
 * This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

 * This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

 * You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapleLib.WzLib
{
	public class WzHeader
	{
        private string ident;
        private string copyright;
        private ulong fsize;
        private uint fstart;

        public string Ident
        {
            get { return ident; }
            set { ident = value; }
        }

        public string Copyright
        {
            get { return copyright; }
            set { copyright = value; }
        }

        public ulong FSize
        {
            get { return fsize; }
            set { fsize = value; }
        }

		public uint FStart 
        {
            get { return fstart; }
            set { fstart = value; }
        }

        public void RecalculateFileStart()
        {
            fstart = (uint)(ident.Length + sizeof(ulong) + sizeof(uint) + copyright.Length + 1);
        }

		public static WzHeader GetDefault()
		{
			WzHeader header = new WzHeader();
			header.ident = "PKG1";
			header.copyright = "Package file v1.0 Copyright 2002 Wizet, ZMS";
			header.fstart = 60;
			header.fsize = 0;
			return header;
		}
	}
}