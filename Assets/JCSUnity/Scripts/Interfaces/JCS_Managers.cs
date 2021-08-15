/**
 * $File: JCS_Managers.cs $
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
    /// Interface of managers.
    /// 
    /// Manager have to be in every scene 
    /// in Unity Engine's scene system.
    /// </summary>
    public class JCS_Managers<T> : MonoBehaviour
    {
        /* Variables */

        public static T instance = default(T);

        /* Setter & Getter */

        /* Functions */

    }
}
