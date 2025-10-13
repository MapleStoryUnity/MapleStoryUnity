/**
 * $File: JCS_PortalSettings.cs $
 * $Date: 2016-12-10 04:28:19 $
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
    /// Settings for the game that uses the portal.
    /// </summary>
    public class JCS_PortalSettings : JCS_Settings<JCS_PortalSettings>
    {
        /* Variables */

        [Separator("Check Variables (JCS_PortalSettings)")]

        [Tooltip("Current Portal Lable are.")]
        [ReadOnly]
        public JCS_PortalLabel sceneLabel = JCS_PortalLabel.NONE;

        [Separator("Initialize Variables (JCS_PortalSettings)")]

        [Tooltip("Reset the player position to portal?")]
        public bool resetPositionAtStart = true;

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
        protected override void TransferData(JCS_PortalSettings _old, JCS_PortalSettings _new)
        {
            _new.resetPositionAtStart = _old.resetPositionAtStart;
        }
    }
}
