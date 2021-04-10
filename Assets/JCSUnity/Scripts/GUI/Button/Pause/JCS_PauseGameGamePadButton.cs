/**
 * $File: JCS_PauseGameGamePadButton.cs $
 * $Date: 2017-10-27 11:58:09 $
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
    /// Pause the game with button. (Game Pad)
    /// 
    /// ATTENTION: this should have at least one "JCS_GamePadButton" with this.
    /// </summary>
    public class JCS_PauseGameGamePadButton
    : MonoBehaviour
    {
        [Header("** Require Variables (JCS_PauseGameGamePadButton) **")]

        [Tooltip(@"You need this variable in order get the button 
work properly.")]
        [SerializeField]
        private JCS_GamePadButton mJCSGamePadButton = null;


        private void Awake()
        {
            // try to get the component.
            if (mJCSGamePadButton == null)
                this.mJCSGamePadButton = this.GetComponent<JCS_GamePadButton>();

            if (mJCSGamePadButton != null)
                mJCSGamePadButton.SetCallback(PauseGame);
        }

        /// <summary>
        /// Pause the game.
        /// </summary>
        public void PauseGame()
        {
            // turn on the game pause button.
            JCS_GameManager.instance.GAME_PAUSE = true;
        }
    }
}
