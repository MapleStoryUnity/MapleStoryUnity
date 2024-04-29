/**
 * $File: JCS_DestroyObjectWithScreen.cs $
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
    /// Destroy the gameobject when the object is no longer render 
    /// on the screen.
    /// </summary>
    public class JCS_DestroyObjectWithScreen : MonoBehaviour
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_DestroyObjectWithScreen)")]

        [Tooltip("Trigger event flag.")]
        [SerializeField]
        private bool mDestroyWhenOutOfScreen = true;

        /* Setter & Getter */

        public bool DestroyWhenOutOfScreen { get { return this.mDestroyWhenOutOfScreen; } set { this.mDestroyWhenOutOfScreen = value; } }

        /* Functions */

        private void OnBecameInvisible()
        {
            if (!DestroyWhenOutOfScreen)
                return;

            Destroy(this.gameObject);
        }
    }
}
