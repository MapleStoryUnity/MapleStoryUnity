/**
 * $File: JCS_RelativeFreezePositionAction.cs $
 * $Date: 2017-05-11 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Action freeze the game object by relative distance.
    /// </summary>
    public class JCS_RelativeFreezePositionAction : JCS_UnityObject
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_RelativeFreezePositionAction)")]

        [Tooltip("Target transform to set the position relatively by distance.")]
        [SerializeField]
        private Transform mTargetTrans = null;

        [Tooltip("How far the game object going to be freeze.")]
        [SerializeField]
        private Vector3 mDistance = Vector3.zero;

        [Tooltip("Use local position instead?")]
        [SerializeField]
        private bool mIsLocalPosition = false;

        /* Setter & Getter */

        public Transform targetTransform { get { return mTargetTrans; } set { mTargetTrans = value; } }
        public Vector3 distance { get { return mDistance; } set { mDistance = value; } }
        public bool isLocalPosition { get { return mIsLocalPosition; } set { mIsLocalPosition = value; } }

        /* Functions */

        private void LateUpdate()
        {
            if (mTargetTrans == null)
                return;


            if (mIsLocalPosition)
            {
                Vector3 newPos = localPosition;

                /**
                 * No need to let the target transform to set to local.
                 * Just no reason the target transform relative position 
                 * will need a local position.
                 */
                newPos.x = mTargetTrans.transform.position.x + mDistance.x;
                newPos.y = mTargetTrans.transform.position.y + mDistance.y;
                newPos.z = mTargetTrans.transform.position.z + mDistance.z;

                localPosition = newPos;
            }
            else
            {
                Vector3 newPos = position;

                /**
                 * No need to let the target transform to set to local.
                 * Just no reason the target transform relative position 
                 * will need a local position.
                 */
                newPos.x = mTargetTrans.transform.position.x + mDistance.x;
                newPos.y = mTargetTrans.transform.position.y + mDistance.y;
                newPos.z = mTargetTrans.transform.position.z + mDistance.z;

                position = newPos;
            }
        }
    }
}
