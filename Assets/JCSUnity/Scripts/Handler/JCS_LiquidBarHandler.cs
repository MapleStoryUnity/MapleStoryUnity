/**
 * $File: JCS_LiquidBarHandler.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Handle liquid bar bar, etc.
    /// </summary>
    public class JCS_LiquidBarHandler
        : MonoBehaviour
    {
        /* Variables */

        [Header("** Initialize Variables (JCS_LiquidBarHandler) **")]

        [Tooltip("General Liquid bar u want to handle.")]
        [SerializeField]
        private JCS_LiquidBar mLiquidBar = null;


        /* Setter & Getter */

        public JCS_LiquidBar LiquidBar { get { return this.mLiquidBar; } }


        /* Functions */

        private void Awake()
        {
            if (mLiquidBar == null)
                mLiquidBar = this.GetComponent<JCS_LiquidBar>();
        }

        /// <summary>
        /// Delta the current value.
        /// </summary>
        /// <param name="deltaVal"> delta to current value's value </param>
        public void SetCurrentValue(float val)
        {
            mLiquidBar.SetCurrentValue(val);
        }

        /// <summary>
        /// Full the liquid bar.
        /// </summary>
        public void Full()
        {
            mLiquidBar.Full();
        }

        /// <summary>
        /// zero the liquid bar.
        /// </summary>
        public void Lack()
        {
            mLiquidBar.Lack();
        }

        /// <summary>
        /// Check if the value are able to cast.
        /// Mana value must higher than the cast value.
        /// </summary>
        /// <param name="val"> value to cast </param>
        /// <returns> true: able to cast, 
        ///           false: not able to cast </returns>
        public bool IsAbleToCast(float val)
        {
            return mLiquidBar.IsAbleToCast(val);
        }

        /// <summary>
        /// Check if able to cast the spell, 
        /// if true cast it.
        /// </summary>
        /// <param name="val"> value to cast </param>
        public bool IsAbleToCastCast(float val)
        {
            return mLiquidBar.IsAbleToCastCast(val);
        }

        /// <summary>
        /// Delta the current value.
        /// </summary>
        /// <param name="deltaVal"> delta to current value's value </param>
        public void DeltaCurrentValue(float val)
        {
            mLiquidBar.DeltaCurrentValue(val);
        }

        /// <summary>
        /// Delta to current value in absolute positive value.
        /// </summary>
        public void DeltaAdd(float val)
        {
            mLiquidBar.DeltaAdd(val);
        }

        /// <summary>
        /// Delta to current value in absolute negative value.
        /// </summary>
        public void DeltaSub(float val)
        {
            mLiquidBar.DeltaSub(val);
        }

        /// <summary>
        /// Attach 2d live object so it will follow the
        /// value from this object.
        /// </summary>
        /// <param name="obj"> Info for liquid bar to follow. </param>
        public void AttachInfo(JCS_LiquidBarInfo info)
        {
            mLiquidBar.AttachInfo(info);
        }
    }
}
