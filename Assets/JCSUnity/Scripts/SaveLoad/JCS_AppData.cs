/**
 * $File: JCS_AppData.cs $
 * $Date: 2020-07-10 12:15:20 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright ?2020 by Shen, Jen-Chieh $
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
        /// <summary>
        /// Get complete save data path.
        /// </summary>
        public static string SavePath()
        {
            var gs = JCS_GameSettings.instance;
            string path = JCS_Path.Combine(Application.persistentDataPath, gs.DATA_PATH);
            return path;
        }
    }
}
