/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mx
{
    public class MxCScript : Mx
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        public override bool Enable() { return true; }

        /// <summary>
        /// Remove missing scripts from a game object and its children.
        /// </summary>
        private static void RemoveMissingScripts(GameObject obj)
        {
            if (obj == null)
                return;

            int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(obj);

            if (count > 0)
            {
                Undo.RegisterCompleteObjectUndo(obj, "Remove missing scripts");
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
            }

            // Apply to children as well.
            for (int index = 0; index < obj.transform.childCount; ++index)
            {
                Transform child = obj.transform.GetChild(index);

                RemoveMissingScripts(child.gameObject);
            }
        }

        [Interactive(
            icon: "d_cs Script Icon",
            summary: "Remove all missing scripts in the scene and prefabs")]
        public static void RemoveMissingScripts()
        {
            Scene scene = SceneManager.GetActiveScene();

            GameObject[] objs = scene.GetRootGameObjects();

            foreach (GameObject obj in objs)
            {
                RemoveMissingScripts(obj);
            }

            List<string> files = MxEditorUtil.DefaultFiles("*.prefab");

            foreach (string file in files)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(file);

                RemoveMissingScripts(prefab);
            }

            AssetDatabase.SaveAssets();
        }
    }
}
