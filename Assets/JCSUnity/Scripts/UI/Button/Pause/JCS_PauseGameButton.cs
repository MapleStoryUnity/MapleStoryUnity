/**
 * $File: JCS_PauseGameButton.cs $
 * $Date: 2017-02-24 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Pause the game with button.
    /// </summary>
    public class JCS_PauseGameButton :
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
