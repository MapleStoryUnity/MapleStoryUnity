/**
 * $File: JCS_ToggleSign.cs $
 * $Date: 2018-08-21 23:11:05 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Sign of the toggle button.
    /// </summary>
    [RequireComponent(typeof(JCS_TransformTweener))]
    [RequireComponent(typeof(JCS_ColorTweener))]
    public class JCS_ToggleSign
        : MonoBehaviour
    {
        /* Variables */

        private JCS_TransformTweener mTransformTweener = null;

        private JCS_ColorTweener mColorTweener = null;

        /* Setter & Getter */

        public JCS_TransformTweener TransformTweener { get { return this.mTransformTweener; } }
        public JCS_ColorTweener ColorTweener { get { return this.mColorTweener; } }


        /* Functions */

        private void Awake()
        {
            this.mTransformTweener = this.GetComponent<JCS_TransformTweener>();
            this.mColorTweener = this.GetComponent<JCS_ColorTweener>();
        }
    }
}
