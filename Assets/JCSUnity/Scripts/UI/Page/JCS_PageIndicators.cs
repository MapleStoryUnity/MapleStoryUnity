/**
 * $File: JCS_PageIndicators.cs $
 * $Date: 2021-08-18 16:49:25 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JCSUnity
{
    /// <summary>
    /// Display page indicator for the mobile like UI/UX (dribbles).
    /// </summary>
    public class JCS_PageIndicators : MonoBehaviour
    {
        /* Variables */

        [Header("** Initialize Variables (JCS_PageIndicators) **")]

        [Tooltip("List of image containers to display indicators.")]
        [SerializeField]
        private List<Image> mIndicators = null;

        [Tooltip("Image represnts a active indicator.")]
        [SerializeField]
        private Sprite mActiveSprite = null;

        [Tooltip("Image represnts a inactive indicator.")]
        [SerializeField]
        private Sprite mInactiveSprite = null;

        /* Setter & Getter */

        public List<Image> Indicators { get { return this.mIndicators; } set { this.mIndicators = value; } }
        public Sprite ActiveSprite { get { return this.mActiveSprite; } set { this.mActiveSprite = value; } }
        public Sprite InactiveSprite { get { return this.mInactiveSprite; } set { this.mInactiveSprite = value; } }

        /* Functions */

        private void Awake()
        {
            SetSprite(mInactiveSprite);
        }

        /// <summary>
        /// Light the indicator that corresponds to the PAGE.
        /// </summary>
        /// <param name="page"> Target page we want the indicator to notify the user. </param>
        public void SetPage(int page)
        {
            SetSprite(mInactiveSprite);

            if (!JCS_Util.WithInArrayRange(page, mIndicators))
            {
                JCS_Debug.LogWarning("Page indicators out of range exception");
                return;
            }

            mIndicators[page].sprite = mActiveSprite;
        }

        /// <summary>
        /// Apply SPRITE to all indicator's image containers.
        /// </summary>
        /// <param name="sprite"> The sprite to apply. </param>
        private void SetSprite(Sprite sprite)
        {
            this.mIndicators = JCS_Util.RemoveEmptySlotIncludeMissing(this.mIndicators);

            foreach (Image ind in mIndicators)
            {
                ind.sprite = sprite;
            }
        }
    }
}
