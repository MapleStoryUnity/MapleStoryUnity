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
using MyBox;

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

        [Separator("⚡️ Runtime Variables (JCS_AlphaObject)")]

        [Tooltip("Alpha value trying to approach. (0 ~ 1)")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float mTargetAlpha = 1.0f;

        [Tooltip("How fast the alpha channel changes.")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 5.0f)]
        private float mFadeFriction = 1.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public float targetAlpha { get { return mTargetAlpha; } set { mTargetAlpha = value; } }
        public float fadeFriction { get { return mFadeFriction; } set { mFadeFriction = value; } }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        /* Functions */

        private void Update()
        {
            if (mAlpha == mTargetAlpha)
                return;

            mAlpha += (mTargetAlpha - mAlpha) / mFadeFriction * JCS_Time.ItTime(mTimeType);

            localAlpha = mAlpha;
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
                        if (mRenderer)
                            mRecordColor = mRenderer.material.color;
                    }
                    break;
                case JCS_UnityObjectType.UI:
                    {
                        if (mImage)
                            mRecordColor = mImage.color;
                    }
                    break;
                case JCS_UnityObjectType.SPRITE:
                    {
                        if (mSpriteRenderer)
                            mRecordColor = mSpriteRenderer.color;
                    }
                    break;
                case JCS_UnityObjectType.TEXT:
                    {
                        if (mText)
                            mRecordColor = mText.color;
                    }
                    break;
#if TMP_PRO
                case JCS_UnityObjectType.TMP:
                    {
                        if (mTextMesh)
                            mRecordColor = mTextMesh.color;
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
            mTargetAlpha = alpha;
            mFadeFriction = friction;
        }
    }
}
