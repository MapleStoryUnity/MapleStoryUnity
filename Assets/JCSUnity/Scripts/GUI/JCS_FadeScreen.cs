/**
 * $File: JCS_FadeScreen.cs $
 * $Date: 2017-09-11 05:09:46 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Default fade screen wrapper.
    /// </summary>
    [RequireComponent(typeof(JCS_FadeObject))]
    public class JCS_FadeScreen : MonoBehaviour
    {
        /* Variables */

        private JCS_FadeObject mFadeObject = null;

        /* Setter & Getter */

        public JCS_FadeObject FadeObject { get { return this.mFadeObject; } }

        /* Functions */

        private void Awake()
        {
            this.mFadeObject = this.GetComponent<JCS_FadeObject>();
        }
    }
}
