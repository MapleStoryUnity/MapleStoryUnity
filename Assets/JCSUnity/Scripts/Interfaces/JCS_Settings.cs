/**
 * $File: JCS_Settings.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Interface of all setting class.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class JCS_Settings<T> : JCS_InstanceNew<T>
        where T : MonoBehaviour
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Check singleton for keep the new one.
        /// </summary>
        /// <param name="_new"> new instance </param>
        protected virtual void CheckInstance(T _new)
        {
            base.CheckInstance(_new, true);
        }
    }
}
