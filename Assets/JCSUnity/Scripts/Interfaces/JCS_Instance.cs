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
    public class JCS_Instance<T> : MonoBehaviour
    {
        /* Variables */

        public static T instance = default(T);

        /* Setter & Getter */

        /* Functions */

    }
}
