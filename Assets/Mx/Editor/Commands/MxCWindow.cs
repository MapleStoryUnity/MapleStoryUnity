/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mx
{
    public class MxCWindow : Mx
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        public override bool Enable() { return true; }

        [Interactive(
            icon: "TestFailed",
            summary: "Exit the Unity editor application")]
        public static void KillUnity()
        {
            EditorApplication.Exit(0);
        }

        [Interactive(
            icon: "winbtn_win_restore_a@2x", 
            summary: "Find and make focus on the targeted window")]
        public static void SwitchToWindow()
        {
            var windows = Resources.FindObjectsOfTypeAll<EditorWindow>().ToList();

            windows.RemoveAt(0);  // Remove the Mx completion window.

            var windowss = MxUtil.ToListString(windows);

            CompletingRead("Switch to window: ", windowss,
                (answer, _) =>
                {
                    int index = windowss.IndexOf(answer);

                    EditorWindow window = windows[index];
                    window.Focus();
                });
        }

        [Interactive(
            icon: "winbtn_win_restore_a@2x", 
            summary: "Maximize the scene view")]
        public static void ToggleSceneViewMaximized()
        {
            SceneView[] views = Resources.FindObjectsOfTypeAll<SceneView>();

            if (views.Length == 0)
                return;

            views[0].maximized = !views[0].maximized;
        }
    }
}
