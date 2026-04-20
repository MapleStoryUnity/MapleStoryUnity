/**
 * $File: JCS_GameManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// The overall game manager.
    /// </summary>
    public class JCS_GameManager : JCS_Manager<JCS_GameManager>
    {
        /* Variables */

        // Callback after the game is done initialize. (system used)
        private Action mOnSystemAfterInitialize = null;

        // Callback after the game is done initialize.
        private Action mOnAfterInitialize = null;

        [Separator("📋 Check Variables (JCS_GameManager)")]

        [Tooltip("Is game done initialize?")]
        [SerializeField]
        [ReadOnly]
        private bool mDoneInitialize = false;

        private JCS_Player mPlayer = null;

        /* Setter & Getter */

        public bool doneInitialized { get { return mDoneInitialize; } }
        public bool gamePaused
        {
            get { return JCS_Glob.pausem.paused; }
            set
            {
                // check if need the game pause the same as the value previously
                // set. In order to save some perforce by enable/disable all the
                // JCS_PauseAction in the game.
                if (JCS_Glob.pausem.paused != value)
                {
                    if (value)
                        JCS_Glob.pausem.Pause();
                    else
                        JCS_Glob.pausem.Unpause();
                }
            }
        }
        public JCS_Player player { get { return mPlayer; } set { mPlayer = value; } }

        /* Functions */

        private void Awake()
        {
            RegisterInstance(this);

            Invoke(nameof(OnFirstFrame), JCS_Consts.FIRST_FRAME_INVOKE_TIME);
        }

        /// <summary>
        /// Register event run on the first frame of the game. (system used)
        /// </summary>
        public void RegisterOnSystemAfterInit(Action action)
        {
            // Already initialize, just execute it.
            if (mDoneInitialize)
            {
                action?.Invoke();
                return;
            }

            mOnSystemAfterInitialize += action;
        }

        /// <summary>
        /// Register event run on the first frame of the game.
        /// </summary>
        public void RegisterOnAfterInit(Action action)
        {
            // Already initialize, just execute it.
            if (mDoneInitialize)
            {
                action?.Invoke();
                return;
            }

            mOnAfterInitialize += action;
        }

        /// <summary>
        /// Run only once on the first frame.
        /// </summary>
        private void OnFirstFrame()
        {
            ExecDoneInitialize();
        }

        /// <summary>
        /// Set the game done initialize flag.
        /// </summary>
        private void ExecDoneInitialize()
        {
            if (mDoneInitialize)
                return;

            mDoneInitialize = true;

            mOnSystemAfterInitialize?.Invoke();

            mOnAfterInitialize?.Invoke();
        }
    }
}
