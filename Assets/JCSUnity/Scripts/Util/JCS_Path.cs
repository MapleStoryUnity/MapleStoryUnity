/**
 * $File: JCS_Path.cs $
 * $Date: 2021-04-11 21:43:26 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using System.IO;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// The utility module for handling path issues.
    /// </summary>
    public static class JCS_Path
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Return the normalized path.
        /// </summary>
        public static string Normalize(string path)
        {
            return path.Replace(
                Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar);
        }

        /// <summary>
        /// Safe way to combine multiple path to one path with slash.
        /// </summary>
        /// <param name="list"> List of path. </param>
        /// <returns> Converted path. </returns>
        public static string Combine(params string[] list)
        {
            string result = list[0];

            for (int index = 1; index < list.Length; ++index)
            {
                string path = list[index];
                result += "/" + path;
            }

            result = Normalize(result);
            result = result.Replace("//", "/");

            return result;
        }

        /// <summary>
        /// Convert a path to asset compatible path.
        /// 
        /// The returned string should start with `Assets/`.
        /// </summary>
        public static string ToAssetPath(string path)
        {
            return path.Replace(Application.dataPath, "Assets");
        }

        /// <summary>
        /// Convert an asset path to data path.
        /// 
        /// This returns an absolute full path.
        /// </summary>
        public static string ToDataPath(string path)
        {
            if (path.StartsWith(Application.dataPath))
                return path;

            return Combine(Application.dataPath, path);
        }
    }
}
