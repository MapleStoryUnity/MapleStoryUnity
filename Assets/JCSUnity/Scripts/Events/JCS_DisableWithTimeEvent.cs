/**
 * $File: JCS_DisableWithTimeEvent.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Disable the gameobject after a certain time.
    /// </summary>
    public class JCS_DisableWithTimeEvent
        : MonoBehaviour
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_DisableWithTimeEvent) **")]

        [Tooltip("Time to disable.")]
        [SerializeField]
        private float mTime = 2;

        private float mTimer = 0;

        /* Setter & Getter */

        /* Functions */

        private void Update()
        {
            mTimer += Time.deltaTime;

            if (mTime < mTimer)
            {
                // reset timer.
                mTimer = 0;

                // disable this object
                this.gameObject.SetActive(false);
            }
        }
    }
}
