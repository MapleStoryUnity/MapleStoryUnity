/**
 * $File: JCS_QuitApplicationOnLoadEvent.cs $
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
    /// Event that will quit the application.
    /// </summary>
    public class JCS_QuitApplicationOnLoadEvent : MonoBehaviour
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        private void Start()
        {
            Application.Quit();
        }
    }
}
