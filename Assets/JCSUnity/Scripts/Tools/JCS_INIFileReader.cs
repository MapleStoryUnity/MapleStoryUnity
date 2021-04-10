/**
 * $File: JCS_INIFileReader.cs $
 * $Date: 2017-04-14 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// This class help read the .ini/.properties file.
    /// </summary>
    public class JCS_INIFileReader
    {
        /// <summary>
        /// Read the .ini file and return it values.
        /// </summary>
        /// <param name="path_to_file"> path to the file. </param>
        /// <returns> dictionary object with all value in .ini file. </returns>
        public static Dictionary<string, string> ReadINIFile(string path_to_file)
        {
            var data = new Dictionary<string, string>();

            string[] allLine = File.ReadAllLines(path_to_file);
            foreach (var row in allLine)
            {
                // skip the comment.
                if (CheckIfComment(row))
                    continue;

                data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));
            }

            return data;
        }

        /// <summary>
        /// Check the line is a comment.
        /// </summary>
        /// <param name="line"> line to check </param>
        /// <returns> 
        /// true : is a comment line.
        /// false : is data value.
        /// </returns>
        public static bool CheckIfComment(string line)
        {
            for (int index = 0;
                index < line.Length;
                ++index)
            {
                var ch = line[index];

                if (ch != ' ' && ch != '#')
                    return false;

                // check if first character the comment character.
                if (ch == '#')
                    return true;

            }

            return false;
        }

        /// <summary>
        /// Return the dictionary value.
        /// </summary>
        /// <param name="dic"> dictionary object. </param>
        /// <param name="key"> key if the dictionary </param>
        /// <returns> value from dictionary and key. </returns>
        public static string GetDicData(Dictionary<string, string> dic, string key)
        {
            if (dic == null)
                return "null references...";

            return dic[key];
        }

    }
}
