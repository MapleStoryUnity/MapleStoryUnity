/**
 * $File: JCS_DialogueObject.cs $
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
    /// Base class of Game Window.
    /// </summary>
    [RequireComponent(typeof(JCS_GameWindow))]
    public class JCS_DialogueObject : JCS_PanelRoot
    {
        /* Variables */

        [Separator("⚡️ Runtime Variables (JCS_DialogueObject)")]

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

        [Header("🔍 Sound")]

        [Tooltip("Sound player for this component.")]
        [SerializeField]
        private JCS_SoundPlayer mSoundPlayer = null;

        [Tooltip("Sound when open this window.")]
        [SerializeField]
        private AudioClip mOpenWindowClip = null;

        [Tooltip("Sound when close this window.")]
        [SerializeField]
        private AudioClip mCloseWindowClip = null;

        /* Setter & Getter */

        public int dialogueIndex { get { return mDialogueIndex; } }
        public KeyCode openKey { get { return mOpenKey; } set { mOpenKey = value; } }
        public JCS_DialogueType dialogueType { get { return mDialogueType; } }
        public JCS_PanelType panelType { get { return mPanelType; } }
        public bool IsWindowOpened() { return mIsVisible; }

        public JCS_SoundPlayer soundPlayer { get { return mSoundPlayer; } set { mSoundPlayer = value; } }
        public AudioClip openWindowClip { get { return mOpenWindowClip; } set { mOpenWindowClip = value; } }
        public AudioClip closeWindowClip { get { return mCloseWindowClip; } set { mCloseWindowClip = value; } }

        /* Functions */

#if !(UNITY_5_4_OR_NEWER)
        private void OnLevelWasLoaded()
        {
            mRectTransform = GetComponent<RectTransform>();

            var uim = JCS_UIManager.instance;

            // Once we load the scene we need to let new object 
            // in the scene know about us!
            uim.SetDialogue(mDialogueType, this);

            // add to open window list if the window is open!
            AddToOpenWindowList();

            ResetDialogue();
        }
#endif

        protected override void Awake()
        {
#if (UNITY_5_4_OR_NEWER)
            mRectTransform = GetComponent<RectTransform>();
            if (mRectTransform == null)
                return;

            // this is the new way of doing the "OnLevelWasLoaded" 
            // function call after version 5.4
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
            {
                var uim = JCS_UIManager.FirstInstance();

                // Once we load the scene we need to let new object 
                // in the scene know about us!
                uim.SetDialogue(mDialogueType, this);

                // add to open window list if the window is open!
                AddToOpenWindowList();

                ResetDialogue();
            };
#endif

            if (mSoundPlayer == null)
                mSoundPlayer = GetComponent<JCS_SoundPlayer>();

            base.Awake();

            var ss = JCS_SoundSettings.FirstInstance();

            // Assign Default Audio
            {
                if (mOpenWindowClip == null)
                    mOpenWindowClip = ss.clipWindowOpen;

                if (mCloseWindowClip == null)
                    mCloseWindowClip = ss.clipWindowClose;
            }
        }

        protected override void Start()
        {
            var uim = JCS_UIManager.FirstInstance();

            uim.SetDialogue(mDialogueType, this);

            base.Start();

            // pause the app
            JCS_AppManager.APP_PAUSE = true;
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
                        Destroy();
                    }
                    break;
                case JCS_PanelType.RECORD_PANEL:
                    {
                        Hide();
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
        /// </summary>
        public override void Show(bool mute = false)
        {
            base.Show(mute);

            // set focus dialogue
            if (dialogueType == JCS_DialogueType.PLAYER_DIALOGUE)
                JCS_UIManager.FirstInstance().SetDialogue(JCS_DialogueType.PLAYER_DIALOGUE, this);

            // let UIManager know the window is opened
            SwapToTheLastOpenWindowList();

            if (!mute)
                JCS_SoundPlayer.PlayByAttachment(mSoundPlayer, mOpenWindowClip, JCS_SoundMethod.PLAY_SOUND);
        }

        /// <summary>
        /// Hide the dialogue in the game.
        /// </summary>
        public override void Hide(bool mute = false)
        {
            base.Hide(mute);

            RemoveFromOpenWindowList();

            if (!mute)
                JCS_SoundPlayer.PlayByAttachment(mSoundPlayer, mCloseWindowClip, JCS_SoundMethod.PLAY_SOUND);
        }

        /// <summary>
        /// Toggle this dialgoue show and hide.
        /// </summary>
        public void ToggleVisibility()
        {
            if (mIsVisible)
                Hide();
            else
                Show();

            ResetDialogue();
        }

        /// <summary>
        /// Move the last child of the current child will make the 
        /// panel in front of any other GUI in the current panel.
        /// </summary>
        public void MoveToTheLastChild()
        {
            JCS_Util.MoveToTheLastChild(transform);

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
            JCS_UIManager.FirstInstance().GetOpenWindow().AddLast(this);
        }

        /// <summary>
        /// Remove the windwo current in the list, so will record down
        /// the window that are close/hide in the current game scene.
        /// </summary>
        private void RemoveFromOpenWindowList()
        {
            JCS_UIManager.FirstInstance().GetOpenWindow().Remove(this);
        }

        /// <summary>
        /// Swap this panel to the last panel in the list.
        /// </summary>
        private void SwapToTheLastOpenWindowList()
        {
            // TODO(jenchieh): optimize

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
