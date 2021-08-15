/**
 * $File: JCS_ExitAppGamePadButton.cs $
 * $Date: 2017-10-27 11:56:25 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Button exit the application. (Gamepad)
    /// </summary>
    public class JCS_ExitAppGamePadButton : JCS_GamePadButton
    {
        public override void JCS_OnClickCallback()
        {
            // load exit button scene.
            JCS_SceneManager.instance.LoadScene("JCS_ApplicationCloseSimulateSceneOnline");
        }
    }
}
