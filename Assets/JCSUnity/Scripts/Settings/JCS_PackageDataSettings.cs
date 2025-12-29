/**
 * $File: JCS_PackageDataSettings.cs $
 * $Date: $
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
    /// Package data settings.
    /// </summary>
    public class JCS_PackageDataSettings : JCS_Settings<JCS_PackageDataSettings>
    {
        /* Variables */

        [Separator("🌱 Initialize Variables (JCS_PackageDataSettings)")]

        [Tooltip("Package Data copyright information.")]
        public string copyright = "Copyright 2017 JCSUnity, Taiwan";

        [Tooltip("Package Data version information.")]
        public string version = "Package file v1.0";

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            CheckInstance(this);
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
        protected override void TransferData(JCS_PackageDataSettings _old, JCS_PackageDataSettings _new)
        {
            _new.copyright = _old.copyright;
            _new.version = _old.version;
        }
    }
}
