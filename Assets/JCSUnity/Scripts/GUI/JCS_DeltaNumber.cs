/**
 * $File: JCS_DeltaNumber.cs $
 * $Date: 2017-03-10 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// System that control 0-9 number sprites and create 
    /// a counter effect.
    /// </summary>
    public class JCS_DeltaNumber
        : MonoBehaviour
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

        [Tooltip(@"Current score rendering..., do not use this to 
check value. Because this will always be animate.")]
        [SerializeField]
        private int mCurrentScore = 0;

        [Tooltip("Default null slot. [Default: transparent_256.png]")]
        [SerializeField]
        private Sprite mScoreNull = null;
        [Tooltip("Score text 0.")]
        [SerializeField]
        private Sprite mScoreText0 = null;
        [Tooltip("Score text 1.")]
        [SerializeField]
        private Sprite mScoreText1 = null;
        [Tooltip("Score text 2.")]
        [SerializeField]
        private Sprite mScoreText2 = null;
        [Tooltip("Score text 3.")]
        [SerializeField]
        private Sprite mScoreText3 = null;
        [Tooltip("Score text 4.")]
        [SerializeField]
        private Sprite mScoreText4 = null;
        [Tooltip("Score text 5.")]
        [SerializeField]
        private Sprite mScoreText5 = null;
        [Tooltip("Score text 6.")]
        [SerializeField]
        private Sprite mScoreText6 = null;
        [Tooltip("Score text 7.")]
        [SerializeField]
        private Sprite mScoreText7 = null;
        [Tooltip("Score text 8.")]
        [SerializeField]
        private Sprite mScoreText8 = null;
        [Tooltip("Score text 9.")]
        [SerializeField]
        private Sprite mScoreText9 = null;

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

        [Header("- Min/Max Settings (JCS_DeltaNumber)")]

        [Tooltip("Maxinum score.")]
        [SerializeField]
        private int mMaxScore = 999;

        [Tooltip("Mininum score.")]
        [SerializeField]
        private int mMinScore = 0;

        [Header("- Delta Animation Settings (JCS_DeltaNumber)")]

        [Tooltip(@"This will make the score have the transition 
between, setting to a new score. If you want the score set directly, you 
should disable this effect for best purpose.")]
        [SerializeField]
        private bool mDeltaToCurrentScore = false;

        [Tooltip("Toward to this score, if mDeltaToCurrentScore active.")]
        [SerializeField]
        private int mTargetScore = 0;

        [Tooltip("How fast the score animate.")]
        [SerializeField]
        [Range(0.01f, 1.0f)]
        private float mAnimScoreTime = 0;

        // timer for animation the score
        private float mAnimScoreTimer = 0;

        [Tooltip("How much the delta value add up.")]
        [SerializeField] [Range(1, 1000)]
        private int mDeltaProduct = 1;

        /* Setter & Getter */

        public bool IsEnable { get { return this.mIsEnable; } }
        public bool ClearEmptyLeftZero { get { return this.mClearEmptyLeftZero; } set { this.mClearEmptyLeftZero = value; } }
        public bool DeltaToCurrentScore { get { return this.mDeltaToCurrentScore; } set { this.mDeltaToCurrentScore = value; } }
        public int TargetScore
        {
            get { return this.mTargetScore; }
            set
            {
                this.mTargetScore = value;

                // by setting the delta score will enable the 
                // delta to current score effect.
                this.mDeltaToCurrentScore = true;
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

            // init score to current.
            UpdateScore();
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            DoDeltaCurrentScore();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKey(mDeltaToA))
                UpdateScore(mDeltaValueA);
            if (JCS_Input.GetKey(mDeltaToB))
                UpdateScore(mDeltaValueB);
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

            for (int index = 0;
                index < mDigitsRendererSlot.Length;
                ++index)
            {
                mDigitsRendererSlot[index].LocalEnabled = act;
            }

            this.mIsEnable = act;
        }

        /// <summary>
        /// Update the score GUI.
        /// </summary>
        public void UpdateScore()
        {
            UpdateScore(mCurrentScore);
        }

        /// <summary>
        /// Update the score GUI
        /// </summary>
        /// <param name="score"> score apply to show </param>
        public void UpdateScore(int score, bool anime = false)
        {
            int targetScore = score;

            if (targetScore < mMinScore)
                targetScore = mMinScore;
            else if (targetScore > mMaxScore)
                targetScore = mMaxScore;

            if (mDeltaToCurrentScore)
            {
                if (!anime)
                    mTargetScore = targetScore;
            }
            else
            {
                // apply to current score
                mCurrentScore = targetScore;
            }

            DoScoreGUI(targetScore);

            // Do if visible on zero effect.
            DoVisibleOnZero(targetScore);

            DoScoreAlign(targetScore);

        }

        /// <summary>
        /// Update the space to each digit.
        /// </summary>
        /// <param name="interval"> target interval </param>
        public void UpdateIntervalForEachDigit(float interval)
        {
            // update interval
            this.mDigitInterval = interval;

            for (int digit = 0;
                digit < mDigitsRendererSlot.Length;
                ++digit)
            {
                if (mDigitsRendererSlot[digit] == null)
                {
                    JCS_Debug.LogError(
                        "Digit slot cannot be null references...");
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
        /// Do the GUI score logic.
        /// </summary>
        /// <param name="score"> score to apply. </param>
        private void DoScoreGUI(int score)
        {
            // check the first non zero from the left.
            bool meetNonZero = false;

            for (int digit = mDigitsRendererSlot.Length - 1;
                digit >= 0;
                --digit)
            {
                if (mDigitsRendererSlot[digit] == null)
                {
                    JCS_Debug.LogError(
                        "Digit slot cannot be null references...");
                    continue;
                }

                int valDigit = JCS_Mathf.GetSingleDigit(digit + 1, score);

                mDigitsRendererSlot[digit].LocalSprite = GetSingleDigitSprite(valDigit);

                if (mClearEmptyLeftZero)
                {
                    /**
                     * Last digit is zero, we set to zero. so skip it.
                     */
                    if (valDigit != -1)
                        meetNonZero = true;

                    if (!meetNonZero)
                        mDigitsRendererSlot[digit].LocalSprite = mScoreNull;
                }
            }
        }

        /// <summary>
        /// Main algorithm to approach to targe score.
        /// </summary>
        private void DoDeltaCurrentScore()
        {
            // check if the effect active.
            if (!mDeltaToCurrentScore)
                return;

            // if score the same, return.
            if (mCurrentScore == mTargetScore)
                return;

            mAnimScoreTimer += Time.deltaTime;

            // check if timer reach the time.
            if (mAnimScoreTimer < mAnimScoreTime)
                return;

            // find out to increase/decrease the score.
            if (mCurrentScore < mTargetScore)
            {
                mCurrentScore += mDeltaProduct;

                if (mCurrentScore > mTargetScore)
                    mCurrentScore = mTargetScore;
            }
            else
            {
                mCurrentScore -= mDeltaProduct;

                if (mCurrentScore < mTargetScore)
                    mCurrentScore = mTargetScore;
            }

            UpdateScore(mCurrentScore, true);

            // reset timer
            mAnimScoreTimer = 0;
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
                case 0: return mScoreText0;
                case 1: return mScoreText1;
                case 2: return mScoreText2;
                case 3: return mScoreText3;
                case 4: return mScoreText4;
                case 5: return mScoreText5;
                case 6: return mScoreText6;
                case 7: return mScoreText7;
                case 8: return mScoreText8;
                case 9: return mScoreText9;
            }

            // default return 'zero' sprite.
            return mScoreText0;
        }

        /// <summary>
        /// Do visible on zero effect after updating score.
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
        /// Do score align after updating score.
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
            for (int digit = 0;
                digit < mDigitsRendererSlot.Length;
                ++digit)
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
