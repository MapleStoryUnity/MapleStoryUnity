/**
 * $File: JCS_JSONData.cs $
 * $Date: 2019-10-16 14:26:21 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2019 by Shen, Jen-Chieh $
 */
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace JCSUnity
{
    /// <summary>
    /// Interface to store game data in JSON format.
    /// </summary>
    [System.Serializable]
    public abstract class JCS_JSONData : JCS_AppData
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Save the applicatiion data into JSON file format.
        /// </summary>
        /// <typeparam name="T"> type of the data save. </typeparam>
        /// <param name="filePath"> where to save. </param>
        /// <param name="fileName"> name of the file you want to save. </param>
        public override void Save<T>(string filePath, string fileName)
        {
            string fullFilePath = filePath + fileName;

            Save<T>(fullFilePath);
        }

        /// <summary>
        /// Save the applicatiion data into JSON file format.
        /// </summary>
        /// <typeparam name="T"> type of the data save. </typeparam>
        /// <param name="filePath"> where to save. </param>
        /// <param name="fileName"> name of the file you want to save. </param>
        public override void Save<T>(string fullFilePath)
        {
            string filePath = Path.GetDirectoryName(fullFilePath);

            JCS_IO.CreateDirectory(filePath);

            InitJCSFile();

            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                byte[] info = new UTF8Encoding(true).GetBytes(json);
                stream.Write(info, 0, info.Length);
            }
        }

        /// <summary>
        /// Load the applicatiion data from a directory file path.
        /// </summary>
        /// <typeparam name="T"> type of the applicatiion data you want to load. </typeparam>
        /// <param name="filePath"> file directory, location, path. </param>
        /// <param name="fileName"> name of the file you want to load. </param>
        /// <returns>
        /// Full applicatiion data. If the file does not exists returns 
        /// null references.
        /// </returns>
        public static T LoadFromFile<T>(string filePath, string fileName)
        {
            string fullFilePath = filePath + fileName;

            return LoadFromFile<T>(fullFilePath);
        }

        /// <summary>
        /// Load the applicatiion data from a directory file path.
        /// </summary>
        /// <typeparam name="T"> type of the applicatiion data you want to load. </typeparam>
        /// <param name="fullFilePath"> file path to the file. </param>
        /// <returns>
        /// Full applicatiion data. If the file does not exists returns 
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
                return JsonConvert.DeserializeObject<T>(contents);
            }
        }
    }
}
