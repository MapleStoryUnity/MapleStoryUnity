/**
 * $File: JCS_UIManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Manage all the dialogue in the scene.
    /// </summary>
    public class JCS_UIManager : JCS_Manager<JCS_UIManager>
    {
        /* Variables */

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_UIManager)")]

        [Tooltip("Test this component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;
#endif

        [Separator("Check Variables (JCS_UIManager)")]

        [Tooltip("List of canvas.")]
        [SerializeField]
        [ReadOnly]
        private List<JCS_Canvas> mCanvases = null;

        [Tooltip("Global undo redo system.")]
        [SerializeField]
        [ReadOnly]
        private JCS_UndoRedoSystem mGlobalUndoRedoSystem = null;

        [Separator("Initialize Variables (JCS_UIManager)")]

        [Tooltip("Game Play UI (Game Layer - Only One)")]
        [SerializeField]
        private JCS_DialogueObject mGameUI = null;          // the most common one!

        [Tooltip("System dialogue (Application Layer - Only One)")]
        [SerializeField]
        private JCS_DialogueObject mForceDialogue = null;

        // Game Dialogue (Game Layer - could have multiple one)
        [Tooltip("Dialogue player are focusing")]
        [SerializeField]
        private JCS_DialogueObject mFocusGameDialogue = null;

        // List of all the window that are opened!
        private LinkedList<JCS_DialogueObject> mOpenWindow = null;

        [Header("Screen")]

        [Tooltip("Panel that could do the fade loose focus effect.")]
        [SerializeField]
        private JCS_FadeScreen mFadeScreen = null;

        /* Setter & Getter */

        public List<JCS_Canvas> canvases { get { return mCanvases; } }
        public void SetDialogue(JCS_DialogueType type, JCS_DialogueObject jdo)
        {
            switch (type)
            {
                case JCS_DialogueType.GAME_UI:
                    {
                        if (mGameUI != null)
                        {
                            //Debug.LogError("Failed to set \"In Game Dialogue\"...");
                            return;
                        }

                        mGameUI = jdo;
                    }
                    break;
                case JCS_DialogueType.PLAYER_DIALOGUE:
                    {
                        //if (mFocusGameDialogue != null)
                        //{
                        //    Debug.LogError("Failed to set \"In Game Dialogue\"...");
                        //    return;
                        //}

                        mFocusGameDialogue = jdo;
                    }
                    break;
                case JCS_DialogueType.SYSTEM_DIALOGUE:
                    {
                        if (mForceDialogue != null)
                        {
                            //Debug.LogError("Failed to set \"Force Dialogue\"...");
                            return;
                        }

                        mForceDialogue = jdo;
                    }
                    break;
            }
        }
        public JCS_DialogueObject GetDialogue(JCS_DialogueType type)
        {
            switch (type)
            {
                // Game UI
                case JCS_DialogueType.GAME_UI:
                    return mGameUI;

                // Dialogue Box
                case JCS_DialogueType.PLAYER_DIALOGUE:
                    return mFocusGameDialogue;
                case JCS_DialogueType.SYSTEM_DIALOGUE:
                    return mForceDialogue;
            }

            Debug.LogError("Failed to get Dialogue -> " + type);
            return null;
        }
        public LinkedList<JCS_DialogueObject> GetOpenWindow() { return mOpenWindow; }
        public JCS_FadeScreen fadeScreen { get { return mFadeScreen; } set { mFadeScreen = value; } }
        public JCS_UndoRedoSystem GetGlobalUndoRedoSystem() { return mGlobalUndoRedoSystem; }

        /* Functions */

        private void Awake()
        {
            RegisterInstance(this);

            mOpenWindow = new LinkedList<JCS_DialogueObject>();

            mGlobalUndoRedoSystem = gameObject.AddComponent<JCS_UndoRedoSystem>();
        }
        private void Start()
        {
            // pop the fade screen.
            string path = JCS_UISettings.FADE_SCREEN_PATH;
            mFadeScreen = JCS_Util.Instantiate(path).GetComponent<JCS_FadeScreen>();
        }

#if UNITY_EDITOR
        private void Update()
        {
            Test();
        }

        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDown(KeyCode.A))
            {
                Color col = Color.red;
                col.a = 0.6f;

                Focus(col);
            }

            if (JCS_Input.GetKeyDown(KeyCode.S))
            {
                Unfocus();
            }
        }
#endif

        /// <summary>
        /// Add a canvas to the group.
        /// </summary>
        public void AddCanvas(JCS_Canvas canvas)
        {
            mCanvases.Add(canvas);
            mCanvases = JCS_Array.RemoveEmptyMissing(mCanvases);
            mCanvases = mCanvases.OrderBy(x => x.canvas.sortingOrder).ToList();
        }

        /// <summary>
        /// Return the canvas by name.
        /// </summary>
        public JCS_Canvas CanvasByName(string name)
        {
            mCanvases = JCS_Array.RemoveEmptyMissing(mCanvases);

            foreach (JCS_Canvas canvas in mCanvases)
            {
                if (canvas.name == name)
                    return canvas;
            }

            return null;
        }

        /// <summary>
        /// Hide the last open dialogue in the current scene.
        /// </summary>
        public void HideTheLastOpenDialogue()
        {
            // return if nothing in the list
            if (GetOpenWindow().Count == 0)
                return;

            JCS_DialogueObject dialogue = GetOpenWindow().Last.Value;

            // once it hide it will remove from the list it self!
            dialogue.Hide();
        }

        /// <summary>
        /// Hide all the open dialgoue in the current scene.
        /// </summary>
        public void HideAllOpenDialogue()
        {
            // return if nothing in the list
            if (GetOpenWindow().Count == 0)
                return;

            while (GetOpenWindow().Count != 0)
            {
                GetOpenWindow().First.Value.Hide(true);
            }
        }

        /// <summary>
        /// This will run the default value by the inspector's setting.
        /// </summary>
        /// <param name="time"> time to fade. </param>
        public void Focus()
        {
            JCS_FadeObject fadeObj = mFadeScreen.fadeObject;
            fadeObj.FadeIn();
        }

        /// <summary>
        /// This will run the default value by the inspector's setting.
        /// </summary>
        /// <param name="time"> time to fade. </param>
        public void Focus(float time)
        {
            JCS_FadeObject fadeObj = mFadeScreen.fadeObject;
            fadeObj.FadeIn(time);
        }

        /// <summary>
        /// This will run the default value by the inspector's setting.
        /// </summary>
        /// <param name="time"> time to fade. </param>
        public void Focus(Color color)
        {
            float alpha = color.a;

            JCS_FadeObject fadeObj = mFadeScreen.fadeObject;

            Color fakeColor = color;
            fakeColor.a = 0;

            fadeObj.localColor = fakeColor;
            fadeObj.fadeInAmount = alpha;
            fadeObj.FadeIn();
        }

        /// <summary>
        /// Fade a screen to cetain amount of value.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="color"></param>
        public void Focus(float time, Color color)
        {
            float alpha = color.a;

            JCS_FadeObject fadeObj = mFadeScreen.fadeObject;

            Color fakeColor = color;
            fakeColor.a = 0;

            fadeObj.localColor = fakeColor;
            fadeObj.fadeInAmount = alpha;
            fadeObj.FadeIn(time);
        }

        /// <summary>
        /// Fade out the screen, back to original amount of value.
        /// </summary>
        public void Unfocus()
        {
            JCS_FadeObject fadeObj = mFadeScreen.fadeObject;
            fadeObj.FadeOut();
        }

        /// <summary>
        /// Fade out the screen, back to original amount of value.
        /// </summary>
        public void Unfocus(float time)
        {
            JCS_FadeObject fadeObj = mFadeScreen.fadeObject;
            fadeObj.FadeOut(time);
        }
    }
}
