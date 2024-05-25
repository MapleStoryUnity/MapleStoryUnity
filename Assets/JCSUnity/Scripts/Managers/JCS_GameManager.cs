/**
 * $File: JCS_GameManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Make sure u have this execute first!!!
    /// </summary>
    public class JCS_GameManager : JCS_Manager<JCS_GameManager>
    {
        /* Variables */

        // Callback after the game is done initialize.
        public EmptyFunction afterGameInitializeCallback = null;

        [Separator("Check Variable (JCS_GameManager)")]

        [Tooltip("Is game done initialize?")]
        [SerializeField]
        [ReadOnly]
        private bool mGameDoneInitialize = false;

        private JCS_Player mPlayer = null;

        /* Setter & Getter */

        public bool GAME_DONE_INITIALIZE { get { return this.mGameDoneInitialize; } }
        public bool GAME_PAUSE
        {
            get { return JCS_PauseManager.instance.Paused; }
            set
            {
                var pm = JCS_PauseManager.instance;

                // check if need the game pause the same as the value previously
                // set. In order to save some perforce by enable/disable all the
                // JCS_PauseAction in the game.
                if (pm.Paused != value)
                {
                    if (value)
                        pm.Pause();
                    else
                        pm.Unpause();
                }
            }
        }
        public JCS_Player Player { get { return this.mPlayer; } set { this.mPlayer = value; } }

        /* Functions */

        private void Awake()
        {
            instance = this;

            SetSpecificGameTypeGameManager();
        }


        private void Update()
        {
            SetGameDoneInitializeFlag();
        }

        /// <summary>
        /// Add specific game manager type.
        /// </summary>
        private void SetSpecificGameTypeGameManager()
        {
            var gs = JCS_GameSettings.instance;

            switch (gs.GAME_TYPE)
            {
                case JCS_GameType.GAME_2D:
                    this.gameObject.AddComponent<JCS_2DGameManager>();
                    break;
            }
        }

        /// <summary>
        /// Set the game done initialize flag.
        /// </summary>
        private void SetGameDoneInitializeFlag()
        {
            if (this.mGameDoneInitialize)
                return;

            this.mGameDoneInitialize = true;

            if (afterGameInitializeCallback != null)
                afterGameInitializeCallback.Invoke();
        }
    }
}

