/**
 * $File: JCS_DestroyReminder.cs $
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
    /// Object remind the the current transform 
    /// should be destroy but did not destroyed.
    /// </summary>
    public class JCS_DestroyReminder
        : MonoBehaviour
    {
        /* Variables */

        private float mTimer = 0;
        private float mRemindPerTime = 5;

        /* Setter & Getter */

        /* Functions */

        private void Update()
        {
            mTimer += Time.deltaTime;

            if (mRemindPerTime < mTimer)
            {
                JCS_Debug.LogReminder(
                     "Object you should destroy but you did not...");

#if (UNITY_EDITOR)
                gameObject.name = "Object you should destroy but you did not...";
#endif
            }
        }
    }
}
