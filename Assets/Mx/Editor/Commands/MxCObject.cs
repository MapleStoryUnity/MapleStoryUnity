/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Mx
{
    public class MxCObject : Mx
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        public override bool Enable() { return true; }

        [Interactive(summary: "Collapse GmaeObjects in hierarchy view")]
        public static void CollapseGameObjects()
        {
            EditorWindow hierarchyWindow = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow").GetField("s_LastInteractedHierarchy", MxEditorUtil.STATIC_FLAGS).GetValue(null) as EditorWindow;
            if (hierarchyWindow)
            {
#if UNITY_2018_3_OR_NEWER
                object hierarchyTreeOwner = hierarchyWindow.GetType().GetField("m_SceneHierarchy", MxEditorUtil.INSTANCE_FLAGS).GetValue(hierarchyWindow);
#else
			    object hierarchyTreeOwner = hierarchyWindow;
#endif
                object hierarchyTree = hierarchyTreeOwner.GetType().GetField("m_TreeView", MxEditorUtil.INSTANCE_FLAGS).GetValue(hierarchyTreeOwner);
                if (hierarchyTree != null)
                {
                    List<int> expandedSceneIDs = new List<int>(4);
                    foreach (string expandedSceneName in (IEnumerable<string>)hierarchyTreeOwner.GetType().GetMethod("GetExpandedSceneNames", MxEditorUtil.INSTANCE_FLAGS).Invoke(hierarchyTreeOwner, null))
                    {
                        Scene scene = SceneManager.GetSceneByName(expandedSceneName);
                        if (scene.IsValid())
                            expandedSceneIDs.Add(scene.GetHashCode()); // GetHashCode returns m_Handle which in turn is used as the Scene's instanceID by SceneHierarchyWindow
                    }

                    MxEditorUtil.CollapseTreeViewController(hierarchyWindow, hierarchyTree, (TreeViewState<int>)hierarchyTreeOwner.GetType().GetField("m_TreeViewState", MxEditorUtil.INSTANCE_FLAGS).GetValue(hierarchyTreeOwner), expandedSceneIDs);
                }
            }
        }
    }
}
