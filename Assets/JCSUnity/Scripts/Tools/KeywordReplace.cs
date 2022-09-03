#if UNITY_EDITOR
/**
 * $File: KeywordReplace.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEditor;
using UnityEngine;

/// <summary>
/// Source: http://forum.unity3d.com/threads/c-script-template-how-to-make-custom-changes.273191/
/// Author: hpjohn
/// Modefied: Jen-Chieh Shen
/// </summary>
public class KeywordReplace : UnityEditor.AssetModificationProcessor
{
    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        int index = path.LastIndexOf(".");

        if (index < 0)
            return;

        string file = path.Substring(index);

        if (file != ".cs" && file != ".js" && file != ".boo")
            return;

        index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;
        file = System.IO.File.ReadAllText(path);

        string dateAndTimeVar = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string yearTime = System.DateTime.Now.ToString("yyyy");

        file = file.Replace("#CREATIONDATE#", dateAndTimeVar + "");
        file = file.Replace("#CREATEYEAR#", yearTime + "");
        file = file.Replace("#PROJECTNAME#", PlayerSettings.productName);
        file = file.Replace("#SMARTDEVELOPERS#", PlayerSettings.companyName);

        System.IO.File.WriteAllText(path, file);
        AssetDatabase.Refresh();
    }
}

#endif
