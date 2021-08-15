/**
 * $File: JCS_IgnoreDialogueObject.cs $
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
    /// Scene will ignore this panel and not brought into next scene.
    /// </summary>
    public class JCS_IgnoreDialogueObject : MonoBehaviour
    {
        // empty type to ignore panel

        private void Start()
        {
            // Find the correct parent depend on the mode
            // developer choose and do the command
            SetParentObjectByMode();
        }

        private void SetParentObjectByMode()
        {
            Transform parentObject = null;

            // if is Resize UI is enable than add Dialogue under
            // resize ui transform
            if (JCS_UISettings.instance.RESIZE_UI)
                parentObject = JCS_Canvas.instance.GetResizeUI().transform;
            // Else we add it directly under the Canvas
            else
                parentObject = JCS_Canvas.instance.GetCanvas().transform;

            // set it to parent
            this.gameObject.transform.SetParent(parentObject);
        }
    }
}
