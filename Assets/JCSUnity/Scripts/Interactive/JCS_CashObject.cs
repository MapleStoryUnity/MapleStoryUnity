/**
 * $File: JCS_CashObject.cs $
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
    /// In game item represents cash.
    /// </summary>
    public class JCS_CashObject : JCS_Item
    {
        /* Variables */

        [Separator("Initialize Variables (JCS_CashObject)")]

        [Tooltip("Value represent to this cash object.")]
        [SerializeField]
        [Range(1, 99999999)]
        protected int mCashValue = 1;

        [Header("Randomize")]

        [Tooltip("Randomize the cash value at init time?")]
        [SerializeField]
        protected bool mRandomizeCashValueEffect = false;

        [Tooltip("How much it effect the actual cash value.")]
        [SerializeField]
        [Range(0, 500)]
        protected int mRandomizeCashValue = 0;

        /* Setter & Getter */

        public int cashValue { get { return mCashValue; } set { mCashValue = value; } }
        public bool randomizeCashValueEffect { get { return mRandomizeCashValueEffect; } set { mRandomizeCashValueEffect = value; } }
        public int randomizeCashValue { get { return mRandomizeCashValue; } set { mRandomizeCashValue = value; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            // no effect happens
            if (!mRandomizeCashValueEffect ||
                mRandomizeCashValue == 0)
                return;

            // randomize the cash value a bit.
            mCashValue += JCS_Random.Range(-mRandomizeCashValue, mRandomizeCashValue + 1);
        }
    }
}
