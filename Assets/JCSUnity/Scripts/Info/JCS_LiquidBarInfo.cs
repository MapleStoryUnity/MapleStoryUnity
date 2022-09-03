/**
 * $File: JCS_LiquidBarInfo.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Info to plugin to liquid bar, in order to sync to information between 
    /// the value and the GUI layer.
    /// </summary>
    public class JCS_LiquidBarInfo : MonoBehaviour
    {
        /* Variables */

        [Header("** Check Variables (JCS_LiquidBarInfo) **")]

        [Tooltip("Liquid bar this info object holds.")]
        [SerializeField]
        protected JCS_LiquidBar mLiquidBar = null;

        [Header("** Runtime Variables (JCS_LiquidBarInfo) **")]

        [Tooltip("Tag name to identify the same components.")]
        [SerializeField]
        protected string mTagName = "";

        [Tooltip("Current value in liquid bar.")]
        [SerializeField]
        [Range(JCS_LiquidBar.MIN_LIQUID_BAR_VALUE, JCS_LiquidBar.MAX_LIQUID_BAR_VALUE)]
        protected int mCurrentValue = 50;

        [Tooltip("Min value in liquid bar.")]
        [SerializeField]
        [Range(JCS_LiquidBar.MIN_LIQUID_BAR_VALUE, JCS_LiquidBar.MAX_LIQUID_BAR_VALUE)]
        protected int mMinValue = 0;

        [Tooltip("Max value in liquid bar.")]
        [SerializeField]
        [Range(JCS_LiquidBar.MIN_LIQUID_BAR_VALUE, JCS_LiquidBar.MAX_LIQUID_BAR_VALUE)]
        protected int mMaxValue = 100;

        /* Setter & Getter */

        public JCS_LiquidBar LiquidBar { get { return this.mLiquidBar; } set { this.mLiquidBar = value; } }

        public string TagName { get { return this.mTagName; } set { this.mTagName = value; } }
        public int CurrentValue
        {
            get { return this.mCurrentValue; }
            set
            {
                this.mCurrentValue = value;

                // cannot lower than min value
                if (mCurrentValue < mMinValue)
                    mCurrentValue = mMinValue;

                // cannot higher than max value
                if (mCurrentValue >= mMaxValue)
                    mCurrentValue = mMaxValue;
            }
        }
        public int MinValue { get { return this.mMinValue; } set { this.mMinValue = value; } }
        public int MaxValue { get { return this.mMaxValue; } set { this.mMaxValue = value; } }

        /* Functions */

    }
}
