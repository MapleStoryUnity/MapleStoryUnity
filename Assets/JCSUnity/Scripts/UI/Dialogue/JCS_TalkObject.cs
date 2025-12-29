/**
 * $File: JCS_TalkObject.cs $
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
    /// Make the game object talkable.
    /// </summary>
    public class JCS_TalkObject : MonoBehaviour
    {
        /* Variables */

        [Separator("🌱 Initialize Variable (JCS_TalkObject)")]

        [Tooltip("Current dialogue script this talk object holds.")]
        [SerializeField]
        private JCS_DialogueScript mDialogueScript = null;

        /* Setter & Getter */

        /* Functions */

        private void OnMouseOver()
        {
            if (JCS_Input.OnMouseDoubleClick(JCS_MouseButton.LEFT))
            {
                var ds = JCS_DialogueSystem.FirstInstance();

                if (ds != null)
                {
                    // active dialogue system.
                    ds.ActiveDialogue(mDialogueScript);
                }
            }
        }

        /// <summary>
        /// Create the character image.
        /// </summary>
        /// <param name="sp"></param>
        private void CreateCharacterImage(Sprite sp)
        {
            if (sp == null)
                return;

            // ..
        }
    }
}
