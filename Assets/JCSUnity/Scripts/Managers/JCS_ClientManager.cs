/**
 * $File: JCS_ClientManager.cs $
 * $Date: 2017-09-01 18:10:41 $
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
    /// Manager manage the clients that connected to this client.
    /// </summary>
    public class JCS_ClientManager
        : JCS_Managers<JCS_ClientManager>
    {
        /* Variables */

        /**
         * Please fill up the local client.
         */
        public static JCS_Client LOCAL_CLIENT = null;

        [Tooltip("All the player in the scene now.")]
        [SerializeField]
        private List<JCS_Client> mClients = new List<JCS_Client>();

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            instance = this;
        }

        /// <summary>
        /// Add the client to the manager.
        /// </summary>
        /// <param name="client"></param>
        public void AddClient(JCS_Client client)
        {
            mClients.Add(client);
        }

        /// <summary>
        /// Remove the client from the manager.
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(JCS_Client client)
        {
            mClients.Remove(client);
        }
    }
}
