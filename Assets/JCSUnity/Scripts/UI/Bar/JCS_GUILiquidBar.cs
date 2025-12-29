/**
 * $File: JCS_GUILiquidBar.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// GUI liquid bar like health bar, mana bar etc.
    /// Is specific for Unity Engine's Canvas system.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_GUILiquidBar : JCS_LiquidBar
    {
        /* Variables */

        private RectTransform mRectTransform = null;
        private RectTransform mMaskRectTransform = null;

#if UNITY_EDITOR
        [Separator("🧪 Helper Variables (JCS_GUILiquidBar)")]

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

        [Separator("📋 Check Variabless (JCS_GUILiquidBar)")]

        [Tooltip("The panel root object.")]
        [SerializeField]
        [ReadOnly]
        private JCS_PanelRoot mPanelRoot = null;

        [SerializeField]
        [ReadOnly]
        private Vector3 mMaskTargetPosition = Vector3.zero;

        [Separator("🌱 Initialize Variables (JCS_GUILiquidBar)")]

        [Tooltip("please set this ")]
        [SerializeField]
        private Mask mMask = null;


        // TODO(jenchieh): Somewhat this work, better if i get the logic right 
        // then this can be optimized.
        private uint mCountToGetContainerData = 0;

        /* Setter & Getter */

        public RectTransform GetRectTransform() { return mRectTransform; }
        public Mask GetMask() { return mMask; }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            // record down the position
            mRectTransform = GetComponent<RectTransform>();

            // Get panel root, in order to calculate the correct distance base
            // on the resolution.
            mPanelRoot = JCS_PanelRoot.GetFromParent(transform);

            if (mMask == null)
            {
                Debug.LogError("No mask applied");
                return;
            }

            transform.SetParent(mMask.transform);
            mMaskRectTransform = mMask.GetComponent<RectTransform>();

            mMaskTargetPosition = mMaskRectTransform.localPosition;

            // min value cannot be lower or equal to max value
            if (mMinValue >= mMaxValue)
                mMinValue = mMaxValue + 1;  // force min < max value
        }

        protected void LateUpdate()
        {
#if UNITY_EDITOR
            Test();
#endif

            base.Update();

            // check all components needed are avaliable.
            if (mMask == null)
                return;

            // update value.
            UpdateInfo();

            GetContainerData();

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
                Debug.LogError("Max value you set cannot be lower than min value.");
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
                Debug.LogError("Min value you set cannot be higher than max value.");
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
        /// Full the liquid bar.
        /// </summary>
        public override void Full()
        {
            SetCurrentValue(mMaxValue);
        }

        /// <summary>
        /// Zero out the liquid bar.
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
        /// Returns current value in liquid bar.
        /// </summary>
        /// <returns> value in liquid bar </returns>
        public override float GetCurrentValue()
        {
            return mCurrentValue;
        }

        /// <summary>
        /// Get the container data so it could so the correct position check
        /// and value check.
        /// </summary>
        private void GetContainerData()
        {
            if (mCountToGetContainerData == 2)
                return;

            // NOTE(jenchieh): missing comment.
            mRectTransform.SetParent(mMaskRectTransform.parent);

            mMaskTargetPosition = mRectTransform.localPosition;

            // find min max position, base on the algin side.
            switch (GetAlign())
            {
                case JCS_Align.ALIGN_LEFT:
                    {
                        mMaxPos = mRectTransform.localPosition.x;
                        mMinPos = mMaxPos - (mMaskRectTransform.sizeDelta.x * mMaskRectTransform.localScale.x);
                    }
                    break;

                case JCS_Align.ALIGN_RIGHT:
                    {
                        mMaxPos = mRectTransform.localPosition.x;
                        mMinPos = mMaxPos + (mMaskRectTransform.sizeDelta.x * mMaskRectTransform.localScale.x);
                    }
                    break;

                case JCS_Align.ALIGN_TOP:
                    {
                        mMaxPos = mRectTransform.localPosition.y;
                        mMinPos = mMaxPos + (mMaskRectTransform.sizeDelta.y * mMaskRectTransform.localScale.y);
                    }
                    break;

                case JCS_Align.ALIGN_BOTTOM:
                    {
                        mMaxPos = mRectTransform.localPosition.y;
                        mMinPos = mMaxPos - (mMaskRectTransform.sizeDelta.y * mMaskRectTransform.localScale.y);
                    }
                    break;
            }

            // NOTE(jenchieh): missing comment.
            mRectTransform.SetParent(mMaskRectTransform);

            // do starting percent
            FixPercentage();

            ++mCountToGetContainerData;
        }

        /// <summary>
        /// Fix the percentage of the liquid bar shown.
        /// </summary>
        private void FixPercentage()
        {
            if (mCurrentValue < mMinValue || mCurrentValue > mMaxValue)

            {
                Debug.LogError("Value should with in min(" + mMinValue + ") ~ max(" + mMaxValue + ") value");
                return;
            }

            float realValue = mMaxValue - mMinValue;
            float currentPercentage = mCurrentValue / realValue;

            float realDistance = (mMaxPos - mMinPos) * currentPercentage;

            switch (GetAlign())
            {
                case JCS_Align.ALIGN_LEFT:
                case JCS_Align.ALIGN_RIGHT:
                    {
                        mMaskTargetPosition.x = mMinPos + realDistance;
                    }
                    break;
                case JCS_Align.ALIGN_BOTTOM:
                case JCS_Align.ALIGN_TOP:
                    {
                        mMaskTargetPosition.y = mMinPos + realDistance;
                    }
                    break;
            }
        }

        /// <summary>
        /// Movement of the liquid bar.
        /// </summary>
        private void TowardToTargetValue()
        {
            Vector3 speed = (mMaskTargetPosition - mMaskRectTransform.localPosition) / mDeltaFriction * JCS_Time.ItTime(mTimeType);
            Vector3 tmpSpeed = speed;

            // TODO(jenchieh): It's weird that these seem to fix the issue
            // when resolution isn't the full targeted resoltuion (generally
            // 1920 x 1080).
            //
            // But the speed `mDeltaFriction` doesn't apply; meaning it doesn't
            // grow in a proportional way with the resolution ratio/scale.
            if (mPanelRoot != null)
            {
                switch (GetAlign())
                {
                    case JCS_Align.ALIGN_LEFT:
                    case JCS_Align.ALIGN_RIGHT:
                        {
                            tmpSpeed *= mPanelRoot.panelDeltaWidthRatio;
                        }
                        break;
                    case JCS_Align.ALIGN_BOTTOM:
                    case JCS_Align.ALIGN_TOP:
                        {
                            tmpSpeed *= mPanelRoot.panelDeltaHeightRatio;
                        }
                        break;
                }
            }

            mMaskRectTransform.localPosition += tmpSpeed;
            mRectTransform.localPosition -= speed;

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

            if (IsEmpty())
            {
                // do min call back.
                if (callback_min != null)
                    callback_min.Invoke();

                mReachMin = true;
            }

            if (IsFull())
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
            float distance = Vector3.Distance(mMaskTargetPosition, mMaskRectTransform.localPosition);

            if (distance > mDistanceThreshold)
            {
                mReachMinVis = false;
                mReachMaxVis = false;
                return;
            }

            if (!mReachMinVis && IsEmpty())
            {
                // do min call back.
                if (callback_min_vis != null)
                    callback_min_vis.Invoke();

                mReachMinVis = true;
            }

            if (!mReachMaxVis && IsFull())
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
            // without info attach, we cannot
            // do anything.
            if (mInfo == null)
                return;

            SetMinValue(mInfo.minValue);
            SetMaxValue(mInfo.maxValue);

            SetCurrentValue(mInfo.currentValue);
        }
    }
}
