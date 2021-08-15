/**
 * $File: JCS_2DLight.cs $
 * $Date: 2017-04-06 22:37:25 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

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

        [Header("** Runtime Variables (JCS_2DLight) **")]

        [Tooltip("Flag for active this component.")]
        [SerializeField]
        private bool mActive = true;

        [Header("** Min / Max Settings (JCS_2DLight) **")]

        [Tooltip("Mininum value of the light can fade.")]
        [SerializeField] [Range(0, 1.0f)]
        private float mMinFadeValue = 0.0f;

        [Tooltip("Maxinum value of the light can fade.")]
        [SerializeField]
        [Range(0, 1.0f)]
        private float mMaxFadeValue = 1.0f;

        /* Setter & Getter */

        public bool Active { get { return this.mActive; } set { this.mActive = value; } }

        public float MinFadeValue { get { return this.mMinFadeValue; } set { this.mMinFadeValue = value; } }
        public float MaxFadeValue { get { return this.mMaxFadeValue; } set { this.mMaxFadeValue = value; } }

        /* Functions */

        private void Awake()
        {
            this.mAlphaObject = this.GetComponent<JCS_AlphaObject>();
            this.mAdjustTimeTrigger = this.GetComponent<JCS_AdjustTimeTrigger>();

            this.mAdjustTimeTrigger.actions = DoFade;
        }

        /// <summary>
        /// Do the fade algorithm.
        /// </summary>
        private void DoFade()
        {
            this.mAlphaObject.TargetAlpha = JCS_Random.Range(mMinFadeValue, mMaxFadeValue);
        }
    }
}
