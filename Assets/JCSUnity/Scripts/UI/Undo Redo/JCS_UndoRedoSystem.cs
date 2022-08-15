/**
 * $File: JCS_UndoRedoSystem.cs $
 * $Date: 2018-08-25 21:26:05 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Undo Redo system manager.
    /// </summary>
    public class JCS_UndoRedoSystem : MonoBehaviour
    {
        /* Variables */

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_UndoRedoSystem) **")]

        [Tooltip("Test this component with key?")]
        [SerializeField]
        private bool mTestWithKey = false;

        [Tooltip("Undo key.")]
        [SerializeField]
        private JCS_KeyWith mUndoKey = new JCS_KeyWith
        {
            comb = JCS_KeyCombination.ALT,
            key = KeyCode.Z,
        };

        [Tooltip("Redo key.")]
        [SerializeField]
        private JCS_KeyWith mRedoKey = new JCS_KeyWith
        {
            comb = JCS_KeyCombination.ALT,
            key = KeyCode.Y,
        };

        [Tooltip("Key to clear all the undo redo history data.")]
        [SerializeField]
        private KeyCode mClearAllUndoRedoHistoryKey = KeyCode.J;
#endif

        [Header("** Check Variables (JCS_UndoRedoSystem) **")]

        [Tooltip("All of the undo redo component this system handles.")]
        [SerializeField]
        private List<JCS_UndoRedoComponent> mAllUndoRedoComp = new List<JCS_UndoRedoComponent>();

        [Tooltip("List of next undo component.")]
        [SerializeField]
        private List<JCS_UndoRedoComponent> mUndoComp = new List<JCS_UndoRedoComponent>();

        [Tooltip("List of next redo component.")]
        [SerializeField]
        private List<JCS_UndoRedoComponent> mRedoComp = new List<JCS_UndoRedoComponent>();

        /* Setter & Getter */

        /* Functions */

#if (UNITY_EDITOR)
        private void Update()
        {
            Test();
        }

        private void Test()
        {
            if (!mTestWithKey)
                return;

            if (JCS_Input.GetKeyDownWith(mUndoKey))
                UndoComponent();

            if (JCS_Input.GetKeyDownWith(mRedoKey))
                RedoComponent();

            if (JCS_Input.GetKeyDown(mClearAllUndoRedoHistoryKey))
                ClearAllUndoRedoHistory();
        }
#endif

        /// <summary>
        ///  Undo next component.
        /// </summary>
        public void UndoComponent()
        {
            JCS_UndoRedoComponent undoComp = JCS_Util.ListPopBack(mUndoComp);

            if (undoComp == null)
                return;

            undoComp.Undo();

            mRedoComp.Add(undoComp);
        }

        /// <summary>
        /// Redo next component.
        /// </summary>
        public void RedoComponent()
        {
            JCS_UndoRedoComponent redoComp = JCS_Util.ListPopBack(mRedoComp);

            if (redoComp == null)
                return;

            redoComp.Redo();

            mUndoComp.Add(redoComp);
        }

        /// <summary>
        /// Add a undo redo component to the system in order to 
        /// get manage.
        /// </summary>
        /// <param name="comp"></param>
        public void AddUndoRedoComponentToSystem(JCS_UndoRedoComponent comp)
        {
            mAllUndoRedoComp.Add(comp);
        }

        /// <summary>
        /// Add component to next undo component.
        /// </summary>
        /// <param name="undoComp"></param>
        public void AddUndoComponent(JCS_UndoRedoComponent undoComp)
        {
            mUndoComp.Add(undoComp);
        }

        /// <summary>
        /// Add component to next redo component.
        /// </summary>
        /// <param name="redoComp"></param>
        public void AddRedoComponent(JCS_UndoRedoComponent redoComp)
        {
            mRedoComp.Add(redoComp);
        }

        /// <summary>
        /// Record down the previous data before we do 
        /// undo/redo action.
        /// </summary>
        public void RecordPrevData()
        {
            for (int index = 0; index < mAllUndoRedoComp.Count; ++index)
            {
                JCS_UndoRedoComponent comp = mAllUndoRedoComp[index];

                if (comp == null)
                    continue;

                comp.RecordPrevData();
            }
        }

        /// <summary>
        /// Stop recording undo/redo.
        /// </summary>
        public void StopRecordingAll()
        {
            for (int index = 0; index < mAllUndoRedoComp.Count; ++index)
            {
                JCS_UndoRedoComponent comp = mAllUndoRedoComp[index];

                if (comp == null)
                    continue;

                comp.StopRecording();
            }
        }

        /// <summary>
        /// Start recording undo/redo.
        /// </summary>
        public void StartRecordingAll()
        {
            for (int index = 0; index < mAllUndoRedoComp.Count; ++index)
            {
                JCS_UndoRedoComponent comp = mAllUndoRedoComp[index];

                if (comp == null)
                    continue;

                comp.StartRecording();
            }
        }

        /// <summary>
        /// Clear all undo component queue.
        /// </summary>
        public void ClearUndoComp()
        {
            // Clear all undo components' history data.
            for (int index = 0; index < mUndoComp.Count; ++index)
            {
                JCS_UndoRedoComponent comp = mUndoComp[index];

                if (comp == null)
                    continue;

                comp.ClearAllUndo();
            }

            // Clear it.
            mUndoComp.Clear();
        }

        /// <summary>
        /// Clear all redo component queue.
        /// </summary>
        public void ClearRedoComp()
        {
            // Clear all repo components' history data.
            for (int index = 0; index < mRedoComp.Count; ++index)
            {
                JCS_UndoRedoComponent comp = mRedoComp[index];

                if (comp == null)
                    continue;

                comp.ClearAllRedo();
            }

            // Clear it.
            mRedoComp.Clear();
        }

        /// <summary>
        /// Clear all the undo/redo history data.
        /// </summary>
        public void ClearAllUndoRedoHistory()
        {
            // Clear all undo/repo components' history data.
            for (int index = 0; index < mAllUndoRedoComp.Count; ++index)
            {
                JCS_UndoRedoComponent comp = mAllUndoRedoComp[index];

                if (comp == null)
                    continue;

                comp.ClearAllUndoRedoHistory();
            }

            // Clear all undo redo component history data.
            ClearUndoComp();
            ClearRedoComp();
        }

        /// <summary>
        /// Check if there is undo history?
        /// </summary>
        /// <returns>
        /// true, there is at least one undo history.
        /// false, there is no undo history.
        /// </returns>
        public bool ThereIsUndoHistory()
        {
            return (mUndoComp.Count != 0);
        }

        /// <summary>
        /// Check if there is redo history?
        /// </summary>
        /// <returns>
        /// true, there is at least one redo history.
        /// false, there is no redo history.
        /// </returns>
        public bool ThereIsRedoHistory()
        {
            return (mRedoComp.Count != 0);
        }

        /// <summary>
        /// Check if there is undo/redo history?
        /// </summary>
        /// <returns>
        /// true, there is at least one undo/redo history.
        /// false, there is no undo/redo history.
        /// </returns>
        public bool ThereIsUndoOrRedoHistory()
        {
            return (ThereIsUndoHistory() || ThereIsRedoHistory());
        }
    }
}
