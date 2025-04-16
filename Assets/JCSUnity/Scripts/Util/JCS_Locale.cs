/**
 * $File: JCS_Locale.cs $
 * $Date: 2025-04-15 01:14:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2025 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Localization utilities.
    /// </summary>
    public static class JCS_Locale
    {
        /* Variables */

        /// <summary>
        /// Dictionary convert system language to its name.
        /// </summary>
        public static Dictionary<SystemLanguage, string> LANGEUAGE_IT_NAME = new()
        {
            { SystemLanguage.Unknown, "-"},

            { SystemLanguage.Afrikaans, "Afrikaans"},
            { SystemLanguage.Arabic, "عربي"},
            { SystemLanguage.Basque, "euskara"},
            { SystemLanguage.Belarusian, "беларускі"},
            { SystemLanguage.Bulgarian, "белоруски"},
            { SystemLanguage.Chinese, "中文"},
            { SystemLanguage.Czech, "čeština"},
            { SystemLanguage.Danish, "dansk"},
            { SystemLanguage.Dutch, "Nederlands"},
            { SystemLanguage.English, "English"},
            { SystemLanguage.Estonian, "eesti keel"},
            { SystemLanguage.Faroese, "Føroysk"},
            { SystemLanguage.Finnish, "suomalainen"},
            { SystemLanguage.French, "Français"},
            { SystemLanguage.German, "Deutsch"},
            { SystemLanguage.Greek, "ελληνικά"},
            { SystemLanguage.Hebrew, "עִברִית"},
            { SystemLanguage.Hungarian, "magyar"},
            { SystemLanguage.Icelandic, "íslenskur"},
            { SystemLanguage.Indonesian, "indónesíska"},
            { SystemLanguage.Italian, "Italiano"},
            { SystemLanguage.Japanese, "日本語"},
            { SystemLanguage.Korean, "한국인"},
            { SystemLanguage.Latvian, "latviski"},
            { SystemLanguage.Lithuanian, "lietuvių"},
            { SystemLanguage.Norwegian, "norsk"},
            { SystemLanguage.Polish, "Polski"},
            { SystemLanguage.Portuguese, "Português"},
            { SystemLanguage.Romanian, "română"},
            { SystemLanguage.Russian, "Русский"},
            { SystemLanguage.SerboCroatian, "srpskohrvatski"},
            { SystemLanguage.Slovak, "slovenský"},
            { SystemLanguage.Slovenian, "slovenski"},
            { SystemLanguage.Spanish, "Español"},
            { SystemLanguage.Swedish, "svenska"},
            { SystemLanguage.Thai, "แบบไทย"},
            { SystemLanguage.Turkish, "Türkçe"},
            { SystemLanguage.Ukrainian, "українська"},
            { SystemLanguage.Vietnamese, "Tiếng Việt"},
            { SystemLanguage.ChineseSimplified, "简体中文"},
            { SystemLanguage.ChineseTraditional, "繁體中文"},
            { SystemLanguage.Hindi, "हिंदी"},
        };

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Convert system language to string identifier.
        /// </summary>
        public static string SystemLangToString(SystemLanguage systemLanguage)
        {
            return LANGEUAGE_IT_NAME[systemLanguage];
        }

        /// <summary>
        /// Convert string identifier to system language.
        /// </summary>
        public static SystemLanguage StringToSystemLang(string text)
        {
            foreach (KeyValuePair<SystemLanguage, string> entry in LANGEUAGE_IT_NAME)
            {
                if (entry.Value == text)
                    return entry.Key;
            }

            return SystemLanguage.Unknown;
        }
    }
}
