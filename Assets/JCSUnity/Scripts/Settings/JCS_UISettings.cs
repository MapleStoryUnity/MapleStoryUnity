/**
 * $File: JCS_UISettings.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// UI related settings.
    /// </summary>
    public class JCS_UISettings : JCS_Settings<JCS_UISettings>
    {
        /* Variables */

        // No one care about how black screen look so I just make it unseen 
        // in the inspector.
        public static string BLACK_SCREEN_PATH = "LevelDesignUI/JCS_BlackScreen";
        public static string BLACK_SLIDE_SCREEN_PATH = "LevelDesignUI/JCS_BlackSlideScreen";
        public static string BLACK_SCREEN_NAME = "JCS_BlackScreen";
        public static string WHITE_SCREEN_PATH = "LevelDesignUI/JCS_WhiteScreen";
        public static string WHITE_SCREEN_NAME = "JCS_WhiteScreen";

        public static string FADE_SCREEN_PATH = "LevelDesignUI/JCS_FadeScreen";
        public static string FADE_SCREEN_NAME = "JCS_FadeScreen";

        [Separator("Initialize Variables (JCS_UISettings)")]

        [Tooltip("Resize the UI in runtime?")]
        public bool RESIZE_UI = true;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            CheckInstance(this);
        }

        /// <summary>
        /// Make limit so not all the data override the by the new data!
        /// </summary>
        /// <param name="_old"> old data we copy from </param>
        /// <param name="_new"> new data we copy to </param>
        protected override void TransferData(JCS_UISettings _old, JCS_UISettings _new)
        {
            // ResizeUI option should always be the same!
            _new.RESIZE_UI = _old.RESIZE_UI;
        }
    }
}
