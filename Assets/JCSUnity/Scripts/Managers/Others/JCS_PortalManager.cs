/**
 * $File: JCS_PortalManager.cs $
 * $Date: 2016-12-10 04:11:15 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Handle all the portal in the scene.
    /// </summary>
    public class JCS_PortalManager : JCS_Managers<JCS_PortalManager>
    {
        /* Variables */

        [Header("** Initialize Variables (JCS_PortalManager) **")]

        [Tooltip("All the portal in the scene. Plz setup mannually.")]
        [SerializeField]
        private JCS_2DPortal[] mPortals = null;

        /* Setter & Getter */

        public JCS_2DPortal[] Portals { get { return this.mPortals; } }

        /* Functions */

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            SetPlayerToPortalByLabel();
        }

        /// <summary>
        /// Set the player to current portal label.
        /// </summary>
        private void SetPlayerToPortalByLabel()
        {
            JCS_PortalSettings jcsPs = JCS_PortalSettings.instance;

            // check manager exists?
            if (jcsPs == null)
                return;

            // check effect enable?
            if (!jcsPs.RESET_POSITION_AT_START)
                return;

            mPortals = JCS_Utility.RemoveEmptySlotIncludeMissing<JCS_2DPortal>(mPortals);

            foreach (JCS_2DPortal portal in mPortals)
            {
                if (portal.PortalLabel == jcsPs.SCENE_PORTAL_LABEL)
                {
                    // get the player
                    JCS_Player player = JCS_PlayerManager.instance.GetActivePlayer();

                    // NOTE(jenchieh): this uses execution order in Unity Engine.
                    // move the player to portal position.
                    player.transform.position = portal.transform.position;

                    // set the camera position
                    JCS_Camera.main.SetPosition(
                        portal.transform.position.x, 
                        portal.transform.position.y);

                    break;
                }
            }
        }
    }
}
