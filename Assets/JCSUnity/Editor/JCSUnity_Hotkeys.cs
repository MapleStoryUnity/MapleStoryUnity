#if UNITY_EDITOR
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
        private static EditorWindow _mouseOverWindow;

        [MenuItem("JCSUnity/Hotkeys/Select Inspector under mouse cursor (use hotkey) #&q", false, 50)]
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

        [MenuItem("JCSUnity/Hotkeys/Toggle Lock &q", false, 50)]
        private static void ToggleInspectorLock()
        {
            if (_mouseOverWindow == null)
            {
                if (!EditorPrefs.HasKey("LockableInspectorIndex"))
                    EditorPrefs.SetInt("LockableInspectorIndex", 0);
                int i = EditorPrefs.GetInt("LockableInspectorIndex");

                Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
                Object[] findObjectsOfTypeAll = Resources.FindObjectsOfTypeAll(type);
                _mouseOverWindow = (EditorWindow)findObjectsOfTypeAll[i];
            }

            if (_mouseOverWindow != null && _mouseOverWindow.GetType().Name == "InspectorWindow")
            {
                Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
                PropertyInfo propertyInfo = type.GetProperty("isLocked");
                bool value = (bool)propertyInfo.GetValue(_mouseOverWindow, null);
                propertyInfo.SetValue(_mouseOverWindow, !value, null);
                _mouseOverWindow.Repaint();
            }
        }

        [MenuItem("JCSUnity/Hotkeys/Clear Console #&c", false, 50)]
        private static void ClearConsole()
        {
            Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditorInternal.LogEntries");
            type.GetMethod("Clear").Invoke(null, null);
        }
    }
}
#endif
