/**
 * $File: JCS_SpriteRendererAction.cs $
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
    /// Action flip the sprite renderer to the vector direction.
    /// </summary>
    public class JCS_SpriteRendererAction : MonoBehaviour
    {
        /* Variables */

        /* Down compatible. */
        private SpriteRenderer mSpriteRenderer = null;
        private Vector3 mLastPosition = Vector3.zero;

        [Separator("⚡️ Runtime Variables (JCS_SpriteRendererAction)")]

        [Tooltip("List of all the sprite renderer components.")]
        [SerializeField]
        private SpriteRenderer[] mSpriteRenderers = null;

        [Header("🔍 -- X Facing --")]

        [Tooltip("Enable/Disabel the effect on X axis.")]
        [SerializeField]
        private bool mFreezeX = false;

        [Tooltip("Effect only when direction is horizontal.")]
        [SerializeField]
        private bool mIsFacingRight = true;

        [Header("🔍 -- Y Facing --")]

        [Tooltip("Enable/Disabel the effect on Y axis.")]
        [SerializeField]
        private bool mFreezeY = false;

        [Tooltip("Effect only when direction is vertical.")]
        [SerializeField]
        private bool mIsFacingUp = true;

        /* Setter & Getter */

        public bool isFacingUp { get { return mIsFacingUp; } }
        public bool isFacingRight { get { return mIsFacingRight; } }
        public bool freezeX { get { return mFreezeX; } set { mFreezeX = value; } }
        public bool freezeY { get { return mFreezeY; } set { mFreezeY = value; } }

        /* Functions */

        private void Awake()
        {
            // try to get the sprite renderer
            mSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            Vector3 currentPos = transform.position;

            // if the position are the same, meaning the object
            // is idle. (have not been move)
            if (currentPos == mLastPosition)
                return;

            DoSpriteAction(mSpriteRenderer, currentPos);

            foreach (SpriteRenderer sr in mSpriteRenderers)
            {
                DoSpriteAction(sr, currentPos);
            }

            // update last position
            mLastPosition = currentPos;
        }

        /// <summary>
        /// Do the sprite action.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="currentPos"></param>
        private void DoSpriteAction(SpriteRenderer sr, Vector3 currentPos)
        {
            if (sr == null)
                return;

            if (!mFreezeX)
            {
                // object going left
                if (currentPos.x < mLastPosition.x)
                {
                    if (mIsFacingRight)
                        sr.flipX = false;
                    else
                        sr.flipX = true;
                }
                // object going right
                else if (currentPos.x > mLastPosition.x)
                {
                    if (mIsFacingRight)
                        sr.flipX = true;
                    else
                        sr.flipX = false;
                }

                if (JCS_Util.WithInRange(90, 270, transform.localEulerAngles.z))
                    sr.flipX = !sr.flipX;
            }

            if (!mFreezeY)
            {
                // object going down
                if (currentPos.y < mLastPosition.y)
                {
                    if (mIsFacingUp)
                        sr.flipY = false;
                    else
                        sr.flipY = true;
                }
                // object going up
                else if (currentPos.y > mLastPosition.y)
                {
                    if (mIsFacingUp)
                        sr.flipY = true;
                    else
                        sr.flipY = false;
                }

                // TODO(JenChieh): this have not test yet!!!
                if (JCS_Util.WithInRange(90, 270, transform.localEulerAngles.z))
                    sr.flipY = !sr.flipY;
            }
        }
    }
}
