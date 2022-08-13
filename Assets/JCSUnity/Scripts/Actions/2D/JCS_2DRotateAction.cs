/**
 * $File: JCS_2DRotateAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Action that make gameobject rotates.
    /// </summary>
    public class JCS_2DRotateAction : MonoBehaviour , JCS_IAction
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_2DRotateAction) **")]

        [Tooltip("Do the action?")]
        [SerializeField]
        private bool mAction = true;

        [Tooltip("How fast it rotates?")]
        [SerializeField] [Range(-1000.0f, 1000.0f)]
        private float mTurnSpeed = 1000.0f;

        [Tooltip("Rotate direction.")]
        [SerializeField]
        private JCS_2DFaceType mRotateDirection = JCS_2DFaceType.FACE_LEFT;

        /* Setter & Getter */

        public bool Action { get { return this.mAction; } set { this.mAction = value; } }
        public float TurnSpeed { get { return this.mTurnSpeed; } set { this.mTurnSpeed = value; } }
        public JCS_2DFaceType RotateDirection { get { return this.mRotateDirection; } set { this.mRotateDirection = value; } }

        /* Functions */

        private void Update()
        {
            if (!mAction)
                return;

            this.transform.Rotate(Vector3.forward * mTurnSpeed * -((int)mRotateDirection) * Time.deltaTime);
        }
    }
}
