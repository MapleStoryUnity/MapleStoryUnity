/**
 * $File: JCS_ButtonSelectionGroup.cs $
 * $Date: 2017-10-07 14:18:04 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Group certain button and wait for selection instead of 
    /// just using the mouse to select the key. Usually work 
    /// with gamepad/joystick/console.
    /// </summary>
    public class JCS_ButtonSelectionGroup : MonoBehaviour
    {
        /* Variables */

        // Callback triggered when selection has changed.
        public Action selectionChanged = null;

        /// <summary>
        /// Direction of this control.
        /// </summary>
        public enum Direction
        {
            UP,
            DOWN,
            RIGHT,
            LEFT,
        };

        [Separator("Check Variables (JCS_ButtonSelectionGroup)")]

        [Tooltip("Current selecting index.")]
        [SerializeField]
        private int mCurrentSelectIndex = 0;

        [Separator("Runtime Variables (JCS_ButtonSelectionGroup)")]

        [Tooltip("Every time 'OnEnable' function occurs, reset selections.")]
        [SerializeField]
        private bool mOnEnableResetSelections = false;

        [Tooltip("While reseting the selections this will get choose to be the first selected selection.")]
        [SerializeField]
        private JCS_ButtonSelection mStartingSelection = null;

        [Tooltip("List of all the selections this group handle.")]
        [SerializeField]
        private List<JCS_ButtonSelection> mSelections = null;


        /* Setter & Getter */

        public bool onEnableResetSelections { get { return mOnEnableResetSelections; } set { mOnEnableResetSelections = value; } }
        public JCS_ButtonSelection startingSelection { get { return mStartingSelection; } set { mStartingSelection = value; } }


        /* Functions */

        private void Awake()
        {
            this.mSelections = JCS_Array.RemoveEmpty(mSelections);

            // let them know the grouper.
            foreach (JCS_ButtonSelection bs in mSelections)
                bs.buttonSelectionGroup = this;

            selectionChanged = EmptyCallbackSelectionChanged;
        }

        private void Start()
        {
            // reset once.
            ResetAllSelections();
        }

        private void OnEnable()
        {
            if (!mOnEnableResetSelections)
                return;

            // reset once.
            ResetAllSelections();
        }

        /// <summary>
        /// Add a selection into list.
        /// </summary>
        /// <param name="selection"></param>
        public void AddSelection(JCS_ButtonSelection selection)
        {
            mSelections.Add(selection);
        }

        /// <summary>
        /// Remove a selection from the list.
        /// </summary>
        /// <param name="selection"></param>
        public void RemoveSelection(JCS_ButtonSelection selection)
        {
            mSelections.Remove(selection);
        }

        /// <summary>
        /// Reset the selection group to starting status.
        /// </summary>
        public void ResetAllSelections()
        {
            CloseAllSelections();

            // start with the starting selection.
            if (mStartingSelection != null && !mStartingSelection.skip)
            {
                mCurrentSelectIndex = -1;

                SelectSelection(mStartingSelection);
            }
            /* If not start with starting selection, find the next one automatically. */
            else
            {
                mCurrentSelectIndex = mSelections.Count;

                if (!IsAllSelectionsSkip())
                    NextSelection();
            }
        }

        /// <summary>
        /// Close the selection area. Nothing will be high-lighted.
        /// </summary>
        public void CloseAllSelections()
        {
            foreach (JCS_ButtonSelection item in mSelections)
                item.active = false;

            mCurrentSelectIndex = 0;
        }

        /// <summary>
        /// Run the selection button.
        /// </summary>
        public void OkaySelection()
        {
            mSelections[mCurrentSelectIndex].DoSelection();
        }

        /// <summary>
        /// Change to the next button selection. (One dimensional)
        /// </summary>
        /// <returns> true, action made. false, no action made. </returns>
        public bool NextSelection()
        {
            if (IsAllSelectionsSkip())
                return false;

            int tempSelectIndex = mCurrentSelectIndex;
            ++tempSelectIndex;

            SelectSelection(tempSelectIndex);

            // if skip keep looking for the next selection.
            if (mSelections[mCurrentSelectIndex].skip)
                NextSelection();

            return true;
        }

        /// <summary>
        /// Change to the previous button selection. (One dimensional)
        /// </summary>
        /// <returns> true, action made. false, no action made. </returns>
        public bool PrevSelection()
        {
            if (IsAllSelectionsSkip())
                return false;

            int tempSelectIndex = mCurrentSelectIndex;
            --tempSelectIndex;

            SelectSelection(tempSelectIndex);

            // if skip keep looking for the previous selection.
            if (mSelections[mCurrentSelectIndex].skip)
                PrevSelection();

            return true;
        }

        /// <summary>
        /// Selection this selection.
        /// </summary>
        /// <param name="selectionIndex"> index to select. </param>
        public void SelectSelection(int selectionIndex, bool hoverCheck = false)
        {
            if (hoverCheck)
            {
                if (JCS_Util.WithInRange(selectionIndex, mSelections))
                {
                    if (mSelections[selectionIndex].skip)
                        return;
                }
            }

            // no need to do anything.
            if (mCurrentSelectIndex == selectionIndex)
                return;

            if (JCS_Util.WithInRange(mCurrentSelectIndex, mSelections))
            {
                // disable current active selection.
                mSelections[mCurrentSelectIndex].active = false;
            }

            this.mCurrentSelectIndex = selectionIndex;

            this.mCurrentSelectIndex = JCS_Array.LoopIn(this.mCurrentSelectIndex, mSelections);

            // active the new active selection.
            mSelections[mCurrentSelectIndex].active = true;

            if (selectionChanged != null)
                selectionChanged.Invoke();
        }
        public void SelectSelectionHover(int selectionIndex)
        {
            SelectSelection(selectionIndex, true);
        }

        /// <summary>
        /// Selection this selection.
        /// 
        /// ATTENTION(jenchieh): this can only use by the 'Event 
        /// Trigger' component.
        /// </summary>
        /// <param name="selection"> selection to select. </param>
        public void SelectSelection(JCS_ButtonSelection selection, bool hoverCheck = false)
        {
            if (selection == null)
                return;

            /* 
             * Time complexity: O(n)
             * 
             * NOTE(jenchieh): might need to change this if we there are 
             * more than 30 selections.
             */
            for (int index = 0; index < mSelections.Count; ++index)
            {
                JCS_ButtonSelection bs = mSelections[index];

                if (bs == selection)
                {
                    SelectSelection(index, hoverCheck);
                    return;
                }
            }

            Debug.LogError(@"Try to select a selection, but seems like the 
selection is not in the group...");
        }
        public void SelectSelectionHover(JCS_ButtonSelection selection)
        {
            SelectSelection(selection, true);
        }

        /// <summary>
        /// Selection this selection.
        /// </summary>
        /// <param name="selection"> selection to select. </param>
        public void SelectSelection(PointerEventData data, JCS_ButtonSelection selection)
        {
            SelectSelection(selection);
        }

        /// <summary>
        /// Select the selection on the top. (Two Dimensional)
        /// </summary>
        /// <returns> true, action made. false, no action made. </returns>
        public bool UpSelection()
        {
            if (IsAllSelectionsSkip())
                return false;

            if (IsAllSelectionsIsSkipUp())
                return false;

            SelectSelection(GetButtonSelectionByDirection(mSelections[mCurrentSelectIndex], Direction.UP));

            // if skip keep looking for the previous selection.
            if (mSelections[mCurrentSelectIndex].skip)
                UpSelection();

            return true;
        }

        /// <summary>
        /// Select the selection on the down. (Two Dimensional)
        /// </summary>
        /// <returns> true, action made. false, no action made. </returns>
        public bool DownSelection()
        {
            if (IsAllSelectionsSkip())
                return false;

            if (IsAllSelectionsIsSkipDown())
                return false;

            SelectSelection(GetButtonSelectionByDirection(mSelections[mCurrentSelectIndex], Direction.DOWN));

            // if skip keep looking for the previous selection.
            if (mSelections[mCurrentSelectIndex].skip)
                DownSelection();

            return true;
        }

        /// <summary>
        /// Select the selection on the right. (Two Dimensional)
        /// </summary>
        /// <returns> true, action made. false, no action made. </returns>
        public bool RightSelection()
        {
            if (IsAllSelectionsSkip())
                return false;

            if (IsAllSelectionsIsSkipRight())
                return false;

            SelectSelection(GetButtonSelectionByDirection(mSelections[mCurrentSelectIndex], Direction.RIGHT));

            // if skip keep looking for the previous selection.
            if (mSelections[mCurrentSelectIndex].skip)
                RightSelection();

            return true;
        }

        /// <summary>
        /// Select the selection on the left. (Two Dimensional)
        /// </summary>
        /// <returns> true, action made. false, no action made. </returns>
        public bool LeftSelection()
        {
            if (IsAllSelectionsSkip())
                return false;

            if (IsAllSelectionsIsSkipLeft())
                return false;

            SelectSelection(GetButtonSelectionByDirection(mSelections[mCurrentSelectIndex], Direction.LEFT));

            // if skip keep looking for the previous selection.
            if (mSelections[mCurrentSelectIndex].skip)
                LeftSelection();

            return true;
        }

        

        /// <summary>
        /// Get the selection depends on the direction.
        /// </summary>
        /// <param name="bs"> Button Selection object to use to find out the actual button selection. </param>
        /// <param name="direction"> Target direction. </param>
        /// <returns>
        /// Button selection in target button selection's up/down/right/left button selection.
        /// </returns>
        public JCS_ButtonSelection GetButtonSelectionByDirection(JCS_ButtonSelection bs, Direction direction)
        {
            switch (direction)
            {
                case Direction.UP: return bs.upSelection;
                case Direction.DOWN: return bs.downSelection;
                case Direction.RIGHT: return bs.rightSelection;
                case Direction.LEFT: return bs.leftSelection;
            }

            Debug.Log("Failed to get button selection by direction, this should not happens...");
            return null;
        }

        private void EmptyCallbackSelectionChanged() { }

        /// <summary>
        /// Check if all selections are skipped?
        /// </summary>
        /// <returns>
        /// true: all selections are skipped.
        /// false: at least one selection is not skipped.
        /// </returns>
        private bool IsAllSelectionsSkip()
        {
            foreach (JCS_ButtonSelection item in mSelections)
            {
                if (!item.skip)
                    return false;
            }

            CloseAllSelections();
            return true;
        }

        /// <summary>
        /// Is all the button selections in Up starting from current 
        /// selection index skip?
        /// </summary>
        /// <returns>
        /// true: all of the are skip.
        /// false: at least one button selections is not skip.
        /// </returns>
        private bool IsAllSelectionsIsSkipUp()
        {
            return IsAllSelectionsIsSkipDirection(mSelections[mCurrentSelectIndex], Direction.UP);
        }

        /// <summary>
        /// Is all the button selections in Down starting from current 
        /// selection index skip?
        /// </summary>
        /// <returns>
        /// true: all of the are skip.
        /// false: at least one button selections is not skip.
        /// </returns>
        private bool IsAllSelectionsIsSkipDown()
        {
            return IsAllSelectionsIsSkipDirection(mSelections[mCurrentSelectIndex], Direction.DOWN);
        }

        /// <summary>
        /// Is all the button selections in Right starting from current 
        /// selection index skip?
        /// </summary>
        /// <returns>
        /// true: all of the are skip.
        /// false: at least one button selections is not skip.
        /// </returns>
        private bool IsAllSelectionsIsSkipRight()
        {
            return IsAllSelectionsIsSkipDirection(mSelections[mCurrentSelectIndex], Direction.RIGHT);
        }

        /// <summary>
        /// Is all the button selections in Left starting from current 
        /// selection index skip?
        /// </summary>
        /// <returns>
        /// true: all of the are skip.
        /// false: at least one button selections is not skip.
        /// </returns>
        private bool IsAllSelectionsIsSkipLeft()
        {
            return IsAllSelectionsIsSkipDirection(mSelections[mCurrentSelectIndex], Direction.LEFT);
        }

        /// <summary>
        /// Is all the button selection in this direction skip?
        /// </summary>
        /// <param name="bs"> starting button selection. </param>
        /// <param name="direction"> direction to loop to. </param>
        /// <returns>
        /// true: yes, all selections in this direction about this button selection is skip!
        /// false: no, at least on selection is not skip in this direction behind this diection.
        /// </returns>
        private bool IsAllSelectionsIsSkipDirection(JCS_ButtonSelection bs, Direction direction)
        {
            JCS_ButtonSelection newBs = GetButtonSelectionByDirection(bs, direction);

            // if the chain break, meaning all the selections is skip in 
            // this direction.
            if (newBs == null)
                return true;

            // if not skip, break the loop.
            if (!newBs.skip)
                return false;

            return IsAllSelectionsIsSkipDirection(newBs, direction);
        }
    }
}
