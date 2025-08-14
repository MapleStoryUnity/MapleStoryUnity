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

        public JCS_GUIType guiType { get { return this.mGUIType; } set { this.mGUIType = value; } }
        public InputField inputField { get { return this.mInputField; } }
        public Slider slider { get { return this.mSlider; } }
        public Toggle toggle { get { return this.mToggle; } }
        public JCS_Switch itSwitch { get { return this.mSwitch; } }
        public Dropdown dropdown { get { return this.mDropdown; } }
        public Scrollbar scrollbar { get { return this.mScrollBar; } }

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
                        this.mInputField = this.GetComponent<InputField>();
                    }
                    break;

                case JCS_GUIType.DROP_DOWN:
                    {
                        this.mDropdown = this.GetComponent<Dropdown>();
                    }
                    break;

                case JCS_GUIType.SLIDER:
                    {
                        this.mSlider = this.GetComponent<Slider>();
                    }
                    break;

                case JCS_GUIType.SCROLL_BAR:
                    {
                        this.mScrollBar = this.GetComponent<Scrollbar>();
                    }
                    break;

                case JCS_GUIType.TOGGLE:
                    {
                        this.mToggle = this.GetComponent<Toggle>();
                        this.mSwitch = this.GetComponent<JCS_Switch>();
                    }
                    break;
            }
        }
    }
}
