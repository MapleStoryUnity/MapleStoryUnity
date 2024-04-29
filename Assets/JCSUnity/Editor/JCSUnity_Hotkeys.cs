/**
 * $File: JCSUnity_HotKeys.cs $
 * $Date: 2017-06-04 01:38:14 $
 * $Revision: $
 * $Creator: Abomb - SOURCE: https://forum.unity3d.com/threads/shortcut-key-for-lock-inspector.95815/ $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JCSUnity
{
    /// <summary>
    /// A bunch of useful hotkeys.
    /// </summary>
    public class JCSUnity_Hotkeys
    {
        /* Variables*/

        private const string MI_BaseName = JCSUnity_EditorWindow.MI_BaseName + "/Hotkeys";

        public const int MI_BasePriority = JCSUnity_EditorWindow.MI_BasePriority;

        private static EditorWindow _mouseOverWindow;

        /* Setter & Getter */

        /* Functions */

        [MenuItem(MI_BaseName + "/Select Inspector under mouse cursor (use hotkey) #&q", false, MI_BasePriority + 50)]
        private static void SelectLockableInspector()
        {
            if (EditorWindow.mouseOverWindow.GetType().Name == "InspectorWindow")
            {
                _mouseOverWindow = EditorWindow.mouseOverWindow;
                Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
                Object[] findObjectsOfTypeAll = Resources.FindObjectsOfTypeAll(type);
                int indexOf = findObjectsOfTypeAll.ToList().IndexOf(_mouseOverWindow);
                EditorPrefs.SetInt("LockableInspectorIndex", indexOf);
            }
        }

        [MenuItem(MI_BaseName + "/Clear Console #&c", false, MI_BasePriority + 50)]
        private static void ClearConsole()
        {
            Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditorInternal.LogEntries");
            type.GetMethod("Clear").Invoke(null, null);
        }
    }
}
