/**
 * $File: JCSUnity_EditortUtil.cs $
 * $Date: 2021-04-18 16:35:05 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using System;
using UnityEditor;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Utility for Editor Layout
    /// </summary>
    public static class JCSUnity_EditortUtil
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        public static string FormKey(string name)
        {
            return JCSUnity_EditorWindow.NAME + "." + name;
        }

        public static void CreateGroup(Action func, bool flexibleSpace = false)
        {
            BeginHorizontal(() =>
            {
                BeginVertical(() =>
                {
                    Indent(func);
                });
            },
            flexibleSpace);
        }

        public static void BeginHorizontal(Action func, bool flexibleSpace = false)
        {
            GUILayout.BeginHorizontal();
            if (flexibleSpace) GUILayout.FlexibleSpace();
            func.Invoke();
            GUILayout.EndHorizontal();
        }

        public static void BeginVertical(Action func)
        {
            GUILayout.BeginVertical("box");
            func.Invoke();
            GUILayout.EndVertical();
        }

        public static void Indent(Action func)
        {
            EditorGUI.indentLevel++;
            func.Invoke();
            EditorGUI.indentLevel--;
        }

        public static bool Foldout(bool foldout, string content, Action func, string texName = "")
        {
            Texture tex = FindTexture(texName);

            GUIContent guic = (tex) ? new GUIContent(" " + content, tex) : new GUIContent(content);

            foldout = EditorGUILayout.Foldout(foldout, guic);

            if (foldout)
                CreateGroup(func);

            return foldout;
        }

        /// <summary>
        /// Get a texture from its source filename.
        /// </summary>
        public static Texture FindTexture(string texName)
        {
            Texture tex = (texName == "") ? null : EditorGUIUtility.FindTexture(texName);
            return tex;
        }

        /// <summary>
        /// Spawn prefab object by file path.
        /// </summary>
        public static GameObject Instantiate(string path)
        {
            var asset = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object)) as GameObject;
            var currentPrefab = GameObject.Instantiate(asset);
            return currentPrefab;
        }
    }
}
