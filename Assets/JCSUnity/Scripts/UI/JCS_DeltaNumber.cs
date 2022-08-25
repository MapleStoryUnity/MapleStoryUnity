/**
 * $File: JCS_DeltaNumber.cs $
 * $Date: 2017-03-10 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// System that control 0-9 number sprites and create 
    /// a counter effect.
    /// </summary>
    public class JCS_DeltaNumber : MonoBehaviour
    {
        /* Variables */

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_DeltaNumber) **")]

        [Tooltip("Test component with key.")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key delta to value A.")]
        [SerializeField]
        private KeyCode mDeltaToA = KeyCode.A;

        [Tooltip("Value delta A.")]
        [SerializeField]
        private int mDeltaValueA = 0;

        [Tooltip("Key delta to value B.")]
        [SerializeField]
        private KeyCode mDeltaToB = KeyCode.B;

        [Tooltip("Value delta B.")]
        [SerializeField]
        private int mDeltaValueB = 64;
#endif

        [Header("** Check Variables (JCS_DeltaNumber) **")]

        [Tooltip("Is current effect enabled?")]
        [SerializeField]
        private bool mIsEnable = true;

        [Tooltip("How many visible digit?")]
        [SerializeField]
        private int mCurrentDigitCount = 1;

        [Header("** Initialize Variables (JCS_DeltaNumber) **")]

        [Tooltip(@"Current number rendering..., do not use this to 
check value. Because this will always be animate.")]
        [SerializeField]
        private int mCurrentNumber = 0;

        [Tooltip("Default null slot. [Default: transparent_256.png]")]
        [SerializeField]
        private Sprite mNumberNull = null;
        [Tooltip("Number image 0.")]
        [SerializeField]
        private Sprite mNumberText0 = null;
        [Tooltip("Number image 1.")]
        [SerializeField]
        private Sprite mNumberText1 = null;
        [Tooltip("Number image 2.")]
        [SerializeField]
        private Sprite mNumberText2 = null;
        [Tooltip("Number image 3.")]
        [SerializeField]
        private Sprite mNumberText3 = null;
        [Tooltip("Number image 4.")]
        [SerializeField]
        private Sprite mNumberText4 = null;
        [Tooltip("Number image 5.")]
        [SerializeField]
        private Sprite mNumberText5 = null;
        [Tooltip("Number image 6.")]
        [SerializeField]
        private Sprite mNumberText6 = null;
        [Tooltip("Number image 7.")]
        [SerializeField]
        private Sprite mNumberText7 = null;
        [Tooltip("Number image 8.")]
        [SerializeField]
        private Sprite mNumberText8 = null;
        [Tooltip("Number image 9.")]
        [SerializeField]
        private Sprite mNumberText9 = null;

        [Tooltip("Each digit, the more length the more digit.")]
        [SerializeField]
        private JCS_UnityObject[] mDigitsRendererSlot = null;

        [Tooltip("Interval between each digit.")]
        [SerializeField]
        [Range(0.1f, 4000.0f)]
        private float mDigitInterval = 0.5f;

        [Header("** Runtime Variables (JCS_DeltaNumber) **")]

        [Tooltip("Clear all the empty zero from the left.")]
        [SerializeField]
        private bool mClearEmptyLeftZero = false;

        [Tooltip("Is visible when is zero?")]
        [SerializeField]
        private bool mVisibleOnZero = true;

        [Tooltip("Align side.")]
        [SerializeField]
        private JCS_TextAlign mTextAlign = JCS_TextAlign.RIGHT;

        [Header("- Min/Max (JCS_DeltaNumber)")]

        [Tooltip("Maxinum number.")]
        [SerializeField]
        private int mMaxNumber = 999;

        [Tooltip("Mininum number.")]
        [SerializeField]
        private int mMinNumber = 0;

        [Header("- Animation (JCS_DeltaNumber)")]

        [Tooltip(@"This will make the number have the transition 
between, setting to a new number. If you want the number set directly, you 
should disable this effect for best purpose.")]
        [SerializeField]
        private bool mDeltaToCurrentNumber = true;

        [Tooltip("Toward to this number, if mDeltaToCurrentScore active.")]
        [SerializeField]
        private int mTargetNumber = 0;

        [Tooltip("How fast the number animate.")]
        [SerializeField]
        [Range(0.01f, 1.0f)]
        private float mAnimNumberTime = 0;

        // timer for animation the number
        private float mAnimNumberTimer = 0;

        [Tooltip("How much the delta value add up.")]
        [SerializeField]
        [Range(1, 1000)]
        private int mDeltaProduct = 1;

        /* Setter & Getter */

        public bool IsEnable { get { return this.mIsEnable; } }
        public bool ClearEmptyLeftZero { get { return this.mClearEmptyLeftZero; } set { this.mClearEmptyLeftZero = value; } }
        public bool DeltaToCurrentNumber { get { return this.mDeltaToCurrentNumber; } set { this.mDeltaToCurrentNumber = value; } }
        public int TargetNumber
        {
            get { return this.mTargetNumber; }
            set
            {
                this.mTargetNumber = value;

                // by setting the delta number will enable the delta to current
                // number effect.
                this.mDeltaToCurrentNumber = true;
            }
        }
        public bool VisibleOnZero { get { return this.mVisibleOnZero; } set { this.mVisibleOnZero = value; } }
        public JCS_TextAlign TextAlign { get { return this.mTextAlign; } set { this.mTextAlign = value; } }

        /* Functions */

        private void Awake()
        {
            // update all the unity object.
            foreach (JCS_UnityObject digit in mDigitsRendererSlot)
            {
                if (digit == null)
                    continue;

                digit.UpdateUnityData();
            }

            // update interval
            UpdateIntervalForEachDigit();

            // init number to current.
            UpdateNumber();
        }

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif

            DoDeltaCurrentNumber();
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKey(mDeltaToA))
                UpdateNumber(mDeltaValueA);
            if (JCS_Input.GetKey(mDeltaToB))
                UpdateNumber(mDeltaValueB);
        }
#endif

        /// <summary>
        /// Set enable/disable all digit render slot.
        /// </summary>
        public void EnableDigitsRendererSlot(bool act)
        {
            // Do nothing if already the same.
            if (mIsEnable == act)
                return;

            for (int index = 0; index < mDigitsRendererSlot.Length; ++index)
            {
                mDigitsRendererSlot[index].LocalEnabled = act;
            }

            this.mIsEnable = act;
        }

        /// <summary>
        /// Update the number GUI.
        /// </summary>
        public void UpdateNumber()
        {
            UpdateNumber(mCurrentNumber);
        }

        /// <summary>
        /// Update the number GUI
        /// </summary>
        /// <param name="number"> number apply to show </param>
        public void UpdateNumber(int number, bool anime = false)
        {
            int targetNumber = number;

            if (targetNumber < mMinNumber)
                targetNumber = mMinNumber;
            else if (targetNumber > mMaxNumber)
                targetNumber = mMaxNumber;

            if (mDeltaToCurrentNumber)
            {
                if (!anime)
                    mTargetNumber = targetNumber;
            }
            else
            {
                // apply to current number
                mCurrentNumber = targetNumber;
            }

            DoScoreGUI(targetNumber);

            // Do if visible on zero effect.
            DoVisibleOnZero(targetNumber);

            DoScoreAlign(targetNumber);

        }

        /// <summary>
        /// Update the space to each digit.
        /// </summary>
        /// <param name="interval"> target interval </param>
        public void UpdateIntervalForEachDigit(float interval)
        {
            // update interval
            this.mDigitInterval = interval;

            for (int digit = 0; digit < mDigitsRendererSlot.Length; ++digit)
            {
                if (mDigitsRendererSlot[digit] == null)
                {
                    JCS_Debug.LogError("Digit slot cannot be null references");
                    continue;
                }

                Vector3 newPos = mDigitsRendererSlot[digit].LocalPosition;
                newPos.x += -(interval * digit);
                mDigitsRendererSlot[digit].LocalPosition = newPos;
            }
        }

        /// <summary>
        /// Update the space to each digit.
        /// </summary>
        public void UpdateIntervalForEachDigit()
        {
            UpdateIntervalForEachDigit(mDigitInterval);
        }

        /// <summary>
        /// Do the GUI number logic.
        /// </summary>
        /// <param name="number"> number to apply. </param>
        private void DoScoreGUI(int number)
        {
            // check the first non zero from the left.
            bool meetNonZero = false;

            for (int digit = mDigitsRendererSlot.Length - 1; digit >= 0; --digit)
            {
                if (mDigitsRendererSlot[digit] == null)
                {
                    JCS_Debug.LogError(
                        "Digit slot cannot be null references...");
                    continue;
                }

                int valDigit = JCS_Mathf.GetSingleDigit(digit + 1, number);

                mDigitsRendererSlot[digit].LocalSprite = GetSingleDigitSprite(valDigit);

                if (mClearEmptyLeftZero)
                {
                    /**
                     * Last digit is zero, we set to zero. so skip it.
                     */
                    if (valDigit != -1)
                        meetNonZero = true;

                    if (!meetNonZero)
                        mDigitsRendererSlot[digit].LocalSprite = mNumberNull;
                }
            }
        }

        /// <summary>
        /// Main algorithm to approach to targe number.
        /// </summary>
        private void DoDeltaCurrentNumber()
        {
            // check if the effect active.
            if (!mDeltaToCurrentNumber)
                return;

            // if number the same, return.
            if (mCurrentNumber == mTargetNumber)
                return;

            mAnimNumberTimer += Time.deltaTime;

            // check if timer reach the time.
            if (mAnimNumberTimer < mAnimNumberTime)
                return;

            // find out to increase/decrease the number.
            if (mCurrentNumber < mTargetNumber)
            {
                mCurrentNumber += mDeltaProduct;

                if (mCurrentNumber > mTargetNumber)
                    mCurrentNumber = mTargetNumber;
            }
            else
            {
                mCurrentNumber -= mDeltaProduct;

                if (mCurrentNumber < mTargetNumber)
                    mCurrentNumber = mTargetNumber;
            }

            UpdateNumber(mCurrentNumber, true);

            // reset timer
            mAnimNumberTimer = 0;
        }

        /// <summary>
        /// Return a digit sprite.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private Sprite GetSingleDigitSprite(int num)
        {
            switch (num)
            {
                case 0: return mNumberText0;
                case 1: return mNumberText1;
                case 2: return mNumberText2;
                case 3: return mNumberText3;
                case 4: return mNumberText4;
                case 5: return mNumberText5;
                case 6: return mNumberText6;
                case 7: return mNumberText7;
                case 8: return mNumberText8;
                case 9: return mNumberText9;
            }

            // default return 'zero' sprite.
            return mNumberText0;
        }

        /// <summary>
        /// Do visible on zero effect after updating number.
        /// </summary>
        private void DoVisibleOnZero(int targetScore)
        {
            if (mVisibleOnZero)
                return;

            if (targetScore == 0)
            {
                EnableDigitsRendererSlot(false);
            }
            else
            {
                EnableDigitsRendererSlot(true);
            }
        }

        /// <summary>
        /// Do number align after updating number.
        /// </summary>
        private void DoScoreAlign(int targetScore)
        {
            // digit cannot be zero, must at least be one.
            int newDigitCount = JCS_Mathf.DigitCountIncludeZero(targetScore);

            int diffDigitCount = newDigitCount - mCurrentDigitCount;

            // No difference, no need to do anything.
            if (diffDigitCount == 0)
                return;

            mCurrentDigitCount = newDigitCount;

            // We don't need to do align if the visible
            // digit count would not change.
            if (!mClearEmptyLeftZero)
                return;

            switch (mTextAlign)
            {
                case JCS_TextAlign.CENTER:
                    {
                        MoveDigits(mDigitInterval * diffDigitCount / 2);
                    }
                    break;
                case JCS_TextAlign.RIGHT:
                    {
                        // Default is right, no need to do anything.
                    }
                    break;
                case JCS_TextAlign.LEFT:
                    {
                        MoveDigits(mDigitInterval * diffDigitCount);
                    }
                    break;
            }
        }


        /// <summary>
        /// Move digits.
        /// </summary>
        /// <param name="deltaX"></param>
        private void MoveDigits(float deltaX)
        {
            for (int digit = 0; digit < mDigitsRendererSlot.Length; ++digit)
            {
                if (mDigitsRendererSlot[digit] == null)
                {
                    JCS_Debug.LogError(
                        "Digit slot cannot be null references...");
                    continue;
                }

                Vector3 newPos = mDigitsRendererSlot[digit].LocalPosition;
                newPos.x += deltaX;
                mDigitsRendererSlot[digit].LocalPosition = newPos;
            }
        }
    }
}
