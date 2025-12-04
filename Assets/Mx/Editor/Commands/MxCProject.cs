/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace Mx
{
    public class MxCProject : Mx
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        public override bool Enable() { return true; }

        [Interactive(
            icon: "d_FolderEmpty Icon",
            summary: "Show project path in the file browser")]
        public static void OpenProjectPath()
        {
            EditorUtility.RevealInFinder(Application.dataPath);
        }

        [Interactive(summary: "Collapse folders in Project Browser")]
        public static void CollapseFolders()
        {
            EditorWindow projectWindow = typeof(EditorWindow).Assembly.GetType("UnityEditor.ProjectBrowser").GetField("s_LastInteractedProjectBrowser", MxEditorUtil.STATIC_FLAGS).GetValue(null) as EditorWindow;
            if (projectWindow)
            {
                object assetTree = projectWindow.GetType().GetField("m_AssetTree", MxEditorUtil.INSTANCE_FLAGS).GetValue(projectWindow);
                if (assetTree != null)
                    MxEditorUtil.CollapseTreeViewController(projectWindow, assetTree, (TreeViewState<int>)projectWindow.GetType().GetField("m_AssetTreeState", MxEditorUtil.INSTANCE_FLAGS).GetValue(projectWindow));

                object folderTree = projectWindow.GetType().GetField("m_FolderTree", MxEditorUtil.INSTANCE_FLAGS).GetValue(projectWindow);
                if (folderTree != null)
                {
                    object treeViewDataSource = folderTree.GetType().GetProperty("data", MxEditorUtil.INSTANCE_FLAGS).GetValue(folderTree, null);
                    int searchFiltersRootInstanceID = (int)typeof(EditorWindow).Assembly.GetType("UnityEditor.SavedSearchFilters").GetMethod("GetRootInstanceID", MxEditorUtil.STATIC_FLAGS).Invoke(null, null);
                    bool isSearchFilterRootExpanded = (bool)treeViewDataSource.GetType().GetMethod("IsExpanded", MxEditorUtil.INSTANCE_FLAGS, null, new System.Type[] { typeof(int) }, null).Invoke(treeViewDataSource, new object[] { searchFiltersRootInstanceID });

                    MxEditorUtil.CollapseTreeViewController(projectWindow, folderTree, (TreeViewState<int>)projectWindow.GetType().GetField("m_FolderTreeState", MxEditorUtil.INSTANCE_FLAGS).GetValue(projectWindow), isSearchFilterRootExpanded ? new int[1] { searchFiltersRootInstanceID } : null);

                    // Preserve Assets and Packages folders' expanded states because they aren't automatically preserved inside ProjectBrowserColumnOneTreeViewDataSource.SetExpandedIDs
                    // https://github.com/Unity-Technologies/UnityCsReference/blob/e740821767d2290238ea7954457333f06e952bad/Editor/Mono/ProjectBrowserColumnOne.cs#L408-L420
                    InternalEditorUtility.expandedProjectWindowItemIds = (EntityId[])treeViewDataSource.GetType().GetMethod("GetExpandedIDs", MxEditorUtil.INSTANCE_FLAGS).Invoke(treeViewDataSource, null);

                    TreeViewItem<int> rootItem = (TreeViewItem<int>)treeViewDataSource.GetType().GetField("m_RootItem", MxEditorUtil.INSTANCE_FLAGS).GetValue(treeViewDataSource);
                    if (rootItem.hasChildren)
                    {
                        foreach (TreeViewItem<int> item in rootItem.children)
                            EditorPrefs.SetBool("ProjectBrowser" + item.displayName, (bool)treeViewDataSource.GetType().GetMethod("IsExpanded", MxEditorUtil.INSTANCE_FLAGS, null, new System.Type[] { typeof(int) }, null).Invoke(treeViewDataSource, new object[] { item.id }));
                    }
                }
            }
        }
    }
}
