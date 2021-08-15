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

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_GUILiquidBar) **")]

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

        [Header("** Check Variables (JCS_GUILiquidBar) **")]

        [SerializeField]
        private Vector3 mMaskTargetPosition = Vector3.zero;

        [Header("** Initialize Variables (JCS_GUILiquidBar) **")]

        [Tooltip("please set this ")]
        [SerializeField]
        private Mask mMask = null;


        // TODO(jenchieh): Somewhat this work, better if i get the logic right 
        // then this can be optimized.
        private uint mCountToGetContainerData = 0;

        /* Setter & Getter */

        public RectTransform GetRectTransform() { return this.mRectTransform; }
        public Mask GetMask() { return this.mMask; }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            // record down the position
            mRectTransform = this.GetComponent<RectTransform>();

            if (mMask == null)
            {
                JCS_Debug.LogError("No mask applied");
                return;
            }

            this.transform.SetParent(mMask.transform);
            mMaskRectTransform = this.mMask.GetComponent<RectTransform>();

            mMaskTargetPosition = this.mMaskRectTransform.localPosition;

            // min value cannot be lower or equal to max value
            if (mMinValue >= mMaxValue)
                mMinValue = mMaxValue + 1;  // force min < max value
        }

        protected void LateUpdate()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            base.Update();

            // check all components needed are
            // avaliable.
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

#if (UNITY_EDITOR)
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
                SetCurrentValue(MaxValue / 2.0f);

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
            this.mInfo = info;

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
                JCS_Debug.LogError("Max value you set cannot be lower than min value.");
                return;
            }

            this.mMaxValue = val;

            if (mInfo != null)
                mInfo.MaxValue = (int)val;

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
                JCS_Debug.LogError("Min value you set cannot be higher than max value.");
                return;
            }

            this.mMinValue = val;

            if (mInfo != null)
                mInfo.MinValue = (int)val;

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

            this.mCurrentValue = val;

            // cannot lower than min container value
            if (mCurrentValue < mMinValue)
                mCurrentValue = mMinValue;

            // cannot higher than max container value
            if (mCurrentValue > mMaxValue)
                mCurrentValue = mMaxValue;

            if (mInfo != null)
                mInfo.CurrentValue = (int)mCurrentValue;

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
        /// <returns> true: able to cast, 
        ///           false: not able to cast </returns>
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
            return this.mCurrentValue;
        }

        /// <summary>
        /// Get the container data so it could so the correct 
        /// position check and value check.
        /// </summary>
        private void GetContainerData()
        {
            if (mCountToGetContainerData == 2)
                return;

            // NOTE(jenchieh): missing comment.
            this.mRectTransform.SetParent(mMaskRectTransform.parent);

            mMaskTargetPosition = this.mRectTransform.localPosition;

            // find min max position, 
            // base on the algin side.
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
            this.mRectTransform.SetParent(mMaskRectTransform);

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
                JCS_Debug.LogError("Value should with in min(" + mMinValue +
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
            Vector3 speed = (mMaskTargetPosition - mMaskRectTransform.localPosition) / mDeltaFriction * Time.deltaTime;
            mMaskRectTransform.localPosition += speed;
            mRectTransform.localPosition -= speed;
        }

        /// <summary>
        /// Do the recover timer and add per timer is reached.
        /// </summary>
        private void DoRecover()
        {
            if (!mRecoverEffect)
                return;

            mRecoverTimer += Time.deltaTime;

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
        /// Follow the value info attached.
        /// </summary>
        private void UpdateInfo()
        {
            // without info attach, we cannot
            // do anything.
            if (Info == null)
                return;

            SetMinValue(mInfo.MinValue);
            SetMaxValue(mInfo.MaxValue);

            SetCurrentValue(mInfo.CurrentValue);
        }
    }
}
