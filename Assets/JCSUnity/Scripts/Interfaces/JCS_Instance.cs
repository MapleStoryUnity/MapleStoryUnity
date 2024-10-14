/**
 * $File: JCS_Instance.cs $
 * $Date: 2021-12-26 01:31:16 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Singleton instance interface.
    /// </summary>
    public abstract class JCS_Instance<T> : MonoBehaviour
    {
        /* Variables */

        public static T instance = default(T);

        /* Setter & Getter */

        /* Functions */

    }

    /// <summary>
    /// Singleton instance interface to keep the old instance.
    /// </summary>
    public class JCS_InstanceOld<T> : JCS_Instance<T>
        where T : MonoBehaviour
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Check singleton for keep the old one.
        /// </summary>
        /// <param name="_new"> Current instance. </param>
        protected void CheckInstance(T _new)
        {
            // Destory the new one; and keep the old one.
            if (instance != null)
            {
                Destroy(_new.gameObject);
                return;
            }

            // Only assign once!
            instance = _new;
        }
    }

    /// <summary>
    /// Singleton instance interface to keep the new instance.
    /// </summary>
    public class JCS_InstanceNew<T> : JCS_Instance<T>
        where T : MonoBehaviour
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Check singleton for keep the new one.
        /// </summary>
        /// <param name="_new"> Current instance. </param>
        protected void CheckInstance(T _new)
        {
            // Destory the old one!
            if (instance != null)
                Destroy(instance);

            // Assign the new one!
            instance = _new;
        }
    }
}
