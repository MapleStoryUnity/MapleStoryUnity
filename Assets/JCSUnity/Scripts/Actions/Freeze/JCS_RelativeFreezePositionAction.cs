/**
 * $File: JCS_RelativeFreezePositionAction.cs $
 * $Date: 2017-05-11 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Action freeze the gameobject by relative distance.
    /// </summary>
    public class JCS_RelativeFreezePositionAction : JCS_UnityObject
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_RelativeFreezePositionAction) **")]

        [Tooltip("Target transform to set the position relatively by distance.")]
        [SerializeField]
        private Transform mTargetTrans = null;

        [Tooltip("How far the gameobject going to be freeze.")]
        [SerializeField]
        private Vector3 mDistance = Vector3.zero;

        [Tooltip("Use local position instead?")]
        [SerializeField]
        private bool mIsLocalPosition = false;

        /* Setter & Getter */

        public Transform TargetTransform { get { return this.mTargetTrans; } set { this.mTargetTrans = value; } }
        public Vector3 Distance { get { return this.mDistance; } set { this.mDistance = value; } }
        public bool IsLocalPosition { get { return this.mIsLocalPosition; } set { this.mIsLocalPosition = value; } }

        /* Functions */

        private void LateUpdate()
        {
            if (mTargetTrans == null)
                return;


            if (mIsLocalPosition)
            {
                Vector3 newPos = this.LocalPosition;

                /**
                 * No need to let the target transform to set to local.
                 * Just no reason the target transform relative position 
                 * will need a local position.
                 */
                newPos.x = mTargetTrans.transform.position.x + mDistance.x;
                newPos.y = mTargetTrans.transform.position.y + mDistance.y;
                newPos.z = mTargetTrans.transform.position.z + mDistance.z;

                this.LocalPosition = newPos;
            }
            else
            {
                Vector3 newPos = this.Position;

                /**
                 * No need to let the target transform to set to local.
                 * Just no reason the target transform relative position 
                 * will need a local position.
                 */
                newPos.x = mTargetTrans.transform.position.x + mDistance.x;
                newPos.y = mTargetTrans.transform.position.y + mDistance.y;
                newPos.z = mTargetTrans.transform.position.z + mDistance.z;

                this.Position = newPos;
            }
        }
    }
}
