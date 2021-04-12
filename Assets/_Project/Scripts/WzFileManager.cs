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

        private Dictionary<string, WzMainDirectory> mWzDirs = new Dictionary<string, WzMainDirectory>();

        public Dictionary<string, WzFile> mWzFiles = new Dictionary<string, WzFile>();

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

        private bool HasDataFile { get { return File.Exists(WzPath("Data")); } }
        public WzMainDirectory GetMainDirectoryByName(string name) { return mWzDirs[name]; }
        public WzDirectory String { get { return GetMainDirectoryByName("String").MainDir; } }

        /* Functions */

        private void Awake()
        {
            instance = this;

            mPath = JCS_Path.Combine(Application.dataPath, mPath);
        }

        public string WzPath(string name)
        {
            return JCS_Path.Combine(mPath, name + ".wz");
        }

        public WzFile GetWzFile(string name)
        {
            if (!mWzFiles.ContainsKey(name))
                LoadWzFile(name);
            return mWzFiles[name];
        }

        public WzDirectory GetWzDir(string name)
        {
            if (!mWzDirs.ContainsKey(name))
                LoadWzFile(name);
            return mWzDirs[name].MainDir;
        }

        public void UnloadWzFile(WzFile file)
        {
            file.Dispose();
            mWzFiles.Remove(Path.GetFileNameWithoutExtension(file.name));
        }

        private bool LoadWzFile(string name)
        {
            try
            {
                WzFile wzf = new WzFile(WzPath(name), mVersion, mEncVersion);
                wzf.ParseWzFile();
                {
                    mWzFiles[name] = wzf;
                    mWzDirs[name] = new WzMainDirectory(wzf);
                }
                return true;
            }
            catch (Exception e)
            {
                JCS_Debug.LogError("Error initializing " + name + ".wz (" + e.Message + ").\r\nCheck that the directory is valid and the file is not in use.");
                return false;
            }
        }

        private bool LoadDataWzFile(string name)
        {
            try
            {
                WzFile wzf = new WzFile(WzPath(name), mEncVersion);
                wzf.ParseWzFile();
                {
                    mWzFiles[name] = wzf;
                    mWzDirs[name] = new WzMainDirectory(wzf);
                    foreach (WzDirectory mainDir in wzf.WzDirectory.WzDirectories)
                    {
                        mWzDirs[mainDir.Name.ToLower()] = new WzMainDirectory(wzf, mainDir);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                JCS_Debug.LogError("Error initializing " + name + ".wz (" + e.Message + ").\r\nCheck that the directory is valid and the file is not in use.");
                return false;
            }
        }
    }
}
