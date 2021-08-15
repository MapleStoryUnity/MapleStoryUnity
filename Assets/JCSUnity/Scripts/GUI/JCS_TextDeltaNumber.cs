/**
 * $File: JCS_TextDeltaNumber.cs $
 * $Date: 2019-07-16 15:45:03 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2019 by Shen, Jen-Chieh $
 */

/* NOTE: If you are using `TextMesh Pro` uncomment this line.
 */
#define TMP_PRO

using UnityEngine;
using UnityEngine.UI;

#if TMP_PRO
using TMPro;
#endif

namespace JCSUnity
{
    /// <summary>
    /// Like `JCS_DeltaNumber`, but instead of altering the sprite we
    /// alter text instead.
    /// </summary>
    public class JCS_TextDeltaNumber : MonoBehaviour
    {
        /* Variables */

#if UNITY_EDITOR
        [Header("** Helper Variables (JCS_TextDeltaNumber) **")]

        [Tooltip("Test component with key.")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key to delta number to value a.")]
        [SerializeField]
        private KeyCode mDeltaToA = KeyCode.K;

        [Tooltip("Target value a.")]
        [SerializeField]
        private float mValueA = -10.0f;

        [Tooltip("Key to delta number to value b.")]
        [SerializeField]
        private KeyCode mDeltaToB = KeyCode.L;

        [Tooltip("Target value b.")]
        [SerializeField]
        private float mValueB = 10.0f;
#endif

        [Header("** Check Variables (JCS_TextDeltaNumber) **")]

        [Tooltip("Flag to check if is currently effecting.")]
        [SerializeField]
        private bool mActive = false;

        [Tooltip("Full string to display.")]
        [SerializeField]
        private string mFullString = "";

        [Tooltip("Target number to display, or to delta to.")]
        [SerializeField]
        private float mTargetNumber = 0.0f;

        [Header("** Initialize Variables (JCS_TextDeltaNumber) **")]

        [Tooltip("Target text renderer.")]
        [SerializeField]
        private Text mTextContainer = null;

#if TMP_PRO
        [Tooltip("Target text renderer.")]
        [SerializeField]
        private TextMeshPro mTextMesh = null;
#endif

        [Header("** Runtime Variables (JCS_TextDeltaNumber) **")]

        [Tooltip("Current number that will turn into string.")]
        [SerializeField]
        private float mCurrentNumber = 0.0f;

        [Tooltip("Ensure add a plus sign if the numer is positive.")]
        [SerializeField]
        private bool mPlusSignWhenPositive = false;

        [Tooltip("String added before rendering the number.")]
        [SerializeField]
        private string mPreString = "";

        [Tooltip("String added after rendering the number.")]
        [SerializeField]
        private string mPostString = "";

        [Tooltip("Place you want to round the decimal.")]
        [SerializeField]
        [Range(0, 15)]
        private int mRoundPlace = 2;

        [Tooltip("How fast the number animate.")]
        [SerializeField]
        [Range(0.01f, 1.0f)]
        private float mAnimNumberTime = 0.01f;

        private float mAnimNumberTimer = 0;

        [Tooltip("How much the delta value add up.")]
        [SerializeField]
        [Range(0.01f, 1000.0f)]
        private float mDeltaProduct = 1;

        /* Setter/Getter */

        public bool Active { get { return this.mActive; } }
        public float TargetNumber { get { return this.mTargetNumber; } }
        public Text TextContainer { get { return this.mTextContainer; } set { this.mTextContainer = value; } }
#if TMP_PRO
        public TextMeshPro TextMesh { get { return this.mTextMesh; } set { this.mTextMesh = value; } }
#endif
        public string FullString { get { return this.mFullString; } }
        public int RoundPlace { get { return this.mRoundPlace; } set { this.mRoundPlace = value; } }
        public float CurrentNumber { get { return this.mCurrentNumber; } set { this.mCurrentNumber = value; } }
        public string PreString { get { return this.mPreString; } set { this.mPreString = value; } }
        public string PostString { get { return this.mPostString; } set { this.mPostString = value; } }
        public float AnimNumberTime { get { return this.mAnimNumberTime; } set { this.mAnimNumberTime = value; } }
        public float DeltaProduct { get { return this.mDeltaProduct; } set { this.mDeltaProduct = value; } }

        /* Functions */

        private void Update()
        {
#if UNITY_EDITOR
            TestInput();
#endif

            DoDeltaCurrentScore();
        }

#if UNITY_EDITOR
        private void TestInput()
        {
            if (!mTestWithKey)
                return;

            if (Input.GetKeyDown(mDeltaToA))
                UpdateNumber(mValueA);
            if (Input.GetKeyDown(mDeltaToB))
                UpdateNumber(mValueB);
        }
#endif

        /// <summary>
        /// Start the text delta number.
        /// </summary>
        /// <param name="targetNumber"> Number target to delt to. </param>
        /// <param name="anime"> Set the number directly. </param>
        public void UpdateNumber(float targetNumber, bool anime = true)
        {
            this.mTargetNumber = targetNumber;

            if (anime)
                mActive = true;
            else
            {
                mActive = false;  // To ensure, just deactive it.
                this.mCurrentNumber = targetNumber;
            }
        }

        /// <summary>
        /// Main algorithm to approach to targe score.
        /// </summary>
        private void DoDeltaCurrentScore()
        {
            if (!mActive)
                return;

            if (System.Math.Round(mTargetNumber, mRoundPlace) == System.Math.Round(mCurrentNumber, mRoundPlace))
            {
                mActive = false;
                return;
            }

            mAnimNumberTimer += Time.deltaTime;

            if (mAnimNumberTimer < mAnimNumberTime)
                return;

            float additionNumber = 1.0f / Mathf.Pow(10.0f, mRoundPlace);

            bool wasLarger = (mTargetNumber < mCurrentNumber);

            additionNumber *= mDeltaProduct;

            if (wasLarger)
                additionNumber = JCS_Mathf.ToNegative(additionNumber);

            mCurrentNumber += additionNumber;

            if ((wasLarger && mTargetNumber > mCurrentNumber) ||
                (!wasLarger && mTargetNumber < mCurrentNumber))
                mCurrentNumber = mTargetNumber;

            UpdateTextRender();

            // Reset timer.
            this.mAnimNumberTimer = 0.0f;
        }

        /// <summary>
        /// Actually make the text render on the screen.
        /// </summary>
        private void UpdateTextRender()
        {
#if TMP_PRO
            if (mTextContainer == null && mTextMesh == null)
#else
            if (mTextContainer == null)
#endif
            {
                JCS_Debug.LogError("Text slot cannot be null references...");
                return;
            }

            double renderNumber = System.Math.Round(mCurrentNumber, mRoundPlace);
            string renderNumberString = renderNumber.ToString();
            if (mPlusSignWhenPositive && JCS_Mathf.IsPositive(renderNumber))
                renderNumberString = "+" + renderNumberString;

            mFullString
                = PreString
                + renderNumberString
                + PostString;

            if (mTextContainer)
                mTextContainer.text = mFullString;
#if TMP_PRO
            if (mTextMesh)
                mTextMesh.text = mFullString;
#endif
        }
    }
}
