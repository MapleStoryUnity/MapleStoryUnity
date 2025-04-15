/**
 * $File: JCS_DropdownObject.cs $
 * $Date: 2025-04-15 01:14:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2025 by Shen, Jen-Chieh $
 */

/* NOTE: If you are using `TextMesh Pro` uncomment this line.
 */
#define TMP_PRO

using UnityEngine;
using UnityEngine.UI;
using MyBox;

#if TMP_PRO
using TMPro;
#endif

namespace JCSUnity
{
    /// <summary>
    /// Holds all type of UI dropdown objects.
    /// </summary>
    public class JCS_DropdownObject : MonoBehaviour
    {
        /* Variables */

        [Separator("Initialize Variables (JCS_DropdownObject)")]

        [Tooltip("Target dropdown renderer.")]
        [SerializeField]
        protected Dropdown mDropdownLegacy = null;

#if TMP_PRO
        [Tooltip("Target dropdown renderer. (TMP)")]
        [SerializeField]
        protected TMP_Dropdown mDropdownTMP = null;
#endif

        /* Setter & Getter */

        public Dropdown DropdownLegacy { get { return this.mDropdownLegacy; } set { this.mDropdownLegacy = value; } }
#if TMP_PRO
        public TMP_Dropdown DropdownTMP { get { return this.mDropdownTMP; } set { this.mDropdownTMP = value; } }
#endif

        public int value
        {
            get
            {
                if (this.mDropdownLegacy)
                    return this.mDropdownLegacy.value;

                return this.mDropdownTMP.value;
            }

            set
            {
                if (this.mDropdownLegacy)
                    this.mDropdownLegacy.value = value;

                if (this.mDropdownTMP)
                    this.mDropdownTMP.value = value;
            }
        }

        /* Functions */

        /// <summary>
        /// Clear all options.
        /// </summary>
        public void ClearOptions()
        {
            if (this.mDropdownLegacy)
                this.mDropdownLegacy.ClearOptions();

            if (this.mDropdownTMP)
                this.mDropdownTMP.ClearOptions();
        }
    }
}
