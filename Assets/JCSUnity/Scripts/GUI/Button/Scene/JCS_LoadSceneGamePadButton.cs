/**
 * $File: JCS_LoadSceneGamePadButton.cs $
 * $Date: 2017-10-27 11:44:51 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Button will load the target scene. (Gamepad)
    /// </summary>
    public class JCS_LoadSceneGamePadButton
        : JCS_GamePadButton
    {
        /* Variables */

        [Header("** Initialize Variables (JCS_LoadSceneGamePadButton) **")]

        [Tooltip("Platform you want to target. NONE means all platform, so it will just load the scene")]
        [SerializeField]
        private JCS_PlatformType mPlatformType = JCS_PlatformType.NONE;

        [Tooltip("Scene name you want to load the scene.")]
        [SerializeField]
        private string mSceneName = "JCS_ApplicationCloseSimulate";

        [Tooltip("Screen color when load the scene.")]
        [SerializeField]
        private Color mScreenColor = Color.black;

        [Tooltip("Keep BGM playing when load scene?")]
        [SerializeField]
        private bool mKeppBGM = false;


        /* Setter & Getter */

        public string SceneName { get { return this.mSceneName; } set { this.mSceneName = value; } }
        public Color ScreenColor { get { return this.mScreenColor; } set { this.mScreenColor = value; } }
        public bool KeppBGM { get { return this.mKeppBGM; } set { this.mKeppBGM = value; } }


        /* Functions */

        public override void JCS_OnClickCallback()
        {
            // none meaning all platform so just load the scene.
            if (mPlatformType != JCS_PlatformType.NONE)
            {
                // if the button and the platform are not the same, 
                // dont load the scene and do nothing.
                if (mPlatformType != JCS_ApplicationManager.instance.PlatformType)
                    return;
            }

            JCS_SceneManager.instance.LoadScene(mSceneName, mScreenColor, mKeppBGM);
        }
    }
}
