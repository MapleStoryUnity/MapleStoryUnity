/**
 * $File: JCS_3DLiquidBar.cs $
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
    /// If finding for 2D Liquid bar to do the health bar effect,
    /// please use the "JCS_GUILiquidBar" instead.
    /// </summary>
    public class JCS_3DLiquidBar : JCS_LiquidBar
    {
        /* Variables */

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_3DLiquidBar)")]

        [Tooltip("Test functionalities works?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Make the bar to min value.")]
        [SerializeField]
        private KeyCode mLackKey = KeyCode.J;

        [Tooltip("Make the bar to max value.")]
        [SerializeField]
        private KeyCode mFullKey = KeyCode.K;

        [Tooltip("Make the bar to half value.")]
        [SerializeField]
        private KeyCode mHalfKey = KeyCode.H;

        [Tooltip("Fixed the value if the min and max value has changed.")]
        [SerializeField]
        private KeyCode mFixedKey = KeyCode.X;
#endif

        [Separator("Check Variables (JCS_3DLiquidBar)")]

        [SerializeField]
        [ReadOnly]
        private Vector3 mMaskTargetPosition = Vector3.zero;

        [Separator("Initilaize Variables (JCS_3DLiquidBar)")]

        [Tooltip("Sprite mask that mask out the inner bar sprite.")]
        [SerializeField]
        private SpriteMask mSpriteMask = null;

        [Tooltip("Please put the under texture bar here.")]
        [SerializeField]
        private SpriteRenderer mBarSpriteRenderer = null;

        /* Setter & Getter */

        /* Functions */

        protected void Start()
        {
            GetContainerData();
        }

        protected void LateUpdate()
        {
#if UNITY_EDITOR
            Test();
#endif

            // check all components needed are
            // avaliable.
            if (mBarSpriteRenderer == null)
                return;

            if (mSpriteMask == null)
                return;

            // update value.
            UpdateInfo();

            // do recover
            DoRecover();

            // do gui movement
            TowardToTargetValue();
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(mLackKey))
                Lack();
            if (JCS_Input.GetKeyDown(mFullKey))
                Full();

            // half
            if (JCS_Input.GetKeyDown(mHalfKey))
                SetCurrentValue(maxValue / 2.0f);

            if (JCS_Input.GetKeyDown(mFixedKey))
                FixPercentage();
        }
#endif

        /// <summary>
        /// Attach 2d live object so it will follow the
        /// value from this object.
        /// </summary>
        /// <param name="obj"> Info for liquid bar to follow. </param>
        public override void AttachInfo(JCS_LiquidBarInfo info)
        {
            mInfo = info;

            UpdateInfo();
        }

        /// <summary>
        /// Set the maxinum value of this container.
        /// </summary>
        /// <param name="val"> value to max </param>
        public override void SetMaxValue(float val)
        {
            if (val <= mMinValue)
            {
                Debug.LogError("Max value you set can't be lower than min value.");
                return;
            }

            mMaxValue = val;

            if (mInfo != null)
                mInfo.maxValue = (int)val;

            FixPercentage();
        }

        /// <summary>
        /// Set the mininum value of this container.
        /// </summary>
        /// <param name="val"> value to min </param>
        public override void SetMinValue(float val)
        {
            if (val >= mMaxValue)
            {
                Debug.LogError("Min value you set can be higher than max value");
                return;
            }

            mMinValue = val;

            if (mInfo != null)
                mInfo.minValue = (int)val;

            FixPercentage();
        }

        /// <summary>
        /// Set the value directly.
        /// </summary>
        /// <param name="val"> value to set </param>
        public override void SetCurrentValue(float val)
        {
            if (!mOverrideZero)
            {
                if (mReachMin)
                {
                    mCurrentValue = mMinValue;
                    return;
                }
                else if (mReachMax)
                {
                    mCurrentValue = mMaxValue;
                    return;
                }
            }

            mCurrentValue = val;

            // cannot lower than min container value
            if (mCurrentValue < mMinValue)
                mCurrentValue = mMinValue;

            // cannot higher than max container value
            if (mCurrentValue > mMaxValue)
                mCurrentValue = mMaxValue;

            if (mInfo != null)
                mInfo.currentValue = (int)mCurrentValue;

            FixPercentage();

            // do call back
            DoCallback();
        }

        /// <summary>
        /// Delta the current value.
        /// </summary>
        /// <param name="deltaVal"> delta to current value's value </param>
        public override void DeltaCurrentValue(float deltaVal)
        {
            SetCurrentValue(mCurrentValue + deltaVal);
        }

        /// <summary>
        /// Delta to current value in absolute positive value.
        /// </summary>
        public override void DeltaAdd(float val)
        {
            DeltaCurrentValue(JCS_Mathf.ToPositive(val));
        }

        /// <summary>
        /// Delta to current value in absolute negative value.
        /// </summary>
        public override void DeltaSub(float val)
        {
            DeltaCurrentValue(JCS_Mathf.ToNegative(val));
        }

        /// <summary>
        /// Full the liquid bar.
        /// </summary>
        public override void Full()
        {
            SetCurrentValue(mMaxValue);
        }

        /// <summary>
        /// Zero the liquid bar.
        /// </summary>
        public override void Lack()
        {
            SetCurrentValue(mMinValue);
        }

        /// <summary>
        /// Return true, if liquid bar is empty.
        /// </summary>
        public override bool IsEmpty()
        {
            return mCurrentValue == mMinValue;
        }

        /// <summary>
        /// Return true, if liquid bar is full.
        /// </summary>
        public override bool IsFull()
        {
            return mCurrentValue == mMaxValue;
        }

        /// <summary>
        /// Return true, if liquid bar visually is empty.
        /// </summary>
        public override bool IsVisuallyEmpty()
        {
            return mReachMinVis && IsEmpty();
        }

        /// <summary>
        /// Return true, if liquid bar  visually is full.
        /// </summary>
        public override bool IsVisuallyFull()
        {
            return mReachMaxVis && IsFull();
        }

        /// <summary>
        /// Check if the value are able to cast.
        /// Mana value must higher than the cast value.
        /// </summary>
        /// <param name="val"> value to cast </param>
        /// <returns> 
        /// true, able to cast, 
        /// false, not able to cast 
        /// </returns>
        public override bool IsAbleToCast(float val)
        {
            // able to cast the spell
            if (GetCurrentValue() >= JCS_Mathf.ToPositive(val))
                return true;

            // not able to cast the spell
            return false;
        }

        /// <summary>
        /// Check if able to cast the spell, 
        /// if true cast it.
        /// </summary>
        /// <param name="val"> value to cast </param>
        public override bool IsAbleToCastCast(float val)
        {
            // able to cast the spell, cast it.
            if (IsAbleToCast(val))
            {
                // cost the mana, and ready to cast the spell
                DeltaCurrentValue(val);
                return true;
            }

            // not enough value, cannot cast the spell...
            return false;
        }

        /// <summary>
        /// Current value in liquid bar.
        /// </summary>
        /// <returns> value in liquid bar </returns>
        public override float GetCurrentValue()
        {
            return mCurrentValue;
        }

        /// <summary>
        /// Get the container data so it could so the correct 
        /// position check and value check.
        /// </summary>
        private void GetContainerData()
        {
            if (mBarSpriteRenderer == null)
                return;

            if (mSpriteMask == null)
            {
                Debug.LogError("No sprite mask attached");
                return;
            }

            // find the width and height of the image from sprite renderer
            Rect maskRect = mSpriteMask.sprite.rect;
            Vector2 spriteSize = new Vector2(maskRect.width, maskRect.height);
            float worldUnit = 100.0f;
            Vector2 maskSize = new Vector2(spriteSize.x / worldUnit, spriteSize.y / worldUnit);

            switch (GetAlign())
            {
                case JCS_Align.ALIGN_LEFT:
                    {
                        mMaxPos = mSpriteMask.transform.localPosition.x;
                        mMinPos = mMaxPos - (maskSize.x * mSpriteMask.transform.localScale.x);
                    }
                    break;

                case JCS_Align.ALIGN_RIGHT:
                    {
                        mMaxPos = mSpriteMask.transform.localPosition.x;
                        mMinPos = mMaxPos + (maskSize.x * mSpriteMask.transform.localScale.x);
                    }
                    break;

                case JCS_Align.ALIGN_TOP:
                    {
                        mMaxPos = mSpriteMask.transform.localPosition.y;
                        mMinPos = mMaxPos + (maskSize.y * mSpriteMask.transform.localScale.y);
                    }
                    break;

                case JCS_Align.ALIGN_BOTTOM:
                    {
                        mMaxPos = mSpriteMask.transform.localPosition.y;
                        mMinPos = mMaxPos - (maskSize.y * mSpriteMask.transform.localScale.y);
                    }
                    break;
            }

            // do starting percent
            FixPercentage();
        }

        /// <summary>
        /// Fix the percentage of the liquid bar shown.
        /// </summary>
        private void FixPercentage()
        {
            if (mCurrentValue < mMinValue || mCurrentValue > mMaxValue)

            {
                Debug.LogWarning("Value should with in min(" + mMinValue +
                    ") ~ max(" + mMaxValue + ") value");
                return;
            }

            float realValue = mMaxValue - mMinValue;
            float currentPercentage = mCurrentValue / realValue;

            float realDistance = (mMaxPos - mMinPos) * currentPercentage;

            switch (GetAlign())
            {
                case JCS_Align.ALIGN_LEFT:
                case JCS_Align.ALIGN_RIGHT:
                    mMaskTargetPosition.x = mMinPos + realDistance;
                    break;
                case JCS_Align.ALIGN_BOTTOM:
                case JCS_Align.ALIGN_TOP:
                    mMaskTargetPosition.y = mMinPos + realDistance;
                    break;
            }
        }

        /// <summary>
        /// Movement of the liquid bar.
        /// </summary>
        private void TowardToTargetValue()
        {
            Vector3 speed = (mMaskTargetPosition - mSpriteMask.transform.localPosition) / mDeltaFriction * JCS_Time.ItTime(mTimeType);
            mSpriteMask.transform.localPosition += speed;

            DoVisuallyCallback();
        }

        /// <summary>
        /// Do the recover timer and add per timer is reached.
        /// </summary>
        private void DoRecover()
        {
            if (!mRecoverEffect)
                return;

            mRecoverTimer += JCS_Time.ItTime(mTimeType);

            if (mRecoverTimer < mTimeToRecover)
                return;

            // do recover
            DeltaCurrentValue(mRecoverValue);

            // reset timer.
            mRecoverTimer = 0.0f;
        }

        /// <summary>
        /// Do call back if call back was there.
        /// </summary>
        private void DoCallback()
        {
            mReachMin = false;
            mReachMax = false;

            if (mCurrentValue == mMinValue)
            {
                // do min call back.
                if (callback_min != null)
                    callback_min.Invoke();

                mReachMin = true;
            }

            if (mCurrentValue == mMaxValue)
            {

                if (callback_max != null)
                    callback_max.Invoke();

                mReachMax = true;
            }
        }

        /// <summary>
        /// Do callback after visualizing liquid bar.
        /// </summary>
        private void DoVisuallyCallback()
        {
            float distance = Vector3.Distance(mMaskTargetPosition, mSpriteMask.transform.localPosition);

            if (distance > mDistanceThreshold)
            {
                mReachMinVis = false;
                mReachMaxVis = false;
                return;
            }

            if (!mReachMinVis && mCurrentValue == mMinValue)
            {
                // do min call back.
                if (callback_min_vis != null)
                    callback_min_vis.Invoke();

                mReachMinVis = true;
            }

            if (!mReachMaxVis && mCurrentValue == mMaxValue)
            {
                if (callback_max_vis != null)
                    callback_max_vis.Invoke();

                mReachMaxVis = true;
            }
        }

        /// <summary>
        /// Follow the value info attached.
        /// </summary>
        private void UpdateInfo()
        {
            // without info attach, we can't do anything.
            if (mInfo == null)
                return;

            SetMinValue(mInfo.minValue);
            SetMaxValue(mInfo.maxValue);

            SetCurrentValue(mInfo.currentValue);
        }
    }
}
