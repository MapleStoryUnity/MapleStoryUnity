/**
 * $File: JCS_CharacterControllerInfo.cs $
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
    /// Provide all the info that scripter needed for 
    /// scripting the character's movement, collision 
    /// detection, etc.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class JCS_CharacterControllerInfo : MonoBehaviour
    {
        /* Variables */

        private CharacterController mCharacterController = null;

        [Separator("📋 Check Variabless (JCS_CharacterControllerInfo)")]

        [Tooltip("Width of the character controller.")]
        [SerializeField]
        [ReadOnly]
        private float mWidth = 0;

        [Tooltip("Height of the character controller.")]
        [SerializeField]
        [ReadOnly]
        private float mHeight = 0;

        /* Setter & Getter */

        public CharacterController GetCharacterController() { return mCharacterController; }
        public bool isGrounded { get { return mCharacterController.isGrounded; } }
        public float width { get { return mWidth; } }
        public float height { get { return mHeight; } }
        public float halfWidth { get { return width / 2.0f; } }
        public float HhalfHeight { get { return height / 2.0f; } }

        /* Functions */

        private void Awake()
        {
            mCharacterController = GetComponent<CharacterController>();

            Vector2 widthHeight = JCS_Physics.GetColliderWidthHeight(mCharacterController);

            mWidth = widthHeight.x;
            mHeight = widthHeight.y;
        }
    }
}
