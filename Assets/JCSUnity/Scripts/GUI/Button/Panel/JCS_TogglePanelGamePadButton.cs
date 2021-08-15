/**
 * $File: JCS_TogglePanelGamePadButton.cs $
 * $Date: 2020-04-02 12:37:09 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2020 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Button that toggle one panel. (Game Pad)
    /// </summary>
    public class JCS_TogglePanelGamePadButton : JCS_ToggleGamePadButton
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_TogglePanelGamePadButton) **")]

        [Tooltip("Panel (dialogue) you want to toggle.")]
        [SerializeField]
        private JCS_DialogueObject[] mDialogueObjects = null;

        [Tooltip("Panel (tween) you want to toggle.")]
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

        protected virtual void Start()
        {
            acitveFunc += ActivePanel;
            deactiveFunc += DeactivePanel;
        }

        private void ActivePanel()
        {
            JCS_GUIUtil.ActivePanels(mDialogueObjects, mPlaySound);
            JCS_GUIUtil.ActivePanels(mTweenPanels);
        }

        private void DeactivePanel()
        {
            JCS_GUIUtil.DeactivePanels(mDialogueObjects, mPlaySound);
            JCS_GUIUtil.DeactivePanels(mTweenPanels);
        }
    }
}
