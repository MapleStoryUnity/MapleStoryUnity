/**
 * $File: JCS_PauseGameGamepadButton.cs $
 * $Date: 2017-10-27 11:58:09 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Pause the game with button. (Gamepad)
    /// </summary>
    public class JCS_PauseGameGamepadButton : JCS_GamepadButton
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        public override void OnClick()
        {
            PauseGame();
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
