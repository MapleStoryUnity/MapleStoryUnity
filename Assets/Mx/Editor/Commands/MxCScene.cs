/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;

namespace Mx
{
    public class MxCScene : Mx
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        public override bool Enable() { return true; }

        [Interactive(
            icon: "UnityLogo",
            summary: "Switch to scene")]
        public static void SwitchToScene()
        {
            CompletingRead("Switch to scene: ", MxEditorUtil.GetFiles("*.unity"),
                (answer, summary) =>
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        EditorSceneManager.OpenScene(answer);
                });
        }

        /// <summary>
        /// Execution when hovering a GameObject in the scene.
        /// </summary>
        private static void OnFind(Dictionary<string, GameObject> objs, string name)
        {
            GameObject obj = objs[name];
            MxEditorUtil.FocusInSceneView(obj);
        }

        [Interactive(
            icon: "d_Search Icon", 
            summary: "Find GameObject in scene")]
        public static void FindGameObjectInScene()
        {
            var objs = MxEditorUtil.FindObjectsByType<GameObject>();

            var tuple2 = MxEditorUtil.CompletionGameObjects(objs);

            var dic21 = tuple2.Item1;
            var dic22 = tuple2.Item2;

            CompletingRead("Find GameObject in scene: ",
                dic22,
                (name, _) => { OnFind(dic21, name); },
                (name, _) => { OnFind(dic21, name); });
        }

        [Interactive(
            icon: "d_Search Icon", 
            summary: "Find GameObject in scene by tag")]
        public static void FindGameObjectWithTag()
        {
            CompletingRead("Enter tag name: ", InternalEditorUtility.tags.ToList(),
                (tag, _) =>
                {
                    var objs = GameObject.FindGameObjectsWithTag(tag).ToList();

                    var tuple2 = MxEditorUtil.CompletionGameObjects(objs);

                    var dic21 = tuple2.Item1;
                    var dic22 = tuple2.Item2;

                    CompletingRead("Find GameObject with tag: (" + tag + ") ",
                        dic22,
                        (name, _) => { OnFind(dic21, name); },
                        (name, _) => { OnFind(dic21, name); });
                });
        }

        [Interactive(
            icon: "d_Search Icon", 
            summary: "Find GameObject in sceen by type")]
        public static void FindGameObjectsByType()
        {
            var tuple = MxEditorUtil.CompletionComponents();

            var dic1 = tuple.Item1;
            var dic2 = tuple.Item2;

            CompletingRead("Enter component name: ", dic2, (name, _) =>
            {
                Type type = dic1[name];

                var objs = MxEditorUtil.FindObjectsByType(type);

                var tuple2 = MxEditorUtil.CompletionGameObjects(objs);

                var dic21 = tuple2.Item1;
                var dic22 = tuple2.Item2;

                CompletingRead("Find " + type + " in scene: ", dic22,
                    (name, _) => { OnFind(dic21, name); },
                    (name, _) => { OnFind(dic21, name); });
            });
        }
    }
}
