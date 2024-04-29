/**
 * $File: JCS_2DClimbableObject.cs $
 * $Date: 2017-05-02 14:57:23 $
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
    /// Base class for all the 2d climable object.
    /// 
    /// 2DRope, 2DLadder, etc.
    /// </summary>
    [RequireComponent(typeof(JCS_OrderLayerObject))]
    public abstract class JCS_2DClimbableObject : MonoBehaviour
    {
        /* Variables */

        /*
         * To have this component in order to let the 
         * character's sorting layer infont or behind the 
         * climbing object. 
         */
        private JCS_OrderLayerObject mOrderLayerObject = null;

        [Separator("Initilaize Variable (JCS_2DClimbableObject)")]

        [Tooltip("Ground/Platform the ladder lean on.")]
        [SerializeField]
        protected JCS_2DPositionPlatform mPositionPlatform = null;

        /* Setter & Getter */

        public JCS_OrderLayerObject OrderLayerObject { get { return this.mOrderLayerObject; } }
        public JCS_2DPositionPlatform PositionPlatform { get { return this.mPositionPlatform; } }

        /* Functions */

        protected virtual void Awake()
        {
            this.mOrderLayerObject = this.GetComponent<JCS_OrderLayerObject>();
        }

        /// <summary>
        /// Something that needed to check in update can be design here.
        /// This function should get call by when the player is done climbing.
        /// </summary>
        public abstract void ClimbableUpdate();

        /// <summary>
        /// Check if is on top of the lean platform.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>
        /// true: is on top of the box.
        /// false: vice versa.
        /// </returns>
        public virtual bool IsOpTopOfLeanPlatform(JCS_Player player)
        {
            if (mPositionPlatform == null || player == null)
                return false;

            bool isTopOfBox = JCS_Physics.TopOfBox(
                       player.GetCharacterController(),
                       mPositionPlatform.GetPlatformCollider());

            return isTopOfBox;
        }
    }
}
