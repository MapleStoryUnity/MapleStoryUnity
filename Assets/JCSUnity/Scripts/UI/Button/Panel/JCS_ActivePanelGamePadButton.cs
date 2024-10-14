/**
 * $File: JCS_ActivePanelGamepadButton.cs $
 * $Date: 2017-10-27 12:32:31 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Active panel button. (Gamepad)
    /// </summary>
    public class JCS_ActivePanelGamepadButton : JCS_GamepadButton
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_ActivePanelGamepadButton)")]

        [Tooltip("Transform to be active.")]
        [SerializeField]
        private Transform[] mTransforms = null;

        [Tooltip("Panels to be active.")]
        [SerializeField]
        private JCS_PanelRoot[] mPanelRoots = null;

        [Tooltip("Tween Panels to be active.")]
        [SerializeField]
        private JCS_TweenPanel[] mTweenPanels = null;

        [Tooltip("Play dialogue sound.")]
        [SerializeField]
        private bool mPlaySound = true;

        /* Setter & Getter */

        public Transform[] transforms { get { return this.mTransforms; } }
        public JCS_PanelRoot[] PanelRoots { get { return this.mPanelRoots; } }
        public JCS_TweenPanel[] TweenPanels { get { return this.mTweenPanels; } }

        public bool PlaySound { get { return this.mPlaySound; } set { this.mPlaySound = value; } }

        /* Functions */

        public override void OnClick()
        {
            JCS_Util.SetActive(mTransforms, true);

            JCS_UIUtil.ActivePanels(mPanelRoots, mPlaySound);
            JCS_UIUtil.ActivePanels(mTweenPanels);
        }
    }
}
