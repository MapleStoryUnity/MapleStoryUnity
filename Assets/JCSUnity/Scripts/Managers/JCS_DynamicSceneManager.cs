/**
 * $File: JCS_DynamicSceneManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Take care of all the scene layout.
    /// </summary>
    public class JCS_DynamicSceneManager : JCS_Managers<JCS_DynamicSceneManager>
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            instance = this;

            SetSpecificGameTypeDynamicSceneManager();
        }

        /// <summary>
        /// Base on different type of game use different 
        /// type of manager.
        /// </summary>
        private void SetSpecificGameTypeDynamicSceneManager()
        {
            JCS_GameSettings gs = JCS_GameSettings.instance;

            switch (gs.GAME_TYPE)
            {
                case JCS_GameType.GAME_2D:
                    this.gameObject.AddComponent<JCS_2DDynamicSceneManager>();
                    break;
            }
        }
    }
}
