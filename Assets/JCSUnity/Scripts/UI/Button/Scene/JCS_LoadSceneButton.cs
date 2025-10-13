/**
 * $File: JCS_LoadSceneButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.SceneManagement;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Button will load the target scene.
    /// </summary>
    public class JCS_LoadSceneButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        /* Variables */

        [Separator("Initialize Variables (JCS_LoadSceneButton)")]

        [Tooltip("Platform you want to target. NONE means all platform, so it will just load the scene")]
        [SerializeField]
        private JCS_PlatformType mPlatformType = JCS_PlatformType.NONE;

        [Tooltip("Scene name you want to load the scene; if empy, load the next scene instead.")]
        [SerializeField]
        [Scene]
        private string mSceneName = "";

        [Tooltip("Reload the current scene, and ignore the target scene name.")]
        [SerializeField]
        private bool mReloadScene = false;

        [Tooltip("Screen color when load the scene.")]
        [SerializeField]
        private Color mScreenColor = Color.black;

        [Tooltip("Keep BGM playing when load scene?")]
        [SerializeField]
        private bool mKeppBGM = false;

        [Tooltip("The way to load scene.")]
        [SerializeField]
        private LoadSceneMode mMode = LoadSceneMode.Single;

        /* Setter & Getter */

        public string sceneName { get { return mSceneName; } set { mSceneName = value; } }
        public bool reloadScene { get { return mReloadScene; } set { mReloadScene = value; } }
        public Color screenColor { get { return mScreenColor; } set { mScreenColor = value; } }
        public bool keppBGM { get { return mKeppBGM; } set { mKeppBGM = value; } }
        public LoadSceneMode mode { get { return mMode; } set { mMode = value; } }

        /* Functions */

        public override void OnClick()
        {
            // none meaning all platform so just load the scene.
            if (mPlatformType != JCS_PlatformType.NONE)
            {
                // if the button and the platform are not the same, 
                // dont load the scene and do nothing.
                if (mPlatformType != JCS_AppManager.FirstInstance().platformType)
                    return;
            }

            string sceneName = JCS_SceneManager.GetSceneNameByOption(mSceneName, mReloadScene);

            float fadeInTime = JCS_SceneSettings.FirstInstance().TimeIn();

            JCS_SceneManager.FirstInstance().LoadScene(sceneName, mMode,
                fadeInTime, mScreenColor, mKeppBGM);
        }
    }
}
