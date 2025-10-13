/**
 * $File: JCS_ResponsiveFitAll.cs $
 * $Date: 2021-09-06 15:49:01 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Force the rect transform to match to full screen.
    /// </summary>
    public class JCS_ResponsiveFitAll : MonoBehaviour
    {
        /* Variables */

        private RectTransform mRectTransform = null;

        /* Setter & Getter */

        /* Functions */

        private void Start()
        {
            mRectTransform = GetComponent<RectTransform>();

            List<RectTransform> childs = JCS_Util.ForceDetachChildren(mRectTransform);

            JCS_Util.ReattachSelf(mRectTransform, (parent) =>
            {
                mRectTransform.localScale = Vector3.one;
                mRectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            });

            JCS_Util.AttachChildren(mRectTransform, childs);

            mRectTransform.localPosition = Vector3.zero;
        }
    }
}
