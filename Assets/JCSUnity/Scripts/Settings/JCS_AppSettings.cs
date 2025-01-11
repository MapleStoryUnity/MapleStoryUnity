/**
 * $File: JCS_AppSettings.cs $
 * $Date: 2018-09-08 16:51:42 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
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

        public EmptyFunction SAVE_APP_DATA_FUNC = null;
        public EmptyFunction LOAD_APP_DATA_FUNC = null;  // NOT USED

        // Callback when application starts. This should only run once
        // per application is starts.
        public EmptyFunction onApplicationStarts = null;

        [Separator("Check Variables (JCS_AppSettings)")]

        [Tooltip("Is the application start?")]
        [ReadOnly]
        public bool APPLICATION_STARTS = false;

        [Separator("Initialize Variables (JCS_AppSettings)")]

        [Tooltip("Enable to overwrite the default frame rate.")]
        public bool SET_FRAME_RATE = false;

        [Tooltip("Target frame rate.")]
        [Range(30, 120)]
        public int FRAME_RATE = 120;

        [Separator("Runtime Variables (JCS_AppSettings)")]

        [Header("- Save Load")]

        [Tooltip("Data folder path.")]
        public string DATA_PATH = "/Data_jcs/";

        [Tooltip("Data file extension.")]
        public string DATA_EXTENSION = ".jcs";

        [Tooltip("Save when switching the scene.")]
        public bool SAVE_ON_SWITCH_SCENE = true;

        [Tooltip("Save when app exit.")]
        public bool SAVE_ON_EXIT_APP = true;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            CheckInstance(this);

            if (SET_FRAME_RATE)
                Application.targetFrameRate = FRAME_RATE;
        }

        private void Start()
        {
            // If already starts we don't need to enable the flag.
            if (!APPLICATION_STARTS)
                JCS_GameManager.instance.onAfterInitialize += DoApplicationStart;
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
            _new.SET_FRAME_RATE = _old.SET_FRAME_RATE;
            _new.FRAME_RATE = _old.FRAME_RATE;
            _new.APPLICATION_STARTS = _old.APPLICATION_STARTS;

            _new.DATA_PATH = _old.DATA_PATH;
            _new.DATA_EXTENSION = _old.DATA_EXTENSION;
            _new.SAVE_ON_EXIT_APP = _old.SAVE_ON_EXIT_APP;
            _new.SAVE_ON_SWITCH_SCENE = _old.SAVE_ON_SWITCH_SCENE;
        }

        /// <summary>
        /// Enable applicatin start flag after the first 'Update()'
        /// function runs, here we have 'JCS_GameManager' to be our 
        /// 'Update()' pioneer runner.
        /// </summary>
        private void DoApplicationStart()
        {
            if (APPLICATION_STARTS)
                return;

            if (onApplicationStarts != null)
                onApplicationStarts.Invoke();

            APPLICATION_STARTS = true;
        }
    }
}
