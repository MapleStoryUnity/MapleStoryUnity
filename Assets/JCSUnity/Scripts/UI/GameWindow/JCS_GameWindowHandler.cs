﻿/**
 * $File: JCS_GameWindowHandler.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Object to create the instance of Game Window.
    /// </summary>
    public class JCS_GameWindowHandler : JCS_Settings<JCS_GameWindowHandler>
    {
        /* Variables */

        [Header("** System Dialogue **")]

        [Tooltip("System UI instances.")]
        [SerializeField]
        private JCS_DialogueObject[] mSystemUI = null;

        [Header("** Game User Interface **")]

        [Tooltip("Game UI instance.")]
        [SerializeField]
        private JCS_DialogueObject mGameUI = null;

        [Header("** NPC Dialogue **")]

        [Tooltip("NPC dialogue instance.")]
        [SerializeField]
        private JCS_DialogueObject mNPCDialogue = null;

        // List of all the Game Window we are going to use in the game
        [Header("** Player Dialogue **")]

        [Tooltip("Player dialgoue instances.")]
        [SerializeField]
        private JCS_DialogueObject[] mPlayerDialogue = null;

        /* Setter & Getter */

        public JCS_DialogueObject GetPlayerDialogueAt(int index) { return this.mPlayerDialogue[index]; }
        public JCS_DialogueObject NPCDialogue { get { return this.mNPCDialogue; } set { this.mNPCDialogue = value; } }
        public JCS_DialogueObject[] PlayerDialogue { get { return this.mPlayerDialogue; } set { this.mPlayerDialogue = value; } }
        public JCS_DialogueObject GameUI { get { return this.mGameUI; } set { this.mGameUI = value; } }
        public JCS_DialogueObject[] SystemUI { get { return this.mSystemUI; } set { this.mSystemUI = value; } }

        /* Functions */

        private void Awake()
        {
            instance = CheckSingleton(instance, this);
        }

        private void Start()
        {
            PopSystemUI();
            PopGameUI();
            PopNPCDialogue();
            PopPlayerDialogue();
        }

        /// <summary>
        /// Show the game UI.
        /// </summary>
        public void ShowGameUI()
        {
            if (mGameUI == null)
            {
                JCS_Debug.LogError("Game UI is not an avialiable references");
                return;
            }

            mGameUI.Show(true);
        }

        /// <summary>
        /// Hide the game UI.
        /// </summary>
        public void HideGameUI()
        {
            if (mGameUI == null)
            {
                JCS_Debug.LogError("Game UI is not an avialiable references");
                return;
            }

            mGameUI.Hide(true);
        }

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected override void TransferData(JCS_GameWindowHandler _old, JCS_GameWindowHandler _new)
        {
            _new.SystemUI = _old.SystemUI;
            _new.NPCDialogue = _old.NPCDialogue;
            _new.GameUI = _old.GameUI;
            _new.PlayerDialogue = _old.PlayerDialogue;
        }

        /// <summary>
        /// Pop the NPC dialogue.
        /// </summary>
        private void PopNPCDialogue()
        {
            PopDialogue(mNPCDialogue);
        }

        /// <summary>
        /// Pop the game UI.
        /// </summary>
        private void PopGameUI()
        {
            PopDialogue(mGameUI);
        }

        /// <summary>
        /// Pop player dialogue.
        /// </summary>
        private void PopPlayerDialogue()
        {
            JCS_DialogueObject[] temp = (JCS_DialogueObject[])mPlayerDialogue.Clone();
            PopDialogue(temp);
        }

        /// <summary>
        /// Pop system UI.
        /// </summary>
        private void PopSystemUI()
        {
            JCS_DialogueObject[] temp = (JCS_DialogueObject[])mSystemUI.Clone();
            PopDialogue(temp);
        }

        /// <summary>
        /// Pop one single dialogue.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private JCS_DialogueObject PopDialogue(JCS_DialogueObject obj)
        {
            if (obj == null)
                return null;

            obj = (JCS_DialogueObject)JCS_Util.SpawnGameObject(obj);
            obj.Show();
            obj.OpenKey = KeyCode.None;

            return obj;
        }

        /// <summary>
        /// Pop all the dialogues in the array.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private JCS_DialogueObject[] PopDialogue(JCS_DialogueObject[] list)
        {
            if (list.Length == 0)
                return null;

            for (int index = 0; index < list.Length; ++index)
            {
                if (list[index] == null)
                    continue;

                list[index] = (JCS_DialogueObject)JCS_Util.SpawnGameObject(list[index]);
                list[index].Hide();
            }

            return list;
        }
    }
}
