#if UNITY_EDITOR
/**
 * $File: ReadSceneNames.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Read all the scene that are put into build setting.
/// </summary>
[ExecuteInEditMode]
public class ReadSceneNames : MonoBehaviour
{
    /* Variables */

    public static ReadSceneNames instance = null;

    /* Setter & Getter */

    /* Functions */

    private void OnGUI()
    {
        instance = this;

        // read the scene
        Reset();
    }

    private void Awake()
    {
        instance = this;

        // read the scene
        Reset();
    }

    /// <summary>
    /// Check the scene in the build setting are good to load.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public static bool CheckSceneAvailable(string sceneName)
    {
        foreach (string scene in ReadSceneNames.instance.SCENES_IN_BUILD_SETTING)
        {
            // scene is good to load.
            if (scene == sceneName)
                return true;
        }

        // no scene name in build setting, 
        // no allowed to load the scene.
        return false;
    }


    //----------------------------------------------------------
    // Source: http://answers.unity3d.com/questions/33263/how-to-get-names-of-all-available-levels.html
    //

    public string[] SCENES_IN_BUILD_SETTING = { };

    private static string[] ReadNames()
    {
        List<string> temp = new List<string>();
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                temp.Add(name);
            }
        }
        return temp.ToArray();
    }

    [MenuItem("CONTEXT/ReadSceneNames/Update Scene Names")]
    private static void UpdateNames(UnityEditor.MenuCommand command)
    {
        ReadSceneNames context = (ReadSceneNames)command.context;
        context.SCENES_IN_BUILD_SETTING = ReadNames();
    }

    private void Reset()
    {
        SCENES_IN_BUILD_SETTING = ReadNames();
    }
}
#endif
