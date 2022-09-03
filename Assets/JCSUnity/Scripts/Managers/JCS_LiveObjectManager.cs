/**
 * $File: JCS_LiveObjectManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */

namespace JCSUnity
{
    /// <summary>
    /// Handle of all the AI in the scene.
    /// </summary>
    public class JCS_LiveObjectManager : JCS_Manager<JCS_LiveObjectManager>
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            SetSpecificGameTypeGameManager();
        }

        /// <summary>
        /// Base on different type of game add different type of manager.
        /// </summary>
        private void SetSpecificGameTypeGameManager()
        {
            var gs = JCS_GameSettings.instance;

            switch (gs.GAME_TYPE)
            {
                case JCS_GameType.GAME_2D:
                    this.gameObject.AddComponent<JCS_2DLiveObjectManager>();
                    break;
            }
        }
    }
}
