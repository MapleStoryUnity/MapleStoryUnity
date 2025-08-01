/**
 * $File: JCS_PauseAction.cs $
 * $Date: 2017-02-24 05:57:37 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

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
    public class JCS_PauseAction : MonoBehaviour
    {
        /* Variables */

        [Separator("Check Variables (JCS_PauseAction)")]

        [Tooltip("Record enabled state; make disabled behaviours reamin disabled.")]
        [SerializeField]
        [ReadOnly]
        private List<bool> mRecordEnabled = null;

        [Separator("Runtime Variables (JCS_PauseAction)")]

        [Tooltip(@"Select the behaviour component and drag it into the list so the 
pause manager will take care of the pause object. If you are working on game that 
does not have pause, then this script is basically not the good serve for you.")]
        [SerializeField]
        private Behaviour[] mActionList = null;

        /* Setter & Getter */

        /* Functions */

        private void Start()
        {
            JCS_PauseManager.FirstInstance().AddActionToList(this);
        }

        /// <summary>
        /// Enable/Disable the behaviour component in the list.
        /// </summary>
        public void EnableBehaviourInTheList(bool act = true)
        {
            // Before pausing.
            bool pausing = !act;

            if (pausing)
                mRecordEnabled.Clear();

            int index = 0;

            foreach (Behaviour b in mActionList)
            {
                if (b == null)
                    continue;

                if (pausing)
                    mRecordEnabled.Add(b.enabled);

                b.enabled = (pausing) ? act : mRecordEnabled[index];

                ++index;
            }
        }
    }
}
