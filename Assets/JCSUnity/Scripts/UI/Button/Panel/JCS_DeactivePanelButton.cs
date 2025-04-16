/**
 * $File: JCS_DeactivePanelButton.cs $
 * $Date: 2017-09-04 16:37:26 $
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
    /// Deactive panel button.
    /// </summary>
    public class JCS_DeactivePanelButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        /* Variables */

        [Separator("Runtime Variables (JCS_DeactivePanelButton)")]

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
