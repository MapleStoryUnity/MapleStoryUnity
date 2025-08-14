/**
 * $File: JCS_2DLight.cs $
 * $Date: 2017-04-06 22:37:25 $
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
    /// 2D light like 'MapleStory ' v62 login screen.
    /// </summary>
    [RequireComponent(typeof(JCS_AlphaObject))]
    [RequireComponent(typeof(JCS_AdjustTimeTrigger))]
    public class JCS_2DLight : MonoBehaviour
    {
        /* Variables */

        private JCS_AlphaObject mAlphaObject = null;

        private JCS_AdjustTimeTrigger mAdjustTimeTrigger = null;

        [Separator("Runtime Variables (JCS_2DLight)")]

        [Tooltip("Flag for active this component.")]
        [SerializeField]
        private bool mActive = true;

        [Header("- Min / Max")]

        [Tooltip("Mininum value of the light can fade.")]
        [SerializeField] [Range(0, 1.0f)]
        private float mMinFadeValue = 0.0f;

        [Tooltip("Maxinum value of the light can fade.")]
        [SerializeField]
        [Range(0, 1.0f)]
        private float mMaxFadeValue = 1.0f;

        /* Setter & Getter */

        public bool active { get { return mActive; } set { mActive = value; } }

        public float minFadeValue { get { return mMinFadeValue; } set { mMinFadeValue = value; } }
        public float maxFadeValue { get { return mMaxFadeValue; } set { mMaxFadeValue = value; } }

        /* Functions */

        private void Awake()
        {
            mAlphaObject = GetComponent<JCS_AlphaObject>();
            mAdjustTimeTrigger = GetComponent<JCS_AdjustTimeTrigger>();

            mAdjustTimeTrigger.onAction = DoFade;
        }

        /// <summary>
        /// Do the fade algorithm.
        /// </summary>
        private void DoFade()
        {
            mAlphaObject.targetAlpha = JCS_Random.Range(mMinFadeValue, mMaxFadeValue);
        }
    }
}
