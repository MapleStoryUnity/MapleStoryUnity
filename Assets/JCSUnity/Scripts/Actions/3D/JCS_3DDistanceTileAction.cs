/**
 * $File: JCS_3DDistanceTileAction.cs $
 * $Date: 2017-04-07 02:04:46 $
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
    /// Move a game object in certain distance then set the game object
    /// back to original position relative to the game object that moved.
    /// </summary>
    [RequireComponent(typeof(JCS_3DGoStraightAction))]
    public class JCS_3DDistanceTileAction : MonoBehaviour
    {
        /* Variables */

        public EmptyFunction beforeResetCallback = null;
        public EmptyFunction afterResetCallback = null;

        private Vector3 mOriginPos = Vector3.zero;

        [Separator("Runtime Variables (JCS_3DDistanceTileAction)")]

        [Tooltip(@"Reset to this position. If this is null, we use original position instead.")]
        [SerializeField]
        private Transform mResetTrans = null;

        [Tooltip("How long this game object could travel.")]
        [SerializeField]
        [Range(0.0f, 30000.0f)]
        private float mDistance = 0.0f;

        [Tooltip("Use the local position instead of global position.")]
        [SerializeField]
        private bool mUseLocalPosition = false;

        /* Setter & Getter */
        public Transform ResetTrans { get { return this.mResetTrans; } set { this.mResetTrans = value; } }
        public float Distance { get { return this.mDistance; } set { this.mDistance = value; } }
        public bool UseLocalPosition { get { return this.mUseLocalPosition; } set { this.mUseLocalPosition = value; } }

        /* Functions */

        private void Start()
        {
            // NOTE(jenchieh): prevent if other component want to change
            // this position, we have the record position at function
            // 'Start' rather than 'Awake'
            if (mUseLocalPosition)
                this.mOriginPos = this.transform.localPosition;
            else
                this.mOriginPos = this.transform.position;
        }

        private void Update()
        {
            // Find out the distance between the current moved
            // position and the starting position.
            float distance;
            if (mUseLocalPosition)
                distance = Vector3.Distance(this.transform.localPosition, GetResetPosition());
            else
                distance = Vector3.Distance(this.transform.position, GetResetPosition());

            // check if the distance reach?
            if (distance < mDistance)
                return;

            // set back to original position.
            ResetPosition();
        }

        /// <summary>
        /// Set back to starting position.
        /// </summary>
        public void ResetPosition()
        {
            if (beforeResetCallback != null)
                beforeResetCallback.Invoke();

            if (mUseLocalPosition)
                this.transform.localPosition = GetResetPosition();
            else
                this.transform.position = GetResetPosition();

            if (afterResetCallback != null)
                afterResetCallback.Invoke();
        }

        /// <summary>
        /// Get the correct reset position base on the variables.
        /// </summary>
        private Vector3 GetResetPosition()
        {
            if (mResetTrans != null)
                return mResetTrans.position;
            return mOriginPos;
        }
    }
}
