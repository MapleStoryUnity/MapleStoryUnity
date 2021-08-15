/**
 * $File: JCS_GameManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    public delegate void AfterGameInitializeCallback();

    /// <summary>
    /// Make sure u have this execute first!!!
    /// </summary>
    public class JCS_GameManager : JCS_Managers<JCS_GameManager>
    {
        /* Variables */

        // Callback after the game is done initialize.
        public AfterGameInitializeCallback afterGameInitializeCallback = null;

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_GameManager) **")]

        [Tooltip("Test this module?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Key to toggle game pause/unpause.")]
        [SerializeField]
        private KeyCode mToggleGamePause = KeyCode.P;

        // helper tool for level designer to do 
        // some cool effect in the game.
        [Tooltip("Adjustable current time scale")]
        [SerializeField]
        [Range(0, 1)]
        public float TIME_SCALE = 1;
#endif

        [Header("** Check Variable (JCS_GameManager) **")]

        [Tooltip("Is the game pasue?")]
        [SerializeField]
        private bool mGamePause = false;

        [Tooltip("Is game done initialize?")]
        [SerializeField]
        private bool mGameDoneInitialize = false;

        private JCS_Player mJCSPlayer = null;
        private JCS_GameSettings mJCSGameSettings = null;

        /* Setter & Getter */

        public bool GAME_DONE_INITIALIZE { get { return this.mGameDoneInitialize; } }
        public bool GAME_PAUSE
        {
            get { return this.mGamePause; }
            set
            {
                // check if need the game pause the same as the value
                // previously set. In order to save some perforce by 
                // enable/disable all the JCS_PauseAction in the game.
                if (mGamePause != value)
                {
                    JCS_PauseManager.instance.PauseTheWholeGame(value);
                }

                this.mGamePause = value;
            }
        }
        public void SetJCSPlayer(JCS_Player player)
        {
            this.mJCSPlayer = player;
        }
        public JCS_Player GetJCSPlayer() { return this.mJCSPlayer; }
        public void SetJCSGameSettings(JCS_GameSettings gs) { this.mJCSGameSettings = gs; }
        public JCS_GameSettings GetJCSGameSettings() { return this.mJCSGameSettings; }

        /* Functions */

        private void Awake()
        {
            instance = this;
            
            SetSpecificGameTypeGameManager();

#if (UNITY_EDITOR)
            // Check time scale
            if (TIME_SCALE != 1)
            {
                JCS_Debug.LogReminder("Current time scale [" + TIME_SCALE + "] isn't one.");
            }
#endif
        }


        private void Update()
        {
#if (UNITY_EDITOR)
            SetTimeScale();

            TestPauseGame();
#endif

            SetGameDoneInitializeFlag();
        }

#if (UNITY_EDITOR)
        /// <summary>
        /// Keep set the time to the time scale, 
        /// so make it make the inspector slide bar can 
        /// control the time scale of the actual time scale
        /// from UnityEngine's API "Time class".
        /// </summary>
        private void SetTimeScale()
        {
            Time.timeScale = TIME_SCALE;
        }

        private void TestPauseGame()
        {
            if (!mTestWithKey)
                return;

            if (Input.GetKeyDown(mToggleGamePause))
            {
                GAME_PAUSE = !GAME_PAUSE;
            }
        }
#endif

        /// <summary>
        /// Add specific game manager type.
        /// </summary>
        private void SetSpecificGameTypeGameManager()
        {
            JCS_GameSettings gs = JCS_GameSettings.instance;

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

