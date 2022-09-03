/**
 * $File: JCS_AlphaObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */

/* NOTE: If you are using `TextMesh Pro` uncomment this line.
 */
#define TMP_PRO

using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Transition with alpha channel.
    /// </summary>
    public class JCS_AlphaObject : JCS_UnityObject
    {
        /* Variables */

        private Color mRecordColor;

        private float mAlpha = 1;

        [Header("** Runtime Variables (JCS_AlphaObject) **")]

        [Tooltip("Alpha value trying to approach. (0 ~ 1)")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mTargetAlpha = 1.0f;

        [Tooltip("How fast the alpha channel changes.")]
        [SerializeField]
        [Range(0.1f, 5.0f)]
        private float mFadeFriction = 1.0f;

        /* Setter & Getter */

        public float TargetAlpha { get { return this.mTargetAlpha; } set { this.mTargetAlpha = value; } }
        public float FadeFriction { get { return this.mFadeFriction; } set { this.mFadeFriction = value; } }

        /* Functions */

        private void Update()
        {
            if (mAlpha == mTargetAlpha)
                return;

            this.mAlpha += (mTargetAlpha - mAlpha) / mFadeFriction * Time.deltaTime;

            this.LocalAlpha = this.mAlpha;
        }

        /// <summary>
        /// Get unity specific data by type.
        /// </summary>
        public override void UpdateUnityData()
        {
            base.UpdateUnityData();

            switch (GetObjectType())
            {
                case JCS_UnityObjectType.GAME_OBJECT:
                    {
                        if (this.mRenderer)
                            this.mRecordColor = this.mRenderer.material.color;
                    }
                    break;
                case JCS_UnityObjectType.UI:
                    {
                        if (this.mImage)
                            this.mRecordColor = this.mImage.color;
                    }
                    break;
                case JCS_UnityObjectType.SPRITE:
                    {
                        if (this.mSpriteRenderer)
                            this.mRecordColor = this.mSpriteRenderer.color;
                    }
                    break;
                case JCS_UnityObjectType.TEXT:
                    {
                        if (this.mText)
                            this.mRecordColor = this.mText.color;
                    }
                    break;
#if TMP_PRO
                case JCS_UnityObjectType.TMP:
                    {
                        if (this.mTextMesh)
                            this.mRecordColor = this.mTextMesh.color;
                    }
                    break;
#endif
            }
        }

        /// <summary>
        /// Fade to specific value.
        /// </summary>
        /// <param name="alpha"> target alpha. </param>
        /// <param name="friction"> how fast it fades? </param>
        public void FadeTo(float alpha, float friction)
        {
            this.mTargetAlpha = alpha;
            this.mFadeFriction = friction;
        }
    }
}
