/**
 * $File: JCS_AppData.cs $
 * $Date: 2020-07-10 12:15:20 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2020 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Base application data structure.
    /// </summary>
    [Serializable]
    public abstract class JCS_AppData
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_AppData)")]

        [Tooltip("The copyright information.")]
        public string copyright = "";

        [Tooltip("The resource version.")]
        public string version = "";

        [Tooltip("Set to true when the data is initialized.")]
        [SerializeField]
        [ReadOnly]
        private bool mInitialized = false;

        /* Setter & Getter */

        /* Functions */

        protected JCS_AppData()
        {
            InitFile();
        }

        protected void InitFile()
        {
            var pds = JCS_PackageDataSettings.FirstInstance();

            if (pds == null)
                return;

            copyright = pds.copyright;
            version = pds.version;

            // Set init flag.
            mInitialized = true;
        }

        /// <summary>
        /// Return true if the data is initialized and ready to use.
        /// </summary>
        public bool Initialized()
        {
            return mInitialized;
        }

        /// <summary>
        /// Save the data.
        /// </summary>
        public abstract void Save<T>(string filePath, string fileName);
        public abstract void Save<T>(string fullFilePath);

        /// <summary>
        /// Return the complete saved data path.
        /// </summary>
        public static string SavePath()
        {
            var apps = JCS_AppSettings.FirstInstance();
            string path = JCS_Path.Combine(Application.persistentDataPath, apps.dataPath);
            return path;
        }
    }
}
