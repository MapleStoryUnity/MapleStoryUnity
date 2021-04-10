/**
 * $File: JCS_DialogueObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// Base class of Game Window.
    /// </summary>
    [RequireComponent(typeof(JCS_GameWindow))]
    public class JCS_DialogueObject
        : JCS_PanelRoot
    {
        /* Variables */

        [Header("** Runtime Variables (JCS_DialogueObject) **")]

        [Tooltip("Unique index to open the dialogue.")]
        [SerializeField]
        private int mDialogueIndex = -1;

        [Tooltip("Key to open this dialogue.")]
        [SerializeField]
        private KeyCode mOpenKey = KeyCode.None;

        [Tooltip("Dialogue type for priority.")]
        [SerializeField]
        private JCS_DialogueType mDialogueType = JCS_DialogueType.PLAYER_DIALOGUE;

        [Tooltip("Type for positioning the panel every time it opens.")]
        [SerializeField]
        private JCS_PanelType mPanelType = JCS_PanelType.RESET_PANEL;

        [Header("- Sound")]

        [Tooltip("Sound player for 3D sounds calculation.")]
        [SerializeField]
        private JCS_SoundPlayer mSoundPlayer = null;

        [Tooltip("Sound when open this dialouge window.")]
        [SerializeField]
        private AudioClip mOpenWindowClip = null;

        [Tooltip("Sound when close this dialouge window.")]
        [SerializeField]
        private AudioClip mCloseWindowClip = null;

        /* Setter & Getter */

        public int DialogueIndex { get { return this.mDialogueIndex; } }
        public KeyCode OpenKey { get { return this.mOpenKey; } set { this.mOpenKey = value; } }
        public JCS_DialogueType DialogueType { get { return this.mDialogueType; } }
        public JCS_PanelType PanelType { get { return this.mPanelType; } }
        public bool IsWindowOpened() { return this.mIsVisible; }

        public JCS_SoundPlayer SoundPlayer { get { return this.mSoundPlayer; } set { this.mSoundPlayer = value; } }
        public AudioClip OpenWindowClip { get { return this.mOpenWindowClip; } set { this.mOpenWindowClip = value; } }
        public AudioClip CloseWindowClip { get { return this.mCloseWindowClip; } set { this.mCloseWindowClip = value; } }

        /* Functions */

#if !(UNITY_5_4_OR_NEWER)
        // Called by Unity after a new level was loaded.
        private void OnLevelWasLoaded()
        {
            this.mRectTransform = this.GetComponent<RectTransform>();

            JCS_UIManager uim = JCS_UIManager.instance;

            // Once we load the scene we need to let new object 
            // in the scene know about us!
            uim.SetJCSDialogue(mDialogueType, this);

            // add to open window list if the window is open!
            AddToOpenWindowList();

            ResetDialogue();
        }
#endif

        protected override void Awake()
        {
#if (UNITY_5_4_OR_NEWER)
            this.mRectTransform = this.GetComponent<RectTransform>();
            if (mRectTransform == null)
                return;

            // this is the new way of doing the "OnLevelWasLoaded" 
            // function call after version 5.4
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
            {
                JCS_UIManager uim = JCS_UIManager.instance;

                // Once we load the scene we need to let new object 
                // in the scene know about us!
                uim.SetJCSDialogue(mDialogueType, this);

                // add to open window list if the window is open!
                AddToOpenWindowList();

                ResetDialogue();
            };
#endif

            if (mSoundPlayer == null)
                this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            base.Awake();

            JCS_SoundSettings ss = JCS_SoundSettings.instance;


            // Assign Default Audio
            {
                if (mOpenWindowClip == null)
                    this.mOpenWindowClip = ss.DEFAULT_OPEN_WINDOW_CLIP;

                if (mCloseWindowClip == null)
                    this.mCloseWindowClip = ss.DEFAULT_CLOSE_WINDOW_CLIP;
            }
        }

        protected override void Start()
        {
            JCS_UIManager uim = JCS_UIManager.instance;

            uim.SetJCSDialogue(mDialogueType, this);

            base.Start();

            // pause the app
            JCS_ApplicationManager.APP_PAUSE = true;
        }

        protected void Update()
        {
            mOriginalScale = mRectTransform.localScale;
            mOriginalPosition = mRectTransform.localPosition;
            mOriginalRotation = mRectTransform.localRotation;

            ProcessInput();
        }

        /// <summary>
        /// Decide what panel is this panel going to be.
        /// </summary>
        public void DoPanelType()
        {
            switch (mPanelType)
            {
                case JCS_PanelType.RESET_PANEL:
                    {
                        DestroyDialogue();
                    }
                    break;
                case JCS_PanelType.RECORD_PANEL:
                    {
                        HideDialogue();
                    }
                    break;
                case JCS_PanelType.RECORD_PANEL_TO_DATABASE:
                    {
                        // send packet
                    }
                    break;
            }
        }

        /// <summary>
        /// Show the dialogue in the game.
        /// 
        /// NOTE(jenchieh): this will play the default sound.
        /// </summary>
        public void ShowDialogue()
        {
            ShowDialogueWithoutSound();

            // set focus dialogue
            if (DialogueType == JCS_DialogueType.PLAYER_DIALOGUE)
                JCS_UIManager.instance.SetJCSDialogue(JCS_DialogueType.PLAYER_DIALOGUE, this);

            // let UIManager know the window is opened
            SwapToTheLastOpenWindowList();

            JCS_SoundPlayer.PlayByAttachment(mSoundPlayer, mOpenWindowClip, JCS_SoundMethod.PLAY_SOUND);
        }

        /// <summary>
        /// Hide the dialogue without the sound.
        /// </summary>
        public override void HideDialogueWithoutSound()
        {
            base.HideDialogueWithoutSound();

            RemoveFromOpenWindowList();
        }

        /// <summary>
        /// Hide the dialogue in the game.
        /// 
        /// NOTE(jenchieh): this will play the defualt sound.
        /// </summary>
        public void HideDialogue()
        {
            HideDialogueWithoutSound();

            JCS_SoundPlayer.PlayByAttachment(mSoundPlayer, mCloseWindowClip, JCS_SoundMethod.PLAY_SOUND);
        }

        /// <summary>
        /// Toggle this dialgoue show and hide.
        /// </summary>
        public void ToggleVisibility()
        {
            if (mIsVisible)
                HideDialogue();
            else
                ShowDialogue();

            ResetDialogue();
        }

        /// <summary>
        /// Move the last child of the current child will make the 
        /// panel in front of any other GUI in the current panel.
        /// </summary>
        public void MoveToTheLastChild()
        {
            JCS_Utility.MoveToTheLastChild(this.transform);

            // Once it move to the last child, meaning the window have been focus.
            SwapToTheLastOpenWindowList();
        }

        /// <summary>
        /// Add the panel to window open list, so will record what window 
        /// is actual open in the current game scene.
        /// </summary>
        private void AddToOpenWindowList()
        {
            switch (mDialogueType)
            {
                case JCS_DialogueType.GAME_UI:
                case JCS_DialogueType.SYSTEM_DIALOGUE:
                    return;
            }

            // first check the window is open or not open
            if (!IsWindowOpened())
                return;

            // add to the list so the manager know what window is opened
            JCS_UIManager.instance.GetOpenWindow().AddLast(this);
        }

        /// <summary>
        /// Remove the windwo current in the list, so will record down
        /// the window that are close/hide in the current game scene.
        /// </summary>
        private void RemoveFromOpenWindowList()
        {
            JCS_UIManager.instance.GetOpenWindow().Remove(this);
        }

        /// <summary>
        /// Swap this panel to the last panel in the list.
        /// </summary>
        private void SwapToTheLastOpenWindowList()
        {
            // TODO(JenChieh): optimize

            // remove the list
            RemoveFromOpenWindowList();

            // add back to the list (so it will be the last)
            AddToOpenWindowList();
        }

        /// <summary>
        /// Process the input base on platform
        /// </summary>
        private void ProcessInput()
        {
            if (JCS_Input.GetKeyDown(mOpenKey))
            {
                ToggleVisibility();
            }
        }
    }
}
