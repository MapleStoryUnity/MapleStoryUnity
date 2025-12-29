/**
 * $File: JCS_DropdownObject.cs $
 * $Date: 2025-04-15 01:14:38 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright � 2025 by Shen, Jen-Chieh $
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

        [Separator("🌱 Initialize Variables (JCS_DropdownObject)")]

        [Tooltip("Target dropdown renderer.")]
        [SerializeField]
        protected Dropdown mDropdownLegacy = null;

#if TMP_PRO
        [Tooltip("Target dropdown renderer. (TMP)")]
        [SerializeField]
        protected TMP_Dropdown mDropdownTMP = null;
#endif

        /* Setter & Getter */

        public Dropdown DropdownLegacy { get { return mDropdownLegacy; } set { mDropdownLegacy = value; } }
#if TMP_PRO
        public TMP_Dropdown DropdownTMP { get { return mDropdownTMP; } set { mDropdownTMP = value; } }
#endif

        public int value
        {
            get
            {
                if (mDropdownLegacy)
                    return mDropdownLegacy.value;

                return mDropdownTMP.value;
            }

            set
            {
                if (mDropdownLegacy)
                    mDropdownLegacy.value = value;

                if (mDropdownTMP)
                    mDropdownTMP.value = value;
            }
        }

        /* Functions */

        /// <summary>
        /// Clear all options.
        /// </summary>
        public void ClearOptions()
        {
            if (mDropdownLegacy)
                mDropdownLegacy.ClearOptions();

            if (mDropdownTMP)
                mDropdownTMP.ClearOptions();
        }
    }
}
