/**
 * $File: JCS_UnPauseGameGamePadButton.cs $
 * $Date: 2017-10-27 12:07:48 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// UnPause the game with button. (Game Pad)
    /// 
    /// ATTENTION: this should have at least one "JCS_GamePadButton" with this.
    /// </summary>
    public class JCS_UnPauseGameGamePadButton : MonoBehaviour
    {
        [Header("** Require Variables (JCS_UnPauseGameGamePadButton) **")]

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
            {
                mJCSGamePadButton.SetCallback(UnPauseGame);
            }
        }

        /// <summary>
        /// UnPause the game.
        /// </summary>
        public void UnPauseGame()
        {
            // turn on the game pause button.
            JCS_GameManager.instance.GAME_PAUSE = false;
        }
    }
}
