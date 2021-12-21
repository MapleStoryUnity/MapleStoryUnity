/**
 * $File: JCS_ActivePanelButton.cs $
 * $Date: 2017-09-04 16:32:23 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Active panel button.
    /// </summary>
    public class JCS_ActivePanelButton : JCS_Button
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_ActivePanelButton) **")]

        [Tooltip("Panels to be active.")]
        [SerializeField]
        private JCS_DialogueObject[] mDialogueObjects = null;

        [Tooltip("Tween Panels to be active.")]
        [SerializeField]
        private JCS_TweenPanel[] mTweenPanels = null;

        [Tooltip("Play dialogue sound.")]
        [SerializeField]
        private bool mPlaySound = true;

        /* Setter & Getter */

        public JCS_DialogueObject[] DialogueObjects { get { return this.mDialogueObjects; } }
        public JCS_TweenPanel[] TweenPanels { get { return this.mTweenPanels; } }

        public bool PlaySound { get { return this.mPlaySound; } set { this.mPlaySound = value; } }

        /* Functions */

        public override void OnClick()
        {
            JCS_GUIUtil.ActivePanels(mDialogueObjects, mPlaySound);
            JCS_GUIUtil.ActivePanels(mTweenPanels);
        }
    }
}
