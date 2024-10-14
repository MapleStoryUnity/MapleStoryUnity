/**
 * $File: JCS_DeactivePanelGamepadButton.cs $
 * $Date: 2017-10-27 12:33:45 $
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
    /// Deactive panel button. (Gamepad)
    /// </summary>
    public class JCS_DeactivePanelGamepadButton : JCS_GamepadButton
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_DeactivePanelGamepadButton)")]

        [Tooltip("Transforms to be deactive.")]
        [SerializeField]
        private Transform[] mTransforms = null;

        [Tooltip("Panels to be deactive.")]
        [SerializeField]
        private JCS_PanelRoot[] mPanelRoots = null;

        [Tooltip("Tween Panels to be deactive.")]
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
            JCS_Util.SetActive(mTransforms, false);

            JCS_UIUtil.DeactivePanels(mPanelRoots, mPlaySound);
            JCS_UIUtil.DeactivePanels(mTweenPanels);
        }
    }
}
