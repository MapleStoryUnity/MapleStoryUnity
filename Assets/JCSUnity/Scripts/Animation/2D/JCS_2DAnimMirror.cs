/**
 * $File: JCS_2DAnimMirror.cs $
 * $Date: 2017-05-09 11:53:08 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Completely mimic a JCS_2DAnimation's variables.
    ///   - Frame
    ///   - Sorting Layer
    ///   - Color
    ///   - Flip X/Y
    /// </summary>
    public class JCS_2DAnimMirror : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_2DAnimMirrorAction)")]

        [Tooltip("Is this component active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Animation that we are going to use to mimic.")]
        [SerializeField]
        private JCS_2DAnimation mMirrorAnimation = null;

        [Tooltip("Animations going to follow the main animation.")]
        [SerializeField]
        private List<JCS_2DAnimation> mMimicAnimations = null;

        [Tooltip("Set the same frame index.")]
        [SerializeField]
        private bool mMimicFrame = true;

        [Tooltip("Set the same sorting order.")]
        [SerializeField]
        private bool mMimicSortingOrder = true;

        [Tooltip("Set the same color.")]
        [SerializeField]
        private bool mMimicColor = true;

        [Tooltip(@"Set the same flip x and flip y. If not SpriteRenderer 
use negative scale instead.")]
        [SerializeField]
        private bool mMimcFlip = true;

        /* Setter & Getter */

        public bool active { get { return mActive; } set { mActive = value; } }
        public JCS_2DAnimation nirrorAnimation { get { return mMirrorAnimation; } set { mMirrorAnimation = value; } }
        public List<JCS_2DAnimation> nimicAnimations { get { return mMimicAnimations; } }
        public bool mimicFrame { get { return mMimicFrame; } set { mMimicFrame = value; } }
        public bool mimicSortingOrder { get { return mMimicSortingOrder; } set { mMimicSortingOrder = value; } }
        public bool mimicColor { get { return mMimicColor; } set { mMimicColor = value; } }
        public bool mimcFlip { get { return mMimcFlip; } set { mMimcFlip = value; } }

        /* Functions */

        private void Awake()
        {
            InitMimicAnimations();
        }

        private void LateUpdate()
        {
            if (!mActive)
                return;

            DoMimicAnimations();
        }

        /// <summary>
        /// Initialize all the minicing animations.
        /// </summary>
        private void InitMimicAnimations()
        {
            foreach (JCS_2DAnimation anim in mMimicAnimations)
            {
                if (anim == null)
                    continue;

                anim.active = false;
                anim.playOnAwake = false;
                anim.loop = false;
            }
        }

        /// <summary>
        /// Do all the mimicing for all the mimic animations.
        /// 
        /// Simply just have them exact the same frame.
        /// </summary>
        private void DoMimicAnimations()
        {
            /* Cannot have mirror animation as a null reference... */
            if (mMirrorAnimation == null)
                return;

            SpriteRenderer mirrorSR = (SpriteRenderer)mMirrorAnimation.localType;

            foreach (JCS_2DAnimation anim in mMimicAnimations)
            {
                if (anim == null)
                    continue;

                if (mMimicFrame)
                {
                    if (mMirrorAnimation.localSprite == null)
                        anim.localSprite = null;
                    else
                        anim.PlayFrame(mMirrorAnimation.currentPlayingFrame);
                }

                if (mMimcFlip)
                {
                    anim.localFlipX = mMirrorAnimation.localFlipX;
                    anim.localFlipY = mMirrorAnimation.localFlipY;
                }


#if UNITY_EDITOR
                if (mMirrorAnimation.GetObjectType() != JCS_UnityObjectType.SPRITE ||
                    anim.GetObjectType() != JCS_UnityObjectType.SPRITE)
                {
                    Debug.LogError(
                        "Mimic order layer and mimic color has to be sprite renderer, not something else...");
                    continue;
                }
#endif

                if (mMimicColor)
                {
                    anim.localColor = mMirrorAnimation.localColor;
                }

                SpriteRenderer animSR = (SpriteRenderer)anim.localType;

                if (mMimicSortingOrder)
                {
                    animSR.sortingOrder = mirrorSR.sortingOrder;
                }
            }
        }
    }
}
