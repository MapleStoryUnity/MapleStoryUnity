/**
 * $File: JCS_HueController.cs $
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
    /// Simulate the hue control.
    /// </summary>
    public class JCS_HueController : MonoBehaviour
    {
        /* Variables */

        [Separator("Check Variables (JCS_HueController)")]

        // record down the highest r/g/b color.
        [SerializeField]
        [Range(0, 1)]
        [ReadOnly]
        private float mHighestRGB = 0;

        [SerializeField]
        [ReadOnly]
        private Color mTargetColor = Color.white;

        [SerializeField]
        [ReadOnly]
        private Color[] mListColor = null;

        [Separator("Runtime Variables (JCS_HueController)")]

        [Tooltip("Any Graphic with color component.")]
        [SerializeField]
        private Graphic mColorGraphic = null;

        [Tooltip("How fast the hue changes?")]
        [SerializeField]
        [Range(0.01f, 10.0f)]
        private float mFriction = 1.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_DeltaTimeType mDeltaTimeType = JCS_DeltaTimeType.DELTA_TIME;

        private int mListCounter = 0;

        /* Setter & Getter */

        public JCS_DeltaTimeType DeltaTimeType { get { return this.mDeltaTimeType; } set { this.mDeltaTimeType = value; } }

        /* Functions */

        private void Start()
        {
            if (mColorGraphic == null)
            {
                JCS_Debug.LogError("Do nothing without Graphic object assign");
                return;
            }

            mTargetColor = mColorGraphic.color;

            mHighestRGB = GetHigestRGB(mTargetColor);

            mListColor = GetListOfColor();
        }

        private void Update()
        {
            DoCycle();

            TowardColor();

        }

        /// <summary>
        /// Cycle throguh the color list.
        /// </summary>
        private void DoCycle()
        {
            if (mTargetColor != mColorGraphic.color)
                return;

            // reset the next color
            SetNextColorByList();
        }

        /// <summary>
        /// Set the next color depends on the list.
        /// </summary>
        private void SetNextColorByList()
        {
            ++mListCounter;

            if (mListCounter >= mListColor.Length)
                mListCounter = 0;

            mTargetColor = mListColor[mListCounter];
        }

        /// <summary>
        /// Movement of the color change.
        /// </summary>
        private void TowardColor()
        {
            mColorGraphic.color += (mTargetColor - mColorGraphic.color) / mFriction * JCS_Time.DeltaTime(mDeltaTimeType);
        }

        /// <summary>
        /// Initialize the color and list all 
        /// the possible color.
        /// </summary>
        /// <returns> 
        /// All the possible color with 
        /// the hue settings. 
        /// </returns>
        private Color[] GetListOfColor()
        {
            Color[] tempColors = new Color[6];

            tempColors[0] = new Color(mHighestRGB, 0, 0);
            tempColors[1] = new Color(mHighestRGB, mHighestRGB, 0);
            tempColors[2] = new Color(0, mHighestRGB, 0);
            tempColors[3] = new Color(0, mHighestRGB, mHighestRGB);
            tempColors[4] = new Color(0, 0, mHighestRGB);
            tempColors[5] = new Color(mHighestRGB, 0, mHighestRGB);

            return tempColors;
        }

        /// <summary>
        /// Get the highest value from r,g,b
        /// </summary>
        /// <param name="color"> color to get </param>
        /// <returns> highest value. </returns>
        private float GetHigestRGB(Color color)
        {
            float rg = JCS_Mathf.Max(color.r, color.g);

            return JCS_Mathf.Max(color.b, rg);
        }
    }
}
