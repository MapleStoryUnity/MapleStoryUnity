/**
 * $File: JCS_2DPositionPlatform.cs $
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
    /// Accurate platform calculate the movement 
    /// base on the position.
    /// </summary>
    public class JCS_2DPositionPlatform : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_2DPositionPlatform)")]

        [Tooltip("Collider will detect to see if the player could stand ontop of the platform collider.")]
        [SerializeField]
        private BoxCollider mPlatformTrigger = null;

        [Tooltip("Collider will actually stop the player.")]
        [SerializeField]
        private BoxCollider mPlatformCollider = null;

        [Tooltip("Can this platform be down jump?")]
        [SerializeField]
        private bool mCanBeDownJump = true;

        [Tooltip(@"Can this platform be ignore by the fly action? If 
true meaning the fly action object cannot go throught this platform.")]
        [SerializeField]
        private bool mCannotBeGoThrough = false;

        /* Setter & Getter */

        public BoxCollider GetPlatformTrigger() { return this.mPlatformTrigger; }
        public BoxCollider GetPlatformCollider() { return this.mPlatformCollider; }
        public bool CannotBeGoThrough { get { return this.mCannotBeGoThrough; } }

        /* Functions */

        protected void Start()
        {
            // add to list
            JCS_2DGameManager.instance.AddPlatformList(this);
        }

        protected void OnTriggerStay(Collider other)
        {
            CharacterController cc = other.GetComponent<CharacterController>();

            if (cc == null)
                return;

            bool isTopOfBox = JCS_Physics.TopOfBoxWithSlope(cc, mPlatformCollider);

            if (isTopOfBox)
            {
                Physics.IgnoreCollision(
                    mPlatformCollider,
                    cc,
                    false);
            }
            else
            {
                Physics.IgnoreCollision(
                    mPlatformCollider,
                    cc,
                    true);
            }

            JCS_2DSideScrollerPlayer p = other.GetComponent<JCS_2DSideScrollerPlayer>();
            if (p == null)
                return;

            bool isJumpDown = p.IsDownJump();

            if (!mCanBeDownJump)
            {
                // if cannot be down jump, fore it to false.
                isJumpDown = false;
            }

            if (p.CharacterState == JCS_2DCharacterState.CLIMBING ||
                isJumpDown)
            {
                // IMPORTANT(JenChieh): Note that IgnoreCollision will reset 
                // the trigger state of affected colliders, 
                // so you might receive OnTriggerExit and 
                // OnTriggerEnter messages in response to 
                // calling this.
                Physics.IgnoreCollision(
                    mPlatformCollider,
                    p.GetCharacterController(),
                    true);
            }

            if (isJumpDown && isTopOfBox)
            {
                if (JCS_PlatformSettings.instance != null)
                {

                    /**
                     * Make the player go down ward, so it will not stop by
                     * the other collision detection. In order not to let the 
                     * render frame goes off. set the value as small as possible.
                     */
                    p.VelY = -JCS_PlatformSettings.instance.POSITION_PLATFORM_DOWN_JUMP_FORCE;
                }
                else
                {
                    JCS_Debug.Log(
                        "No platform setting, could not set the down jump force...");
                }
            }

            p.ResetingCollision = true;
        }
    }
}
