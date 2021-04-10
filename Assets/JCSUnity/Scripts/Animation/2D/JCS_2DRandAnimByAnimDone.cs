/**
 * $File: JCS_2DRandAnimByAnimDone.cs $
 * $Date: 2017-05-14 21:04:50 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Play a random animation if the current animation is done 
    /// playing it.
    /// </summary>
    [RequireComponent(typeof(JCS_2DAnimator))]
    public class JCS_2DRandAnimByAnimDone
        : MonoBehaviour
    {
        /* Variables */

        private JCS_2DAnimator m2DAnimator = null;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            this.m2DAnimator = this.GetComponent<JCS_2DAnimator>();

            foreach (JCS_2DAnimation anim in m2DAnimator.Animations)
            {
                if (anim == null)
                    continue;

                // set the call back.
                anim.donePlayingAnimCallback = RandomPlayAnimationInAnimator;
            }
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
             * Just pick a random animation from the 
             * animator's animation array. 
             */
            int randIndex = JCS_Random.Range(0, animLength);

            // play this animation
            m2DAnimator.DoAnimation(randIndex);
        }
    }
}
