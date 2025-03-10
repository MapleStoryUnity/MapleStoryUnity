/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace Mx
{
    public class MxCFile : Mx
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        public override bool Enable() { return true; }
        
        [Interactive(
            icon: "d_Search Icon",
            summary: "Find file")]
        public static void FindFile()
        {
            CompletingRead("Find file: ", MxEditorUtil.DefaultFiles(),
                (path, _) =>
                { InternalEditorUtility.OpenFileAtLineExternal(path, 1); },
                (path, _) =>
                { MxEditorUtil.HighlightAsset(path); });
        }

        [Interactive(
            icon: "d_Search Icon", 
            summary: "Find the file and open it externally")]
        public static void FindFileExternal()
        {
            CompletingRead("Find file externally: ", MxEditorUtil.DefaultFiles(),
                (path, _) => { Application.OpenURL(path); },
                (path, _) =>
                { MxEditorUtil.HighlightAsset(path); });
        }

        [Interactive(
            icon: "d_Search Icon", 
            summary: "Find file by type of the file")]
        public static void FindFileByType()
        {
            CompletingRead("Find file by type: ", new Dictionary<string, MxItem>()
            {
                { "All"       , new MxItem(summary: "*.*"     , icon: "") },
                { "Scene"     , new MxItem(summary: "*.unity" , icon: "UnityLogo") },
                { "Prefab"    , new MxItem(summary: "*.prefab", icon: "d_Prefab Icon") },
                { "Texture"   , new MxItem(summary: "*.png"   , icon: typeof(Texture)) },
                { "Material"  , new MxItem(summary: "*.mat"   , icon: typeof(Material)) },
                { "Mesh"      , new MxItem(summary: "*.mesh"  , icon: typeof(Mesh)) },
                { "C# script" , new MxItem(summary: "*.cs"    , icon: "cs Script Icon") },
                { "Text"      , new MxItem(summary: "*.txt"   , icon: typeof(TextAsset)) },
                { "Font"      , new MxItem(summary: "*.ttf"   , icon: typeof(Font)) },
            },
            (type, item) =>
            {
                string pattern = item.summary;
                string[] patterns = pattern.Split('|');

                CompletingRead("Find file by type: (" + type + ") ",
                    MxEditorUtil.DefaultFiles(pattern),
                    (path, _) =>
                    { InternalEditorUtility.OpenFileAtLineExternal(path, 1); }, 
                    (path, _) => 
                    { MxEditorUtil.HighlightAsset(path); });
            });
        }

        [Interactive(
            icon: "d_Search Icon", 
            summary: "Find file by wildcard pattern")]
        public static void FindFileByWildcard()
        {
            ReadString("Wildcard pattern: ", (pattern, _) =>
            {
                CompletingRead("Find file by wildcard: (" + pattern + ") ",
                   MxEditorUtil.DefaultFiles(pattern),
                   (path, _) =>
                   { InternalEditorUtility.OpenFileAtLineExternal(path, 1); },
                   (path, _) =>
                   { MxEditorUtil.HighlightAsset(path); });
            });
        }
    }
}
