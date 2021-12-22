#if (UNITY_EDITOR)
/**
 * $File: JCS_InputController.cs $
 * $Date: 2017-10-07 06:12:14 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEditor;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Control the Unity's 'InputManager' settings.
    /// 
    /// SOURCE(jenchieh): http://plyoung.appspot.com/blog/manipulating-input-manager-in-script.html
    /// </summary>
    public class JCS_InputController : MonoBehaviour
    {
        public static int GAMEPAD_COUNT = 0;  // How many gamepad in this game?

        public static int SelectGamepadType = 0;
        public static string[] GamepadPlatform = {
            "Select Platform",

            /* Sony Play Station */
            "PS",
            "PS2",
            "PS3",
            "PS4",

            /* Microsoft XBox */
            "XBox",
            "XBox 360",
            "XBox One",
        };

        /// <summary>
        /// Get the serialize property from Unity's 'InputManager' 
        /// element structure.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
        {
            SerializedProperty child = parent.Copy();
            child.Next(true);
            do
            {
                if (child.name == name) return child;
            }
            while (child.Next(false));
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axisName"></param>
        /// <returns></returns>
        private static bool AxisDefined(string axisName)
        {
            SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
            SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

            axesProperty.Next(true);
            axesProperty.Next(true);
            while (axesProperty.Next(false))
            {
                SerializedProperty axis = axesProperty.Copy();
                axis.Next(true);
                if (axis.stringValue == axisName) return true;
            }
            return false;
        }

        /// <summary>
        /// Input manager's element struct.
        /// </summary>
        public class InputAxis
        {
            public string name;
            public string descriptiveName;
            public string descriptiveNegativeName;
            public string negativeButton;
            public string positiveButton;
            public string altNegativeButton;
            public string altPositiveButton;

            public float gravity;
            public float dead;
            public float sensitivity;

            public bool snap = false;
            public bool invert = false;

            public JCS_AxisType type;

            public int axis;
            public int joyNum;
        }

        /// <summary>
        /// Add an axis to Input Manager.
        /// </summary>
        /// <param name="axis"></param>
        private static void AddAxis(InputAxis axis, bool redefined = false)
        {
            if (!redefined)
            {
                if (AxisDefined(axis.name))
                    return;
            }

            SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
            SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

            axesProperty.arraySize++;
            serializedObject.ApplyModifiedProperties();

            SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

            GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
            GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
            GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
            GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
            GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
            GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
            GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
            GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
            GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
            GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
            GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
            GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
            GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
            GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
            GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

            serializedObject.ApplyModifiedProperties();
        }


        /// <summary>
        /// Clear all the input manager settings.
        /// 
        /// ATTENTION(jenchieh): this will clear everything out from
        /// Unity's 'InputManager'. Use it carefully.
        /// </summary>
        public static void ClearInputManagerSettings()
        {
            SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
            SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
            axesProperty.ClearArray();
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Design the input we want to add here.
        /// </summary>
        public static void SetupInputManager()
        {
            switch (SelectGamepadType)
            {
                case 0:  /* ==> Select Platform <== */

                    break;

                /* Sony Play Station */
                case 1:  /* ==> PS <== */

                    break;
                case 2:  /* ==> PS2 <== */

                    break;
                case 3:  /* ==> PS3 <== */

                    break;
                case 4:  /* ==> PS4 <== */
                    SetupPS4Joystick();
                    break;

                /* Microsoft XBox */
                case 5:  /* ==> XBox <== */
                    
                    break;
                case 6:  /* ==> XBox 360 <== */
                    SetupXBox360Joystick();
                    break;
                case 7:  /* ==> XBox One <== */

                    break;
            }
        }

        /// <summary>
        /// Do the default input manager settings. (For revert)
        /// </summary>
        public static void DefaultInputManagerSettings()
        {
            int axisOffset = 1;

            #region setting

            AddAxis(new InputAxis()
            {
                name = "Horizontal",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "left",
                positiveButton = "right",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 3,
                dead = 0.001f,
                sensitivity = 3,
                snap = true,
                invert = false,
                type = 0,
                axis = 0 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Vertical",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "down",
                positiveButton = "up",
                altNegativeButton = "s",
                altPositiveButton = "w",
                gravity = 3,
                dead = 0.001f,
                sensitivity = 3,
                snap = true,
                invert = false,
                type = 0,
                axis = 0 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Fire1",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "left ctrl",
                altNegativeButton = "",
                altPositiveButton = "mouse 0",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                snap = false,
                invert = false,
                type = 0,
                axis = 0 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Fire2",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "left alt",
                altNegativeButton = "",
                altPositiveButton = "mouse 1",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                snap = false,
                invert = false,
                type = 0,
                axis = 0 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Fire3",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "left shift",
                altNegativeButton = "",
                altPositiveButton = "mouse 2",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                snap = false,
                invert = false,
                type = 0,
                axis = 0 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Jump",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "space",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                snap = false,
                invert = false,
                type = 0,
                axis = 0 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Mouse X",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0,
                sensitivity = 0.1f,
                snap = false,
                invert = false,
                type = JCS_AxisType.MouseMovement,
                axis = 0 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Mouse Y",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0,
                sensitivity = 0.1f,
                snap = false,
                invert = false,
                type = JCS_AxisType.MouseMovement,
                axis = 1 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Mouse ScrollWheel",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0,
                sensitivity = 0.1f,
                snap = false,
                invert = false,
                type = JCS_AxisType.MouseMovement,
                axis = 2 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Horizontal",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0.19f,
                sensitivity = 1,
                snap = false,
                invert = false,
                type = JCS_AxisType.JoystickAxis,
                axis = 0 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Vertical",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 0,
                dead = 0.19f,
                sensitivity = 1,
                snap = false,
                invert = true,
                type = JCS_AxisType.JoystickAxis,
                axis = 1 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Fire1",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "joystick button 0",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                snap = false,
                invert = false,
                type = JCS_AxisType.KeyOrMouseButton,
                axis = 0 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Fire2",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "joystick button 1",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                snap = false,
                invert = false,
                type = JCS_AxisType.KeyOrMouseButton,
                axis = 0 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Fire3",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "joystick button 2",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                snap = false,
                invert = false,
                type = JCS_AxisType.KeyOrMouseButton,
                axis = 0 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Jump",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "joystick button 3",
                altNegativeButton = "",
                altPositiveButton = "",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                snap = false,
                invert = false,
                type = JCS_AxisType.KeyOrMouseButton,
                axis = 0 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Submit",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "return",
                altNegativeButton = "",
                altPositiveButton = "joystick button 0",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                snap = false,
                invert = false,
                type = JCS_AxisType.KeyOrMouseButton,
                axis = 0 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Submit",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "enter",
                altNegativeButton = "",
                altPositiveButton = "space",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                snap = false,
                invert = false,
                type = JCS_AxisType.KeyOrMouseButton,
                axis = 0 + axisOffset,
                joyNum = 0,
            }, true);

            AddAxis(new InputAxis()
            {
                name = "Cancel",
                descriptiveName = "",
                descriptiveNegativeName = "",
                negativeButton = "",
                positiveButton = "escape",
                altNegativeButton = "",
                altPositiveButton = "joystick button 1",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                snap = false,
                invert = false,
                type = JCS_AxisType.KeyOrMouseButton,
                axis = 0 + axisOffset,
                joyNum = 0,
            }, true);

            #endregion
        }

        /// <summary>
        /// Input setting for PS4's gamepad.
        /// </summary>
        public static void SetupPS4Joystick()
        {
            float defalutSenstivity = JCS_InputSettings.DEFAULT_SENSITIVITY;
            float defaultDead = JCS_InputSettings.DEFAULT_DEAD;
            float defaultGravity = JCS_InputSettings.DEFAULT_GRAVITY;

            for (int joystickNum = 0; joystickNum < GAMEPAD_COUNT; ++joystickNum)
            {
                foreach (JCS_JoystickButton val in JCS_Util.GetValues<JCS_JoystickButton>())
                {
                    if (val == JCS_JoystickButton.NONE)
                        continue;

                    // add axis definition.
                    AddAxis(new InputAxis()
                    {
                        name = JCS_InputSettings.GetJoystickButtonIdName(joystickNum, val),
                        positiveButton = JCS_InputSettings.GetPositiveNameByLabel(val),
                        dead = defaultDead,
                        gravity = defaultGravity,
                        sensitivity = defalutSenstivity,
                        type = JCS_InputSettings.GetAxisType(val),
                        invert = JCS_InputSettings.IsInvert(val),
                        axis = (int)JCS_InputSettings.GetAxisChannel(val),
                        joyNum = joystickNum,
                    });
                }
            }
        }

        /// <summary>
        /// Input setting for XBox 360's gamepad.
        /// </summary>
        public static void SetupXBox360Joystick()
        {
            float defalutSenstivity = JCS_InputSettings.DEFAULT_SENSITIVITY;
            float defaultDead = JCS_InputSettings.DEFAULT_DEAD;
            float defaultGravity = JCS_InputSettings.DEFAULT_GRAVITY;

            for (int joystickNum = 0; joystickNum < GAMEPAD_COUNT; ++joystickNum)
            {
                foreach (JCS_JoystickButton val in JCS_Util.GetValues<JCS_JoystickButton>())
                {
                    if (val == JCS_JoystickButton.NONE)
                        continue;

                    // add axis definition.
                    AddAxis(new InputAxis()
                    {
                        name = JCS_InputSettings.GetJoystickButtonIdName(joystickNum, val),
                        positiveButton = JCS_InputSettings.GetPositiveNameByLabel(val),
                        dead = defaultDead,
                        gravity = defaultGravity,
                        sensitivity = defalutSenstivity,
                        type = JCS_InputSettings.GetAxisType(val),
                        invert = JCS_InputSettings.IsInvert(val),
                        axis = (int)JCS_InputSettings.GetAxisChannel(val),
                        joyNum = joystickNum,
                    });
                }
            }
        }

    }
}
#endif
