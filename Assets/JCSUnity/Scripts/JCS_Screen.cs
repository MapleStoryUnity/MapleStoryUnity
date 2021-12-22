/**
 * $File: JCS_Screen.cs $
 * $Date: 2020-12-17 19:44:50 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Enhanced version of `Screen` class from built-in Unity
    /// </summary>
    public static class JCS_Screen
    {
        /// <summary>
        /// Screen's width includes safe area view calculation.
        /// </summary>
        public static float width
        {
            get
            {
#if UNITY_EDITOR
                return Screen.width;
#elif UNITY_IOS || UNITY_ANDROID
                return Screen.safeArea.width;
#else
                return Screen.width;
#endif
            }
        }

        /// <summary>
        /// Screen's height includes safe area view calculation.
        /// </summary>
        public static float height
        {
            get
            {
#if UNITY_EDITOR
                return Screen.height;
#elif UNITY_IOS || UNITY_ANDROID
                return Screen.safeArea.height;
#else
                return Screen.height;
#endif
            }
        }

        /// <summary>
        /// Safe way to se screen's resolution base on platforms.
        /// </summary>
        public static void SetResolution(float width, float height, bool fullscreen, int preferredRefreshRate = 0)
        {
            SetResolution((int)width, (int)height, fullscreen, preferredRefreshRate);
        }
        public static void SetResolution(int width, int height, bool fullscreen, int preferredRefreshRate = 0)
        {
#if !(UNITY_IOS || UNITY_ANDROID)
            Screen.SetResolution(width, height, fullscreen, preferredRefreshRate);
#endif
        }
    }
}
