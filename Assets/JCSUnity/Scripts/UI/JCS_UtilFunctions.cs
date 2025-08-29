/**
 * $File: JCS_UtilFunctions.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Util functions put here, usually systematic.
    /// </summary>
    public class JCS_UtilFunctions : MonoBehaviour
    {
        /// <summary>
        /// Pop the JCS_BlackScreen object.
        /// </summary>
        public static void PopBlackScreen()
        {
            string path = JCS_UISettings.BLACK_SCREEN_PATH;
            var bs = JCS_Util.Instantiate(path).GetComponent<JCS_BlackScreen>();

            if (bs == null)
            {
                Debug.LogError("GameObject without `JCS_BlackScreen` component attached");
                return;
            }

            JCS_SceneManager.FirstInstance().SetBlackScreen(bs);
        }

        /// <summary>
        /// Pop the JCS_BlackSlideScreen object.
        /// </summary>
        public static void PopBlackSlideScreen()
        {
            string path = JCS_UISettings.BLACK_SLIDE_SCREEN_PATH;
            var bs = JCS_Util.Instantiate(path).GetComponent<JCS_BlackSlideScreen>();

            if (bs == null)
            {
                Debug.LogError("GameObject without `JCS_BlackScreen` component attached");
                return;
            }

            JCS_SceneManager.FirstInstance().SetBlackSlideScreen(bs);
        }

        /// <summary>
        /// Spawn a white screen.
        /// </summary>
        public static void PopWhiteScreen()
        {
            string path = JCS_UISettings.WHITE_SCREEN_PATH;
            var ws = JCS_Util.Instantiate(path).GetComponent<JCS_WhiteScreen>();

            if (ws == null)
            {
                Debug.LogError("GameObject without `JCS_WhiteScreen` component attached");
                return;
            }

            JCS_SceneManager.FirstInstance().SetWhiteScreen(ws);
        }
    }
}
