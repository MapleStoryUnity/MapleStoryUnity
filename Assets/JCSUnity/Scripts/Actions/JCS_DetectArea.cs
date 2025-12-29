/**
 * $File: JCS_DetectArea.cs $
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
    /// The collider to detect "JCS_DetectAreaObject".
    /// </summary>
    public class JCS_DetectArea : MonoBehaviour
    {
        /* Variables */

        private JCS_DetectAreaAction mDetectAreaAction = null;

        [Separator("📋 Check Variabless (JCS_DetectArea)")]

        [SerializeField]
        [ReadOnly]
        private Collider mCollider = null;

        /* Setter & Getter */

        public void SetAction(JCS_DetectAreaAction da) { mDetectAreaAction = da; }

        /* Functions */

        private void Awake()
        {
            mCollider = GetComponent<Collider>();

            if (mCollider == null)
                Debug.LogError("No collider attached to do the dectect action");
            else
                mCollider.enabled = false;
        }

        private void Start()
        {
            if (mCollider != null)
                mCollider.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            // check if anything we target are in there
            var dao = other.GetComponent<JCS_DetectAreaObject>();
            if (dao == null)
                return;

            // if cannot damage
            if (!dao.GetLiveObject().canDamage)
                return;

            if (!JCS_GameSettings.FirstInstance().tribeDamageEachOther)
            {
                // if both player does not need to add in to list.
                // or if both enemy does not need to add in to list.
                if (dao.GetLiveObject().isPlayer == mDetectAreaAction.GetLiveObject().isPlayer)
                    return;
            }

            // add it to the list
            mDetectAreaAction.AddDetectedObject(dao);
        }

        private void OnTriggerExit(Collider other)
        {
            // if the target thing left
            var dao = other.GetComponent<JCS_DetectAreaObject>();
            if (dao == null)
                return;

            // remove from the list.
            mDetectAreaAction.RemoveDetectedObject(dao);
        }
    }
}
