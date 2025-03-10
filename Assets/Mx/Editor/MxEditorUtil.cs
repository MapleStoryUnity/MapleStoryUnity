/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Mx
{
    public delegate void EmptyFunction();

    public static class MxEditorUtil
    {
        /* Variables */

        public const string KEY = "Mx";

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Construct a key to store `EditorPrefs` registry.
        /// </summary>
        public static string FormKey(string name)
        {
            // XXX: Registery is one instance per history!
            return Application.dataPath + "." + Mx.NAME + "." + name;
        }

        public static void BeginHorizontal(EmptyFunction func, bool flexibleSpace = false)
        {
            GUILayout.BeginHorizontal();
            if (flexibleSpace) GUILayout.FlexibleSpace();
            func.Invoke();
            GUILayout.EndHorizontal();
        }

        public static void BeginVertical(EmptyFunction func)
        {
            GUILayout.BeginVertical("box");
            func.Invoke();
            GUILayout.EndVertical();
        }

        public static void Indent(EmptyFunction func)
        {
            EditorGUI.indentLevel++;
            func.Invoke();
            EditorGUI.indentLevel--;
        }

        public static void LabelField(string text)
        {
            float width = EditorStyles.label.CalcSize(new GUIContent(text)).x;
            EditorGUILayout.LabelField(text, GUILayout.Width(width));
        }

        public static void ResetButton(EmptyFunction func)
        {
            if (!GUILayout.Button("Reset"))
                return;

            func.Invoke();
            MxSettings.data.SavePref();
        }

        /// <summary>
        /// Return all files match with patterns.
        /// </summary>
        public static List<string> GetFiles(params string[] patterns)
        {
            // Remove duplicates, prevent iterate file tree repreatedly!
            patterns = patterns.Distinct().ToArray();

            List<string> paths = new();

            foreach (string pattern in patterns)
            {
                string[] files = Directory.GetFiles("Assets", pattern, SearchOption.AllDirectories);

                paths = paths.Union(files).ToList();
            }

            return paths.ToList();
        }

        /// <summary>
        /// Return a list of file path excludes .meta files.
        /// </summary>
        public static List<string> DefaultFiles(string pat = "*.*")
        {
            return GetFiles(pat)
                .Where(name => !name.EndsWith(".meta"))
                .ToList();
        }

        /// <summary>
        /// Return a list of default components.
        /// </summary>
        private static List<Type> DefaultComponents()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(UnityEngine.Component)))
                .ToList();
        }

        /// <summary>
        /// Return completion data for components.
        /// </summary>
        public static (Dictionary<string, Type>, Dictionary<string, MxItem>) 
            CompletionComponents()
        {
            List<Type> components = DefaultComponents();

            return 
                // Item 1 is used to callback.
                (components.ToDictionary(x => x.ToString(), x => x),
                // Item 2 is the display data.
                components
                .ToDictionary(x => x.ToString(), x => new MxItem(
                    summary: x.Summary(),
                    icon: MxUtil.FindTexture(x))));
        }

        /// <summary>
        /// Find object by type.
        /// </summary>
        public static List<GameObject> FindObjectsByType<T>(FindObjectsSortMode sortMode = FindObjectsSortMode.None)
            where T : UnityEngine.Object
        {
            return UnityEngine.Object.FindObjectsByType<T>(sortMode)
                .Select(x => x.GetComponent<Transform>().gameObject)
                .ToList();
        }
        public static List<GameObject> FindObjectsByType(Type type, FindObjectsSortMode sortMode = FindObjectsSortMode.None)
        {
            return UnityEngine.Object.FindObjectsByType(type, sortMode)
                .Select(x => x.GetComponent<Transform>().gameObject)
                .ToList();
        }

        /// <summary>
        /// Return completion data for GameObjects.
        /// </summary>
        public static (Dictionary<string, GameObject>, Dictionary<string, MxItem>)
            CompletionGameObjects(List<GameObject> objs)
        {
            Dictionary<string, GameObject> dic1 = new();
            Dictionary<string, MxItem> dic2 = new();

            Dictionary<string, int> repeated = new();

            for (int index = 0; index < objs.Count; ++index)
            {
                GameObject obj = objs[index];
                string name = "(" + objs[index].GetInstanceID() + ") " + obj.name;

                if (dic1.ContainsKey(name))
                {
                    ++repeated[name];  // Add count!
                    dic2[name].summary = repeated[name].ToString();

                    continue;
                }

                repeated.Add(name, 1);
                dic1.Add(name, obj);
                dic2.Add(name, new MxItem(
                    summary: repeated[name].ToString(),  // Summary is the repeated count!
                    icon:    MxUtil.FindTexture(obj)));
            }

            return (dic1, dic2);
        }

        #region EditorPrefs
        /// <summary>
        /// Extends `EditorPrefs` to set the list of strings.
        /// </summary>
        public static void SetList(string key, List<string> lst)
        {
            string result = "";

            foreach (string item in lst)
            {
                result += item + ":";
            }

            EditorPrefs.SetString(key, result);
        }

        /// <summary>
        /// Extends `EditorPrefs` to get the list of strings.
        /// </summary>
        public static List<string> GetList(string key)
        {
            string reg = EditorPrefs.GetString(key);

            return reg.Split(":").ToList();
        }
        #endregion

        /// <summary>
        /// Focus a GameObject in scene view.
        /// </summary>
        public static void FocusInSceneView(GameObject obj)
        {
            Selection.activeGameObject = obj;
            SceneView.FrameLastActiveSceneView();
        }

        /// <summary>
        /// Highlight asset by path.
        /// </summary>
        public static void HighlightAsset(string path)
        {
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(path);
        }

        #region TreeView
        public const BindingFlags INSTANCE_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        public const BindingFlags STATIC_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        /// <summary>
        /// Collapse TreeView controller.
        /// </summary>
        public static void CollapseTreeViewController(EditorWindow editorWindow, object treeViewController, TreeViewState treeViewState, IList<int> additionalInstanceIDsToExpand = null)
        {
            object treeViewDataSource = treeViewController.GetType().GetProperty("data", INSTANCE_FLAGS).GetValue(treeViewController, null);
            List<int> treeViewSelectedIDs = new List<int>(treeViewState.selectedIDs);
            int[] additionalInstanceIDsToExpandArray;
            if (additionalInstanceIDsToExpand != null && additionalInstanceIDsToExpand.Count > 0)
            {
                treeViewSelectedIDs.AddRange(additionalInstanceIDsToExpand);

                additionalInstanceIDsToExpandArray = new int[additionalInstanceIDsToExpand.Count];
                additionalInstanceIDsToExpand.CopyTo(additionalInstanceIDsToExpandArray, 0);
            }
            else
                additionalInstanceIDsToExpandArray = new int[0];

            treeViewDataSource.GetType().GetMethod("SetExpandedIDs", INSTANCE_FLAGS).Invoke(treeViewDataSource, new object[] { additionalInstanceIDsToExpandArray });
#if UNITY_2019_1_OR_NEWER
            treeViewDataSource.GetType().GetMethod("RevealItems", INSTANCE_FLAGS).Invoke(treeViewDataSource, new object[] { treeViewSelectedIDs.ToArray() });
#else
		    foreach( int treeViewSelectedID in treeViewSelectedIDs )
			    treeViewDataSource.GetType().GetMethod( "RevealItem", INSTANCE_FLAGS ).Invoke( treeViewDataSource, new object[] { treeViewSelectedID } );
#endif

            editorWindow.Repaint();
        }
        #endregion

        /// <summary>
        /// Copy string to clipboard.
        /// </summary>
        public static void CopyToClipboard(this string str)
        {
            GUIUtility.systemCopyBuffer = str;
        }
    }
}
