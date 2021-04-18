#if (UNITY_EDITOR)
/**
 * $File: JCSUnity_About.cs $
 * $Date: 2017-01-22 05:03:30 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


namespace JCSUnity
{
    /// <summary>
    /// About info window.
    /// </summary>
    public class JCSUnity_About
        : EditorWindow      // TODO(jenchieh): change to normal window.
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

        private void OnGUI()
        {
            ReadINIFile();

            GUILayout.Label("About JCSUnity", EditorStyles.boldLabel);

            // Informations
            GUILayout.Label("Author: " + EDITOR_INI["author"]);
            GUILayout.Label("Email: " + EDITOR_INI["email"]);
            GUILayout.Label("Project Version: " + EDITOR_INI["project_version"]);

            // GUI.Button that is drawn in the Label style.
            if (GUILayout.Button("Source: " + EDITOR_INI["url"], "Label"))
            {
                string url = EDITOR_INI["url"];
                Application.OpenURL(url);
            }
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
        private static void AboutJCSUnity()
        {
            JCSUnity_About window = (JCSUnity_About)GetWindow(typeof(JCSUnity_About));
            window.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
            window.maxSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
            window.Show();
        }
    }
}

#endif
