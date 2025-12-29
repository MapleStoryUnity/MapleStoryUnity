/**
 * $File: JCS_AppSettings.cs $
 * $Date: 2018-09-08 16:51:42 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Application settings.
    /// </summary>
    public class JCS_AppSettings : JCS_Settings<JCS_AppSettings>
    {
        /* Variables */

        public Action onSaveAppData = null;
        public Action onLoadAppData = null;  // NOT USED

        // Callback when application starts. This should only run once
        // per application is starts.
        public Action onApplicationStarts = null;

        [Separator("📋 Check Variabless (JCS_AppSettings)")]

        [Tooltip("Is the application start?")]
        [ReadOnly]
        public bool appStarts = false;

        [Separator("🌱 Initialize Variables (JCS_AppSettings)")]

        [Tooltip("Enable to overwrite the default frame rate.")]
        public bool setFrameRate = false;

        [Tooltip("Target frame rate.")]
        [Range(-1, 120)]
        public int frameRate = 120;

        [Separator("⚡️ Runtime Variables (JCS_AppSettings)")]

        [Header("🔍 Save Load")]

        [Tooltip("Data folder path.")]
        public string dataPath = "/Data_jcs/";

        [Tooltip("Data file extension.")]
        public string dataExt = ".jcs";

        [Tooltip("Save when switching the scene.")]
        public bool saveOnSwitchScene = true;

        [Tooltip("Save when app exit.")]
        public bool saveOnExitApp = true;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            CheckInstance(this);

            if (setFrameRate)
                Application.targetFrameRate = frameRate;
        }

        private void Start()
        {
            // If already starts we don't need to enable the flag.
            if (!appStarts)
                JCS_GameManager.FirstInstance().RegisterOnAfterInit(DoApplicationStart);
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
        protected override void TransferData(JCS_AppSettings _old, JCS_AppSettings _new)
        {
            _new.setFrameRate = _old.setFrameRate;
            _new.frameRate = _old.frameRate;
            _new.appStarts = _old.appStarts;

            _new.dataPath = _old.dataPath;
            _new.dataExt = _old.dataExt;
            _new.saveOnSwitchScene = _old.saveOnSwitchScene;
            _new.saveOnExitApp = _old.saveOnExitApp;
        }

        /// <summary>
        /// Enable applicatin start flag after the first 'Update()'
        /// function runs, here we have 'JCS_GameManager' to be our 
        /// 'Update()' pioneer runner.
        /// </summary>
        private void DoApplicationStart()
        {
            if (appStarts)
                return;

            onApplicationStarts?.Invoke();

            appStarts = true;
        }
    }
}
