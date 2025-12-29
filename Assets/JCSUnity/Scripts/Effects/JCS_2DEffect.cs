/**
 * $File: JCS_2DEffect.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// 2D effect base class.
    /// </summary>
    public abstract class JCS_2DEffect : MonoBehaviour
    {
        /* Variables */

        [Separator("📋 Check Variabless (JCS_2DEffect)")]

        [Tooltip("Flag to check if currently the effect active.")]
        [SerializeField]
        [ReadOnly]
        protected bool mEffect = false;

        /* Setter & Getter */

        public bool fffect { get { return mEffect; } set { mEffect = value; } }

        /* Functions */

    }
}
