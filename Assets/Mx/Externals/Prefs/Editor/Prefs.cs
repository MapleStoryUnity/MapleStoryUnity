#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Sabresaurus.PlayerPrefsEditor;

namespace Prefs
{
    public enum PrefType
    { 
        Editor,
        Player,
    }

    public struct PrefPair
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }

    public class Prefs
    {
        public static void DeleteAll(PrefType type)
        {
            if (type == PrefType.Editor)
                EditorPrefs.DeleteAll();
            else
                PlayerPrefs.DeleteAll();
        }

        public static void DeleteKey(PrefType type, string key)
        {
            if (type == PrefType.Editor)
                EditorPrefs.DeleteKey(key);
            else
                PlayerPrefs.DeleteKey(key);
        }

        public static int GetInt(PrefType type, string key, int defaultValue = 0)
        {
            if (type == PrefType.Editor)
                return EditorPrefs.GetInt(key, defaultValue);
            else
                return PlayerPrefs.GetInt(key, defaultValue);
        }

        public static float GetFloat(PrefType type, string key, float defaultValue = 0.0f)
        {
            if (type == PrefType.Editor)
                return EditorPrefs.GetFloat(key, defaultValue);
            else
                return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public static string GetString(PrefType type, string key, string defaultValue = "")
        {
            if (type == PrefType.Editor)
                return EditorPrefs.GetString(key, defaultValue);
            else
                return PlayerPrefs.GetString(key, defaultValue);
        }

        public static bool GetBool(PrefType type, string key, bool defaultValue = false)
        {
            if (type == PrefType.Editor)
                return EditorPrefs.GetBool(key, defaultValue);
            else
                throw new NotSupportedException("PlayerPrefs interface does not natively support bools");
        }

        public static void SetInt(PrefType type, string key, int value)
        {
            if (type == PrefType.Editor)
                EditorPrefs.SetInt(key, value);
            else
                PlayerPrefs.SetInt(key, value);
        }

        public static void SetFloat(PrefType type, string key, float value)
        {
            if (type == PrefType.Editor)
                EditorPrefs.SetFloat(key, value);
            else
                PlayerPrefs.SetFloat(key, value);
        }

        public static void SetString(PrefType type, string key, string value)
        {
            if (type == PrefType.Editor)
                EditorPrefs.SetString(key, value);
            else
                PlayerPrefs.SetString(key, value);
        }

        public static void SetBool(PrefType type, string key, bool value)
        {
            if (type == PrefType.Editor)
                EditorPrefs.SetBool(key, value);
            else
                throw new NotSupportedException("PlayerPrefs interface does not natively support bools");
        }

        public static void Set(PrefType type, Dictionary<string, Type> pref, string key, string value)
        {
            if (pref[key] == typeof(string))
                SetString(type, key, value);
            else if (pref[key] == typeof(bool))
                SetBool(type, key, bool.Parse(value));
            else if (pref[key] == typeof(float))
                SetFloat(type, key, float.Parse(value));
            else if (pref[key] == typeof(int))
                SetInt(type, key, int.Parse(value));
        }

        public static void Save(PrefType type)
        {
            if (type == PrefType.Editor)
            {
                // No Save() method in EditorPrefs
            }
            else
                PlayerPrefs.Save();
        }

        private static readonly System.Text.Encoding ENCODING = new System.Text.UTF8Encoding();

        private static string GetMacOSEditorPrefsPath()
        {
#if UNITY_2017_4_OR_NEWER
            // From Unity Docs: On macOS, EditorPrefs are stored in ~/Library/Preferences/com.unity3d.UnityEditor5.x.plist
            // https://docs.unity3d.com/2017.4/Documentation/ScriptReference/EditorPrefs.html
            string fileName = "com.unity3d.UnityEditor5.x.plist";
#else
            // From Unity Docs: On macOS, EditorPrefs are stored in ~/Library/Preferences/com.unity3d.UnityEditor.plist.
            // https://docs.unity3d.com/2017.3/Documentation/ScriptReference/EditorPrefs.html
            string fileName = "com.unity3d.UnityEditor.plist";
#endif
            // Construct the fully qualified path
            string editorPrefsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library/Preferences"), fileName);
            return editorPrefsPath;
        }

        private static PrefPair[] RetrievePrefsOSX(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                // No existing PlayerPrefs saved (which is valid), so just return an empty array
                return new PrefPair[0];
            }

            // Parse the plist then cast it to a Dictionary
            object plist = Plist.readPlist(path);

            Dictionary<string, object> parsed = plist as Dictionary<string, object>;

            // Convert the dictionary data into an array of PlayerPrefPairs
            var tmpPrefs = new List<PrefPair>(parsed.Count);
            foreach (KeyValuePair<string, object> pair in parsed)
            {
                if (pair.Value.GetType() == typeof(double))
                {
                    // Some float values may come back as double, so convert them back to floats
                    tmpPrefs.Add(new PrefPair() { Key = pair.Key, Value = (float)(double)pair.Value });
                }
                else if (pair.Value.GetType() == typeof(bool))
                {
                    // Unity PlayerPrefs API doesn't allow bools, so ignore them
                }
                else
                {
                    tmpPrefs.Add(new PrefPair() { Key = pair.Key, Value = pair.Value });
                }
            }

            // Return the results
            return tmpPrefs.ToArray();
        }

        private static PrefPair[] RetrievePrefsWindows(
            Microsoft.Win32.RegistryKey registryKey, PrefType type)
        {
            // Parse the registry if the specified registryKey exists
            if (registryKey == null)
            {
                // No existing PlayerPrefs saved (which is valid), so just return an empty array
                return new PrefPair[0];
            }

            // Get an array of what keys (registry value names) are stored
            string[] valueNames = registryKey.GetValueNames();

            // Create the array of the right size to take the saved PlayerPrefs
            PrefPair[] tempPlayerPrefs = new PrefPair[valueNames.Length];

            // Parse and convert the registry saved PlayerPrefs into our array
            int i = 0;
            foreach (string valueName in valueNames)
            {
                string key = valueName;

                // Remove the _h193410979 style suffix used on PlayerPref keys in Windows registry
                int index = key.LastIndexOf("_");
                key = key.Remove(index, key.Length - index);

                // Get the value from the registry
                object ambiguousValue = registryKey.GetValue(valueName);

                // Unfortunately floats will come back as an int (at least on 64 bit) because the float is stored as
                // 64 bit but marked as 32 bit - which confuses the GetValue() method greatly!
                if (ambiguousValue.GetType() == typeof(int) || ambiguousValue.GetType() == typeof(long))
                {
                    // If the PlayerPref is not actually an int then it must be a float, this will evaluate to true
                    // (impossible for it to be 0 and -1 at the same time)
                    if (GetInt(type, key, -1) == -1 && GetInt(type, key, 0) == 0)
                    {
                        // Fetch the float value from PlayerPrefs in memory
                        ambiguousValue = GetFloat(type, key);
                    }
                    else if (type == PrefType.Editor && (GetBool(type, key, true) != true || GetBool(type, key, false) != false))
                    {
                        // If it reports a non default value as a bool, it's a bool not a string
                        ambiguousValue = GetBool(type, key);
                    }
                }
                else if (ambiguousValue.GetType() == typeof(byte[]))
                {
                    // On Unity 5 a string may be stored as binary, so convert it back to a string
                    ambiguousValue = ENCODING.GetString((byte[])ambiguousValue).TrimEnd('\0');
                }

                // Assign the key and value into the respective record in our output array
                tempPlayerPrefs[i] = new PrefPair() { Key = key, Value = ambiguousValue };
                i++;
            }

            // Return the results
            return tempPlayerPrefs;
        }

        private static PrefPair[] GetEditorPrefs()
        {
            Microsoft.Win32.RegistryKey registryKey;

            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                    {
                        string prefPath = GetMacOSEditorPrefsPath();

                        return RetrievePrefsOSX(prefPath);
                    }
                case RuntimePlatform.WindowsEditor:
                    {
                        // Starting Unity 5.5 registry key has " 5.x" suffix: https://docs.unity3d.com/550/Documentation/ScriptReference/EditorPrefs.html
                        // Even though for some versions of Unity docs state that N.x suffix is used where N.x is the major version number,
                        // it's still " 5.x" suffix used for that cases which is probably bug in the docs.
                        // Note that starting 2019.2 docs have " 5.x" suffix: https://docs.unity3d.com/2019.2/Documentation/ScriptReference/EditorPrefs.html
#if UNITY_5_5_OR_NEWER
                        string subKeyPath = "Software\\Unity Technologies\\Unity Editor 5.x";
#else
                        string subKeyPath = "Software\\Unity Technologies\\Unity Editor";
#endif

                        registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(subKeyPath);

                        return RetrievePrefsWindows(registryKey, PrefType.Editor);
                    }
                default:
                    {
                        throw new NotSupportedException("Mx doesn't support this Unity Editor platform");
                    }
            }
        }

        private static PrefPair[] GetPlayerPrefs()
        {
            string companyName = PlayerSettings.companyName;
            string productName = PlayerSettings.productName;

            Microsoft.Win32.RegistryKey registryKey;

            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                    {
                        // From Unity Docs: On Mac OS X PlayerPrefs are stored in ~/Library/Preferences folder, in a file named unity.[company name].[product name].plist, where company and product names are the names set up in Project Settings. The same .plist file is used for both Projects run in the Editor and standalone players.

                        // Construct the plist filename from the project's settings
                        string plistFilename = $"unity.{companyName}.{productName}.plist";
                        // Now construct the fully qualified path
                        string prefPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library/Preferences"), plistFilename);

                        return RetrievePrefsOSX(prefPath);
                    }
                case RuntimePlatform.WindowsEditor:
                    {
                        // From Unity docs: On Windows, PlayerPrefs are stored in the registry under HKCU\Software\[company name]\[product name] key, where company and product names are the names set up in Project Settings.
#if UNITY_5_5_OR_NEWER
                        // From Unity 5.5 editor PlayerPrefs moved to a specific location
                        registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Unity\\UnityEditor\\" + companyName + "\\" + productName);
#else
                        registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\" + companyName + "\\" + productName);
#endif
                        return RetrievePrefsWindows(registryKey, PrefType.Player);
                    }
                default:
                    {
                        throw new NotSupportedException("Mx doesn't support this Unity Editor platform");
                    }
            }
        }

        public static PrefPair[] GetPrefs(PrefType type)
        {
            if (type == PrefType.Editor)
                return GetEditorPrefs();
            else
                return GetPlayerPrefs();
        }

        public static Dictionary<string, string> GetPrefsString(PrefType type)
        {
            PrefPair[] pref = GetPrefs(type);
            return pref.ToDictionary(x => x.Key, x => x.Value.ToString());
        }

        public static Dictionary<string, Type> GetPrefsType(PrefType type)
        {
            PrefPair[] pref = GetPrefs(type);
            return pref.ToDictionary(x => x.Key, x => x.Value.GetType());
        }
    }
}
#endif