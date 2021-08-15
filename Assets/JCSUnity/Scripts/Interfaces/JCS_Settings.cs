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
    public abstract class JCS_Settings<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        /* Variables */

        public static T instance = default(T);

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Force the setting object singleton.
        /// </summary>
        /// <param name="_old"> old instance. </param>
        /// <param name="_new"> new instance. </param>
        protected T CheckSingleton(T _old, T _new)
        {
            if (_old != null)
            {
                TransferData(_old, _new);

                // Delete the old one
                Destroy(_old.gameObject);
            }

            return _new;
        }

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected abstract void TransferData(T _old, T _new);
    }
}
