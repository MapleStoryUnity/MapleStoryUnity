/**
 * $File: JCS_2DGameManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using MyBox;
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

        [Separator("Check Variables (JCS_2DGameManager)")]

        [Tooltip("A list of item object will ignore their collision detection.")]
        [SerializeField]
        private List<JCS_2DPositionPlatform> mPlatforms = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            RegisterInstance(this);
        }

        /// <summary>
        /// Add a platform to the list.
        /// </summary>
        /// <param name="pp"></param>
        public void AddPlatformList(JCS_2DPositionPlatform pp)
        {
            mPlatforms.Add(pp);
        }

        /// <summary>
        /// Set a collider ignore all the platform trigger.
        /// </summary>
        /// <param name="c"> collder to set to ignore trigger. </param>
        public void IgnoreAllPlatformTrigger(Collider c)
        {
            for (int index = 0; index < mPlatforms.Count; ++index)
            {
                Physics.IgnoreCollision(mPlatforms[index].GetPlatformTrigger(),
                    c, true);
            }
        }

        /// <summary>
        /// Set a collider ignore all the platform collider.
        /// </summary>
        /// <param name="c"> collider to set to ignore collider. </param>
        public void IgnoreAllPlatformCollider(Collider c)
        {
            for (int index = 0; index < mPlatforms.Count; ++index)
            {
                Physics.IgnoreCollision(mPlatforms[index].GetPlatformCollider(),
                    c, true);
            }
        }
    }
}
