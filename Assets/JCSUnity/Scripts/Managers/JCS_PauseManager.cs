/**
 * $File: JCS_PauseManager.cs $
 * $Date: 2017-02-24 06:10:05 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// If you are working some kind of game that need the pause 
    /// screen, you definitly need the pause manager to add it onto 
    /// "JCS_Manager" transform in the Hierarchy.
    /// </summary>
    public class JCS_PauseManager : JCS_Manager<JCS_PauseManager>
    {
        /* Variables */

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_PauseManager)")]

        [Tooltip("See curren time scale.")]
        [SerializeField]
        [ReadOnly]
        private float mTimeScale = 1.0f;

        [Tooltip("Turn on to test this behaviour.")]
        [SerializeField]
        private bool mTest = false;

        [Tooltip("Key to toggle game pause/unpause.")]
        [SerializeField]
        private KeyCode mToggleGamePause = KeyCode.P;

        [Header("Increment/Decrement Time")]

        [Tooltip("Key that increment the time.")]
        [SerializeField]
        private KeyCode mIncTime = KeyCode.N;

        [Tooltip("Key that decrement the time.")]
        [SerializeField]
        private KeyCode mDecTime = KeyCode.M;

        [Tooltip("Delta value to add to the time.")]
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float mTimeDelta = 5.0f;
#endif

        [Separator("Check Variables (JCS_PauseManager)")]

        [Tooltip("Set to true if game is paused.")]
        [SerializeField]
        private bool mPaused = false;

        [Tooltip("Target time scale.")]
        [SerializeField]
        [ReadOnly]
        private float mTargetTimeScale = 1.0f;

        [Tooltip("List of pause action in the scene.")]
        [SerializeField]
        [ReadOnly]
        private List<JCS_PauseAction> mPauseActions = null;

        [Separator("Runtime Variables (JCS_PauseManager)")]

        [Tooltip("The default time scale.")]
        [SerializeField]
        private float mDefaultTimeScale = 1.0f;

        [Tooltip(@"Time to resize the pause action list, in seconds.
ATTENTION: this will take certain of performance depends on the pause 
object you have in the list.")]
        [SerializeField]
        [Range(1, 60)]
        private float mResizePauseActionListTime = 20;

        // resize timer.
        private float mResizePauseActionListTimer = 0;

        [Header("Asymptotic")]

        [Tooltip("Do this scene using the specific setting.")]
        [SerializeField]
        private bool mOverrideSetting = false;

        [Tooltip("Pause and unpause with asymptotic transition.")]
        [SerializeField]
        private bool mAsymptotic = false;

        [Tooltip("How fast the asymptotic transition?")]
        [SerializeField]
        [Range(JCS_Constants.FRICTION_MIN, 30.0f)]
        private float mFriction = 0.2f;

        /* Setter & Getter */

        public bool paused { get { return mPaused; } }
        public List<JCS_PauseAction> pausesActions { get { return mPauseActions; } }
        public float resizePauseActionListTime { get { return mResizePauseActionListTime; } set { mResizePauseActionListTime = value; } }
        public float defaultTimeScale { get { return mDefaultTimeScale; } set { mDefaultTimeScale = value; } }

        public bool overrideSetting { get { return mOverrideSetting; } }
        public bool asymptotic { get { return mAsymptotic; } set { mAsymptotic = value; } }
        public float friction { get { return mFriction; } set { mFriction = value; } }

        /* Functions */

        private void Awake()
        {
            RegisterInstance(this);
        }

        private void Start()
        {
            // Make sure the time scale is reset every new scene.
            ResetState();
        }

        private void Update()
        {
#if UNITY_EDITOR
            mTimeScale = Time.timeScale;

            TestPauseGame();
#endif

            ResizePauseActionListPeriodically();
            DoAsymp();
        }

#if UNITY_EDITOR
        private void TestPauseGame()
        {
            if (!mTest)
                return;

            if (Input.GetKeyDown(mToggleGamePause))
            {
                var gm = JCS_GameManager.FirstInstance();

                gm.gamePaused = !gm.gamePaused;
            }

            if (Input.GetKeyDown(mIncTime))
            {
                Time.timeScale += mTimeDelta;

                mTargetTimeScale = Time.timeScale;
            }
            else if (Input.GetKeyDown(mDecTime))
            {
                Time.timeScale -= mTimeDelta;

                mTargetTimeScale = Time.timeScale;
            }
        }
#endif

        /// <summary>
        /// Add the pause action to the list of pause action list, 
        /// in order to get manage by this "pause manager".
        /// </summary>
        public void AddActionToList(JCS_PauseAction pa)
        {
            mPauseActions.Add(pa);
        }

        /// <summary>
        /// Reset pause state.
        /// </summary>
        public void ResetState()
        {
            mTargetTimeScale = mDefaultTimeScale;
            Time.timeScale = mDefaultTimeScale;

            SetPause(false);
        }

        /// <summary>
        /// Pause the game.
        /// </summary>
        public void Pause()
        {
            bool asymp = JCS_PauseSettings.FirstInstance().AsymptoticBaseOnSetting();

            if (asymp)
                mTargetTimeScale = 0.0f;
            else
                Time.timeScale = 0.0f;

            SetPause(true);
        }

        /// <summary>
        /// Unpause the game.
        /// </summary>
        public void Unpause()
        {
            bool asymp = JCS_PauseSettings.FirstInstance().AsymptoticBaseOnSetting();

            if (asymp)
                mTargetTimeScale = mDefaultTimeScale;
            else
                Time.timeScale = mDefaultTimeScale;

            SetPause(false);
        }

        /// <summary>
        /// Toggle between pause/unpause the game.
        /// </summary>
        public void TogglePause()
        {
            if (mPaused)
                Unpause();
            else
                Pause();
        }

        /// <summary>
        /// Set pause/unpause the game.
        /// </summary>
        public void SetPause(bool act = true)
        {
            foreach (JCS_PauseAction pauseAction in mPauseActions)
            {
                if (pauseAction == null)
                    continue;

                // NOTE(jenchieh): the act should be opposite with the 
                // enable /disable.

                // e.g. 
                // 1) Pause the game = disable all behaviour
                pauseAction.EnableBehaviourInTheList(!act);
            }

            // resize the list once.
            RemoveNullRefInPauseActionList();

            mPaused = act;
        }

        /// <summary>
        /// Remove all the null reference object from the list.
        /// </summary>
        private void RemoveNullRefInPauseActionList()
        {
            mPauseActions = JCS_Array.RemoveEmptyMissing(mPauseActions);
        }

        /// <summary>
        /// Resize pause action list in certain time.
        /// </summary>
        private void ResizePauseActionListPeriodically()
        {
            mResizePauseActionListTimer += Time.deltaTime;

            if (mResizePauseActionListTimer < mResizePauseActionListTime)
                return;

            // resize the list
            RemoveNullRefInPauseActionList();

            // reset timer.
            mResizePauseActionListTimer = 0;
        }

        /// <summary>
        /// Do asymptotic pause/unpase transition.
        /// </summary>
        private void DoAsymp()
        {
            bool asymp = JCS_PauseSettings.FirstInstance().AsymptoticBaseOnSetting();

            if (!asymp)
                return;

            float newTS = Time.timeScale;

            newTS += (mTargetTimeScale - Time.timeScale) / mFriction * Time.unscaledDeltaTime;

            // Prevent lower than 0!
            Time.timeScale = Mathf.Max(0.0f, newTS);
        }
    }
}
