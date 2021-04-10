#if (UNITY_EDITOR)
/**
 * $File: JCS_PE_Window.cs $
 * $Date: 2017-10-23 13:58:47 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;


namespace PackageExporter
{
    /// <summary>
    /// Package Exporter window.
    /// </summary>
    public class PackageExporterWindow
        : EditorWindow
    {
        public static PackageExporterWindow instance = null;

        private const string PACKAGE_FOLDER = "Assets";
        private const string DEFAULT_PACKAGE_NAME = "Empty Package Name";
        private const string DELIMITER = "_";
        private const string VERSION_SYMBOL = "v";

        private const string IGNORE_FILE_PATH = "PackageExporter/unityignore";
        private const string TEMPLATE_PATH = "PackageExporter/template";

        private const string IGNORE_FILE_EXT = ".unityignore";

        private const string IGNORE_FILE_TEMPLATE_FILE = "template.unityignore";

        private const int TITLE_SPACE_TOP = 3;
        private const int TITLE_SPACE_BOTTOM = 5;

        private const int EXPORT_ALL_PACKAGES_BUTTON_COUNT = 2;

        private const string REPLACE_PACKAGE_NAME = "[PACKAGE_NAME]";
        private const string REPLACE_CREATION_DATE = "[CREATION_DATE]";
        private const string REPLACE_VERSION_NO = "[VERSION_NO]";


        /// <summary>
        /// Structure of the export packages.
        /// </summary>
        [Serializable]
        public struct ExportPackageStruct
        {
            public string packageName;
            public string versionNo;
        };


        public ExportPackageStruct[] exportPackagesList = { };


        private void OnEnable()
        {
            instance = this;
        }

        private void OnGUI()
        {
            OnEP_Editor();
        }

        /// <summary>
        /// Intialize tile editor.
        /// </summary>
        private void OnEP_Editor()
        {
            GUILayout.Space(TITLE_SPACE_TOP);
            GUILayout.Label("** Packages Settings **", EditorStyles.boldLabel);
            GUILayout.Space(TITLE_SPACE_BOTTOM);

            /* Export the whole list. */
            {
                ScriptableObject target = this;
                SerializedObject so = new SerializedObject(target);
                SerializedProperty stringsProperty = so.FindProperty("exportPackagesList");

                EditorGUILayout.PropertyField(stringsProperty, true);
                so.ApplyModifiedProperties();
            }

            GUILayout.Space(TITLE_SPACE_TOP);
            GUILayout.Label("** Unity Ignore File **", EditorStyles.boldLabel);
            GUILayout.Space(TITLE_SPACE_BOTTOM);

            if (GUILayout.Button("Generate Unity Ignore"))
                GenerateUnityIgnoreFiles();

            GUILayout.Space(TITLE_SPACE_TOP);
            GUILayout.Label("** Export Packages **", EditorStyles.boldLabel);
            GUILayout.Space(TITLE_SPACE_BOTTOM);

            int buttonShown = 0;


            for (int index = 0;
               index < instance.exportPackagesList.Length;
               ++index)
            {
                ExportPackageStruct eps = instance.exportPackagesList[index];

                /* GUI Layout */
                string packageName = eps.packageName;
                string versionNo = eps.versionNo;

                string finalPackageName = packageName + DELIMITER + VERSION_SYMBOL + versionNo;

                if (versionNo == "")
                    finalPackageName = packageName;

                if (packageName == "")
                    finalPackageName = DEFAULT_PACKAGE_NAME;

                string ignoreFilePath = Application.dataPath + "/" + IGNORE_FILE_PATH + "/";
                string ignoreFileName = packageName + IGNORE_FILE_EXT;
                string newIgnoreFullPath = ignoreFilePath + ignoreFileName;

                newIgnoreFullPath = newIgnoreFullPath.Replace("\\", "/");

                if (!File.Exists(newIgnoreFullPath))
                    continue;

                string[] ignoreList = ReadAllLinesWithoutComment(newIgnoreFullPath);

                ++buttonShown;

                /* Assign export button. */
                if (GUILayout.Button("Export -> " + finalPackageName))
                    Export(finalPackageName, ignoreList);
            }

            if (buttonShown >= EXPORT_ALL_PACKAGES_BUTTON_COUNT)
            {
                if (GUILayout.Button("Export All Packages"))
                    ExportAllPackages();
            }
        }

        private static void Export(string packageName, string[] ignoreList)
        {
            string ext = "unitypackage";

            string savePath = EditorUtility.SaveFilePanel("Save Unity Packages", "", packageName, ext);

            string[] exportList = GetAllFilesAndDirInPath();
            List<string> finalExportList = new List<string>();

            foreach (string path in exportList)
            {
                string fixedPath = path.Replace("\\", "/");

                if (IgnoreExportPath(fixedPath))
                    continue;

                fixedPath = MakeValidExportPath(fixedPath);

                // check if this path is ignore by the .unityignore file.
                if (MakeIgnore(fixedPath, ignoreList))
                    continue;

                finalExportList.Add(fixedPath);
            }

            if (savePath == "" || finalExportList.Count == 0)
                return;


            AssetDatabase.ExportPackage(
                finalExportList.ToArray(),
                savePath,
                ExportPackageOptions.Default);

            // show it in file explorer. (GUI)
            EditorUtility.RevealInFinder(savePath);
        }

        private static void ExportAllPackages()
        {
            for (int index = 0;
               index < instance.exportPackagesList.Length;
               ++index)
            {
                ExportPackageStruct eps = instance.exportPackagesList[index];

                /* GUI Layout */
                string packageName = eps.packageName;
                string versionNo = eps.versionNo;

                string finalPackageName = packageName + DELIMITER + VERSION_SYMBOL + versionNo;

                if (versionNo == "")
                    finalPackageName = packageName;

                if (packageName == "")
                    finalPackageName = DEFAULT_PACKAGE_NAME;

                string ignoreFilePath = Application.dataPath + "/" + IGNORE_FILE_PATH + "/";
                string ignoreFileName = packageName + IGNORE_FILE_EXT;
                string newIgnoreFullPath = ignoreFilePath + ignoreFileName;

                newIgnoreFullPath = newIgnoreFullPath.Replace("\\", "/");

                if (!File.Exists(newIgnoreFullPath))
                    GenerateUnityIgnoreFiles();

                string[] ignoreList = ReadAllLinesWithoutComment(newIgnoreFullPath);

                Export(finalPackageName, ignoreList);
            }
        }

        private static void GenerateUnityIgnoreFiles()
        {
            string ignoreTemplatePath = Application.dataPath + "/" + TEMPLATE_PATH + "/";
            string ignoreFileTemplatePath = ignoreTemplatePath + IGNORE_FILE_TEMPLATE_FILE;
            string[] templateLines = File.ReadAllLines(ignoreFileTemplatePath);

            for (int index = 0;
                index < instance.exportPackagesList.Length;
                ++index)
            {
                ExportPackageStruct eps = instance.exportPackagesList[index];

                string packageName = eps.packageName;
                string versionNo = eps.versionNo;

                string ignoreFilePath = Application.dataPath + "/" + IGNORE_FILE_PATH + "/";
                string ignoreFileName = packageName + IGNORE_FILE_EXT;
                string newIgnoreFullPath = ignoreFilePath + ignoreFileName;

                ignoreFileTemplatePath = ignoreFileTemplatePath.Replace("\\", "/");
                newIgnoreFullPath = newIgnoreFullPath.Replace("\\", "/");


                if (!File.Exists(newIgnoreFullPath))
                {
                    FileStream fileStream = new FileStream(newIgnoreFullPath,
                                            FileMode.OpenOrCreate,
                                            FileAccess.ReadWrite,
                                            FileShare.None);

                    // make header, date, info, etc.
                    string[] decoratedTemplateLines = MakeDecoration(templateLines, packageName, versionNo);

                    using (StreamWriter sw = new StreamWriter(fileStream))
                    {
                        foreach (string line in decoratedTemplateLines)
                            sw.WriteLine(line);
                    }
                }
            }

            // reset asset database once.
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Replace the template's keyword to proper header.
        /// </summary>
        /// <param name="templateLines"></param>
        /// <param name="packageName"></param>
        /// <param name="versionNo"></param>
        /// <returns></returns>
        private static string[] MakeDecoration(string[] templateLines, string packageName, string versionNo)
        {
            List<string> decoratedTemplate = new List<string>();

            foreach (string line in templateLines)
            {
                string currentLine = line;

                if (currentLine.Contains(REPLACE_PACKAGE_NAME))
                    currentLine = currentLine.Replace(REPLACE_PACKAGE_NAME, packageName);

                if (currentLine.Contains(REPLACE_VERSION_NO))
                    currentLine = currentLine.Replace(REPLACE_VERSION_NO, versionNo);

                if (currentLine.Contains(REPLACE_CREATION_DATE))
                {
                    string dateAndTimeVar = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    currentLine = currentLine.Replace(REPLACE_CREATION_DATE, dateAndTimeVar);
                }

                decoratedTemplate.Add(currentLine);
            }

            return decoratedTemplate.ToArray();
        }

        /// <summary>
        /// Remove all the possible condition that we don't want to export.
        /// </summary>
        /// <returns>
        /// true: ignore it.
        /// false: don't ignore it, we want it to export.
        /// </returns>
        private static bool IgnoreExportPath(string path)
        {
            string ext = Path.GetExtension(path);

            if (ext == ".meta")
                return true;

            return false;
        }

        /// <summary>
        /// Everything we want to export must be under Assets.
        /// </summary>
        /// <returns></returns>
        private static string MakeValidExportPath(string path)
        {
            int index = path.IndexOf(PACKAGE_FOLDER);
            return path.Substring(index, path.Length - index);
        }


        /// <summary>
        /// Check if this path is ignore by the .unityignore file.
        /// </summary>
        /// <param name="path"> Path to check if we ignore this path? </param>
        /// <param name="ignoreList"> Ignore list to check ignore the file? </param>
        /// <returns>
        /// true: ignore it.
        /// false: don't ignore.
        /// </returns>
        private static bool MakeIgnore(string path, string[] ignoreList)
        {
            // NOTE: Here is actually where we compare the path and ignore path.

            // We use this to get rid of the first part of the path.
            // Cuz all path includes `Assets/` infront!
            const string assetPath = "Assets/";
            
            foreach (string ignorePath in ignoreList)
            {
                // Path we use to compare.
                int len = (ignorePath.Length > path.Length) ? path.Length : ignorePath.Length;
                if (path.Length < assetPath.Length + len)
                    len = path.Length - assetPath.Length;
                // NOTE: Get rid of the `Assets/` infront of the path here!
                string pathCompare = path.Substring(assetPath.Length, len);

                if (pathCompare == ignorePath)
                    return true;
            }

            // don't ignore
            return false;
        }


        /// <summary>
        /// About Package Exporter.
        /// </summary>
        [MenuItem("PackageExporter/Window", false, 1)]
        private static void GeneratePackageExporterWindow()
        {
            PackageExporterWindow window = (PackageExporterWindow)GetWindow(typeof(PackageExporterWindow));
            window.Show();
        }

        /// <summary>
        /// Get all the files and turn into string array.
        /// 
        /// SOURCE(jenchieh): https://stackoverflow.com/questions/12332451/list-all-files-and-directories-in-a-directory-subdirectories
        /// </summary>
        /// <returns></returns>
        private static string[] GetAllFilesAndDirInPath()
        {
            return Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);
        }

        /// <summary>
        /// Read all the lines from a text file without the comment character.
        /// </summary>
        /// <param name="path_to_file"></param>
        /// <returns></returns>
        private static string[] ReadAllLinesWithoutComment(string path_to_file)
        {
            string[] allLine = File.ReadAllLines(path_to_file);

            List<string> cleanLine = new List<string>();

            for (int count = 0;
                count < allLine.Length;
                ++count)
            {
                string line = allLine[count];

                // ignore comment.
                if (CheckIfComment(line))
                    continue;

                cleanLine.Add(line);
            }

            return cleanLine.ToArray();
        }

        /// <summary>
        /// Check the line is a comment.
        /// </summary>
        /// <param name="line"> line to check </param>
        /// <returns> 
        /// true : is a comment line / ignore it.
        /// false : is data value.
        /// </returns>
        public static bool CheckIfComment(string line)
        {
            if (line == "")
                return true;

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
    }
}

#endif
