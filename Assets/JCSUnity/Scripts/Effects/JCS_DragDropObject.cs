/**
 * $File: JCS_DragDropObject.cs $
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
    /// Drag and drop effect.
    /// </summary>
    public class JCS_DragDropObject : MonoBehaviour
    {
        /* Variables */

        private enum DragDrop
        {
            Drag,
            Drop,
        };

        [Separator("⚡️ Runtime Variables (JCS_DragDropObject)")]

        [Tooltip("Type of the drag and drop target.")]
        [SerializeField]
        private JCS_DragDropType mDragDropType = JCS_DragDropType.DialogueBox;

        /* Setter & Getter */

        public JCS_DragDropType dragDropType { get { return mDragDropType; } set { mDragDropType = value; } }

        /* Functions */

        /// <summary>
        /// Call when on drag.
        /// </summary>
        public void OnDrag()
        {
            switch (mDragDropType)
            {
                case JCS_DragDropType.DialogueBox:
                    ProcessGUI(DragDrop.Drag);
                    break;

                case JCS_DragDropType.Inventory:
                    break;

                // TODO(JenChieh): 
                case JCS_DragDropType.GameObject_2D:
                    break;

                case JCS_DragDropType.GameObject_25D:
                    break;

                case JCS_DragDropType.GameObjbect_3D:
                    break;
            }
        }

        /// <summary>
        /// Call when on drop.
        /// </summary>
        public void OnDrop()
        {
            // empty..
        }

        /// <summary>
        /// Process gui drag and drop event.
        /// </summary>
        /// <param name="type"></param>
        private void ProcessGUI(DragDrop type)
        {
            switch (type)
            {
                case DragDrop.Drag:  /* Process Drag */
                    {
                        Vector3 delta = JCS_Input.MouseDeltaPosition();
                        transform.localPosition += delta;
                    }
                    break;
                case DragDrop.Drop:  /* Process Drop */
                    {
                        // empty..
                    }
                    break;
            }
        }
    }
}
