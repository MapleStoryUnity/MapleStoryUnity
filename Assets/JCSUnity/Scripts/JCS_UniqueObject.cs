/**
 * $File: JCS_UniqueObject.cs $
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
    /// Singleton pattern in scripting layer.
    /// </summary>
    public class JCS_UniqueObject : MonoBehaviour
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            // only the root object can do this.
            if (transform.parent == null)
                DontDestroyOnLoad(gameObject);
            else
            {
                Debug.LogWarning("Only the root object can be use DontDestoryOnLoad");
            }
        }
    }
}
