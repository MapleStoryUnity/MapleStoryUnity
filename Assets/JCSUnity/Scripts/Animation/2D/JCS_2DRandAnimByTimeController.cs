/**
 * $File: JCS_2DRandAnimByTimeController.cs $
 * $Date: 2017-05-10 17:49:29 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Randomly play animation in animator object depends on time
    /// object which is the 'JCS_AdjustTimeTrigger' component.
    /// </summary>
    [RequireComponent(typeof(JCS_AdjustTimeTrigger))]
    [RequireComponent(typeof(JCS_2DAnimator))]
    public class JCS_2DRandAnimByTimeController : MonoBehaviour
    {
        /* Variables */

        private JCS_AdjustTimeTrigger mAdjustTimeTrigger = null;

        private JCS_2DAnimator m2DAnimator = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            this.m2DAnimator = this.GetComponent<JCS_2DAnimator>();
            this.mAdjustTimeTrigger = this.GetComponent<JCS_AdjustTimeTrigger>();

            this.mAdjustTimeTrigger.actions = RandomPlayAnimationInAnimator;
        }

        /// <summary>
        /// Play a random animation in the animator.
        /// </summary>
        private void RandomPlayAnimationInAnimator()
        {
            int animLength = m2DAnimator.AnimationsLength;
            if (animLength == 0)
                return;

            /* 
             * Just pick a random animation from the animator's animation array. 
             */
            int randIndex = JCS_Random.Range(0, animLength);

            // play this animation
            m2DAnimator.DoAnimation(randIndex);
        }
    }
}
