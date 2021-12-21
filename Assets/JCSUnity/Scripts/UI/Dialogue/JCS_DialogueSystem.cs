/**
 * $File: JCS_DialogueSystem.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace JCSUnity
{
    /// <summary>
    /// Dialogue system core implementation.
    /// </summary>
    public class JCS_DialogueSystem : MonoBehaviour
    {
        /* Variables */

        // Callback when successfully dispose the dialogue.
        public EmptyFunction callback_dispose = null;

        [Header("** Check Variables (JCS_DialogueSystem) **")]

        [Tooltip("Script to run the current text box.")]
        [SerializeField]
        private JCS_DialogueScript mDialogueScript = null;

        [Tooltip("Message in the text box.")]
        [SerializeField]
        private string mMessage = "";

        [Tooltip("Skip to the end of the message.")]
        [SerializeField]
        private bool mSkip = false;

        [Tooltip("Trigger of checking the scrolling effect is running?")]
        [SerializeField]
        private bool mScrolling = false;

        [Tooltip("Scrolling the select button text?")]
        [SerializeField]
        private bool mScrollingSelectBtnText = false;

        [Tooltip("Check if the dialogue is active or not...")]
        [SerializeField]
        private bool mActive = false;

        [Tooltip("")]
        [SerializeField]
        private string[] mSelectMessage = null;

        public int Mode = 0;
        public int Type = -1;
        public int Selection = -1;

        // checking
        [SerializeField]
        private int mSelectTextIndex = 0;
        [SerializeField]
        private int mRenderSelectTextIndex = 0;

        [Header("** Initialize Variables (JCS_DialogueSystem) **")]

        [Tooltip("If the mouse hover then select the selection.")]
        [SerializeField]
        private bool mMakeHoverSelect = true;

        [Header("** Runtime Variables (JCS_DialogueSystem) **")]

        [Tooltip("Default character image sprite.")]
        [SerializeField]
        private Sprite mTransparentSprite = null;

        [Tooltip("Image displayed at the center.")]
        [SerializeField]
        private Image mCenterImage = null;

        [Tooltip("Image displayed at the left.")]
        [SerializeField]
        private Image mLeftImage = null;

        [Tooltip("Image displayed at the right.")]
        [SerializeField]
        private Image mRightImage = null;

        [Tooltip("Name tag.")]
        [SerializeField]
        private Text mNameTag = null;

        [Tooltip("Main text GUI component to scroll.")]
        [SerializeField]
        private Text mTextBox = null;

        [Tooltip("Speed of scrolling the text.")]
        [SerializeField]
        [Range(0.01f, 10.0f)]
        private float mScrollTime = 0.1f;

        // timer to calculate the scroll time
        private float mScrollTimer = 0.0f;

        [Tooltip("Panel Rect Transfrom.")]
        [SerializeField]
        private RectTransform mPanelTrans = null;

        [Tooltip("Common use.")]
        [SerializeField]
        private JCS_Button mOkBtn = null;
        [Tooltip("Common use.")]
        [SerializeField]
        private JCS_Button mExitBtn = null;

        [Tooltip("Common use.")]
        [SerializeField]
        private JCS_Button mYesBtn = null;
        [Tooltip("Common use.")]
        [SerializeField]
        private JCS_Button mNoBtn = null;

        [Tooltip("Common use.")]
        [SerializeField]
        private JCS_Button mNextBtn = null;
        [Tooltip("Common use.")]
        [SerializeField]
        private JCS_Button mPreviousBtn = null;

        [Tooltip("Quest use.")]
        [SerializeField]
        private JCS_Button mAcceptBtn = null;
        [Tooltip("Quest use.")]
        [SerializeField]
        private JCS_Button mDeclineBtn = null;

        [Tooltip("Button to do select action.")]
        [SerializeField]
        private JCS_Button[] mSelectBtn = null;


        // Text index to make sure 
        // each character in the textbox.
        private int mTextIndex = 0;

        // 解密用. 中間的數字改一下就是selection index!
        private string mSelectStringFront = "#L" + 0 + "##b";
        private string mSelectStringBack = "#k#l";

        [Header("- Optional Variables")]

        [Tooltip("Button selection group for this dialogue system.")]
        [SerializeField]
        private JCS_ButtonSelectionGroup mButtonSelectionGroup = null;

        [Header("- Sound")]

        [Tooltip("Sound plays when active dialogue.")]
        [SerializeField]
        private AudioClip mActiveSound = null;

        [Tooltip("Sound plays when dispose dialogue.")]
        [SerializeField]
        private AudioClip mDisposeSound = null;

        /* Setter & Getter */

        public bool MakeHoverSelect { get { return this.mMakeHoverSelect; } set { this.mMakeHoverSelect = value; } }
        public JCS_DialogueScript DialogueScript { get { return this.mDialogueScript; } set { this.mDialogueScript = value; } }
        public string SelectStringFront { get { return this.mSelectStringFront; } }
        public string SelectStringBack { get { return this.mSelectStringBack; } }
        public JCS_ButtonSelectionGroup ButtonSelectionGroup { get { return this.mButtonSelectionGroup; } set { this.mButtonSelectionGroup = value; } }

        public AudioClip AcitveSound { get { return this.mActiveSound; } set { this.mActiveSound = value; } }
        public AudioClip DisposeSound { get { return this.mDisposeSound; } set { this.mDisposeSound = value; } }

        /* Functions */

        private void Awake()
        {
            // try to get transfrom by 
            // it own current transfrom.
            if (mPanelTrans == null)
                mPanelTrans = this.GetComponent<RectTransform>();

            // set to manager to get manage.
            JCS_UtilitiesManager.instance.SetDiaglogueSystem(this);
        }

        private void Start()
        {
            InitTextBox();
            InitBtnsSet();
            InitImageSet();

            // create the array with the same length with the button call.
            mSelectMessage = new string[mSelectBtn.Length];

            // dispose at the beginning of the game.
            Dispose();
        }

        private void LateUpdate()
        {
            // scrolling text effect.
            ScrollText();

            ScrollSelectBtnText();
        }

        /// <summary>
        /// Start the dialogue, in other word same as start a conversation.
        /// </summary>
        /// <param name="script">
        /// Script to use to run the dialogue.
        /// </param>
        public void ActiveDialogue(JCS_DialogueScript script)
        {
            mDialogueScript = script;

            if (mActive)
            {
                JCS_Debug.LogError("Dialogue System is already active... Failed to active another one.");
                return;
            }

            // check if the script attached is available?
            if (DialogueScript == null)
            {
                JCS_Debug.LogWarning("Can't run dialogue system without the dialogue script");
                return;
            }

            // reset the action, so it will always start
            // from the beginning.
            mDialogueScript.ResetAction();

            // active panel
            PanelActive(true);

            // otherwise active the dialogue
            mActive = true;

            // run the first action.
            RunAction();

            // Play the active dialogue sound.
            JCS_SoundManager.instance.GetGlobalSoundPlayer().PlayOneShot(mActiveSound);
        }

        /// <summary>
        /// Send a choice to current status.
        /// </summary>
        /// <param name="index"> index of the selection call. </param>
        /// <param name="msg"> message display in textbox </param>
        public void SendChoice(int index, string msg)
        {
            if (mSelectBtn.Length <= index)
            {
                JCS_Debug.LogWarning("Select button call is out of range");
                return;
            }

            if (mSelectBtn[index] == null)
            {
                JCS_Debug.LogWarning("There are space in the array but does no assign the value");
                return;
            }

            // set the text to the button.
            mSelectMessage[index] = msg;

            // active the button.
            SelectBtnActive(index, true);

            // Reset button selection group to make the selection
            // pointer/effect to the first selection!
            ResetButtonSelectionGroup();
        }

        /// <summary>
        /// Next button is the only option for current status.
        /// </summary>
        /// <param name="msg"></param>
        public void SendNext(string msg)
        {
            NextBtnActive(true);
            PrevBtnActive(false);

            YesBtnActive(false);
            NoBtnActive(false);

            AcceptBtnActive(false);
            DeclineBtnActive(false);

            OkBtnActive(false);

            // should always enabled, except dispose
            ExitBtnActive(true);

            mMessage = msg;
            mScrolling = true;
        }

        /// <summary>
        /// Current status will be next and prev control/options.
        /// </summary>
        /// <param name="msg"></param>
        public void SendNextPrev(string msg)
        {
            NextBtnActive(true);
            PrevBtnActive(true);

            YesBtnActive(false);
            NoBtnActive(false);

            AcceptBtnActive(false);
            DeclineBtnActive(false);

            OkBtnActive(false);

            // should always enabled, except dispose
            ExitBtnActive(true);

            mMessage = msg;
            mScrolling = true;
        }

        /// <summary>
        /// Okay button for only option.
        /// </summary>
        /// <param name="msg"></param>
        public void SendOk(string msg)
        {
            NextBtnActive(false);
            PrevBtnActive(false);

            YesBtnActive(false);
            NoBtnActive(false);

            AcceptBtnActive(false);
            DeclineBtnActive(false);

            OkBtnActive(true);

            // should always enabled, except dispose
            ExitBtnActive(true);

            mMessage = msg;
            mScrolling = true;
        }

        /// <summary>
        /// Yes/No options for current status.
        /// </summary>
        /// <param name="msg"></param>
        public void SendYesNo(string msg)
        {
            NextBtnActive(false);
            PrevBtnActive(false);

            YesBtnActive(true);
            NoBtnActive(true);

            AcceptBtnActive(false);
            DeclineBtnActive(false);

            OkBtnActive(false);

            // should always enabled, except dispose
            ExitBtnActive(true);

            mMessage = msg;
            mScrolling = true;
        }

        /// <summary>
        /// Only exit button will be the only option.
        /// </summary>
        /// <param name="msg"></param>
        public void SendSimple(string msg)
        {
            NextBtnActive(false);
            PrevBtnActive(false);

            YesBtnActive(false);
            NoBtnActive(false);

            AcceptBtnActive(false);
            DeclineBtnActive(false);

            OkBtnActive(false);

            // should always enabled, except dispose
            ExitBtnActive(true);

            mMessage = msg;
            mScrolling = true;
        }

        /// <summary>
        /// Accept/Decline options.
        /// </summary>
        /// <param name="msg"></param>
        public void SendAcceptDecline(string msg)
        {
            NextBtnActive(false);
            PrevBtnActive(false);

            YesBtnActive(false);
            NoBtnActive(false);

            AcceptBtnActive(true);
            DeclineBtnActive(true);

            OkBtnActive(false);

            // should always enabled, except dispose
            ExitBtnActive(true);

            mMessage = msg;
            mScrolling = true;
        }

        /// <summary>
        /// Send Empty option, expect selections take over it.
        /// 
        /// NOTE(jenchieh): Will better if use Game Pad/Controller/Joystick.
        /// </summary>
        /// <param name="msg"></param>
        public void SendEmpty(string msg)
        {
            NextBtnActive(false);
            PrevBtnActive(false);

            YesBtnActive(false);
            NoBtnActive(false);

            AcceptBtnActive(false);
            DeclineBtnActive(false);

            OkBtnActive(false);

            // should always enabled, except dispose
            ExitBtnActive(true);

            mMessage = msg;
            mScrolling = true;
        }

        /// <summary>
        /// Call this to end the dialogue status.
        /// </summary>
        public void Dispose()
        {
            NextBtnActive(false);
            PrevBtnActive(false);

            YesBtnActive(false);
            NoBtnActive(false);

            AcceptBtnActive(false);
            DeclineBtnActive(false);

            OkBtnActive(false);

            // disable the exit button!
            ExitBtnActive(false);


            // dis-attach the script.
            mDialogueScript = null;

            ResetStats();

            // de-active the panel transform.
            PanelActive(false);

            // de-active dialogue system.
            mActive = false;

            // Check initialize to ignore dispose called at the very beginning!
            if (JCS_GameManager.instance.GAME_DONE_INITIALIZE && callback_dispose != null)
                callback_dispose.Invoke();

            // Play the dispose dialogue sound.
            JCS_SoundManager.instance.GetGlobalSoundPlayer().PlayOneShot(mDisposeSound);
        }

        /// <summary>
        /// Reset all dialogue system.
        /// </summary>
        public void ResetStats()
        {
            mScrolling = false;
            mSkip = false;
            mTextIndex = 0;

            mScrollingSelectBtnText = false;
            mRenderSelectTextIndex = 0;
            mSelectTextIndex = 0;

            // reset message.
            mMessage = "";
            mTextBox.text = "";

            // disable all the select btn
            SelectBtnsActive(false);

            // clear all the message.
            ClearSelectBtnMessage();

            // reset images
            SendCenterImage(mTransparentSprite);
            SendLeftImage(mTransparentSprite);
            SendRightImage(mTransparentSprite);

            ResetButtonSelectionGroup();
        }

        /// <summary>
        /// Send a world message...
        /// 
        /// TODO(jenchieh): online mode...
        /// </summary>
        /// <param name="msg"></param>
        public void WorldMessage(string msg)
        {
            // do broadcast.
        }

        /// <summary>
        /// Set the current status name tag.
        /// </summary>
        /// <param name="name"> name tag's name. </param>
        public void SendNameTag(string name)
        {
#if (UNITY_EDITOR)
            if (mNameTag == null)
            {
                JCS_Debug.LogError("Name tag is not assign but u still trying to access?");
                return;
            }
#endif

            mNameTag.text = name;
        }

        /// <summary>
        /// Set the sprite to the image component. (Center)
        /// 
        /// Do not call this if the component did not attached.
        /// </summary>
        /// <param name="sprite"></param>
        public void SendCenterImage(Sprite sprite)
        {
#if (UNITY_EDITOR)
            if (mCenterImage == null)
            {
                JCS_Debug.LogError("Center image call with image component attached...");
                return;
            }
#endif

            mCenterImage.sprite = sprite;
        }
        /// <summary>
        /// Set the sprite to the image component. (Left)
        /// 
        /// Do not call this if the component did not attached.
        /// </summary>
        /// <param name="sprite"></param>
        public void SendLeftImage(Sprite sprite)
        {
#if (UNITY_EDITOR)
            if (mLeftImage == null)
            {
                JCS_Debug.LogError(
                    "Left image call with image component attached...");
                return;
            }
#endif

            mLeftImage.sprite = sprite;
        }
        /// <summary>
        /// Set the sprite to the image component. (Right)
        /// 
        /// Do not call this if the component did not attached.
        /// </summary>
        /// <param name="sprite"></param>
        public void SendRightImage(Sprite sprite)
        {
#if (UNITY_EDITOR)
            if (mRightImage == null)
            {
                JCS_Debug.LogError(
                    "Right image call with image component attached...");
                return;
            }
#endif

            mRightImage.sprite = sprite;
        }

        /// <summary>
        /// Increament one from page.
        /// </summary>
        public void IncPage()
        {
            ++mDialogueScript.Status;
        }

        /// <summary>
        /// Decreament one from page.
        /// </summary>
        public void DecPage()
        {
            --mDialogueScript.Status;
        }

        /// <summary>
        /// Do scroll text action.
        /// </summary>
        private void ScrollText()
        {
            // check if the effect is on
            if (!mScrolling)
                return;

            // do timer.
            mScrollTimer += Time.deltaTime;

            // check if each index of character in message 
            // is good to display in the text box render queue.
            if (mScrollTimer < mScrollTime)
                return;

            // reset timer
            mScrollTimer = 0;

            if (mMessage == mTextBox.text)
            {
                // make sure no errors
                mScrolling = false;
                mSkip = false;

                // start do the selection 
                // button text scrolling action.
                mScrollingSelectBtnText = true;

                // set directly to the text box.
                mTextBox.text = mMessage;

                // reset text index counter
                mTextIndex = 0;
                return;
            }

            if (mSkip)
            {
                // set directly to the text box.
                mTextBox.text = mMessage;

                mSkip = false;

                // end effect.
                mScrolling = false;
            }
            else
            {
                // do the scrolling
                mTextBox.text = mMessage.Substring(0, mTextIndex);
            }

            // increament the index
            ++mTextIndex;
        }

        /// <summary>
        /// Do the scrolling animation for selection buttons text.
        /// </summary>
        private void ScrollSelectBtnText()
        {
            if (!mScrollingSelectBtnText)
                return;

            // do timer.
            mScrollTimer += Time.deltaTime;

            // they will use the same time system.
            if (mScrollTimer < mScrollTime)
                return;

            // reset timer
            mScrollTimer = 0;

            if (mSelectBtn.Length <= mRenderSelectTextIndex)
            {
                mScrollingSelectBtnText = false;
                mSkip = false;

                // ready to render next page of dialogue.
                mRenderSelectTextIndex = 0;

                return;
            }


            if (// if the text in not active skip it, and render the 
                // next possible active selection.
                !mSelectBtn[mRenderSelectTextIndex].gameObject.activeSelf
                ||
                // if the text are the same skip it too.
                (mSelectMessage[mRenderSelectTextIndex] ==
                mSelectBtn[mRenderSelectTextIndex].ButtonText.text))
            {
                // set directly to the text box.
                mSelectBtn[mRenderSelectTextIndex].ButtonText.text
                    = mSelectMessage[mRenderSelectTextIndex];

                // increament one, start render next selection!
                ++mRenderSelectTextIndex;

                // reset the text index back to zero.
                mSelectTextIndex = 0;

                return;
            }

            if (mSkip)
            {
                // set directly to the text box.
                mSelectBtn[mRenderSelectTextIndex].ButtonText.text
                   = mSelectMessage[mRenderSelectTextIndex];

                mSkip = false;

                // end effect.
                mScrollingSelectBtnText = false;
            }
            else
            {
                // do the scrolling
                mSelectBtn[mRenderSelectTextIndex].ButtonText.text
                    = mSelectMessage[mRenderSelectTextIndex].Substring(0, mSelectTextIndex);
            }

            // increament the index
            ++mSelectTextIndex;
        }

        /// <summary>
        /// Active the panel?
        /// </summary>
        /// <param name="act"></param>
        private void PanelActive(bool act)
        {
            if (mPanelTrans == null)
                return;

            mPanelTrans.gameObject.SetActive(act);
        }
        /// <summary>
        /// Active the next button?
        /// </summary>
        /// <param name="act"></param>
        private void NextBtnActive(bool act)
        {
            if (mNextBtn == null)
                return;

            mNextBtn.gameObject.SetActive(act);
        }
        /// <summary>
        /// Active the previous button?
        /// </summary>
        /// <param name="act"></param>
        private void PrevBtnActive(bool act)
        {
            if (mPreviousBtn == null)
                return;

            mPreviousBtn.gameObject.SetActive(act);
        }
        /// <summary>
        /// Active the no button?
        /// </summary>
        /// <param name="act"></param>
        private void NoBtnActive(bool act)
        {
            if (mNoBtn == null)
                return;

            mNoBtn.gameObject.SetActive(act);
        }
        /// <summary>
        /// Active the yes button?
        /// </summary>
        /// <param name="act"></param>
        private void YesBtnActive(bool act)
        {
            if (mYesBtn == null)
                return;

            mYesBtn.gameObject.SetActive(act);
        }
        /// <summary>
        /// Active the okay button?
        /// </summary>
        /// <param name="act"></param>
        private void OkBtnActive(bool act)
        {
            if (mOkBtn == null)
                return;

            mOkBtn.gameObject.SetActive(act);
        }
        /// <summary>
        /// Active the exit button?
        /// </summary>
        /// <param name="act"></param>
        private void ExitBtnActive(bool act)
        {
            if (mExitBtn == null)
                return;

            mExitBtn.gameObject.SetActive(act);
        }
        /// <summary>
        /// Active the accept button?
        /// </summary>
        /// <param name="act"></param>
        private void AcceptBtnActive(bool act)
        {
            if (mAcceptBtn == null)
                return;

            mAcceptBtn.gameObject.SetActive(act);
        }
        /// <summary>
        /// Active the decline button?
        /// </summary>
        /// <param name="act"></param>
        private void DeclineBtnActive(bool act)
        {
            if (mDeclineBtn == null)
                return;

            mDeclineBtn.gameObject.SetActive(act);
        }
        /// <summary>
        /// Active the current selected button.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="act"></param>
        private void SelectBtnActive(int index, bool act)
        {
            JCS_Button selectBtn = mSelectBtn[index];

            if (selectBtn == null)
                return;

            selectBtn.gameObject.SetActive(act);

            if (selectBtn.ButtonSelection != null)
                selectBtn.ButtonSelection.SetSkip(!act);
        }
        /// <summary>
        /// Active the current selected button.
        /// </summary>
        /// <param name="act"></param>
        private void SelectBtnsActive(bool act)
        {
            for (int index = 0; index < mSelectBtn.Length; ++index)
            {
                SelectBtnActive(index, act);
            }
        }
        /// <summary>
        /// Active the center image?
        /// </summary>
        /// <param name="act"></param>
        private void CenterImageActive(bool act)
        {
            if (mCenterImage == null)
                return;

            mCenterImage.gameObject.SetActive(act);
        }
        /// <summary>
        /// Active the left image?
        /// </summary>
        /// <param name="act"></param>
        private void LeftImageActive(bool act)
        {
            if (mLeftImage == null)
                return;

            mLeftImage.gameObject.SetActive(act);
        }
        /// <summary>
        /// Active the right iamge?
        /// </summary>
        /// <param name="act"></param>
        private void RightImageActive(bool act)
        {
            if (mRightImage == null)
                return;

            mRightImage.gameObject.SetActive(act);
        }

        /// <summary>
        /// Clear all the select button message 
        /// from the array.
        /// </summary>
        private void ClearSelectBtnMessage()
        {
            for (int index = 0;
                index < mSelectMessage.Length;
                ++index)
            {
                mSelectMessage[index] = "";
                mSelectBtn[index].ButtonText.text = "";
            }
        }

        /// <summary>
        /// Initalize all the button.
        /// </summary>
        private void InitBtnsSet()
        {
            if (mOkBtn != null)
                mOkBtn.SetSystemCallback(OkBtnCallback);

            if (mNoBtn != null)
                mNoBtn.SetSystemCallback(NoBtnCallback);

            if (mYesBtn != null)
                mYesBtn.SetSystemCallback(YesBtnCallback);

            if (mNextBtn != null)
                mNextBtn.SetSystemCallback(NextBtnCallback);

            if (mPreviousBtn != null)
                mPreviousBtn.SetSystemCallback(PreviousBtnCallback);

            if (mExitBtn != null)
                mExitBtn.SetSystemCallback(ExitBtnCallback);

            if (mAcceptBtn != null)
                mAcceptBtn.SetSystemCallback(AcceptBtnCallback);

            if (mDeclineBtn != null)
                mDeclineBtn.SetSystemCallback(DeclineBtnCallback);

            for (int index = 0; index < mSelectBtn.Length; ++index)
            {
                JCS_Button btn = mSelectBtn[index];

                if (btn == null)
                    continue;

                btn.SetSystemCallback(SelectionInt, index);

                if (mMakeHoverSelect)
                {
                    if (btn.ButtonSelection == null)
                    {
                        JCS_Debug.LogWarning(@"Cannot make hover select 
because button selection is not attach to all selections in the list...");
                    }
                    else
                    {
                        EventTrigger eventTrigger = btn.GetComponent<EventTrigger>();
                        if (eventTrigger == null)
                            eventTrigger = btn.gameObject.AddComponent<EventTrigger>();

                        JCS_Utility.AddEventTriggerEvent(
                            eventTrigger,
                            EventTriggerType.PointerEnter,
                            mButtonSelectionGroup.SelectSelection,
                            btn.ButtonSelection);
                    }
                }
            }
        }

        /// <summary>
        /// Intialize the images.
        /// </summary>
        private void InitImageSet()
        {
            if (mCenterImage != null)
                mCenterImage.transform.SetParent(this.transform);

            if (mLeftImage != null)
                mLeftImage.transform.SetParent(this.transform);

            if (mRightImage != null)
                mRightImage.transform.SetParent(this.transform);
        }

        /// <summary>
        /// Initialize the text box.
        /// </summary>
        private void InitTextBox()
        {
            // check if text box null references...
            if (mTextBox == null)
            {
                JCS_Debug.LogWarning("You have the dialogue system in the scene, but u did not assign a text box... Try to delete it?");
                return;
            }

            mTextBox.transform.SetParent(this.transform);
        }

        /// <summary>
        /// Run the script once.
        /// </summary>
        private void RunAction()
        {
            if (!mActive)
                return;

            /*
             * NOTE(jenchieh): If there is selections occurs
             * in last status, then we make sure the hover working
             * for selecting the selection.
             * 
             * If I hover the selection 5, then selection should 
             * be 5 even I did notclick on selection five. Next
             * button should just know the selection should be 
             * five for next 'RunAction'.
             */
            MakeHoverSelections();

            // initialize every run (before running the script)
            ResetStats();

            // run the script
            mDialogueScript.Action(Mode, Type, Selection);

            // reset mode,type,selection before next action
            Mode = 0;
            Type = -1;
            Selection = -1;
        }

        /// <summary>
        /// If a selection is hover make sure the selection is been 
        /// chosen for next status.
        /// </summary>
        private void MakeHoverSelections()
        {
            if (mButtonSelectionGroup == null)
                return;

            int hoverChoiceIndex = FindSelectedButton();

            /* Return because no selection is active. */
            if (hoverChoiceIndex == -1)
                return;

            Selection = hoverChoiceIndex;
        }

        /// <summary>
        /// Find the active button selection's index!
        /// </summary>
        /// <returns></returns>
        private int FindSelectedButton()
        {
            for (int index = 0; index < mSelectBtn.Length; ++index)
            {
                JCS_Button jcsbtn = mSelectBtn[index];

                if (jcsbtn == null || jcsbtn.ButtonSelection == null)
                    continue;

                if (jcsbtn.ButtonSelection.Active)
                    return index;
            }

            // Selection was not active. Meaning the send choice
            // did not been called by last status. 
            // 
            // NOTE(jenchieh): We are appoarching to new status right now.
            return -1;
        }


        /// <summary>
        /// What if "Next Button" clicked?
        /// </summary>
        private void NextBtnCallback()
        {
            IncPage();

            RunAction();
        }
        /// <summary>
        /// What if "Previous Button" clicked?
        /// </summary>
        private void PreviousBtnCallback()
        {
            DecPage();

            RunAction();
        }
        /// <summary>
        /// What if "Yes Button" clicked?
        /// </summary>
        private void YesBtnCallback()
        {
            IncPage();
            Selection = 1;

            RunAction();
        }
        /// <summary>
        /// What if "No Button" clicked?
        /// </summary>
        private void NoBtnCallback()
        {
            IncPage();
            Selection = 0;

            RunAction();
        }
        /// <summary>
        /// What if "Ok Button" clicked?
        /// </summary>
        private void OkBtnCallback()
        {
            // when exit button happens, exit the dialogue box.
            Dispose();
        }
        /// <summary>
        /// What if "Exit Button" clicked?
        /// </summary>
        private void ExitBtnCallback()
        {
            // when exit button happens, exit the dialogue box.
            Dispose();
        }
        /// <summary>
        /// What if "Accept Button" clicked?
        /// </summary>
        private void AcceptBtnCallback()
        {
            IncPage();
            Selection = 1;

            RunAction();
        }
        /// <summary>
        /// What if "Decline Button" clicked?
        /// </summary>
        private void DeclineBtnCallback()
        {
            IncPage();
            Selection = 0;

            RunAction();
        }
        /// <summary>
        /// What if "Selection Button" clicked?
        /// </summary>
        private void SelectBtnCallback()
        {
            // inc a page.
            ++mDialogueScript.Status;

            RunAction();
        }

        /* Callback for selection button. */
        private void SelectionInt(int selection)
        {
            Selection = selection;
            SelectBtnCallback();
        }

        /// <summary>
        /// Reset the button selection group for dialogue system's 
        /// selections.
        /// </summary>
        private void ResetButtonSelectionGroup()
        {
            if (mButtonSelectionGroup == null)
                return;

            // reset selections for gamepad selection choice.
            mButtonSelectionGroup.ResetAllSelections();
        }
    }
}
