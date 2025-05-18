/**
 * $File: JCS_Startup.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Hold function when is startup.
    /// </summary>
    public class JCS_Startup
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Initialize the whole application. (In Unity layer.)
        /// </summary>
        /// <returns></returns>
        public static bool InitializeApplication()
        {
            var ns = JCS_NetworkSettings.instance;

            if (!ns.ONLINE_MODE)
                return false;

            Debug.Log("Online Mode is enabled");

            // Create Connection
            if (!JCS_NetworkSettings.CreateNetwork(
                ns.HOST_NAME, ns.PORT,
                JCS_NetworkSettings.GetPresetClientHandler()))
            {
                // Faild handle
                return false;
            }

            // Create keys
            JCS_NetworkManager.CreateKey();

            // 這裡無法判別
            return true;
        }

    }
}
