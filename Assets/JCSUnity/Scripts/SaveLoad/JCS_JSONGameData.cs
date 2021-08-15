/**
 * $File: JCS_JSONGameData.cs $
 * $Date: 2019-10-16 14:26:21 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2019 by Shen, Jen-Chieh $
 */
using System.IO;
using System.Text;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Interface to store game data in JSON format.
    /// </summary>
    [System.Serializable]
    public abstract class JCS_JSONGameData : JCS_GameData
    {
        /* Variables */

        public string Copyright = "";
        public string Version = "";

        /* Setter & Getter */

        /* Functions */

        private void InitJCSFile()
        {
            if (JCS_PackageDataSettings.instance == null)
            {
                JCS_Debug.LogError("Failed to load the copyright and version text");
                return;
            }

            Copyright = JCS_PackageDataSettings.instance.CopyrightString;
            Version = JCS_PackageDataSettings.instance.VersionString;
        }

        /// <summary>
        /// Save the game data into xml file format.
        /// </summary>
        /// <typeparam name="T"> type of the data save. </typeparam>
        /// <param name="filePath"> where to save. </param>
        /// <param name="fileName"> name of the file u want to save. </param>
        public void Save<T>(string filePath, string fileName)
        {
            string fullFilePath = filePath + fileName;

            Save<T>(fullFilePath);
        }

        /// <summary>
        /// Save the game data into xml file format.
        /// </summary>
        /// <typeparam name="T"> type of the data save. </typeparam>
        /// <param name="filePath"> where to save. </param>
        /// <param name="fileName"> name of the file u want to save. </param>
        public void Save<T>(string fullFilePath)
        {
            string filePath = Path.GetDirectoryName(fullFilePath);

            JCS_IO.CreateDirectory(filePath);

            InitJCSFile();

            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                string json = JsonUtility.ToJson(this);
                byte[] info = new UTF8Encoding(true).GetBytes(json);
                stream.Write(info, 0, info.Length);
            }
        }

        /// <summary>
        /// Load the game data from a directory file path.
        /// </summary>
        /// <typeparam name="T"> type of the game data u want to load. </typeparam>
        /// <param name="filePath"> file directory, location, path. </param>
        /// <param name="fileName"> name of the file u want to load. </param>
        /// <returns>
        /// Full game data. If the file does not exists returns 
        /// null references.
        /// </returns>
        public static T LoadFromFile<T>(string filePath, string fileName)
        {
            string fullFilePath = filePath + fileName;

            return LoadFromFile<T>(fullFilePath);
        }

        /// <summary>
        /// Load the game data from a directory file path.
        /// </summary>
        /// <typeparam name="T"> type of the game data u want to load. </typeparam>
        /// <param name="fullFilePath"> file path to the file. </param>
        /// <returns>
        /// Full game data. If the file does not exists returns 
        /// null references.
        /// </returns>
        public static T LoadFromFile<T>(string fullFilePath)
        {
            if (!File.Exists(fullFilePath))
                return default(T);

            using (var stream = new FileStream(fullFilePath, FileMode.Open))
            {
                string contents = "";
                using (var sr = new StreamReader(stream))
                {
                    contents = sr.ReadToEnd();
                }
                return JsonUtility.FromJson<T>(contents);
            }
        }
    }
}
