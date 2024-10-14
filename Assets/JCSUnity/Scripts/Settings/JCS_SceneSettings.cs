/**
 * $File: JCS_SceneSettings.cs $
 * $Date: 2016-10-28 13:59:35 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Handle the scene the better way.
    /// </summary>
    public class JCS_SceneSettings : JCS_Settings<JCS_SceneSettings>
    {
        /* Variables */

        [Separator("Check Variables (JCS_SceneSettings)")]

        [Tooltip("True when is switching scene.")]
        [ReadOnly]
        public bool SWITCHING_SCENE = false;

        [Separator("Runtime Variables (JCS_SceneSettings)")]

        [Tooltip("General Scene fadout time. (For all scene)")]
        [Range(0.0f, 5.0f)]
        public float SCENE_FADEOUT_TIME = 1.5f;

        [Tooltip("General Scene fadein time. (For all scene)")]
        [Range(0.0f, 5.0f)]
        public float SCENE_FADEIN_TIME = 1.5f;

        [Tooltip("Screen color to fade in/out the scene.")]
        public Color SCREEN_COLOR = Color.black;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            instance = CheckInstance(instance, this);
        }

        /// <summary>
        /// Return the time for fade out the scene base on the settings.
        /// </summary>
        public float SceneFadeOutTimeBaseOnSetting()
        {
            var sm = JCS_SceneManager.instance;

            // check if override the setting.
            if (sm.OverrideSetting)
            {
                // return the override value.
                return sm.SceneFadeOutTime;
            }

            // if not override, 
            // return the basic value.
            return SCENE_FADEOUT_TIME;
        }

        /// <summary>
        /// Return the time for fade in the scene base on the settings.
        /// </summary>
        public float SceneFadeInTimeBaseOnSetting()
        {
            var sm = JCS_SceneManager.instance;

            // check if override the setting.
            if (sm.OverrideSetting)
            {
                // return the override value.
                return sm.SceneFadeInTime;
            }

            // if not override, 
            // return the basic value.
            return SCENE_FADEIN_TIME;
        }

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected override void TransferData(JCS_SceneSettings _old, JCS_SceneSettings _new)
        {
            _new.SWITCHING_SCENE = _old.SWITCHING_SCENE;

            _new.SCENE_FADEIN_TIME = _old.SCENE_FADEIN_TIME;
            _new.SCENE_FADEOUT_TIME = _old.SCENE_FADEOUT_TIME;
            _new.SCREEN_COLOR = _old.SCREEN_COLOR;
        }
    }
}
