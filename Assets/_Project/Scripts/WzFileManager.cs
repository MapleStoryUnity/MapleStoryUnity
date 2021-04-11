/**
 * $File: WzFileManager.cs $
 * $Date: 2021-04-11 16:49:31 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCSUnity;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using MapleLib.WzLib.WzStructure.Data;

namespace MSU
{
    /// <summary>
    /// Manages external Wz files.
    /// </summary>
    public class WzFileManager
        : JCS_Managers<WzFileManager>
    {
        /* Variables */

        private List<WzFile> wzFiles = new List<WzFile>();

        [Header("** Initialize Variables (WzFileManager) **")]

        [Tooltip("List fo .wz file names.")]
        [SerializeField]
        private List<string> mFilenames = null;

        [Tooltip("Wz file encoding version.")]
        [SerializeField]
        private WzMapleVersion mEncVersion = WzMapleVersion.GMS;

        [Tooltip("Version number.")]
        [SerializeField]
        private short mVersion = -1;

        [Header("** Runtime Variables (WzFileManager) **")]

        [Tooltip("Path points to wz directory.")]
        [SerializeField]
        private string mPath = "../wz/";

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            instance = this;

            mPath = JCS_Path.Combine(Application.dataPath, mPath);
        }

        public WzFile OpenWzFile(string name)
        {
            var filename = name + ".wz";
            WzFile f = new WzFile(JCS_Path.Combine(mPath, filename), mVersion, mEncVersion);
            wzFiles.Add(f);
            f.ParseWzFile();
            return f;
        }

        public void UnloadWzFile(WzFile file)
        {
            file.Dispose();
            wzFiles.Remove(file);
        }
    }
}
