/**
 * $File: JCS_LiquidBarText.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Text to show the current value for the liquid bar.
    /// </summary>
    [RequireComponent(typeof(JCS_GUILiquidBar))]
    public class JCS_LiquidBarText : MonoBehaviour
    {
        /* Variables */

        // Counter with the liquid bar?
        private JCS_GUILiquidBar mLiquidBar = null;

        [Separator("⚡️ Runtime Variables (JCS_LiquidBarText)")]

        [Tooltip("Text to do the effect.")]
        [SerializeField]
        private Text mCounterText = null;

        [Tooltip("Text Render maxinum of the liquid bar value.")]
        [SerializeField]
        private Text mFullText = null;

        [Tooltip("Sprite render current value of the liquid bar.")]
        [SerializeField]
        private JCS_DeltaNumber mCounterTextSprite = null;

        [Tooltip("Sprite Render maxinum of the liquid bar value.")]
        [SerializeField]
        private JCS_DeltaNumber mFullTextSprite = null;

        /* Setter & Getter */

        public JCS_DeltaNumber counterTextSprite { get { return mCounterTextSprite; } set { mCounterTextSprite = value; } }
        public JCS_DeltaNumber fullTextSprite { get { return mFullTextSprite; } set { mFullTextSprite = value; } }

        /* Functions */

        private void Awake()
        {
            mLiquidBar = GetComponent<JCS_GUILiquidBar>();
        }

        private void LateUpdate()
        {
            DoTextRender();
        }

        /// <summary>
        /// Find the value and set the text to value.
        /// </summary>
        private void DoTextRender()
        {
            if (mCounterText != null)
            {
                mCounterText.text = ((int)mLiquidBar.GetCurrentValue()).ToString();
            }

            if (mFullText != null)
            {
                mFullText.text = mLiquidBar.maxValue.ToString();
            }

            if (mCounterTextSprite != null)
            {
                mCounterTextSprite.UpdateNumber((int)mLiquidBar.GetCurrentValue());
            }

            if (mFullTextSprite != null)
            {
                mFullTextSprite.UpdateNumber((int)mLiquidBar.maxValue);
            }
        }
    }
}
