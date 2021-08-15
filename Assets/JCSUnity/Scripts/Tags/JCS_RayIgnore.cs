/**
 * $File: JCS_RayIgnore.cs $
 * $Date: 2016-09-20 17:52:11 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Tag of ignoring the item.
    /// </summary>
    public class JCS_RayIgnore : MonoBehaviour
    {
        /* Variables */

        // Tags Component.

        // STUDY(jenchieh): tell me if u like the tag system more.

        // DESCRIPTION(jenchieh): All most the same as 
        // the tag system in Unity Engine.
        // Using this have the better control of usage 
        // in component system wise.

        [Tooltip("Attach all tag to child?")]
        [SerializeField]
        private bool mEffectToAllChild = true;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            // check trigger
            if (!mEffectToAllChild)
                return;

            // add tag to all childs
            for (int index = 0;
                index < this.transform.childCount;
                ++index)
            {
                // apply all to child
                this.transform.GetChild(index).gameObject.AddComponent<JCS_RayIgnore>();
            }
        }
    }
}
