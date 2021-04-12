/**
 * $File: WzMainDirectory.cs $
 * $Date: 2021-04-12 20:42:47 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapleLib.WzLib;

namespace MSU
{
    /// <summary>
    /// 
    /// </summary>
    public class WzMainDirectory
    {
        private WzFile file;
        private WzDirectory directory;

        public WzMainDirectory(WzFile file)
        {
            this.file = file;
            this.directory = file.WzDirectory;
        }

        public WzMainDirectory(WzFile file, WzDirectory directory)
        {
            this.file = file;
            this.directory = directory;
        }

        public WzFile File { get { return file; } }
        public WzDirectory MainDir { get { return directory; } }
    }
}
