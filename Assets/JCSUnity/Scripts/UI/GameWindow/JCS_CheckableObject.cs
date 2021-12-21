/**
 * $File: JCS_CheckableObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;

namespace JCSUnity
{
    /// <summary>
    /// Object will pop out a dialogue so there will be 
    /// a description on it.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_CheckableObject : MonoBehaviour
    {
        /* Variables */

#if (UNITY_STANDALONE || UNITY_EDITOR)
        private RectTransform mRectTransform = null;
#endif

        // Item Contain the following data
        private Image mItemImage = null;

        private enum EventType
        {
            ON_MOUSE_OVER,
            ON_MOUSE_DOWN,
            ON_MOUSE_DOUBLE_CLICK
        };


#if (UNITY_STANDALONE || UNITY_EDITOR)
        [Header("** Check Variables (JCS_CheckableObject) **")]

        [SerializeField]
        private bool mShowing = false;
#endif

        [Header("** Runtime Variables (JCS_CheckableObject) **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_CheckableDialogue mDescDialogue = null;

        [Tooltip("Position that dialogue will be at.")]
        [SerializeField]
        private JCS_2D8Direction mSpawnPosition = JCS_2D8Direction.TOP_LEFT;

        [Tooltip("Root panel to calcuate the position.")]
        [SerializeField]
        private RectTransform mRootPanel = null;

        [Header("NOTE: For 3D Object only.")]

        [Tooltip("Event to show the dialogue.")]
        [SerializeField]
        private EventType mEventType = EventType.ON_MOUSE_OVER;

        /* Setter & Getter */

        /* Functions */

        private void Start()
        {
#if (UNITY_STANDALONE || UNITY_EDITOR)
            this.mRectTransform = this.GetComponent<RectTransform>();
#endif
            this.mItemImage = this.GetComponent<Image>();

            JCS_HideDescDialogue();

            mDescDialogue.SetItemSprite(this.mItemImage.sprite);

            if (mRootPanel == null)
            {
                // Try to find the root panel
                JCS_GameWindow gw = this.GetComponentInParent<JCS_GameWindow>();
                if (gw != null)
                {
                    // gw must have rect transform, no worry about this!
                    // and if is a window must have "JCS_GameWindow"!
                    mRootPanel = gw.transform.GetComponent<RectTransform>();
                }
            }
        }
#if (UNITY_EDITOR || UNITY_STANDALONE)
        private void Update()
        {
            if (!mShowing)
                return;

            FollowMouse();

            JCS_HideDescDialogue();
        }
#endif
        private void OnMouseDown()
        {
            if (mEventType != EventType.ON_MOUSE_DOWN)
                return;

            JCS_ShowDescDialogue();
        }
        private void OnMouseOver()
        {
            //Check event
            if (mEventType == EventType.ON_MOUSE_OVER)
            {
                JCS_ShowDescDialogue();
            }
            //Check event
            else if (mEventType == EventType.ON_MOUSE_DOUBLE_CLICK && 
                // Check if double click
                JCS_Input.OnMouseDoubleClick(0))
            {
                JCS_ShowDescDialogue();
            }
        }

        /// <summary>
        /// Show the description dialogue.
        /// </summary>
        public void JCS_ShowDescDialogue()
        {
            if (mDescDialogue == null)
            {
                JCS_Debug.LogError("No dialogue object attached...");
                return;
            }

            mDescDialogue.ShowDialogueWithoutSound();

#if (UNITY_STANDALONE || UNITY_EDITOR)
            mShowing = true;
#endif
        }

        /// <summary>
        /// Hide the description dialogue.
        /// </summary>
        public void JCS_HideDescDialogue()
        {
            if (mDescDialogue == null)
            {
                JCS_Debug.LogError("No dialogue object attached...");
                return;
            }

#if (UNITY_STANDALONE || UNITY_EDITOR)
            // is is still on top of the image, return it
            if (JCS_Utility.MouseOverGUI(mRectTransform, mRootPanel))
                return;
#endif

            mDescDialogue.HideDialogueWithoutSound();

#if (UNITY_STANDALONE || UNITY_EDITOR)
            mShowing = false;
#endif
        }

        /// <summary>
        /// Make dialogue follow the mouse.
        /// </summary>
        private void FollowMouse()
        {
            mDescDialogue.FollowMouse(mSpawnPosition);
        }
    }
}
