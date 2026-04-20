/**
 * $File: JCS_PauseSettings.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2023 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Handle the pause settings.
    /// </summary>
    public class JCS_PauseSettings : JCS_Settings<JCS_PauseSettings>
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_PauseSettings)")]

        [Tooltip("Pause and unpause with asymptotic transition.")]
        public bool asymptotic = false;

        [Tooltip("How fast the asymptotic transition?")]
        [Range(JCS_Consts.FRICTION_MIN, 30.0f)]
        public float friction = 0.2f;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            CheckInstance(this);
        }

        /// <summary>
        /// Return weather to asymptotic on pause/unpause.
        /// </summary>
        public bool Asymptotic()
        {
            // check if override the setting.
            if (JCS_Glob.pausem.overrideSetting)
            {
                // return the override value.
                return JCS_Glob.pausem.asymptotic;
            }

            // if not override, 
            // return the basic value.
            return asymptotic;
        }

        /// <summary>
        /// Return the friction to asymptotic on pause/unpause.
        /// </summary>
        public float Friction()
        {
            // check if override the setting.
            if (JCS_Glob.pausem.overrideSetting)
            {
                // return the override value.
                return JCS_Glob.pausem.friction;
            }

            // if not override, 
            // return the basic value.
            return friction;
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
        protected override void TransferData(JCS_PauseSettings _old, JCS_PauseSettings _new)
        {
            _new.asymptotic = _old.asymptotic;
            _new.friction = _old.friction;
        }
    }
}
