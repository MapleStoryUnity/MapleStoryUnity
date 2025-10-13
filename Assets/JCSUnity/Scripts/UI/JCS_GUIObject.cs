/**
 * $File: JCS_GUIObject.cs $
 * $Date: 2018-08-25 22:33:57 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Cross Unity's GUI system object.
    /// </summary>
    public class JCS_GUIObject : MonoBehaviour
    {
        /* Variables */

        [Separator("Check Variables (JCS_UndoRedoComponent)")]

        [Tooltip("Input Field component.")]
        [SerializeField]
        [ReadOnly]
        protected InputField mInputField = null;

        [Tooltip("Slider component.")]
        [SerializeField]
        [ReadOnly]
        protected Slider mSlider = null;

        [Tooltip("Toggle component.")]
        [SerializeField]
        [ReadOnly]
        protected Toggle mToggle = null;

        [Tooltip("JCS_Toggle component.")]
        [SerializeField]
        [ReadOnly]
        protected JCS_Switch mSwitch = null;

        [Tooltip("Dropdown component.")]
        [SerializeField]
        [ReadOnly]
        protected Dropdown mDropdown = null;

        [Tooltip("Scrollbar component.")]
        [SerializeField]
        [ReadOnly]
        protected Scrollbar mScrollBar = null;

        [Separator("Runtime Variables (JCS_UndoRedoComponent)")]

        [Tooltip("Select GUI type.")]
        [SerializeField]
        protected JCS_GUIType mGUIType = JCS_GUIType.NONE;

        /* Setter & Getter */

        public JCS_GUIType guiType { get { return mGUIType; } set { mGUIType = value; } }
        public InputField inputField { get { return mInputField; } }
        public Slider slider { get { return mSlider; } }
        public Toggle toggle { get { return mToggle; } }
        public JCS_Switch itSwitch { get { return mSwitch; } }
        public Dropdown dropdown { get { return mDropdown; } }
        public Scrollbar scrollbar { get { return mScrollBar; } }

        /* Functions */

        protected virtual void Awake()
        {
            UpdateGUIData();
        }

        /// <summary>
        /// Update the GUI data, to get all the necessary 
        /// informations.
        /// </summary>
        public virtual void UpdateGUIData()
        {
            switch (mGUIType)
            {
                case JCS_GUIType.INPUT_FIELD:
                    {
                        mInputField = GetComponent<InputField>();
                    }
                    break;

                case JCS_GUIType.DROP_DOWN:
                    {
                        mDropdown = GetComponent<Dropdown>();
                    }
                    break;

                case JCS_GUIType.SLIDER:
                    {
                        mSlider = GetComponent<Slider>();
                    }
                    break;

                case JCS_GUIType.SCROLL_BAR:
                    {
                        mScrollBar = GetComponent<Scrollbar>();
                    }
                    break;

                case JCS_GUIType.TOGGLE:
                    {
                        mToggle = GetComponent<Toggle>();
                        mSwitch = GetComponent<JCS_Switch>();
                    }
                    break;
            }
        }
    }
}
