/**
 * $File: JCS_InputSettings.cs $
 * $Date: 2016-10-15 19:37:25 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Handle cross platform input settings.
    /// 
    /// URL(jenchieh): http://wiki.unity3d.com/index.php?title=Xbox360Controller
    /// </summary>
    public class JCS_InputSettings : JCS_Settings<JCS_InputSettings>
    {
        /* Variables */

        public const string HOME_BUTTON = "jcsunity button home";
        public const string JOYSTICK_BUTTON_START = "jcsunity button start";
        public const string JOYSTICK_BUTTON_BACK = "jcsunity button back";

        /* General Gamepad */
        public const string JOYSTICK_BUTTON_X = "jcsunity button x";

        /* XBox Gamepad */
        public const string JOYSTICK_BUTTON_A = "jcsunity button a";
        public const string JOYSTICK_BUTTON_B = "jcsunity button b";
        public const string JOYSTICK_BUTTON_Y = "jcsunity button y";

        /* Play Station Gamepad */
        public const string JOYSTICK_BUTTON_CIR = "jcsunity button cir";
        public const string JOYSTICK_BUTTON_SQR = "jcsunity button sqr";
        public const string JOYSTICK_BUTTON_TRI = "jcsunity button tri";

        public const string JOYSTICK_BUTTON_UP = "jcsunity button up";
        public const string JOYSTICK_BUTTON_DOWN = "jcsunity button down";
        public const string JOYSTICK_BUTTON_RIGHT = "jcsunity button right";
        public const string JOYSTICK_BUTTON_LEFT = "jcsunity button left";

        public const string STICK_RIGHT_X = "jcsunity stick right x";
        public const string STICK_RIGHT_Y = "jcsunity stick right y";

        public const string STICK_LEFT_X = "jcsunity stick left x";
        public const string STICK_LEFT_Y = "jcsunity stick left y";

        public const string JOYSTICK_BUTTON_RT = "jcsunity button right trigger";
        public const string JOYSTICK_BUTTON_LT = "jcsunity button left trigger";

        public const string JOYSTICK_BUTTON_LB = "jcsunity button left bumper";
        public const string JOYSTICK_BUTTON_RB = "jcsunity button right bumper";

        public const float DEFAULT_SENSITIVITY = 1.0f;
        public const float DEFAULT_DEAD = 0.2f;
        public const float DEFAULT_GRAVITY = 2000.0f;

        // constant from Unity.
        public const int MAX_JOYSTICK_COUNT = 12;

        /// <summary>
        /// Map of the joystick.
        /// </summary>
        [Serializable]
        public struct JoystickMap
        {
            [Separator("Check Varaibles (JoystickMap)")]

            [Tooltip("Right stick x value.")]
            public float stickRightXVal;

            [Tooltip("Right stick y value.")]
            public float stickRightYVal;

            [Tooltip("Left stick x value.")]
            public float stickLeftXVal;

            [Tooltip("Left stick y value.")]
            public float stickLeftYVal;

            [Separator("Initialize Varaibles (JoystickMap)")]

            #region Button

            [Tooltip("Home button.")]
            public string homeButton;

            [Tooltip("Start button.")]
            public string joystickButtonStart;

            [Tooltip("Back button.")]
            public string joystickButtonBack;

            [Tooltip("Joystick button A")]
            public string joystickButtonA;

            [Tooltip("Joystick button B")]
            public string joystickButtonB;

            [Tooltip("Joystick button X")]
            public string joystickButtonX;

            [Tooltip("Joystick button Y")]
            public string joystickButtonY;

            [Tooltip("Joystick button up.")]
            public string joystickButtonUp;

            [Tooltip("Joystick button down.")]
            public string joystickButtonDown;

            [Tooltip("Joystick button right.")]
            public string joystickButtonRight;

            [Tooltip("Joystick button left.")]
            public string joystickButtonLeft;

            #endregion


            #region Stick
            [Header("- Stick")]

            [Tooltip("Stick on the right")]
            public string stickRightX;

            [Tooltip("Stick on the right")]
            public string stickRightY;

            [Tooltip("Stick on the left")]
            public string stickLeftX;

            [Tooltip("Stick on the left")]
            public string stickLeftY;

            #endregion


            #region Trigger

            [Header("- Trigger")]

            [Tooltip("Joystick button right trigger.")]
            public string joystickButtonRT;

            [Tooltip("Joystick button left trigger.")]
            public string joystickButtonLT;

            #endregion


            #region Bumper

            [Header("- Bumper")]

            [Tooltip("Joystick button left bumper.")]
            public string joystickButtonLB;

            [Tooltip("Joystick button right bumper.")]
            public string joystickButtonRB;

            #endregion

        };


#if UNITY_EDITOR
        [Separator("Helper Varaibles (JCS_InputSettings)")]

        [Tooltip("All joystick's name.")]
        public string[] joystickNames = null;
#endif

        [Separator("Initialize Varaibles (JCS_InputSettings)")]

        [Tooltip("Targeting game pad going to use in the game.")]
        [SerializeField]
        private JCS_GamepadType mTargetGamePad = JCS_GamepadType.XBOX_360;

        // How many joystick in the game? Do the mapping for these joysticks.
        private JoystickMap[] mJoysticks = new JoystickMap[MAX_JOYSTICK_COUNT];


        [Separator("Runtime Varaibles (JCS_InputSettings)")]

        [Tooltip("Total maxinum game pad will live in game.")]
        [SerializeField]
        [Range(0, MAX_JOYSTICK_COUNT)]
        private int mTotalGamePadInGame = 0;

        /* Setter & Getter */

        public JCS_GamepadType TargetGamePad { get { return this.mTargetGamePad; } }
        public JoystickMap[] Joysticks { get { return this.mJoysticks; } set { this.mJoysticks = value; } }
        public JoystickMap GetJoysitckMapByIndex(int index)
        {
            return this.mJoysticks[index];
        }
        public int TotalGamePadInGame { get { return this.mTotalGamePadInGame; } set { this.mTotalGamePadInGame = value; } }

        /* Functions */

        private void Awake()
        {
            instance = CheckInstance(instance, this);
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            // Just assign to it, in order to update the joystick 
            // names array.
            this.joystickNames = Input.GetJoystickNames();
#endif

            GetJoystickInfo();

            JCS_Input.LateUpdate();
        }

        /// <summary>
        /// Get the joystick button name by joystick button label.
        /// </summary>
        /// <param name="label"> joystick button label </param>
        /// <returns> name of the joystick button. </returns>
        public static string GetJoystickButtonName(JCS_JoystickButton label)
        {
            switch (label)
            {
                case JCS_JoystickButton.NONE: return "";

                case JCS_JoystickButton.HOME_BUTTON: return HOME_BUTTON;

                case JCS_JoystickButton.START_BUTTON: return JOYSTICK_BUTTON_START;
                case JCS_JoystickButton.BACK_BUTTON: return JOYSTICK_BUTTON_BACK;

                case JCS_JoystickButton.BUTTON_A: return JOYSTICK_BUTTON_A;
                case JCS_JoystickButton.BUTTON_B: return JOYSTICK_BUTTON_B;
                case JCS_JoystickButton.BUTTON_X: return JOYSTICK_BUTTON_X;
                case JCS_JoystickButton.BUTTON_Y: return JOYSTICK_BUTTON_Y;

                case JCS_JoystickButton.LEFT_TRIGGER: return JOYSTICK_BUTTON_LT;
                case JCS_JoystickButton.RIGHT_TRIGGER: return JOYSTICK_BUTTON_RT;

                case JCS_JoystickButton.LEFT_BUMPER: return JOYSTICK_BUTTON_LB;
                case JCS_JoystickButton.RIGHT_BUMPER: return JOYSTICK_BUTTON_RB;

                case JCS_JoystickButton.BUTTON_UP: return JOYSTICK_BUTTON_UP;
                case JCS_JoystickButton.BUTTON_DOWN: return JOYSTICK_BUTTON_DOWN;
                case JCS_JoystickButton.BUTTON_LEFT: return JOYSTICK_BUTTON_LEFT;
                case JCS_JoystickButton.BUTTON_RIGHT: return JOYSTICK_BUTTON_RIGHT;

                case JCS_JoystickButton.STICK_RIGHT_X: return STICK_RIGHT_X;
                case JCS_JoystickButton.STICK_RIGHT_Y: return STICK_RIGHT_Y;

                case JCS_JoystickButton.STICK_LEFT_X: return STICK_LEFT_X;
                case JCS_JoystickButton.STICK_LEFT_Y: return STICK_LEFT_Y;
            }


            // this should not happens.
            JCS_Debug.LogWarning(@"Try to get the name with unknown joystick button is not allow...");
            return "";
        }

        /// <summary>
        /// Get the joystick button name by joystick button label.
        /// </summary>
        /// <param name="index"> joystick id. </param>
        /// <param name="label"> joystick button label </param>
        /// <returns> name of the joystick button id. </returns>
        public static string GetJoystickButtonIdName(int index, JCS_JoystickButton label)
        {
            return GetJoystickButtonIdName((JCS_JoystickIndex)index, label);
        }

        /// <summary>
        /// Get the joystick button name by joystick button label.
        /// </summary>
        /// <param name="index"> joystick id. </param>
        /// <param name="label"> joystick button label </param>
        /// <returns> name of the joystick button id. </returns>
        public static string GetJoystickButtonIdName(JCS_JoystickIndex index, JCS_JoystickButton label)
        {
            return GetJoystickButtonName(label) + " " + (int)index;
        }

        /// <summary>
        /// Is the button label a axis joystick value?
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static bool IsAxisJoystickButton(JCS_JoystickButton label)
        {
            switch (label)
            {
                case JCS_JoystickButton.BUTTON_UP:
                case JCS_JoystickButton.BUTTON_DOWN:
                case JCS_JoystickButton.BUTTON_LEFT:
                case JCS_JoystickButton.BUTTON_RIGHT:

                case JCS_JoystickButton.STICK_LEFT_X:
                case JCS_JoystickButton.STICK_LEFT_Y:

                case JCS_JoystickButton.STICK_RIGHT_X:
                case JCS_JoystickButton.STICK_RIGHT_Y:

                case JCS_JoystickButton.LEFT_TRIGGER:
                case JCS_JoystickButton.RIGHT_TRIGGER:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Get Unity's controller naming convention.
        /// </summary>
        /// <param name="label"> label we target to get the naming. </param>
        /// <returns> Naming convention by Unity's InputManager, for getting 
        /// the right device driver id. </returns>
        public static string GetPositiveNameByLabel(JCS_JoystickButton label)
        {
#if UNITY_EDITOR
            if (instance == null)
            {
                switch (JCS_InputController.SelectGamepadType)
                {
                    case 0:  /* ==> Select Platform <== */
                        return "";

                    /* Sony Play Station */
                    case 1:  /* ==> PS <== */
                        return "";
                    case 2:  /* ==> PS2 <== */
                        return "";
                    case 3:  /* ==> PS3 <== */
                        return "";
                    case 4:  /* ==> PS4 <== */
                        return GetPositiveNameByLabel_PS4(label);

                    /* Microsoft XBox */
                    case 5:  /* ==> XBox <== */
                        return "";
                    case 6:  /* ==> XBox 360 <== */
                        return GetPositiveNameByLabel_XBox360(label);
                    case 7:  /* ==> XBox One <== */
                        return "";
                }
            }
            else
            {
#endif
                switch (instance.TargetGamePad)
                {
                    case JCS_GamepadType.ALL:
                        return "";

                    /* Sony Play Station */
                    case JCS_GamepadType.PS:
                        return "";
                    case JCS_GamepadType.PS2:
                        return "";
                    case JCS_GamepadType.PS3:
                        return "";
                    case JCS_GamepadType.PS4:
                        return GetPositiveNameByLabel_PS4(label);

                    /* Microsoft XBox */
                    case JCS_GamepadType.XBOX:
                        return "";
                    case JCS_GamepadType.XBOX_360:
                        return GetPositiveNameByLabel_XBox360(label);
                    case JCS_GamepadType.XBOX_ONE:
                        return "";
                }
#if UNITY_EDITOR
            }
#endif

            return "";
        }

#if UNITY_EDITOR
        /// <summary>
        /// Check if any specific button's buffer need to be invert.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static bool IsInvert(JCS_JoystickButton label)
        {
            switch (JCS_InputController.SelectGamepadType)
            {
                case 0:  /* ==> Select Platform <== */
                    return false;

                /* Sony Play Station */
                case 1:  /* ==> PS <== */
                case 2:  /* ==> PS2 <== */
                case 3:  /* ==> PS3 <== */
                case 4:  /* ==> PS4 <== */
                    {
                        switch (label)
                        {
                            case JCS_JoystickButton.BUTTON_LEFT:
                            case JCS_JoystickButton.BUTTON_DOWN:
                                return true;
                        }
                    }
                    break;

                /* Microsoft XBox */
                case 5:  /* ==> XBox <== */
                case 6:  /* ==> XBox 360 <== */
                case 7:  /* ==> XBox One <== */
                    {
                        switch (label)
                        {
                            case JCS_JoystickButton.BUTTON_LEFT:
                            case JCS_JoystickButton.BUTTON_DOWN:
                            case JCS_JoystickButton.LEFT_TRIGGER:
                                return true;
                        }
                    }
                    break;
            }

            return false;
        }
#endif

#if UNITY_EDITOR
        /// <summary>
        /// Get axis channel for Unity's built-in InputManager.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static JCS_AxisChannel GetAxisChannel(JCS_JoystickButton label)
        {
            switch (JCS_InputController.SelectGamepadType)
            {
                case 0:  /* ==> Select Platform <== */
                    break;

                /* Sony Play Station */
                case 1:  /* ==> PS <== */
                case 2:  /* ==> PS2 <== */
                case 3:  /* ==> PS3 <== */
                case 4:  /* ==> PS4 <== */
                    {
                        switch (label)
                        {
                            case JCS_JoystickButton.BUTTON_UP:
                            case JCS_JoystickButton.BUTTON_DOWN:
                                return JCS_AxisChannel.CHANNEL_08;

                            case JCS_JoystickButton.BUTTON_RIGHT:
                            case JCS_JoystickButton.BUTTON_LEFT:
                                return JCS_AxisChannel.CHANNEL_07;

                            case JCS_JoystickButton.STICK_LEFT_X:
                                return JCS_AxisChannel.X_Axis;
                            case JCS_JoystickButton.STICK_LEFT_Y:
                                return JCS_AxisChannel.Y_Axis;
                            case JCS_JoystickButton.STICK_RIGHT_X:
                                return JCS_AxisChannel.CHANNEL_03;
                            case JCS_JoystickButton.STICK_RIGHT_Y:
                                return JCS_AxisChannel.CHANNEL_06;

                            case JCS_JoystickButton.LEFT_TRIGGER:  // this need to be invert.
                            case JCS_JoystickButton.RIGHT_TRIGGER:
                                return JCS_AxisChannel.CHANNEL_03;
                        }
                    }
                    break;

                /* Microsoft XBox */
                case 5:  /* ==> XBox <== */
                case 6:  /* ==> XBox 360 <== */
                case 7:  /* ==> XBox One <== */
                    {
                        switch (label)
                        {
                            case JCS_JoystickButton.BUTTON_UP:
                            case JCS_JoystickButton.BUTTON_DOWN:
                                return JCS_AxisChannel.CHANNEL_07;

                            case JCS_JoystickButton.BUTTON_RIGHT:
                            case JCS_JoystickButton.BUTTON_LEFT:
                                return JCS_AxisChannel.CHANNEL_06;

                            case JCS_JoystickButton.STICK_LEFT_X:
                                return JCS_AxisChannel.X_Axis;
                            case JCS_JoystickButton.STICK_LEFT_Y:
                                return JCS_AxisChannel.Y_Axis;
                            case JCS_JoystickButton.STICK_RIGHT_X:
                                return JCS_AxisChannel.CHANNEL_04;
                            case JCS_JoystickButton.STICK_RIGHT_Y:
                                return JCS_AxisChannel.CHANNEL_05;

                            case JCS_JoystickButton.LEFT_TRIGGER:  // this need to be invert.
                            case JCS_JoystickButton.RIGHT_TRIGGER:
                                return JCS_AxisChannel.CHANNEL_03;
                        }
                    }
                    break;
            }

            // default.
            return JCS_AxisChannel.X_Axis;
        }
#endif

#if UNITY_EDITOR
        /// <summary>
        /// Get the axis type depends on the joystick button label.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static JCS_AxisType GetAxisType(JCS_JoystickButton label)
        {
            switch (JCS_InputController.SelectGamepadType)
            {
                case 0:  /* ==> Select Platform <== */
                    break;

                /* Sony Play Station */
                case 1:  /* ==> PS <== */
                case 2:  /* ==> PS2 <== */
                case 3:  /* ==> PS3 <== */
                case 4:  /* ==> PS4 <== */
                    {
                        switch (label)
                        {
                            case JCS_JoystickButton.BUTTON_A:
                            case JCS_JoystickButton.BUTTON_B:
                            case JCS_JoystickButton.BUTTON_X:
                            case JCS_JoystickButton.BUTTON_Y:

                            case JCS_JoystickButton.HOME_BUTTON:

                            case JCS_JoystickButton.START_BUTTON:
                            case JCS_JoystickButton.BACK_BUTTON:

                            case JCS_JoystickButton.LEFT_BUMPER:
                            case JCS_JoystickButton.RIGHT_BUMPER:
                                {
                                    return JCS_AxisType.KeyOrMouseButton;
                                }

                            case JCS_JoystickButton.BUTTON_UP:
                            case JCS_JoystickButton.BUTTON_DOWN:
                            case JCS_JoystickButton.BUTTON_LEFT:
                            case JCS_JoystickButton.BUTTON_RIGHT:
                                {
                                    return JCS_AxisType.JoystickAxis;
                                }

                            case JCS_JoystickButton.STICK_LEFT_X:
                            case JCS_JoystickButton.STICK_LEFT_Y:

                            case JCS_JoystickButton.STICK_RIGHT_X:
                            case JCS_JoystickButton.STICK_RIGHT_Y:
                                {
                                    return JCS_AxisType.JoystickAxis;
                                }

                            case JCS_JoystickButton.LEFT_TRIGGER:
                            case JCS_JoystickButton.RIGHT_TRIGGER:
                                {
                                    return JCS_AxisType.KeyOrMouseButton;
                                }
                        }
                    }
                    break;

                /* Microsoft XBox */
                case 5:  /* ==> XBox <== */
                case 6:  /* ==> XBox 360 <== */
                case 7:  /* ==> XBox One <== */
                    {
                        switch (label)
                        {
                            case JCS_JoystickButton.BUTTON_A:
                            case JCS_JoystickButton.BUTTON_B:
                            case JCS_JoystickButton.BUTTON_X:
                            case JCS_JoystickButton.BUTTON_Y:

                            case JCS_JoystickButton.HOME_BUTTON:

                            case JCS_JoystickButton.START_BUTTON:
                            case JCS_JoystickButton.BACK_BUTTON:

                            case JCS_JoystickButton.LEFT_BUMPER:
                            case JCS_JoystickButton.RIGHT_BUMPER:
                                {
                                    return JCS_AxisType.KeyOrMouseButton;
                                }

                            case JCS_JoystickButton.BUTTON_UP:
                            case JCS_JoystickButton.BUTTON_DOWN:
                            case JCS_JoystickButton.BUTTON_LEFT:
                            case JCS_JoystickButton.BUTTON_RIGHT:

                            case JCS_JoystickButton.STICK_LEFT_X:
                            case JCS_JoystickButton.STICK_LEFT_Y:

                            case JCS_JoystickButton.STICK_RIGHT_X:
                            case JCS_JoystickButton.STICK_RIGHT_Y:

                            case JCS_JoystickButton.LEFT_TRIGGER:
                            case JCS_JoystickButton.RIGHT_TRIGGER:
                                {
                                    return JCS_AxisType.JoystickAxis;
                                }
                        }
                    }
                    break;
            }

            

            /* Returns default. */
            return JCS_AxisType.KeyOrMouseButton;
        }
#endif

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected override void TransferData(JCS_InputSettings _old, JCS_InputSettings _new)
        {
            _new.Joysticks = _old.Joysticks;
        }

        /// <summary>
        /// Update the joystick info.
        /// </summary>
        private void GetJoystickInfo()
        {
            // check if any joystick connected.
            if (!JCS_Input.IsJoystickConnected())
                return;

            for (int index = 0; index < mTotalGamePadInGame; ++index)
            {
                JoystickMap joystickMap = GetJoysitckMapByIndex(index);

                // get stick value.
                joystickMap.stickLeftXVal = JCS_Input.GetAxis(index, JCS_JoystickButton.STICK_LEFT_X);
                joystickMap.stickLeftYVal = JCS_Input.GetAxis(index, JCS_JoystickButton.STICK_LEFT_Y);

                joystickMap.stickRightXVal = JCS_Input.GetAxis(index, JCS_JoystickButton.STICK_RIGHT_X);
                joystickMap.stickRightYVal = JCS_Input.GetAxis(index, JCS_JoystickButton.STICK_RIGHT_Y);
            }
        }

        /// <summary>
        /// Get Unity's controller naming convention. (PS4)
        /// </summary>
        /// <param name="label"> label we target to get the naming. </param>
        /// <returns> Naming convention by Unity's InputManager, for getting 
        /// the right device driver id. </returns>
        private static string GetPositiveNameByLabel_PS4(JCS_JoystickButton label)
        {
            switch (label)
            {
                /* PS4 => Circle */
                case JCS_JoystickButton.BUTTON_A: return "joystick button 2";
                /* PS4 => Square */
                case JCS_JoystickButton.BUTTON_B: return "joystick button 0";
                case JCS_JoystickButton.BUTTON_X: return "joystick button 1";
                /* PS4 => Triangle */
                case JCS_JoystickButton.BUTTON_Y: return "joystick button 3";

                /* PS4 => PS*/
                case JCS_JoystickButton.HOME_BUTTON: return "joystick button 12";

                /* PS4 => Options */
                case JCS_JoystickButton.START_BUTTON: return "joystick button 9";
                /* PS4 => Share */
                case JCS_JoystickButton.BACK_BUTTON: return "joystick button 8";

                /* PS4 => R1 */
                case JCS_JoystickButton.RIGHT_BUMPER: return "joystick button 5";
                /* PS4 => L1 */
                case JCS_JoystickButton.LEFT_BUMPER: return "joystick button 4";

                /* PS4 => R2 */
                case JCS_JoystickButton.RIGHT_TRIGGER: return "joystick button 7";
                /* PS4 => L2 */
                case JCS_JoystickButton.LEFT_TRIGGER: return "joystick button 6";
            }

            return "";
        }

        /// <summary>
        /// Get Unity's controller naming convention. (XBox 360)
        /// </summary>
        /// <param name="label"> label we target to get the naming. </param>
        /// <returns> Naming convention by Unity's InputManager, for getting 
        /// the right device driver id. </returns>
        private static string GetPositiveNameByLabel_XBox360(JCS_JoystickButton label)
        {
#if (UNITY_STANDALONE_WIN)
            switch (label)
            {
                case JCS_JoystickButton.BUTTON_A: return "joystick button 0";
                case JCS_JoystickButton.BUTTON_B: return "joystick button 1";
                case JCS_JoystickButton.BUTTON_X: return "joystick button 2";
                case JCS_JoystickButton.BUTTON_Y: return "joystick button 3";

                case JCS_JoystickButton.HOME_BUTTON: return "";

                case JCS_JoystickButton.START_BUTTON: return "joystick button 7";
                case JCS_JoystickButton.BACK_BUTTON: return "joystick button 6";

                case JCS_JoystickButton.RIGHT_BUMPER: return "joystick button 5";
                case JCS_JoystickButton.LEFT_BUMPER: return "joystick button 4";
            }
#elif (UNITY_STANDALONE_LINUX)
            switch (label)
            {
                case JCS_JoystickButton.BUTTON_A: return "joystick button 0";
                case JCS_JoystickButton.BUTTON_B: return "joystick button 1";
                case JCS_JoystickButton.BUTTON_X: return "joystick button 2";
                case JCS_JoystickButton.BUTTON_Y: return "joystick button 3";

                case JCS_JoystickButton.HOME_BUTTON: return "joystick button 15";

                case JCS_JoystickButton.START_BUTTON: return "joystick button 7";
                case JCS_JoystickButton.BACK_BUTTON: return "joystick button 6";

                case JCS_JoystickButton.RIGHT_BUMPER: return "joystick button 5";
                case JCS_JoystickButton.LEFT_BUMPER: return "joystick button 4";
            }
#elif (UNITY_STANDALONE_OSX)
            switch (label)
            {
                case JCS_JoystickButton.BUTTON_A: return "joystick button 16";
                case JCS_JoystickButton.BUTTON_B: return "joystick button 17";
                case JCS_JoystickButton.BUTTON_X: return "joystick button 18";
                case JCS_JoystickButton.BUTTON_Y: return "joystick button 19";

                case JCS_JoystickButton.HOME_BUTTON: return "joystick button 15";

                case JCS_JoystickButton.START_BUTTON: return "joystick button 9";
                case JCS_JoystickButton.BACK_BUTTON: return "joystick button 10";

                case JCS_JoystickButton.RIGHT_BUMPER: return "joystick button 14";
                case JCS_JoystickButton.LEFT_BUMPER: return "joystick button 13";
            }
#endif
            return "";
        }
    }
}
