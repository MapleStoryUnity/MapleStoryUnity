/**
 * $File: JCS_UISettings.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
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
        private const string BLACK_SCREEN_PATH = "UI/System/JCS_BlackScreen";
        private const string BLACK_SLIDE_SCREEN_PATH = "UI/System/JCS_BlackSlideScreen";
        private const string WHITE_SCREEN_PATH = "UI/System/JCS_WhiteScreen";
        private const string VIDEO_TRANSITION_PATH = "UI/System/JCS_VideoTransition";

        private const string FADE_SCREEN_PATH = "UI/System/JCS_FadeScreen";

        [Separator("Initialize Variables (JCS_UISettings)")]

        [Tooltip("Resize the UI in runtime.")]
        public bool resizeUI = true;

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
            _new.resizeUI = _old.resizeUI;
        }

        /// <summary>
        /// Pop the JCS_BlackScreen object.
        /// </summary>
        public static JCS_FadeScreen PopFadeScreen()
        {
            string path = FADE_SCREEN_PATH;
            var fs = JCS_Util.Instantiate(path).GetComponent<JCS_FadeScreen>();

            if (fs == null)
            {
                Debug.LogError("GameObject without `JCS_FadeScreen` component attached");
                return null;
            }

            return fs;
        }

        /// <summary>
        /// Pop the JCS_BlackScreen object.
        /// </summary>
        public static JCS_BlackScreen PopBlackScreen()
        {
            string path = BLACK_SCREEN_PATH;
            var bs = JCS_Util.Instantiate(path).GetComponent<JCS_BlackScreen>();

            if (bs == null)
            {
                Debug.LogError("GameObject without `JCS_BlackScreen` component attached");
                return null;
            }

            return bs;
        }

        /// <summary>
        /// Pop the JCS_BlackSlideScreen object.
        /// </summary>
        public static JCS_BlackSlideScreen PopBlackSlideScreen()
        {
            string path = BLACK_SLIDE_SCREEN_PATH;
            var bs = JCS_Util.Instantiate(path).GetComponent<JCS_BlackSlideScreen>();

            if (bs == null)
            {
                Debug.LogError("GameObject without `JCS_BlackScreen` component attached");
                return null;
            }

            return bs;
        }

        /// <summary>
        /// Spawn a white screen.
        /// </summary>
        public static JCS_WhiteScreen PopWhiteScreen()
        {
            string path = WHITE_SCREEN_PATH;
            var ws = JCS_Util.Instantiate(path).GetComponent<JCS_WhiteScreen>();

            if (ws == null)
            {
                Debug.LogError("GameObject without `JCS_WhiteScreen` component attached");
                return null;
            }

            return ws;
        }

        /// <summary>
        /// Spawn a video transition screen.
        /// </summary>
        public static RawImage PopVideoTransition()
        {
            string path = VIDEO_TRANSITION_PATH;
            var ri = JCS_Util.Instantiate(path).GetComponent<RawImage>();

            if (ri == null)
            {
                Debug.LogError("GameObject without `RawImage` component attached");
                return null;
            }

            return ri;
        }
    }
}
