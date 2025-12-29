/**
 * $File: JCS_2DPlatform.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Platfrom that player could stand on.
    /// </summary>
    public class JCS_2DPlatform : MonoBehaviour
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_2DPlatform)")]

        [Tooltip("Platform's collider that player stand on.")]
        [SerializeField]
        private BoxCollider mPlatformCollider = null;

        [Tooltip("Platform's trigger decide weather or not to active platform collider.")]
        [SerializeField]
        private BoxCollider mPlatformTrigger = null;

        /* Setter & Getter */

        /* Functions */

        protected virtual void Start()
        {
            if (mPlatformCollider == null ||
                mPlatformTrigger == null)
                return;

            Physics.IgnoreCollision(mPlatformCollider,
                    mPlatformTrigger, true);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            JCS_Player player = JCS_GameManager.FirstInstance().player;
            if (player == null)
                return;

            if (other.gameObject.name == player.name)
            {
                Physics.IgnoreCollision(mPlatformCollider,
                    player.GetCharacterController(), true);

            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            JCS_Player player = JCS_GameManager.FirstInstance().player;
            if (player == null)
                return;

            if (other.gameObject.name == player.name)
            {
                Physics.IgnoreCollision(mPlatformCollider,
                    player.GetCharacterController(), false);

            }
        }
    }
}
