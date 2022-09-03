/**
 * $File: JCS_UnpauseGameGamepadButton.cs $
 * $Date: 2017-10-27 12:07:48 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Unpause the game with button. (Gamepad)
    /// </summary>
    public class JCS_UnpauseGameGamepadButton : JCS_GamepadButton
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        public override void OnClick()
        {
            UnpauseGame();
        }

        /// <summary>
        /// Unpause the game.
        /// </summary>
        public void UnpauseGame()
        {
            // turn on the game pause button.
            JCS_GameManager.instance.GAME_PAUSE = false;
        }
    }
}
