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

namespace JCSUnity
{
    /// <summary>
    /// If you are working some kind of game that need the pause 
    /// screen, you definitly need the pause manager to add it onto 
    /// "JCS_Managers" transform in the Hierarchy.
    /// </summary>
    public class JCS_PauseManager : JCS_Managers<JCS_PauseManager>
    {
        /* Variables */

        [Header("** Check Variables (JCS_PauseManager) **")]

        [Tooltip("List of pause action in the scene.")]
        [SerializeField]
        private List<JCS_PauseAction> mPauseActions = null;

        [Header("** Runtime Variables (JCS_PauseManager) **")]

        [Tooltip(@"Time to resize the pause action list, in seconds.
ATTENTION: this will take certain of performance depends on the pause 
object you have in the list.")]
        [SerializeField]
        [Range(1, 60)]
        private float mResizePauseActionListTime = 20;

        // resize timer.
        private float mResizePauseActionListTimer = 0;

        /* Setter & Getter */

        public List<JCS_PauseAction> PausesActions { get { return this.mPauseActions; } }
        public float ResizePauseActionListTime { get { return this.mResizePauseActionListTime; } set { this.mResizePauseActionListTime = value; } }

        /* Functions */

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            ResizePauseActionListPeriodically();
        }

        /// <summary>
        /// Add the pause action to the list of pause action list, 
        /// in order to get manage by this "pause manager".
        /// </summary>
        public void AddActionToList(JCS_PauseAction pa)
        {
            mPauseActions.Add(pa);
        }

        /// <summary>
        /// Pause/Unpause the whole game.
        /// </summary>
        public void PauseTheWholeGame(bool act = true)
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
        }

        /// <summary>
        /// Remove all the null reference object from the list.
        /// </summary>
        private void RemoveNullRefInPauseActionList()
        {
            mPauseActions = JCS_Utility.RemoveEmptySlotIncludeMissing(mPauseActions);
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
    }
}
