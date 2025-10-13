/**
 * $File: JCS_HitDamageAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Simulate the action when object damage the player
    /// by hitting with his body. (physically)
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    // Object with this action must be live object!!
    [RequireComponent(typeof(JCS_2DLiveObject))]
    public class JCS_HitDamageAction : MonoBehaviour
    {
        /* Variables */

        // collider use to detect player to give damage
        private BoxCollider mBoxCollider = null;

        private JCS_AbilityFormat mAbilityFormat = null;

        private JCS_2DLiveObject mLiveObject = null;

        private List<JCS_2DLiveObject> mLiveObjectList = null;

        [Separator("Runtime Variables (JCS_HitDamageAction)")]

        [Tooltip("When the action occurs, play this sound.")]
        [SerializeField]
        private AudioClip mHitSound = null;

        /* Setter & Getter */

        public BoxCollider GetBoxCollider() { return mBoxCollider; }

        /* Functions */

        private void Awake()
        {
            // try to get the ability format from this object.
            mAbilityFormat = GetComponent<JCS_AbilityFormat>();

            mLiveObject = GetComponent<JCS_2DLiveObject>();

            mLiveObjectList = new List<JCS_2DLiveObject>();
        }

        private void Update()
        {
            foreach (JCS_2DLiveObject liveObject in mLiveObjectList)
            {
                DamageLiveObject(liveObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            JCS_2DLiveObject liveObject = other.GetComponent<JCS_2DLiveObject>();
            if (liveObject == null)
                return;

            mLiveObjectList.Add(liveObject);
        }

        private void OnTriggerExit(Collider other)
        {
            JCS_2DLiveObject liveObject = other.GetComponent<JCS_2DLiveObject>();
            if (liveObject == null)
                return;

            mLiveObjectList.Remove(liveObject);
        }

        /// <summary>
        /// Do the damage to live object.
        /// </summary>
        /// <param name="liveObject"></param>
        private void DamageLiveObject(JCS_2DLiveObject liveObject)
        {
            // if cannot damage this live object than do nothing.
            if (!liveObject.canDamage)
                return;

            if (!JCS_GameSettings.FirstInstance().tribeDamageEachOther)
            {
                // if both player does not need to add in to list.
                // or if both enemy does not need to add in to list.
                if (liveObject.isPlayer == mLiveObject.isPlayer)
                    return;
            }

            if (mAbilityFormat == null)
            {
                Debug.Log("You sure to not using any `JCS_AbilityFormat`?");
                return;
            }

            liveObject.ApplyDamageText(
                mAbilityFormat.GetMinDamage(),
                mAbilityFormat.GetMaxDamage(),
                transform.position,
                1,      // hit
                0,
                mHitSound);     // critical chance


            // see if the collider is player.
            JCS_Player p = liveObject.GetComponent<JCS_Player>();
            if (p == null)
                return;

            // if the living object we are attacking is
            // player do the hit effect.
            p.Hit();
        }
    }
}
