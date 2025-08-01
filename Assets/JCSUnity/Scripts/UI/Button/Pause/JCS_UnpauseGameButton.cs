/**
 * $File: JCS_UnPauseGameButton.cs $
 * $Date: 2017-02-24 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Unpause the game with button.
    /// </summary>
    public class JCS_UnpauseGameButton :
#if JCS_USE_GAMEPAD
        JCS_GamepadButton
#else
        JCS_Button
#endif
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
            JCS_GameManager.FirstInstance().GAME_PAUSE = false;
        }
    }
}
