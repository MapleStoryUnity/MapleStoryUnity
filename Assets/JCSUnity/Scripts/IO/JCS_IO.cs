/**
 * $File: JCS_IO.cs $
 * $Date: 2018-08-15 21:30:47 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// IO utilities.
    /// </summary>
    public static class JCS_IO
    {
        /// <summary>
        /// Check if the path is a directory.
        /// </summary>
        /// <param name="path"> path to check. </param>
        /// <returns> 
        /// true, is a directory.
        /// false, is a path.
        /// </returns>
        public static bool IsDirectory(string path)
        {
            // If the target file/directory is not exists will 
            // cause error. Defense it!
            if (!IsFileOrDirectoryExists(path))
            {
                // If file/directory not even exists, don't even 
                // argue the file state!
                return false;
            }

            FileAttributes attr = File.GetAttributes(path);

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                return true;

            return false;
        }

        /// <summary>
        /// Check if the path is a file.
        /// </summary>
        /// <param name="path"> path to check. </param>
        /// <returns>
        /// true, is a file.
        /// false, is a path.
        /// </returns>
        public static bool IsFile(string path)
        {
            // If the target file/directory is not exists will 
            // cause error. Defense it!
            if (!IsFileOrDirectoryExists(path))
            {
                // If file/directory not even exists, don't even 
                // argue the file state!
                return false;
            }

            // If not a directory then is a path.
            return !IsDirectory(path);
        }

        /// <summary>
        /// Check if the path is the file attribute state.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fa"> file attribute. </param>
        /// <returns>
        /// true, is the 'fa' state.
        /// false, vice versa.
        /// </returns>
        public static bool IsFileState(string path, FileAttributes fa)
        {
            // If the target file/directory is not exists will 
            // cause error. Defense it!
            if (!IsFileOrDirectoryExists(path))
            {
                // If file/directory not even exists, don't even 
                // argue the file state!
                return false;
            }

            FileAttributes attr = File.GetAttributes(path);

            if ((attr & fa) == fa)
                return true;

            return false;
        }

        /// <summary>
        /// Check if path either directory or file exists.
        /// </summary>
        /// <param name="path"> file path to check. </param>
        /// <returns>
        /// true, exists. false, not exists.
        /// </returns>
        public static bool IsFileOrDirectoryExists(string path)
        {
            return (Directory.Exists(path) || File.Exists(path));
        }

        /// <summary>
        /// Create directory the safe way.
        /// </summary>
        /// <param name="path"> Path you want to create. </param>
        /// <returns>
        /// Return true, if created; else return false.
        /// </returns>
        public static bool CreateDirectory(string path)
        {
            if (IsFileOrDirectoryExists(path))
                return false;
            Directory.CreateDirectory(path);
            return true;
        }
    }
}
