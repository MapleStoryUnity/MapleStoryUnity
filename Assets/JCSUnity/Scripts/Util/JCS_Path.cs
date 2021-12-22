/**
 * $File: JCS_Path.cs $
 * $Date: 2021-04-11 21:43:26 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Wrapper for .NET module `Path`.
    /// </summary>
    public static class JCS_Path
    {
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
            result = result.Replace("\\", "/");
            result = result.Replace("//", "/");
            return result;
        }
    }
}
