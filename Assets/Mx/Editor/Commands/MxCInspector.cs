/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using System;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Mx
{
    public class MxCInspector : Mx
    {
        /* Variables */

        private static EditorWindow INSPECTOR_WINDOW;

        /* Setter & Getter */

        /* Functions */

        public override bool Enable() { return true; }

        [Interactive(
            icon: "InspectorLock",
            summary: "Toggle Inspector lock")]
        public static void ToggleInspectorLock()
        {
            Type inspectorWindowType = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");

            if (INSPECTOR_WINDOW == null)
            {
                UnityEngine.Object[] findObjectsOfTypeAll = Resources.FindObjectsOfTypeAll(inspectorWindowType);
                INSPECTOR_WINDOW = (EditorWindow)findObjectsOfTypeAll[0];
            }

            if (INSPECTOR_WINDOW != null && INSPECTOR_WINDOW.GetType().Name == "InspectorWindow")
            {
                PropertyInfo isLockedPropertyInfo = inspectorWindowType.GetProperty("isLocked");
                if (isLockedPropertyInfo == null) return;

                bool value = (bool)isLockedPropertyInfo.GetValue(INSPECTOR_WINDOW, null);
                isLockedPropertyInfo.SetValue(INSPECTOR_WINDOW, !value, null);

                INSPECTOR_WINDOW.Repaint();
            }
        }

        // Copied: https://gist.github.com/yasirkula/0b541b0865eba11b55518ead45fba8fc?permalink_comment_id=2242423
        private static void ExpandComponents(Transform trans, bool expand)
        {
            if (trans == null)
            {
                Debug.Log("No GameObject selected");
                return;
            }

            EditorApplication.ExecuteMenuItem("Window/General/Inspector");

            Component[] comps = trans.GetComponents<Component>();

            foreach (Component comp in comps)
                InternalEditorUtility.SetIsInspectorExpanded(comp, expand);

            ActiveEditorTracker tracker = ActiveEditorTracker.sharedTracker;
            for (int i = 0, length = tracker.activeEditors.Length; i < length; i++)
                tracker.SetVisible(i, (expand) ? 1 : 0);

            EditorWindow.focusedWindow.Repaint();
        }

        [Interactive(summary: "Collapse all components in the inspector")]
        public static void CollapseComponents()
        {
            ExpandComponents(Selection.activeTransform, false);
        }

        [Interactive(summary: "Expand all components in the inspector")]
        public static void ExpandComponents()
        {
            ExpandComponents(Selection.activeTransform, true);
        }
    }
}
