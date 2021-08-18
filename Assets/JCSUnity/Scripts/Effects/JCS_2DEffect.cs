/**
 * $File: JCS_2DEffect.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// 2D effect base class.
    /// </summary>
    public abstract class JCS_2DEffect : MonoBehaviour
    {
        /* Variables */

        [Header("** Check Variables (JCS_2DEffect) **")]

        [Tooltip("Flag to check if currently the effect active.")]
        [SerializeField]
        protected bool mEffect = false;

        /* Setter & Getter */

        public bool Effect { get { return this.mEffect; } set { this.mEffect = value; } }

        /* Functions */

    }
}
