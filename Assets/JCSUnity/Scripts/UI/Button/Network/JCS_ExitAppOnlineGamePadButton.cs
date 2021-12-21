/**
 * $File: JCS_ExitAppOnlineGamePadButton.cs $
 * $Date: 2017-10-27 12:17:14 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Exit button with change scene effect. (Game Pad)
    /// Multiplayer Version. 
    /// </summary>
    public class JCS_ExitAppOnlineGamePadButton : JCS_GamePadButton
    {
        public override void OnClick()
        {
            // load exit button scene.
            JCS_SceneManager.instance.LoadScene("JCS_ApplicationCloseSimulateSceneOnline");
        }
    }
}
