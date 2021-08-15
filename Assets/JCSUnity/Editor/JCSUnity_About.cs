#if UNITY_EDITOR
/**
 * $File: JCSUnity_About.cs $
 * $Date: 2017-01-22 05:03:30 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace JCSUnity
{
    /// <summary>
    /// About info window.
    /// </summary>
    public class JCSUnity_About : EditorWindow
    {
        /* Variables*/

        /* all the .ini file located here. */
        public static Dictionary<string, string> EDITOR_INI = new Dictionary<string, string>();

        private static string INI_FILE_PATH = "";
        private static string EDITOR_PROPERTIES_FILENAME = "editor.properties";

        private static int WINDOW_WIDTH = 400;
        private static int WINDOW_HEIGHT = 200;

        /* Setter & Getter */

        /* Functions */

        public JCSUnity_About()
        {
            titleContent = new GUIContent("About JCSUnity");
        }

        private void OnGUI()
        {
            ReadINIFile();

            // Informations
            JCSUnity_EditortUtil.BeginHorizontal(() =>
            {
                GUILayout.Label("Author: ", EditorStyles.boldLabel);
                GUILayout.Label(EDITOR_INI["author"]);
            });

            JCSUnity_EditortUtil.BeginHorizontal(() =>
            {
                GUILayout.Label("Email: ", EditorStyles.boldLabel);
                GUILayout.Label(EDITOR_INI["email"]);
            });

            JCSUnity_EditortUtil.BeginHorizontal(() =>
            {
                GUILayout.Label("Version: ", EditorStyles.boldLabel);
                GUILayout.Label(EDITOR_INI["version"]);
            });

            JCSUnity_EditortUtil.BeginHorizontal(() =>
            {
                GUILayout.Label("Source: ", EditorStyles.boldLabel);

                if (GUILayout.Button(EDITOR_INI["url"], "Label"))
                {
                    string url = EDITOR_INI["url"];
                    Application.OpenURL(url);
                }
            });
        }

        /// <summary>
        /// Read the .ini/.properties file for this editor window.
        /// </summary>
        public static void ReadINIFile()
        {
            INI_FILE_PATH = JCS_Path.Combine(Application.dataPath, "/JCSUnity/Editor/ini/");

            string path = JCS_Path.Combine(INI_FILE_PATH, EDITOR_PROPERTIES_FILENAME);

            EDITOR_INI = JCS_INIFileReader.ReadINIFile(path);
        }

        /// <summary>
        /// About JCSUnity.
        /// </summary>
        [MenuItem("JCSUnity/About", false, 100)]
        public static void AboutJCSUnity()
        {
            var window = CreateInstance<JCSUnity_About>();
            window.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
            window.maxSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
            window.ShowUtility();
        }
    }
}

#endif
