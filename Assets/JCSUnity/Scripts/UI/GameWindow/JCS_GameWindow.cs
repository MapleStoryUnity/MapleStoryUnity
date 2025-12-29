/**
 * $File: JCS_GameWindow.cs $
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
    /// Game Window base on the Dialogue object.
    /// 
    /// Specialize class for panel/dialogue object. This class provide 
    /// more than just the normal panel/dialogue interface.
    /// </summary>
    public class JCS_GameWindow : MonoBehaviour
    {
        /* Variables */

        /// <summary>
        /// Window drag drop event.
        /// </summary>
        private enum DragDrop
        {
            Drag,
            Drop,
        };

        private JCS_DialogueObject mDialogueObject = null;

        [Separator("🌱 Initialize Variables (JCS_GameWindow)")]

        [Tooltip("Drag drop type.")]
        [SerializeField]
        private JCS_DragDropType mType = JCS_DragDropType.DialogueBox;

        /* Setter & Getter */

        /* Functions */

        public void Awake()
        {
            mDialogueObject = GetComponent<JCS_DialogueObject>();
        }

        /// <summary>
        /// On pointer down event.
        /// </summary>
        public void ItPointerDown()
        {
            var uim = JCS_UIManager.FirstInstance();

            mDialogueObject.MoveToTheLastChild();
            if (mDialogueObject.dialogueType == JCS_DialogueType.PLAYER_DIALOGUE)
                uim.SetDialogue(JCS_DialogueType.PLAYER_DIALOGUE, mDialogueObject);
        }

        /// <summary>
        /// On click event.
        /// </summary>
        public void ItClick()
        {
            // default.
        }

        /// <summary>
        /// On drag event.
        /// </summary>
        public void ItOnDrag()
        {
            switch (mType)
            {
                case JCS_DragDropType.DialogueBox:
                    ProcessGUI(DragDrop.Drag);
                    break;

                case JCS_DragDropType.Inventory:
                    break;
            }
        }

        /// <summary>
        /// On drop event.
        /// </summary>
        public void ItOnDrop()
        {
            // empty..
        }

        /// <summary>
        /// Process drag drop event.
        /// </summary>
        /// <param name="type"></param>
        private void ProcessGUI(DragDrop type)
        {
            switch (type)
            {
                // Process Drag
                case DragDrop.Drag:
                    Vector3 delta = JCS_Input.MouseDeltaPosition();
                    transform.localPosition += delta;
                    break;

                // Process Drop
                case DragDrop.Drop:
                    break;
            }
        }
    }
}
