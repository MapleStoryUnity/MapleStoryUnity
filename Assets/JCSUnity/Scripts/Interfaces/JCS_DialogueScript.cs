/**
 * $File: JCS_DialogueScript.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Interface of the dialogue script.
    /// </summary>
    public abstract class JCS_DialogueScript : ScriptableObject
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_DialogueScript)")]

        [Tooltip("use to design the pages.")]
        public int Status = -1;

        /* Setter & Getter */

        public JCS_DialogueSystem ds { get { return JCS_DialogueSystem.FirstInstance(); } }

        /* Functions */

        /// <summary>
        /// Starting point of the dialogue.
        /// </summary>
        /// <param name="mode"> 
        /// 0: quest completed
        /// 1: quest!
        /// </param>
        /// <param name="type">
        /// 
        /// </param>
        /// <param name="selection">
        /// 0: selection 0 / No button / Decline button
        /// 1: selection 1 / Yes button / Accept button
        /// 2: selection 2
        /// , etc...
        /// </param>
        public abstract void Action(int mode, int type, int selection);

        /// <summary>
        /// When the quest is ended already...
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="type"></param>
        /// <param name="selection"></param>
        public virtual void End(int mode, int type, int selection)
        {
            // ..
        }

        /// <summary>
        /// Reset the action.
        /// </summary>
        public virtual void ResetAction()
        {
            Status = -1;
        }

        /// <summary>
        /// Return true if the selection is yes.
        /// </summary>
        public bool IsYes(int selection)
        {
            return selection == 1;
        }

        /// <summary>
        /// Return true if the selection is no.
        /// </summary>
        public bool IsNo(int selection)
        {
            return selection == 0;
        }
    }
}
