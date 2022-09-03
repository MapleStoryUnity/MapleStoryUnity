/**
 * $File: JCS_ActivePanelGamepadButton.cs $
 * $Date: 2017-10-27 12:32:31 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Active panel button. (Gamepad)
    /// </summary>
    public class JCS_ActivePanelGamepadButton : JCS_GamepadButton
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_ActivePanelGamepadButton) **")]

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
            JCS_UIUtil.ActivePanels(mDialogueObjects, mPlaySound);
            JCS_UIUtil.ActivePanels(mTweenPanels);
        }
    }
}
