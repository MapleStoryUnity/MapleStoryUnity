/**
 * $File: JCS_3DPositionTileAction.cs $
 * $Date: 2017-04-10 02:04:46 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// If game object goes a certain range set back to
    /// original positioin.
    /// position.
    /// </summary>
    public class JCS_3DPositionTileAction : MonoBehaviour
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_3DDistanceTileAction)")]

        [Tooltip("Is the action active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip(@"If object goes out of this distance, will
be set to this position in each axis.")]
        [SerializeField]
        private Vector3 mAbsolutePositionInAxis = Vector3.zero;

        [Tooltip("Use the local position instead of global position.")]
        [SerializeField]
        private bool mUseLocalPosition = false;

        [Header("🔍 Max")]

        [Tooltip("If axis x is over this will get reset.")]
        [SerializeField]
        private float mMaxX = float.PositiveInfinity;
        [Tooltip("If axis y is over this will get reset.")]
        [SerializeField]
        private float mMaxY = float.PositiveInfinity;
        [Tooltip("If axis z is over this will get reset.")]
        [SerializeField]
        private float mMaxZ = float.PositiveInfinity;

        [Header("🔍 Min")]

        [Tooltip("If axis x is over this will get reset.")]
        [SerializeField]
        private float mMinX = float.NegativeInfinity;
        [Tooltip("If axis y is over this will get reset.")]
        [SerializeField]
        private float mMinY = float.NegativeInfinity;
        [Tooltip("If axis z is over this will get reset.")]
        [SerializeField]
        private float mMinZ = float.NegativeInfinity;

        /* Setter & Getter */

        public bool active { get { return mActive; } set { mActive = value; } }
        public Vector3 absolutePositionInAxis { get { return mAbsolutePositionInAxis; } set { mAbsolutePositionInAxis = value; } }
        public bool useLocalPosition { get { return mUseLocalPosition; } set { mUseLocalPosition = value; } }
        public float maxX { get { return mMaxX; } set { mMaxX = value; } }
        public float maxY { get { return mMaxY; } set { mMaxY = value; } }
        public float maxZ { get { return mMaxZ; } set { mMaxZ = value; } }
        public float minX { get { return mMinX; } set { mMinX = value; } }
        public float minY { get { return mMinY; } set { mMinY = value; } }
        public float minZ { get { return mMinZ; } set { mMinZ = value; } }

        /* Functions */

        private void Update()
        {
            if (!mActive)
                return;

            Vector3 newPos = transform.position;
            if (mUseLocalPosition)
                newPos = transform.localPosition;

            {
                if (newPos.x < mMinX || newPos.x > mMaxX)
                    newPos.x = mAbsolutePositionInAxis.x;

                if (newPos.y < mMinY || newPos.y > mMaxY)
                    newPos.y = mAbsolutePositionInAxis.y;

                if (newPos.z < mMinZ || newPos.z > mMaxZ)
                    newPos.z = mAbsolutePositionInAxis.z;
            }

            if (mUseLocalPosition)
                transform.localPosition = newPos;
            else
                transform.position = newPos;
        }
    }
}
