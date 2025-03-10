/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using UnityEngine;
using UnityEditor;

namespace Mx
{
    [InitializeOnLoad]
    public static class MxSettings
    {
        /* Variables */

        private static bool prefsLoaded = false;

        /* Setter & Getter */
        public static MxData data { get; private set; } = new MxData();

        /* Functions */

        static MxSettings()
        {
            Init();
        }

#if UNITY_2018_3_OR_NEWER
        private class MxSettingsProvider : SettingsProvider
        {
            public MxSettingsProvider(string path, SettingsScope scopes = SettingsScope.User)
                : base(path, scopes)
            { }

            public override void OnGUI(string searchContext)
            {
                CustomPreferencesGUI();
            }
        }

        [SettingsProvider]
        static SettingsProvider CreateMxSettingsProvider()
        {
            return new MxSettingsProvider("Preferences/Mx", SettingsScope.User);
        }
#else
        [PreferenceItem("Mx")]
#endif
        private static void CustomPreferencesGUI()
        {
            Init();
            Draw();
            SavePref();
        }

        private static void Init()
        {
            if (prefsLoaded)
                return;

            data = new MxData();

            data.Init();

            prefsLoaded = true;
        }

        private static void Draw()
        {
            GUILayout.Space(10);

            data.Draw();
        }

        private static void SavePref()
        {
            if (!GUI.changed)
                return;

            data.SavePref();
        }

    }
}
