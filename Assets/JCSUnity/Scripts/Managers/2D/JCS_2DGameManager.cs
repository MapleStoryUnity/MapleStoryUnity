/**
 * $File: JCS_2DGameManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// 2D Game Specific game manager.
    /// </summary>
    public class JCS_2DGameManager : JCS_Manager<JCS_2DGameManager>
    {
        /* Variables */

        // a list of item object will ignore their collision detection.
        private List<JCS_2DPositionPlatform> mPlatformList = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            instance = this;

            mPlatformList = new List<JCS_2DPositionPlatform>();
        }

        /// <summary>
        /// Add a platform to the list.
        /// </summary>
        /// <param name="pp"></param>
        public void AddPlatformList(JCS_2DPositionPlatform pp)
        {
            mPlatformList.Add(pp);
        }

        /// <summary>
        /// Set a collider ignore all the platform trigger.
        /// </summary>
        /// <param name="c"> collder to set to ignore trigger. </param>
        public void IgnoreAllPlatformTrigger(Collider c)
        {
            for (int index = 0; index < mPlatformList.Count; ++index)
            {
                Physics.IgnoreCollision(mPlatformList[index].GetPlatformTrigger(), 
                    c, true);
            }
        }

        /// <summary>
        /// Set a collider ignore all the platform collider.
        /// </summary>
        /// <param name="c"> collider to set to ignore collider. </param>
        public void IgnoreAllPlatformCollider(Collider c)
        {
            for (int index = 0; index < mPlatformList.Count; ++index)
            {
                Physics.IgnoreCollision(mPlatformList[index].GetPlatformCollider(),
                    c, true);
            }
        }
    }
}
