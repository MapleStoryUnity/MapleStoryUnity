/**
 * $File: JCS_FreezePositionAction.cs $
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
    /// Action freeze the game object's position.
    /// </summary>
    public class JCS_FreezePositionAction : JCS_UnityObject
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_FreezePositionAction)")]

        [Tooltip("Is this action active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Position where this game object freeze at.")]
        [SerializeField]
        private Vector3 mPositionToFreeze = Vector3.zero;

        [Tooltip("Freeze in the local space.")]
        [SerializeField]
        private bool mIsLocalPosition = true;

        [Tooltip("Freeze the position in each axis.")]
        [SerializeField]
        private JCS_Bool3 mFreezePosition = JCS_Bool3.allFalse;

        /* Setter & Getter */

        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public Vector3 PositionToFreeze { get { return this.mPositionToFreeze; } set { this.mPositionToFreeze = value; } }
        public bool IsLocalPosition
        {
            get { return this.mIsLocalPosition; }
            set
            {
                this.mIsLocalPosition = value;

                // get the new freeze position.
                if (mIsLocalPosition)
                    this.mPositionToFreeze = this.LocalPosition;
                else
                    this.mPositionToFreeze = this.Position;
            }
        }
        public JCS_Bool3 FreezePosition { get { return this.mFreezePosition; } set { this.mFreezePosition = value; } }

        /* Functions */

        private void Start()
        {
            // get the new freeze position.
            if (IsLocalPosition)
                this.mPositionToFreeze = this.LocalPosition;
            else
                this.mPositionToFreeze = this.Position;
        }

        private void Update()
        {
            if (!mActive)
                return;

            DoFreezePosition();
        }

        /// <summary>
        /// Freeze position.
        /// </summary>
        private void DoFreezePosition()
        {
            Vector3 newPos = this.Position;
            if (IsLocalPosition)
                newPos = this.LocalPosition;

            /* Freeze position */
            if (mFreezePosition.check1)
                newPos.x = mPositionToFreeze.x;
            if (mFreezePosition.check2)
                newPos.y = mPositionToFreeze.y;
            if (mFreezePosition.check3)
                newPos.z = mPositionToFreeze.z;

            /* Apply new value to transform. */
            if (IsLocalPosition)
                this.LocalPosition = newPos;
            else
                this.Position = newPos;
        }
    }
}
