/**
 * $File: JCS_TimePanel.cs $
 * $Date: 2017-03-23 22:42:22 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// In certain time will do certain panel action.
    /// </summary>
    public class JCS_TimePanel : MonoBehaviour
    {
        /* Variables */

        [Separator("Check Variables (JCS_TimePanel)")]

        [Tooltip("Panels going to do the action.")]
        [SerializeField]
        [ReadOnly]
        private JCS_DialogueObject[] mDialogueObjects = null;

        [Separator("Runtime Variables (JCS_TimePanel)")]

        [Tooltip("Active on awake.")]
        [SerializeField]
        private bool mActiveOnAwake = true;

        [Tooltip("Action this panel will do. default : hide")]
        [SerializeField]
        private JCS_PanelActionType mPanelActionType = JCS_PanelActionType.HIDE;

        [Tooltip("How long does the panel do the action.")]
        [SerializeField]
        [Range(0, 600)]
        private float mDoActionTime = 3.0f;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        // timer
        private float mDoActionTimer = 0;

        [Tooltip("Play panel sound?")]
        [SerializeField]
        private bool mPlayPanelSound = false;

        // check if the action active?
        private bool mActionActive = false;

        /* Setter & Getter */

        public bool ActiveOnAwake { get { return this.mActiveOnAwake; } set { this.mActiveOnAwake = value; } }
        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        /* Functions */

        private void Start()
        {
            if (mActiveOnAwake)
                ActiveAction();
        }

        private void Update()
        {
            DoAction();
        }

        /// <summary>
        /// Active this action.
        /// </summary>
        public void ActiveAction()
        {
            // reset timer.
            mDoActionTimer = 0;

            mActionActive = true;

            // do the opposite first
            JCS_PanelActionType oppositeType = FindOppsiteType(mPanelActionType);

            foreach (JCS_DialogueObject panel in mDialogueObjects)
                DoActionForeachPanel(panel, oppositeType);
        }

        /// <summary>
        /// Do action for all the panel
        /// </summary>
        private void DoAction()
        {
            // check if start the action?
            if (!mActionActive)
                return;

            mDoActionTimer += JCS_Time.ItTime(mTimeType);

            // check if time reach.
            if (mDoActionTimer < mDoActionTime)
                return;

            // do action for all the panel in array.
            foreach (JCS_DialogueObject panel in mDialogueObjects)
                DoActionForeachPanel(panel, mPanelActionType);

            // reset timer.
            mDoActionTimer = 0;
        }

        /// <summary>
        /// Do action according to the panel action type.
        /// </summary>
        /// <param name="panel"> panel to set </param>
        /// <param name="type"> action type </param>
        private void DoActionForeachPanel(JCS_DialogueObject panel, JCS_PanelActionType type)
        {
            if (panel == null)
                return;

            switch (type)
            {
                case JCS_PanelActionType.HIDE:
                    {
                        panel.Hide(!mPlayPanelSound);
                    }
                    break;
                case JCS_PanelActionType.SHOW:
                    {
                        panel.Show(!mPlayPanelSound);
                    }
                    break;
            }
        }

        /// <summary>
        /// Find the oppositve panel action type.
        /// </summary>
        /// <param name="type"> type to find opposite </param>
        /// <returns> opposite type </returns>
        private JCS_PanelActionType FindOppsiteType(JCS_PanelActionType type)
        {
            switch (type)
            {
                case JCS_PanelActionType.HIDE:
                    return JCS_PanelActionType.SHOW;
                case JCS_PanelActionType.SHOW:
                    return JCS_PanelActionType.HIDE;
            }

            // default type.
            return JCS_PanelActionType.NONE;
        }
    }
}
