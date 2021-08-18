/**
 * $File: JCS_3DDistanceTileAction.cs $
 * $Date: 2017-04-07 02:04:46 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    public delegate void ResetCallback();  // Callback when after reset.

    /// <summary>
    /// Move a gameobject in certain distance then set the gameobject
    /// back to original position relative to the gameobject that moved.
    /// </summary>
    [RequireComponent(typeof(JCS_3DGoStraightAction))]
    public class JCS_3DDistanceTileAction : MonoBehaviour
    {
        /* Variables */

        public ResetCallback beforeResetCallback = null;
        public ResetCallback afterResetCallback = null;

        private Vector3 mOriginPos = Vector3.zero;

        [Header("** Runtime Variables (JCS_3DDistanceTileAction) **")]

        [Tooltip("Is this component active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("How long this gameobject could travel.")]
        [SerializeField]
        [Range(0.0f, 30000.0f)]
        private float mDistance = 0.0f;

        [Tooltip("Use the local position instead of global position.")]
        [SerializeField]
        private bool mUseLocalPosition = false;

        /* Setter & Getter */
        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
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
            if (!mActive)
                return;

            // Find out the distance between the current moved
            // position and the starting position.
            float distance = 0.0f;
            if (mUseLocalPosition)
                distance = Vector3.Distance(this.transform.localPosition, mOriginPos);
            else
                distance = Vector3.Distance(this.transform.position, mOriginPos);

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
                this.transform.localPosition = this.mOriginPos;
            else
                this.transform.position = this.mOriginPos;

            if (afterResetCallback != null)
                afterResetCallback.Invoke();
        }
    }
}
