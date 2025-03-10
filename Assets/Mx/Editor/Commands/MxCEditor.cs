/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Mx
{
    public class MxCEditor : Mx
    {
        /* Variables */

        private static readonly List<string> mSkipRoots = new List<string> { "CONTEXT", "&File", "&Help", "Help", "Component", "&Window" };

        /* Setter & Getter */

        /* Functions */

        private static string CreateCommandString(string _command, List<string> _list)
        {
            StringBuilder stringBuilder = new();

            foreach (string s in _list)
            {
                stringBuilder.Append(s.Trim(' ', '&'));
                stringBuilder.Append("/");
            }

            stringBuilder.Append(_command.Trim(' ', '&'));

            return stringBuilder.ToString();
        }

        private static List<string> MenuItemLists()
        {
            List<string> commands = new();

            string sCommands = EditorGUIUtility.SerializeMainMenuToString();
            string[] arCommands = sCommands.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            List<string> lCurrentParents = new List<string>();
            for (int i = 0, imax = arCommands.Length; i < imax; i++)
            {
                string sCurrentRoot = lCurrentParents.Count != 0 ? lCurrentParents.First() : "";

                string sCurrentLine = arCommands[i];
                string sCurrentLineTrimmed = sCurrentLine.TrimStart(' ');
                int iPreviousIndention = lCurrentParents.Count - 1;
                int iCurrentIndention = (sCurrentLine.Length - sCurrentLineTrimmed.Length) / 4;

                int iLastTab = sCurrentLine.LastIndexOf('\t');
                sCurrentLineTrimmed = iLastTab != -1 ? sCurrentLine.Remove(iLastTab, sCurrentLine.Length - iLastTab).Trim() : sCurrentLine.Trim();

                if (sCurrentLineTrimmed == System.String.Empty)
                    continue;

                if (iPreviousIndention > iCurrentIndention)
                {
                    MxUtil.RemoveLast(lCurrentParents);
                    while (iPreviousIndention > iCurrentIndention)
                    {
                        MxUtil.RemoveLast(lCurrentParents);
                        iPreviousIndention--;
                    }

                    if (!mSkipRoots.Contains(sCurrentRoot))
                    {
                        commands.Add(CreateCommandString(sCurrentLineTrimmed, lCurrentParents));
                    }
                    lCurrentParents.Add(sCurrentLineTrimmed);
                }
                else if (iPreviousIndention < iCurrentIndention)
                {
                    if (!mSkipRoots.Contains(sCurrentRoot))
                    {
                        MxUtil.RemoveLast(commands);
                        commands.Add(CreateCommandString(sCurrentLineTrimmed, lCurrentParents));
                    }
                    lCurrentParents.Add(sCurrentLineTrimmed);
                }
                else
                {
                    MxUtil.RemoveLast(lCurrentParents);
                    if (!mSkipRoots.Contains(sCurrentRoot))
                    {
                        commands.Add(CreateCommandString(sCurrentLineTrimmed, lCurrentParents));
                    }
                    lCurrentParents.Add(sCurrentLineTrimmed);
                }
            }

            return commands;
        }

        [Interactive(
            icon: "_Menu@2x",
            summary: "Invokes the menu item in the specified path")]
        public static void ExecutMenuItem()
        {
            CompletingRead("Menu Item: ", MenuItemLists(), (command, _) =>
            {
                EditorApplication.ExecuteMenuItem(command);
            });
        }

        [Interactive(summary: "List the EditorPref key & value; then copy the key to clipboard")]
        public static void EditorPrefGetKey()
        {
            const Prefs.PrefType type = Prefs.PrefType.Editor;

            Dictionary<string, string> prefss = Prefs.Prefs.GetPrefsString(type);

            CompletingRead("Get EditorPref key: ", prefss, (key, _) =>
            {
                key.CopyToClipboard();
                UnityEngine.Debug.Log("Copied EditorPref key: " + key);
            });
        }

        [Interactive(summary: "List the PlayerPref key & value; then copy the key to clipboard")]
        public static void PlayerPrefGetKey()
        {
            const Prefs.PrefType type = Prefs.PrefType.Player;

            Dictionary<string, string> prefss = Prefs.Prefs.GetPrefsString(type);

            CompletingRead("Get PlayerPref key: ", prefss, (key, _) =>
            {
                key.CopyToClipboard();
                UnityEngine.Debug.Log("Copied PlayerPref key: " + key);
            });
        }

        private static string GetPrefPrefix(Prefs.PrefType type)
        {
            return (type == Prefs.PrefType.Editor) ? "[EditorPrefs] " : "[PlayerPrefs] ";
        }

        private static void CreatePref(Prefs.PrefType type, string key)
        {
            CompletingRead("Select type: ", new Dictionary<string, string>()
            {
                { "Bool"  , "True or False" },
                { "Float" , "Any decimal number (10.0f, 30.0f)" },
                { "Int"   , "Any integer number (10, 30)" },
                { "String", "" },
            },
            (createType, _) =>
            {
                ReadString(GetPrefPrefix(type) + "Create `" + key + "` with value", (input, _) =>
                {
                    switch (createType)
                    {
                        case "Bool":
                            Prefs.Prefs.SetBool(type, key, bool.Parse(input));
                            break;
                        case "Float":
                            Prefs.Prefs.SetFloat(type, key, float.Parse(input));
                            break;
                        case "Int":
                            Prefs.Prefs.SetInt(type, key, int.Parse(input));
                            break;
                        case "String":
                            Prefs.Prefs.SetString(type, key, input);
                            break;
                    }
                });
            });
        }

        [Interactive(summary: "Create or update EditorPrefs")]
        public static void EditorPrefSetKey()
        {
            const Prefs.PrefType type = Prefs.PrefType.Editor;

            Dictionary<string, string> prefss = Prefs.Prefs.GetPrefsString(type);
            Dictionary<string, Type> prefst = Prefs.Prefs.GetPrefsType(type);

            CompletingRead(GetPrefPrefix(type) + "Set value: ", prefss, (key, _) =>
            {
                if (prefst.ContainsKey(key))
                {
                    ReadString(GetPrefPrefix(type) + "Update `" + key + "`'s value to ", (input, _) =>
                    { Prefs.Prefs.Set(type, prefst, key, input); });

                    return;
                }

                CreatePref(type, key);
            },
            requiredMatch: false);
        }

        [Interactive(summary: "Create or update PlayerPrefs")]
        public static void PlayerPrefSetKey()
        {
            const Prefs.PrefType type = Prefs.PrefType.Player;

            Dictionary<string, string> prefss = Prefs.Prefs.GetPrefsString(type);
            Dictionary<string, Type> prefst = Prefs.Prefs.GetPrefsType(type);

            CompletingRead(GetPrefPrefix(type) + "Set value: ", prefss, (key, _) =>
            {
                if (prefst.ContainsKey(key))
                {
                    ReadString(GetPrefPrefix(type) + "Update `" + key + "`'s value to ", (input, _) =>
                    { Prefs.Prefs.Set(type, prefst, key, input); });

                    return;
                }

                CreatePref(type, key);
            },
            requiredMatch: false);
        }

        [Interactive(summary: "Delete key in EditorPrefs")]
        public static void EditorPrefDeleteKey()
        {
            const Prefs.PrefType type = Prefs.PrefType.Editor;

            Dictionary<string, string> prefss = Prefs.Prefs.GetPrefsString(type);

            CompletingRead(GetPrefPrefix(type) + "Delet key: ", prefss, (key, _) =>
            { Prefs.Prefs.DeleteKey(type, key); });
        }

        [Interactive(summary: "Delete key in PlayerPrefs")]
        public static void PlayerPrefDeleteKey()
        {
            const Prefs.PrefType type = Prefs.PrefType.Player;

            Dictionary<string, string> prefss = Prefs.Prefs.GetPrefsString(type);

            CompletingRead(GetPrefPrefix(type) + "Delet key: ", prefss, (key, _) =>
            { Prefs.Prefs.DeleteKey(type, key); });
        }

        [Interactive(summary: "Delete all keys in EditorPrefs")]
        public static void EditorPrefDeleteAll()
        {
            const Prefs.PrefType type = Prefs.PrefType.Editor;

            YesOrNo(GetPrefPrefix(type) + "Delet all keys: ", (yes, _) =>
            {
                if (yes == "Yes") Prefs.Prefs.DeleteAll(type);
            });
        }

        [Interactive(summary: "Delete all keys in PlayerPrefs")]
        public static void PlayerPrefDeleteAll()
        {
            const Prefs.PrefType type = Prefs.PrefType.Player;

            YesOrNo(GetPrefPrefix(type) + "Delet all keys: ", (yes, _) =>
            {
                if (yes == "Yes") Prefs.Prefs.DeleteAll(type);
            });
        }

        [Interactive(
            icon: "AssemblyLock",
            summary: "Prevents loading of assemblies when it is inconvenient.")]
        public static void LockReloadAssemblies()
        {
            EditorApplication.LockReloadAssemblies();
        }

        [Interactive(
            icon: "Assembly Icon",
            summary: "Must be called after LockReloadAssemblies, to reenable loading of assemblies.")]
        public static void UnlockReloadAssemblies()
        {
            EditorApplication.UnlockReloadAssemblies();
            EditorUtility.RequestScriptReload();
        }

        [Interactive(
            icon: "cs Script Icon",
            summary: "The Unity Editor reloads script assemblies asynchronously on the next frame.")]
        public static void RequestScriptReload()
        {
            EditorUtility.RequestScriptReload();
        }
    }
}
