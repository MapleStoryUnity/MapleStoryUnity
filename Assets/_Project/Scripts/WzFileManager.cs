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

namespace MSU.Test
{
    /// <summary>
    /// 
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

        [Header("** Runtime Variables (WzFileManager) **")]

        [Tooltip("Path points to wz directory.")]
        [SerializeField]
        private string mPaths = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            instance = this;

            mPaths = Path.Combine(Application.dataPath, mPaths);
        }

        private bool OpenWzFile(string path, WzMapleVersion encVersion, short version, out WzFile file)
        {
            try
            {
                WzFile f = new WzFile(path, version, encVersion);
                wzFiles.Add(f);
                f.ParseWzFile();
                file = f;
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Error initializing " + Path.GetFileName(path) + " (" + e.Message + ").\r\nCheck that the directory is valid and the file is not in use.");
                file = null;
                return false;
            }
        }
    }
}
