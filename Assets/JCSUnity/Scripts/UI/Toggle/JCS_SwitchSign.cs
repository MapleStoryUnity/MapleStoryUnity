/**
 * $File: JCS_SwitchSign.cs $
 * $Date: 2018-08-21 23:11:05 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Sign of the switch button.
    /// </summary>
    [RequireComponent(typeof(JCS_TransformTweener))]
    [RequireComponent(typeof(JCS_ColorTweener))]
    public class JCS_SwitchSign : MonoBehaviour
    {
        /* Variables */

        private JCS_TransformTweener mTransformTweener = null;

        private JCS_ColorTweener mColorTweener = null;

        /* Setter & Getter */

        public JCS_TransformTweener transformTweener { get { return mTransformTweener; } }
        public JCS_ColorTweener colorTweener { get { return mColorTweener; } }

        /* Functions */

        private void Awake()
        {
            mTransformTweener = GetComponent<JCS_TransformTweener>();
            mColorTweener = GetComponent<JCS_ColorTweener>();
        }
    }
}
