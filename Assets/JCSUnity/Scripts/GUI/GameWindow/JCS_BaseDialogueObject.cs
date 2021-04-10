/**
 * $File: JCS_BaseDialogueObject.cs $
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
    /// Common Dialogue Object!
    /// 
    /// Include function, 
    /// Show Dialogue, 
    /// Hide Dialoguem, 
    /// Move To the front of the screen, 
    /// Set the Parent by mode, 
    /// etc.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_BaseDialogueObject
        : MonoBehaviour
    {
        /* Variables */

        protected RectTransform mRectTransform = null;

        // Is current dialgoue visible?
        protected bool mIsVisible = false;

        [Header("** Runtime Variables (JCS_BaseDialogueObject) **")]

        [Tooltip(@"Set the rect transfrom size the same as before playing it.")]
        [SerializeField]
        protected bool mAsOriginalSize = false;

        [Tooltip(@"Set the rect transfrom position the same as before playing it.")]
        [SerializeField]
        protected bool mAsOriginalPosition = false;

        [Tooltip(@"Set the rect transfrom rotation the same as before playing it.")]
        [SerializeField]
        protected bool mAsOriginalRotation = false;

        protected Vector3 mOriginalScale = Vector3.zero;
        protected Vector3 mOriginalPosition = Vector3.zero;
        protected Quaternion mOriginalRotation = new Quaternion(0, 0, 0, 0);

        /* Setter & Getter */

        public bool IsVisible { get { return this.mIsVisible; } }
        public bool AsOriginalSize { get { return this.mAsOriginalSize; } set { this.mAsOriginalSize = value; } }
        public bool AsOriginalPosition { get { return this.mAsOriginalPosition; } set { this.mAsOriginalPosition = value; } }
        public bool AsOriginalRotation { get { return this.mAsOriginalRotation; } set { this.mAsOriginalRotation = value; } }

        /* Functions */

        protected virtual void Awake()
        {
            this.mRectTransform = this.GetComponent<RectTransform>();

            mOriginalScale = mRectTransform.localScale;
            mOriginalPosition = mRectTransform.localPosition;
            mOriginalRotation = mRectTransform.localRotation;

            // Find the correct parent depend on the mode developer choose and do 
            // the command.
            //     - Either "ResizeUI" or "JCS_Canvas"
            SetParentObjectByMode();

            ResetDialogue();
        }

        protected virtual void Start()
        {
            // empty..
        }

        /// <summary>
        /// Reset the dialgoue base on the ratio of the game.
        /// </summary>
        public void ResetDialogue()
        {
            if (mRectTransform == null)
                return;

            // IMPORTANT(JenChieh): override the resize UI part
            if (mAsOriginalSize)
                mRectTransform.localScale = mOriginalScale;
            else
                mRectTransform.localScale = Vector3.one;

            if (mAsOriginalPosition)
                mRectTransform.localPosition = mOriginalPosition;
            else
                mRectTransform.localPosition = Vector3.zero;

            if (mAsOriginalRotation)
                mRectTransform.localRotation = mOriginalRotation;
            else
                mRectTransform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        }

        /// <summary>
        /// Destroy this dialgoue object.
        /// </summary>
        public void DestroyDialogue()
        {
            // start the app
            JCS_ApplicationManager.APP_PAUSE = false;

            // destroy this dialogue
            Destroy(this.gameObject);
        }

        /// <summary>
        /// Show the dialogue without the sound.
        /// </summary>
        public void ShowDialogueWithoutSound()
        {
            mIsVisible = true;

            //this.gameObject.SetActive(true);

            // active all the child object
            for (int index = 0; index < this.transform.childCount; ++index)
            {
                this.transform.GetChild(index).gameObject.SetActive(true);
            }

            JCS_Utility.MoveToTheLastChild(this.transform);
        }

        /// <summary>
        /// Hide the dialogue without the sound.
        /// </summary>
        public virtual void HideDialogueWithoutSound()
        {
            if (mRectTransform == null)
                return;

            mIsVisible = false;

            // Instead of disable the object it self, we deactive all the child object.
            //this.gameObject.SetActive(false);

            // deactive all the child object
            for (int index = 0; index < this.transform.childCount; ++index)
            {
                this.transform.GetChild(index).gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Set the parent object base on the 
        /// fit screen setting.
        /// </summary>
        protected void SetParentObjectByMode()
        {
            JCS_Canvas jcsCanvas = JCS_Canvas.instance;

            if (jcsCanvas == null)
            {
                JCS_Debug.LogReminder("Doesn't use JCS_Canvas object");
                return;
            }

            Transform parentObject = null;

            // if is Resize UI is enable than add Dialogue under resize ui transform
            if (JCS_UISettings.instance.RESIZE_UI)
                parentObject = jcsCanvas.GetResizeUI().transform;
            // Else we add it directly under the Canvas
            else
                parentObject = jcsCanvas.GetCanvas().transform;

            // set it to parent
            this.gameObject.transform.SetParent(parentObject);
        }
    }
}
