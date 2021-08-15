/**
 * $File: JCS_ApplicationSettings.cs $
 * $Date: 2018-09-08 16:51:42 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    public delegate void ApplicationStartsCallback();

    /// <summary>
    /// Application settings.
    /// </summary>
    public class JCS_ApplicationSettings : JCS_Settings<JCS_ApplicationSettings>
    {
        /* Variables */

        // Callback when application starts. This should only run once
        // per application is starts.
        public ApplicationStartsCallback onApplicationStarts = null;

        [Header("** Check Variables (JCS_ApplicationSettings) **")]

        [Tooltip("Is the application start?")]
        public bool APPLICATION_STARTS = false;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            instance = CheckSingleton(instance, this);
        }

        private void Start()
        {
            // If already starts we don't need to enable the flag.
            if (!APPLICATION_STARTS)
                JCS_GameManager.instance.afterGameInitializeCallback += DoApplicationStart;
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
        protected override void TransferData(JCS_ApplicationSettings _old, JCS_ApplicationSettings _new)
        {
            _new.APPLICATION_STARTS = _old.APPLICATION_STARTS;
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
