/**
 * $File: JCS_PortalManager.cs $
 * $Date: 2016-12-10 04:11:15 $
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
    /// Handle all the portal in the scene.
    /// </summary>
    public class JCS_PortalManager : JCS_Manager<JCS_PortalManager>
    {
        /* Variables */

        [Separator("Initialize Variables (JCS_PortalManager)")]

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
            var ps = JCS_PortalSettings.instance;

            // check manager exists?
            if (ps == null)
                return;

            // check effect enable?
            if (!ps.RESET_POSITION_AT_START)
                return;

            mPortals = JCS_Array.RemoveEmptyMissing(mPortals).ToArray();

            foreach (JCS_2DPortal portal in mPortals)
            {
                if (portal.PortalLabel == ps.SCENE_PORTAL_LABEL)
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
