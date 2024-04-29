/**
 * $File: JCS_AppData.cs $
 * $Date: 2020-07-10 12:15:20 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2020 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Base application data structure.
    /// </summary>
    [System.Serializable]
    public abstract class JCS_AppData
    {
        /* Variables */

        private bool mInitialized = false;

        public string Copyright = "";
        public string Version = "";
        
        /* Setter & Getter */

        /* Functions */

        protected void InitJCSFile()
        {
            if (JCS_PackageDataSettings.instance == null)
            {
                JCS_Debug.LogError("Failed to load the copyright and version text");
                return;
            }

            Copyright = JCS_PackageDataSettings.instance.CopyrightString;
            Version = JCS_PackageDataSettings.instance.VersionString;

            this.mInitialized = true;
        }

        /// <summary>
        /// Return true if data is initialized.
        /// </summary>
        public bool Initialized() 
        { 
            return this.mInitialized; 
        }

        public abstract void Save<T>(string filePath, string fileName);
        public abstract void Save<T>(string fullFilePath);

        /// <summary>
        /// Get complete save data path.
        /// </summary>
        public static string SavePath()
        {
            var apps = JCS_AppSettings.instance;
            string path = JCS_Path.Combine(Application.persistentDataPath, apps.DATA_PATH);
            return path;
        }
    }
}
