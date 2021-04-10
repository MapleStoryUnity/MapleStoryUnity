/**
 * $File: JCS_PauseAction.cs $
 * $Date: 2017-02-24 05:57:37 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Action that you will stop the components.
    /// 
    /// Select the behaviour component and drag it into the list 
    /// so the pause manager will take care of the pause object. 
    /// If you are working on game that does not have pause, then 
    /// this script is basically not the good serve for you.
    /// </summary>
    public class JCS_PauseAction
        : MonoBehaviour
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_PauseAction) **")]

        [Tooltip(@"Select the behaviour component and drag it into the list so the 
pause manager will take care of the pause object. If you are working on game that 
does not have pause, then this script is basically not the good serve for you.")]
        [SerializeField]
        private MonoBehaviour[] mActionList = null;

        /* Setter & Getter */

        /* Functions */

        private void Start()
        {
            JCS_PauseManager.instance.AddActionToList(this);
        }

        /// <summary>
        /// Enable/Disable the behaviour component in the list.
        /// </summary>
        public void EnableBehaviourInTheList(bool act = true)
        {
            foreach (MonoBehaviour b in mActionList)
            {
                if (b == null)
                    continue;

                b.enabled = act;
            }
        }
    }
}
