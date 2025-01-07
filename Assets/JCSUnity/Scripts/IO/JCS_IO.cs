/**
 * $File: JCS_IO.cs $
 * $Date: 2018-08-15 21:30:47 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.IO;

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

        /// <summary>
        /// Method to do search directory and get the last file index.
        /// </summary>
        /// <param name="path"> path to search index. </param>
        /// <param name="prefixStr"> Filen name prefix. </param>
        /// <param name="ext"> Filen name extension. </param>
        /// <returns></returns>
        public static int LastFileIndex(string path, string prefixStr, string ext)
        {
            CreateDirectory(path);

            var gs = JCS_GameSettings.instance;

            string fileName = "";
            string curExt = "";
            int last_saved_screenshot = -1;

            foreach (string file in Directory.GetFiles(path))
            {
                fileName = Path.GetFileNameWithoutExtension(file);
                curExt = Path.GetExtension(file);

                // check if is the .png file 
                // (screen shot can only be image file)
                if (!curExt.Equals(ext))
                    continue;

                int index = fileName.IndexOf(prefixStr);
                int len = prefixStr.Length;
                string startOfString = fileName.Substring(0, index);
                string endOfString = fileName.Substring(index + len);
                string cleanPath = startOfString + endOfString;

                last_saved_screenshot = System.Int32.Parse(cleanPath);
            }

            return last_saved_screenshot;
        }

        /// <summary>
        /// Delete all files in directory.
        /// </summary>
        /// <param name="dirPath"> Target delete directory. </param>
        public static void DeleteAllFilesFromDir(string dirPath)
        {
            DirectoryInfo di = new DirectoryInfo(dirPath);

            foreach (FileInfo file in di.GetFiles())
                file.Delete();
        }
    }
}
