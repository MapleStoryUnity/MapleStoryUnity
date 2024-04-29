/**
 * $File: JCS_UIManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
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

        [Header("- UI Screen")]

        [Tooltip("Panel that could do the fade loose focus effect.")]
        [SerializeField]
        private JCS_FadeScreen mFadeScreen = null;

        /* Setter & Getter */

        public List<JCS_Canvas> Canvases { get { return this.mCanvases; } }
        public void SetDialogue(JCS_DialogueType type, JCS_DialogueObject jdo)
        {
            switch (type)
            {
                case JCS_DialogueType.GAME_UI:
                    {
                        if (mGameUI != null)
                        {
                            //JCS_Debug.LogError("Failed to set \"In Game Dialogue\"...");
                            return;
                        }

                        this.mGameUI = jdo;
                    }
                    break;
                case JCS_DialogueType.PLAYER_DIALOGUE:
                    {
                        //if (mFocusGameDialogue != null)
                        //{
                        //    JCS_Debug.LogError("Failed to set \"In Game Dialogue\"...");
                        //    return;
                        //}

                        this.mFocusGameDialogue = jdo;
                    }
                    break;
                case JCS_DialogueType.SYSTEM_DIALOGUE:
                    {
                        if (mForceDialogue != null)
                        {
                            //JCS_Debug.LogError("Failed to set \"Force Dialogue\"...");
                            return;
                        }

                        mForceDialogue = jdo;
                    }
                    break;
            }
        }
        public JCS_DialogueObject GetJCSDialogue(JCS_DialogueType type)
        {
            switch (type)
            {
                // Game UI
                case JCS_DialogueType.GAME_UI:
                    return this.mGameUI;

                // Dialogue Box
                case JCS_DialogueType.PLAYER_DIALOGUE:
                    return this.mFocusGameDialogue;
                case JCS_DialogueType.SYSTEM_DIALOGUE:
                    return this.mForceDialogue;
            }

            JCS_Debug.LogError("Failed to get Dialogue -> " + type);
            return null;
        }
        public LinkedList<JCS_DialogueObject> GetOpenWindow() { return this.mOpenWindow; }
        public JCS_FadeScreen FadeScreen { get { return this.mFadeScreen; } set { this.mFadeScreen = value; } }
        public JCS_UndoRedoSystem GetGlobalUndoRedoSystem() { return this.mGlobalUndoRedoSystem; }

        /* Functions */

        private void Awake()
        {
            instance = this;

            this.mOpenWindow = new LinkedList<JCS_DialogueObject>();

            this.mGlobalUndoRedoSystem = this.gameObject.AddComponent<JCS_UndoRedoSystem>();
        }
        private void Start()
        {
            // pop the fade screen.
            string path = JCS_UISettings.FADE_SCREEN_PATH;
            this.mFadeScreen = JCS_Util.Instantiate(path).GetComponent<JCS_FadeScreen>();
        }

        private void Update()
        {
#if UNITY_EDITOR
            Test();
#endif

#if (UNITY_EDITOR || UNITY_STANDALONE)

            // Exit in game diagloue not in game UI!!
            // (Admin input)
            if (Input.GetKeyDown(KeyCode.Escape))
                JCS_UtilityFunctions.DestoryCurrentDialogue(JCS_DialogueType.PLAYER_DIALOGUE);
#endif
        }

#if UNITY_EDITOR
        private void Test()
        {
            if (!mTestWithKey)
                return;

            //if (JCS_Input.GetKeyDown(KeyCode.A))
            //    JCS_UtilityFunctions.PopIsConnectDialogue();
            //if (JCS_Input.GetKeyDown(KeyCode.S))
            //    JCS_UtilityFunctions.PopSettingDialogue();
            //if (JCS_Input.GetKeyDown(KeyCode.D))
            //    JCS_UtilityFunctions.PopInGameUI();
            //if (JCS_Input.GetKeyDown(KeyCode.F))
            //    JCS_UtilityFunctions.PopTalkDialogue();

            if (JCS_Input.GetKeyDown(KeyCode.A))
            {
                Color col = Color.red;
                col.a = 0.6f;

                Focus(col);
            }

            if (JCS_Input.GetKeyDown(KeyCode.S))
            {
                UnFocus();
            }
        }
#endif

        /// <summary>
        /// Add a canvas to the group.
        /// </summary>
        public void AddCanvas(JCS_Canvas canvas)
        {
            this.mCanvases.Add(canvas);
            mCanvases = JCS_Util.RemoveEmptySlotIncludeMissing(mCanvases);
            mCanvases = SortCanvases_Insertion();
        }
        private List<JCS_Canvas> SortCanvases_Insertion()
        {
            for (int i = 0; i < mCanvases.Count; ++i)
            {
                for (int j = i; j > 0; --j)
                {
                    if (mCanvases[j].canvas.sortingOrder < mCanvases[j - 1].canvas.sortingOrder)
                    {
                        JCS_Canvas temp = mCanvases[j];
                        mCanvases[j] = mCanvases[j - 1];
                        mCanvases[j - 1] = temp;
                    }
                }
            }
            return mCanvases;
        }

        /// <summary>
        /// Hide the last open dialogue in the current scene.
        /// </summary>
        public void HideTheLastOpenDialogue()
        {
            // return if nothing in the list
            if (GetOpenWindow().Count == 0)
                return;

            JCS_DialogueObject jdo = GetOpenWindow().Last.Value;

            // once it hide it will remove from the list it self!
            jdo.Hide();
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
            JCS_FadeObject fadeObj = this.mFadeScreen.FadeObject;
            fadeObj.FadeIn();
        }

        /// <summary>
        /// This will run the default value by the inspector's setting.
        /// </summary>
        /// <param name="time"> time to fade. </param>
        public void Focus(float time)
        {
            JCS_FadeObject fadeObj = this.mFadeScreen.FadeObject;
            fadeObj.FadeIn(time);
        }

        /// <summary>
        /// This will run the default value by the inspector's setting.
        /// </summary>
        /// <param name="time"> time to fade. </param>
        public void Focus(Color color)
        {
            float alpha = color.a;

            JCS_FadeObject fadeObj = this.mFadeScreen.FadeObject;

            Color fakeColor = color;
            fakeColor.a = 0;

            fadeObj.LocalColor = fakeColor;
            fadeObj.FadeInAmount = alpha;
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

            JCS_FadeObject fadeObj = this.mFadeScreen.FadeObject;

            Color fakeColor = color;
            fakeColor.a = 0;

            fadeObj.LocalColor = fakeColor;
            fadeObj.FadeInAmount = alpha;
            fadeObj.FadeIn(time);
        }

        /// <summary>
        /// Fade out the screen, back to original amount of value.
        /// </summary>
        public void UnFocus()
        {
            JCS_FadeObject fadeObj = this.mFadeScreen.FadeObject;
            fadeObj.FadeOut();
        }

        /// <summary>
        /// Fade out the screen, back to original amount of value.
        /// </summary>
        public void UnFocus(float time)
        {
            JCS_FadeObject fadeObj = this.mFadeScreen.FadeObject;
            fadeObj.FadeOut(time);
        }
    }
}
