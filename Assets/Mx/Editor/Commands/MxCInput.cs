/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using UnityEngine;

namespace Mx
{
    public class MxCInput : Mx
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        public override bool Enable() { return true; }


        [Interactive(
            icon: "UnityEditor.SceneHierarchyWindow@2x",
            summary: "List out key code for exploration")]
        public static void ListKeyCode()
        {
            var result = MxUtil.EnumTuple(typeof(KeyCode));
            CompletingRead("Key code: ", result, null);
        }

        //[Interactive(
        //    icon: "",
        //    summary: "Toggle the editor dark/light theme")]
        public static void ToggleEditorTheme()
        {
            //EditorGUIUtility.isProSkin;
        }
    }
}
