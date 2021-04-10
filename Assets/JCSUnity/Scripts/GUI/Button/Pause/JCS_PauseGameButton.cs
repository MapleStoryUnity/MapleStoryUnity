/**
 * $File: JCS_PauseGameButton.cs $
 * $Date: 2017-02-24 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Pause the game with button.
    /// 
    /// ATTENTION: this should have at least one "JCS_Button" with this.
    /// </summary>
    public class JCS_PauseGameButton 
        : MonoBehaviour
    {
        [Header("** Require Variables (JCS_PauseGameButton) **")]

        [Tooltip(@"You need this variable in order get the button 
work properly.")]
        [SerializeField]
        private JCS_Button mJCSButton = null;


        private void Awake()
        {
            // try to get the component.
            if (mJCSButton == null)
                this.mJCSButton = this.GetComponent<JCS_Button>();

            if (mJCSButton != null)
                mJCSButton.SetCallback(PauseGame);
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
