/**
 * $File: JCS_ApplicationQuitOnLoadAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Close the application when the level is loaded.
    /// </summary>
    public class JCS_ApplicationQuitOnLoadAction
        : MonoBehaviour
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            Application.Quit();
        }
    }
}
