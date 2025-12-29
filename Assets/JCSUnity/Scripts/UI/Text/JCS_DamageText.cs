/**
 * $File: JCS_DamageText.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Damage text on the mob.
    /// </summary>
    public class JCS_DamageText : MonoBehaviour
    {
        /* Variables */

        /// <summary>
        /// Text effect when removing text on the screen.
        /// </summary>
        private enum TextEffect
        {
            MOVE_UP,    // move up a bit and fade out
            FADE,       // stay where they are and do fade out
            SLIDE_OUT,  // slide away and fade outs
        };

        private bool mActive = false;

        [Separator("⚡️ Runtime Variables (JCS_DamageText)")]

        [Tooltip("Type of how damage text goes out.")]
        [SerializeField]
        private TextEffect mDamageTextEffectType = TextEffect.FADE;

        [Tooltip("How fast the damage text moving.")]
        // the damage text moving speed
        [SerializeField]
        [Range(0.0f, 300.0f)]
        private float mMoveSpeed = 0.2f;

        [Tooltip("Spacing between each digit.")]
        [SerializeField]
        [Range(0.0f, 3000.0f)]
        private float mSpacing = 0.25f;

        [Tooltip("How fast the damage text fade out.")]
        [SerializeField]
        [Range(0.0f, 30.0f)]
        private float mFadeSpeed = 1.0f;

        [Tooltip("Scene Layer in the render queue.")]
        // the lower order layer in the queue
        [SerializeField]
        private int mBaseOrderLayer = 5;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("🔍 Caplitaize Effect")]

        [Tooltip("The first letter will be bigger then other base on the scale variable below.")]
        [SerializeField]
        private bool mCapitalizeLetter = true;

        [Tooltip("Scale of the first digit.")]
        [SerializeField]
        private Vector3 mCapitalLetterScale = new Vector3(2, 2, 2);

        [Header("🔍 Wave Zigge Effect")]

        [Tooltip("Each digit will goes up and down in order.")]
        [SerializeField]
        private bool mWaveZiggeEffect = true;

        [Tooltip("How much it does up and down.")]
        [SerializeField]
        private float mWaveZigge = 0.1f;

        [Header("🔍 Asymptotic Scale Effect (JCS_DamageText)")]

        [Tooltip("Do the asymptotic scale effect?")]
        [SerializeField]
        private bool mAsymptoticScaleEffect = true;

        [Tooltip("Scale value when doing the asymptotic scale effect.")]
        [SerializeField]
        private float mAsymptoticScale = 0.1f;

        /*
         * Current hold how many digit. 
         * 
         * so if we use 5 digit this time.next time we use 3 digit. 
         * we dont have to spawn three brand new digit to handle. We 
         * simple just reuse the 5 digit we created previously.
         */
        // handle all the sprite in order to change the color
        private List<SpriteRenderer> mSpriteRenderers = null;

        private SpriteRenderer mCriticalSprite = null;

        [Header("🔍 Critical Strike Sprite")]

        [Tooltip("Scale value to critical sprites.")]
        [SerializeField]
        private Vector3 mCritialSpriteScale = Vector3.one;

        [Tooltip("Spacing between each digit on x axis.")]
        [SerializeField]
        private float mSpacingX = 0.5f;

        [Tooltip("Spacing between each digit on y axis.")]
        [SerializeField]
        private float mSpacingY = 0.5f;

        [Tooltip("Randomize the size?")]
        [SerializeField]
        private bool mRandomSize = true;

        [Tooltip("Minimum size value.")]
        [SerializeField]
        private float mMinSize = -1.0f;

        [Tooltip("Maximum size value.")]
        [SerializeField]
        private float mMaxSize = 1.0f;

        // Damage Text
        [Header("🔍 Damage Text Setting (if the game have this kind of feature fill this out!)")]

        [Tooltip("Damage text miss.")]
        [SerializeField]
        private Sprite mDamageTextMiss = null;

        [Tooltip("Damage text critical strike.")]
        [SerializeField]
        private Sprite mCritialStrike = null;

        [Tooltip("Damage text number 0.")]
        [SerializeField]
        private Sprite mDamageText0 = null;

        [Tooltip("Damage text number 1.")]
        [SerializeField]
        private Sprite mDamageText1 = null;

        [Tooltip("Damage text number 2.")]
        [SerializeField]
        private Sprite mDamageText2 = null;

        [Tooltip("Damage text number 3.")]
        [SerializeField]
        private Sprite mDamageText3 = null;

        [Tooltip("Damage text number 4.")]
        [SerializeField]
        private Sprite mDamageText4 = null;

        [Tooltip("Damage text number 5.")]
        [SerializeField]
        private Sprite mDamageText5 = null;

        [Tooltip("Damage text number 6.")]
        [SerializeField]
        private Sprite mDamageText6 = null;

        [Tooltip("Damage text number 7.")]
        [SerializeField]
        private Sprite mDamageText7 = null;

        [Tooltip("Damage text number 8.")]
        [SerializeField]
        private Sprite mDamageText8 = null;

        [Tooltip("Damage text number 9.")]
        [SerializeField]
        private Sprite mDamageText9 = null;

        /* Setter & Getter */

        public bool isActive() { return mActive; }
        public JCS_TimeType timeType { get { return mTimeType; } set { mTimeType = value; } }

        /* Functions */

        private void Awake()
        {
            mSpriteRenderers = new List<SpriteRenderer>();

            if (mCriticalSprite == null && JCS_Util.IsClone(transform))
            {
                var gm = new GameObject();
                mCriticalSprite = gm.AddComponent<SpriteRenderer>();
                gm.transform.SetParent(transform);
#if UNITY_EDITOR
                gm.name = "Criticl Sprite";
#endif
            }
        }

        private void Update()
        {
            if (!mActive)
                return;

            // Do active thing
            DoEffect();
        }

        /// <summary>
        /// Spawn one damage text.
        /// </summary>
        /// <param name="damage"> damage value. </param>
        /// <param name="pos"> effect position. (world space) </param>
        public void SpawnDamageText(int damage, Vector3 pos)
        {
            transform.position = pos;

            int totalDigit = damage.ToString().Length;

            // Number is from `left` to `right`, calculate the very left position.
            float leftPosX = mSpacing * (totalDigit - 1) / 2.0f;

            // if damage lower than equals to one set digit to 1 in order to
            // get one "MISS" text!
            if (damage <= 0)
                totalDigit = 1;

            bool isEvenNumber = false;

            if ((totalDigit % 2) == 0)
                isEvenNumber = true;

            for (int digit = 1; digit <= totalDigit; ++digit)
            {
                // Add one new digit.
                if (mSpriteRenderers.Count <= digit - 1)
                {
                    var gm = new GameObject();
                    var newSr = gm.AddComponent<SpriteRenderer>();

                    // set the parent
                    gm.transform.SetParent(transform);

                    // add to manage
                    mSpriteRenderers.Add(newSr);
                }

                SpriteRenderer sr = mSpriteRenderers[digit - 1];

                // Reset transform values.
                sr.transform.localPosition = Vector3.zero;
                sr.transform.localEulerAngles = Vector3.zero;
                sr.transform.localScale = Vector3.one;

                // get single digit
                int digitNum;
                if (damage <= 0)
                    digitNum = 11;      // miss text
                else
                    digitNum = GetSingleDigit(digit, damage);

                // set the sprite base on digit number
                sr.sprite = GetSingleDigitSprite(digitNum);

                // set the sorting layer
                sr.sortingOrder = mBaseOrderLayer - digit;

                // set the position base on the spacing and target's center!
                Vector3 newPos = sr.transform.localPosition;

                newPos.x = leftPosX - (mSpacing * (digit - 1));

                // do wave effect
                if (mWaveZiggeEffect)
                {
                    float diffZig = mWaveZigge;

                    if ((digit % 2) == 0)
                        diffZig = -diffZig;

                    if (isEvenNumber)
                        diffZig = -diffZig;

                    newPos.y += diffZig;
                }
                sr.transform.localPosition = newPos;

                Vector3 newScale = sr.transform.localScale;
                // Asymptotic scale
                if (mAsymptoticScaleEffect)
                {
                    float diffScale = mAsymptoticScale * digit;
                    newScale.x += diffScale;
                    newScale.y += diffScale;
                    newScale.z += diffScale;
                }

                // Capitalize Effect
                if (mCapitalizeLetter)
                {
                    if (digit == totalDigit)
                    {
                        newScale += mCapitalLetterScale;
                    }
                }

                sr.transform.localScale = newScale;
            }

            // Check if critical texture exist and spawn it
            if (mCritialStrike != null && damage != 0)
            {
                SpriteRenderer sr = mCriticalSprite;

                // Reset transform values.
                sr.transform.localPosition = Vector3.zero;
                sr.transform.localEulerAngles = Vector3.zero;
                sr.transform.localScale = Vector3.one;

                // 最左邊的空位
                int theLeftDigitSpace = (totalDigit + 1);

                // set the sprite base on digit number
                sr.sprite = GetSingleDigitSprite(10);

                sr.sortingOrder = mBaseOrderLayer - theLeftDigitSpace;

                Vector3 newPos = sr.transform.localPosition;
                newPos.x = leftPosX - (mSpacing * totalDigit);

                // Adjust a bit so we have a little control the position of
                // this sprite! (Critical Image)
                newPos.x += mSpacingX;
                newPos.y += mSpacingY;

                sr.transform.localPosition = newPos;

                //---------------
                // Apply Scale
                Vector3 newScale = mCritialSpriteScale;

                if (mRandomSize)
                {
                    // Make sure the setting is correct!
                    if (mMinSize < mMaxSize)
                    {
                        float applyRandom = Random.Range(mMinSize, mMinSize);
                        newScale.x += applyRandom;
                        newScale.y += applyRandom;
                    }
                    else
                        Debug.LogError("Max size cannot be smaller than Min size");
                }

                sr.transform.localScale = newScale;
            }

            // start the effect
            mActive = true;
        }

        /// <summary>
        /// Get the single digit sprite depends on the digit value.
        /// </summary>
        /// <param name="num"> digit value </param>
        /// <returns> sprite of that digit valuye. </returns>
        private Sprite GetSingleDigitSprite(int num)
        {
            switch (num)
            {
                case 0: return mDamageText0;
                case 1: return mDamageText1;
                case 2: return mDamageText2;
                case 3: return mDamageText3;
                case 4: return mDamageText4;
                case 5: return mDamageText5;
                case 6: return mDamageText6;
                case 7: return mDamageText7;
                case 8: return mDamageText8;
                case 9: return mDamageText9;
                case 10: return mCritialStrike;
                case 11: return mDamageTextMiss;
            }

            // careful!
            return null;
        }

        /// <summary>
        /// Get single digit value from a number.
        /// </summary>
        /// <param name="digit"> digit count. </param>
        /// <param name="number"> number use to find digit value. </param>
        /// <returns> digit value. </returns>
        private int GetSingleDigit(int digit, int number)
        {
            int totalDigit = number.ToString().Length;
            if (digit > totalDigit)
                return -1;

            int digitCount = (int)Mathf.Pow(10, digit);

            int remainder = number % digitCount;
            int po = digit - 1;
            int divider = (int)Mathf.Pow(10, po);

            return remainder / divider;
        }

        /// <summary>
        /// Do effect depends on what 'damage text' effect.
        /// </summary>
        private void DoEffect()
        {
            switch (mDamageTextEffectType)
            {
                case TextEffect.MOVE_UP:
                    MoveUp();
                    break;
                case TextEffect.FADE:
                    Fade();
                    break;
                case TextEffect.SLIDE_OUT:
                    SlideOut();
                    break;
            }
        }

        /// <summary>
        /// Do the moving up effect to the damage text.
        /// </summary>
        private void MoveUp()
        {
            Vector3 newPos = transform.position;
            newPos.y += mMoveSpeed * JCS_Time.ItTime(mTimeType);
            transform.position = newPos;

            Fade();
        }

        /// <summary>
        /// Do the fade to the damage text.
        /// </summary>
        private void Fade()
        {
            Color col = new Color();
            float fadeValue = mFadeSpeed * JCS_Time.ItTime(mTimeType);

            foreach (SpriteRenderer sr in mSpriteRenderers)
            {
                col = sr.color;
                col.a -= fadeValue;
                sr.color = col;
            }

            // fade the cirtical sprite too.
            col = mCriticalSprite.color;
            col.a -= fadeValue;
            mCriticalSprite.color = col;

            if (col.a <= 0.0f)
            {
                mActive = false;

                /* No longer using  */
                SetAllDigitToNull();
            }
        }

        /// <summary>
        /// Do the slide out effect to the damage text.
        /// </summary>
        private void SlideOut()
        {
            Fade();
        }

        /// <summary>
        /// Make all the digit sprite in this damage text to null.
        /// So make this damage damage use for next time.
        /// </summary>
        private void SetAllDigitToNull()
        {
            foreach (SpriteRenderer sr in mSpriteRenderers)
            {
                /* 'SpriteRenderer' cannot be null in this case. */
                sr.sprite = null;
                sr.color = Color.white;
            }

            mCriticalSprite.sprite = null;
            mCriticalSprite.color = Color.white;
        }
    }
}
