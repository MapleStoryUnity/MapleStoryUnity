/**
 * $File: JCS_BlackSlideScreen.cs $
 * $Date: 2016-12-10 06:05:30 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Black Slide Screen
    /// </summary>
    [RequireComponent(typeof(JCS_TransformTweener))]
    public class JCS_BlackSlideScreen : MonoBehaviour
    {
        /* Variables */

        private JCS_TransformTweener mTweener = null;

        private Vector3 mStartingPosition = Vector3.zero;

        /* Setter & Getter */

        /* Functions */
        private void Awake()
        {
            this.mTweener = this.GetComponent<JCS_TransformTweener>();
        }

        private void Start()
        {
            // everytime it reload the scene.
            // move to the last child make sure everything get cover by this.
            JCS_Util.MoveToTheLastChild(this.transform);

            this.mStartingPosition = this.transform.localPosition;
        }

        /// <summary>
        /// Start sliding the screen in the scene.
        /// </summary>
        /// <param name="align"> align type </param>
        public void StartSlideIn(JCS_Align align, float time)
        {
            mTweener.DurationX = time;
            mTweener.DurationY = time;
            mTweener.DurationZ = time;

            // tween back to where we are.
            mTweener.DoTween(mStartingPosition);
        }

        /// <summary>
        /// Start sliding the screen out the scene.
        /// </summary>
        /// <param name="align"> align type </param>
        public void StartSlideOut(JCS_Align align, float time)
        {
            Vector2 sizeDelta = JCS_Canvas.GuessCanvas().AppRect.sizeDelta;

            float imageSize = 1200;
            float distanceX = sizeDelta.x + imageSize;
            float distanceY = sizeDelta.y + imageSize;

            // NOTE(jenchieh): this is just some tweeking.
            mTweener.DurationX = time;
            mTweener.DurationY = time;
            mTweener.DurationZ = time;

            Vector3 tweenTo = this.transform.localPosition;

            switch (align)
            {
                // going left showing from right
                // ============--------------
                case JCS_Align.ALIGN_RIGHT:
                    {
                        tweenTo.x = JCS_Mathf.ToNegative(distanceX);
                    }
                    break;

                // going right showing from left
                // -----------=============
                case JCS_Align.ALIGN_LEFT:
                    {
                        tweenTo.x = JCS_Mathf.ToPositive(distanceX);
                    }
                    break;

                case JCS_Align.ALIGN_BOTTOM:
                    {
                        tweenTo.y = JCS_Mathf.ToPositive(distanceY);
                    }
                    break;
                case JCS_Align.ALIGN_TOP:
                    {
                        tweenTo.y = JCS_Mathf.ToNegative(distanceY);
                    }
                    break;
            }

            // start sliding
            mTweener.DoTween(tweenTo);
        }

        /// <summary>
        /// Is done sliding screen?
        /// </summary>
        /// <returns>
        /// true: done sliding
        /// false: not done sliding
        /// </returns>
        public bool IsDoneSliding()
        {
            return mTweener.IsDoneTweening;
        }

        /// <summary>
        /// Move the panel to front
        /// </summary>
        public void MoveToTheLastChild()
        {
            Transform parent = this.transform.parent;

            Vector3 recordPos = this.transform.localPosition;
            Vector3 recordScale = this.transform.localScale;
            Quaternion recordRot = this.transform.localRotation;

            // this part will mess up the transform
            // so we record all we need and set it back
            {
                this.transform.SetParent(null);
                this.transform.SetParent(parent);
            }

            // here we set it back!
            this.transform.localPosition = recordPos;
            this.transform.localScale = recordScale;
            this.transform.localRotation = recordRot;
        }
    }
}
