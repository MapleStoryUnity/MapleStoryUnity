/**
 * $File: JCS_Screen.cs $
 * $Date: 2020-12-17 19:44:50 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
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
        public static int width 
        { 
            get
            {
#if UNITY_2017_2_OR_NEWER
                return (int)Screen.safeArea.width;
#else
                return Screen.width;
#endif
            }
        }

        /// <summary>
        /// Screen's height includes safe area view calculation.
        /// </summary>
        public static int height
        {
            get
            {
#if UNITY_2017_2_OR_NEWER
                return (int)Screen.safeArea.height;
#else
                return Screen.height;
#endif
            }
        }

        /// <summary>
        /// Safe way to se screen's resolution base on platforms.
        /// </summary>
        public static void SetResolution(int width, int height, bool fullscreen, int preferredRefreshRate = 0)
        {
#if !(UNITY_IOS || UNITY_ANDROID)
            Screen.SetResolution(width, height, fullscreen, preferredRefreshRate);
#endif
        }
    }
}
