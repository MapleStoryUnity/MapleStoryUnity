/**
 * $File: JCS_IGLogSystem.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// In Game Log System (IGL).
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(JCS_GUITextPool))]
    public class JCS_IGLogSystem : JCS_UnityObject
    {
        /* Variables */

        public static JCS_IGLogSystem instance = null;

        private JCS_GUITextPool mLogTextPool = null;

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_IGLogSystem)")]

        [Tooltip("Test this component with keys.")]
        public bool testWithKey = false;

        public KeyCode sendTextAKey = KeyCode.Q;

        public KeyCode sendTextBKey = KeyCode.W;

        public KeyCode sendRandomTextArrayKey = KeyCode.E;

        public string textA = "Hello World!";

        public string textB = "EXP: 10";

        public string[] textArray = new string[] { "Ok", "Cool?", "Awesome!" };
#endif

        [Separator("Initialize Variables (JCS_IGLogSystem)")]

        [Tooltip("Space between each log message.")]
        [SerializeField]
        [Range(-300.0f, 300.0f)]
        private float mLogSpacing = 1;

        // vector of log text rendering on the screen.
        private JCS_Vec<JCS_LogText> mRenderLogText = null;

        /* Setter & Getter */

        public float LogSpacing { get { return mLogSpacing; } set { mLogSpacing = value; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            instance = this;

            mLogTextPool = GetComponent<JCS_GUITextPool>();

            mRenderLogText = new JCS_Vec<JCS_LogText>();
        }

#if UNITY_EDITOR
        private void Update()
        {
            Test();
        }

        private void Test()
        {
            if (!testWithKey)
                return;

            if (JCS_Input.GetKeyDown(sendTextAKey))
                SendLogMessage(textA);
            if (JCS_Input.GetKeyDown(sendTextBKey))
                SendLogMessage(textB);
            if (JCS_Input.GetKeyDown(sendRandomTextArrayKey))
            {
                SendLogMessages(textArray);
            }
        }
#endif

        /// <summary>
        /// Make multiple log messages on the screen.
        /// </summary>
        /// <param name="messages"></param>
        public void SendLogMessages(string[] messages)
        {
            foreach (string msg in messages)
            {
                SendLogMessage(msg);
            }
        }

        /// <summary>
        /// Make single log message on the screen.
        /// </summary>
        /// <param name="message"></param>
        public void SendLogMessage(string message)
        {
            // get one log text from the pool
            JCS_LogText logText = mLogTextPool.ExecuteOneFromPool();

            // all pool active?
            if (logText == null)
                return;

            UpdateSpace();

            mRenderLogText.push(logText);

            Vector3 newPos = logText.simpleTrackAction.targetPosition;

            // set back to position.
            // NOTE(jenchieh): 不太懂這邊的原理...
            newPos.y = 0.0f;

            logText.simpleTrackAction.targetPosition = newPos;
            logText.simpleTrackAction.localPosition = newPos;

            // this will set the log text active, 
            // so it wont be reuse until is fade out.
            logText.Execute(message);
        }

        /// <summary>
        /// Remove the log message that are outdated.
        /// </summary>
        /// <param name="txt"></param>
        public void RemoveFromRenderVec(JCS_LogText txt)
        {
            mRenderLogText.slice(txt);
        }

        /// <summary>
        /// Update all current active log messages' spacing.
        /// </summary>
        /// <param name="spaces"></param>
        public void UpdateSpace(int spaces = 1)
        {
            for (int index = 0; index < mRenderLogText.length; ++index)
            {
                JCS_LogText logText = mRenderLogText.at(index);

                logText.simpleTrackAction.DeltaTargetPosY(mLogSpacing * spaces);
            }
        }
    }
}
