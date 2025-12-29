/**
 * $File: JCS_ActivePanelButton.cs $
 * $Date: 2017-09-04 16:32:23 $
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
    /// Active panel button.
    /// </summary>
    public class JCS_ActivePanelButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_ActivePanelButton)")]

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

        public Transform[] transforms { get { return mTransforms; } }
        public JCS_PanelRoot[] panelRoots { get { return mPanelRoots; } }
        public JCS_TweenPanel[] tweenPanels { get { return mTweenPanels; } }

        public bool playSound { get { return mPlaySound; } set { mPlaySound = value; } }

        /* Functions */

        public override void OnClick()
        {
            JCS_Util.SetActive(mTransforms, true);

            JCS_UIUtil.ActivePanels(mPanelRoots, mPlaySound);
            JCS_UIUtil.ActivePanels(mTweenPanels);
        }
    }
}
