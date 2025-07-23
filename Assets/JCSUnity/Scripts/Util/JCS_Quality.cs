/**
 * $File: JCS_Quality.cs $
 * $Date: 2025-07-14 01:14:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2025 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Quality settings utilities.
    /// </summary>
    public static class JCS_Quality
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Return the name of the current quality level.
        /// </summary>
        public static string ItName()
        {
            int level = QualitySettings.GetQualityLevel();

            string text = QualitySettings.names[level];

            return text;
        }
        public static int ItLevel()
        {
            return QualitySettings.GetQualityLevel();
        }

        /// <summary>
        /// Return current quality settings by name.
        /// </summary>
        public static int ByName(string name)
        {
            return QualitySettings.names.IndexOfItem(name);
        }

        /// <summary>
        /// Set the quality level by name.
        /// </summary>
        public static void SetLevel(string name)
        {
            SetLevel(name, false);
        }
        public static void SetLevel(string name, bool applyExpensiveChanges)
        {
            int level = ByName(name);

            SetLevel(level, applyExpensiveChanges);
        }
        // @compatible
        public static void SetLevel(int level)
        {
            SetLevel(level, false);
        }
        public static void SetLevel(int level, bool applyExpensiveChanges)
        {
            QualitySettings.SetQualityLevel(level, applyExpensiveChanges);
        }

        /// <summary>
        /// Return true if the quality level reaches.
        /// </summary>
        public static bool IsAbove(string name)
        {
            return ByName(name) <= ItLevel();
        }

        /// <summary>
        /// Return true if the quality level not reaches.
        /// </summary>
        public static bool IsBelow(string name)
        {
            return ItLevel() < ByName(name);
        }

        /// <summary>
        /// Return true if the quality level is the exact level.
        /// </summary>
        public static bool Is(params string[] names)
        {
            foreach (string name in names)
            {
                if (ByName(name) == ItLevel())
                    return true;
            }

            return false;
        }
    }
}
