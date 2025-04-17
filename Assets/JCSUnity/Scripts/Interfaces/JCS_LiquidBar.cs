/**
 * $File: JCS_LiquidBar.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Liquid bar object's interface declaration.
    /// </summary>
    public abstract class JCS_LiquidBar : MonoBehaviour
    {
        /* Variables */

        public const float MIN_LIQUID_BAR_VALUE = -999999999;
        public const float MAX_LIQUID_BAR_VALUE = 999999999;

        // When value reachs min/max value.
        public Action callback_min = null;
        public Action callback_max = null;

        // When value reachs min/max value. (visually)
        public Action callback_min_vis = null;
        public Action callback_max_vis = null;

        [Separator("Check Variables (JCS_LiquidBar)")]

        [SerializeField]
        protected float mMinPos = 0.0f;
        [SerializeField]
        protected float mMaxPos = 0.0f;

        [SerializeField]
        protected bool mReachMin = false;
        [SerializeField]
        protected bool mReachMax = false;

        [SerializeField]
        protected bool mReachMinVis = false;
        [SerializeField]
        protected bool mReachMaxVis = false;

        [Separator("Initialize Variables (JCS_LiquidBar)")]

        [Tooltip("Once it set to zero, but still override this.")]
        [SerializeField]
        protected bool mOverrideZero = false;

        [Tooltip("Align on which side? (top/bottom/right/left)")]
        [SerializeField]
        protected JCS_Align mAlign = JCS_Align.ALIGN_LEFT;

        [Separator("Runtime Variables (JCS_LiquidBar)")]

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        protected JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Tooltip(@"How fast the liquid bar move approach to target position/value.")]
        [SerializeField]
        [Range(0.01f, 10.0f)]
        protected float mDeltaFriction = 0.2f;

        [Tooltip("Distance threshold to do visualize callback.")]
        [SerializeField]
        [Range(0.0001f, 10.0f)]
        protected float mDistanceThreshold = 0.1f;

        [Tooltip("Mininum value of the liquid bar.")]
        [SerializeField]
        [Range(MIN_LIQUID_BAR_VALUE, MAX_LIQUID_BAR_VALUE)]
        protected float mMinValue = 0.0f;

        [Tooltip("Maxinum value of the liquid bar.")]
        [SerializeField]
        [Range(MIN_LIQUID_BAR_VALUE, MAX_LIQUID_BAR_VALUE)]
        protected float mMaxValue = 100.0f;

        [Tooltip("Current liquid bar value.")]
        [SerializeField]
        protected float mCurrentValue = 50.0f;

        [Separator("Optional Variables (JCS_LiquidBar)")]

        [Tooltip("Liquid bar info that will be use for this liquid bar.")]
        [SerializeField]
        protected JCS_LiquidBarInfo mInfo = null;

        [Tooltip("Information Image set here.")]
        [SerializeField]
        protected Image mInfoImage = null;

        [Header("- Asmptotic Recover")]

        [Tooltip("Enable the recover effect?")]
        [SerializeField]
        protected bool mRecoverEffect = true;

        [Tooltip(@"Time for one recover. 
If the time is minimal will recover like per frame.")]
        [SerializeField]
        [Range(0.01f, 10.0f)]
        protected float mTimeToRecover = 1.0f;

        // timer to do one recover.
        protected float mRecoverTimer = 0.0f;

        [Tooltip(@"Recover Value per time. 
Careful that recover can be damage too.")]
        [SerializeField]
        [Range(-300000.0f, 300000.0f)]
        protected float mRecoverValue = 1.0f;

        [Header("- Additional Settings")]

        [Tooltip("Will try to go back to the original value.")]
        [SerializeField]
        protected bool mBackToRecordRecoverValue = true;

        // record down the value in order to get 
        // back the original value.
        protected float mRecordValue = 0.0f;

        [SerializeField]
        [Range(0.01f, 7.0f)]
        protected float mGetBackFriction = 1.0f;

        /* Setter & Getter */

        public JCS_LiquidBarInfo Info { get { return this.mInfo; } set { this.mInfo = value; } }
        public Image InfoImage { get { return this.mInfoImage; } set { this.mInfoImage = value; } }

        public bool OverrideZero { get { return this.mOverrideZero; } set { this.mOverrideZero = value; } }

        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        public float DeltaFriction { get { return this.mDeltaFriction; } set { this.mDeltaFriction = value; } }
        public float DistanceThreshold { get { return this.mDistanceThreshold; } set { this.mDistanceThreshold = value; } }

        public bool RecoverEffect { get { return this.mRecoverEffect; } set { this.mRecoverEffect = value; } }
        public float TimeToRecover { get { return this.mTimeToRecover; } set { this.mTimeToRecover = value; } }
        public float RecoverValue { get { return this.mRecoverValue; } set { this.mRecoverValue = value; } }

        public float MinValue { get { return this.mMinValue; } }
        public float MaxValue { get { return this.mMaxValue; } }

        public bool BackToRecordRecoverValue { get { return this.mBackToRecordRecoverValue; } set { this.mBackToRecordRecoverValue = value; } }

        public JCS_Align GetAlign() { return this.mAlign; }

        /* Functions */

        protected virtual void Awake()
        {
            // record down the recover value.
            this.mRecordValue = this.mRecoverValue;

            // get the image 
            if (mInfoImage == null)
                this.mInfoImage = this.GetComponent<Image>();

            if (mInfo != null)
                mInfo.LiquidBar = this;
        }

        protected virtual void Update()
        {
            // 1) if the recover effect is off no need to  get back the recorded
            // recover value. 
            // 2) check the effect is on/off.
            if (!mRecoverEffect || !mBackToRecordRecoverValue)
                return;

            // Try to get back the original value (which we record down in 
            // the Awake function).
            this.mRecoverValue += (mRecordValue - mRecoverValue) / mGetBackFriction * JCS_Time.ItTime(mTimeType);
        }

        /// <summary>
        /// Attach 2d live object so it will follow the value from this object.
        /// </summary>
        /// <param name="obj"> Info for liquid bar to follow. </param>
        public abstract void AttachInfo(JCS_LiquidBarInfo info);

        /// <summary>
        /// Current value in liquid bar.
        /// </summary>
        /// <returns> value in liquid bar </returns>
        public abstract float GetCurrentValue();

        /// <summary>
        /// Check if the value are able to cast.
        /// Mana value must higher than the cast value.
        /// </summary>
        /// <param name="val"> value to cast </param>
        /// <returns> true: able to cast, 
        ///           false: not able to cast </returns>
        public abstract bool IsAbleToCast(float val);

        /// <summary>
        /// Check if able to cast the spell, 
        /// if true cast it.
        /// </summary>
        /// <param name="val"> value to cast </param>
        public abstract bool IsAbleToCastCast(float val);

        /// <summary>
        /// Delta to current value in absolute negative value.
        /// </summary>
        public abstract void DeltaSub(float val);

        /// <summary>
        /// Delta to current value in absolute positive value.
        /// </summary>
        public abstract void DeltaAdd(float val);

        /// <summary>
        /// Delta the current value.
        /// </summary>
        /// <param name="deltaVal"> delta to current value's value </param>
        public abstract void DeltaCurrentValue(float deltaVal);

        /// <summary>
        /// zero the liquid bar.
        /// </summary>
        public abstract void Lack();

        /// <summary>
        /// Full the liquid bar.
        /// </summary>
        public abstract void Full();

        /// <summary>
        /// Return true, if liquid bar is empty.
        /// </summary>
        public abstract bool IsEmpty();

        /// <summary>
        /// Return true, if liquid bar is full.
        /// </summary>
        public abstract bool IsFull();

        /// <summary>
        /// Return true, if liquid bar visually is empty.
        /// </summary>
        public abstract bool IsVisuallyEmpty();

        /// <summary>
        /// Return true, if liquid bar visually is full.
        /// </summary>
        public abstract bool IsVisuallyFull();

        /// <summary>
        /// Set the value directly.
        /// </summary>
        /// <param name="val"> value to set </param>
        public abstract void SetCurrentValue(float val);

        /// <summary>
        /// Set the mininum value of this container.
        /// </summary>
        /// <param name="val"> value to min </param>
        public abstract void SetMinValue(float val);

        /// <summary>
        /// Set the maxinum value of this container.
        /// </summary>
        /// <param name="val"> value to max </param>
        public abstract void SetMaxValue(float val);

        /// <summary>
        /// Set the source sprite from the Image component in Unity Engine.
        /// </summary>
        /// <param name="sprite"></param>
        public void SetInfoSprite(Sprite sprite)
        {
            if (mInfoImage == null)
            {
                JCS_Debug.LogError("Can't set the sprite without the image component.");
                return;
            }

            mInfoImage.sprite = sprite;
        }
    }
}
