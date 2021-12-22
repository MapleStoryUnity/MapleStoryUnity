/**
 * $File: JCS_Input.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections.Generic;

namespace JCSUnity
{
    /// <summary>
    /// Joystick enum design here...
    /// </summary>
    public enum JCS_JoystickButton
    {
        NONE,

        /* General Gamepad */
        HOME_BUTTON,    // KeyOrMouseButton  (PS)

        START_BUTTON,   // KeyOrMouseButton  (Options)
        BACK_BUTTON,    // KeyOrMouseButton  (Share)

        BUTTON_A,       // KeyOrMouseButton  (Circle)
        BUTTON_B,       // KeyOrMouseButton  (Square)
        BUTTON_X,       // KeyOrMouseButton
        BUTTON_Y,       // KeyOrMouseButton  (Triangle)

        LEFT_BUMPER,    // KeyOrMouseButton  (L1)
        RIGHT_BUMPER,   // KeyOrMouseButton  (R1)

        LEFT_TRIGGER,   // JoystickAxis  (L2)
        RIGHT_TRIGGER,  // JoystickAxis  (R2)

        BUTTON_UP,      // JoystickAxis
        BUTTON_DOWN,    // JoystickAxis
        BUTTON_LEFT,    // JoystickAxis
        BUTTON_RIGHT,   // JoystickAxis

        STICK_RIGHT_X,  // JoystickAxis
        STICK_RIGHT_Y,  // JoystickAxis

        STICK_LEFT_X,   // JoystickAxis
        STICK_LEFT_Y,   // JoystickAxis
    };

    /// <summary>
    /// Support joystick up to 30 people.
    /// </summary>
    public enum JCS_JoystickIndex
    {
        // Joystick 0 - 10 (1 ~ 11)
        FROM_ALL_JOYSTICK = 0x00,
        JOYSTICK_01 = 0x01,
        JOYSTICK_02 = 0x02,
        JOYSTICK_03 = 0x03,
        JOYSTICK_04 = 0x04,
        JOYSTICK_05 = 0x05,
        JOYSTICK_06 = 0x06,
        JOYSTICK_07 = 0x07,
        JOYSTICK_08 = 0x08,
        JOYSTICK_09 = 0x09,
        JOYSTICK_10 = 0x0A,
        JOYSTICK_11 = 0x0B,
    };

    /// <summary>
    /// Mouse button code definition.
    /// </summary>
    public enum JCS_MouseButton
    {
        NONE = -1,
        LEFT = 0,
        RIGHT = 1,
        MIDDLE = 2
    };

    public delegate void JoystickPlugged();  // Callback when joystick is plugged.
    public delegate void JoystickUnPlugged();  // Callback when joystick is unplugged.

    /// <summary>
    /// Unity's Input class re-wrapper.
    /// </summary>
    public class JCS_Input 
    {
        #region DOUBLE_CLICK
        // Boolean to indentify the first click.
        private static bool CLICK = false;
        // Interval time between the first click and the second click.
        private static float CLICK_TIME = 0.25f;
        // Timer to check with 'CLICK_TIME'.
        private static float CLICK_TIMER = 0.0f;
        #endregion

        #region DRAG
        private static bool START_DRAGGING = false;
        private static bool DRAGGING = false;
        private static Vector3 START_DRAG_POINT = Vector3.zero;
        #endregion

        #region JOYSTICK
        private static Dictionary<string, bool>
            mJoystickKeyPressed = new Dictionary<string, bool>();
        private static Dictionary<string, bool>
            mJoystickKeyReleased = new Dictionary<string, bool>();

        private static List<string> mJoystickKeyWasPreseed = new List<string>();
        private static List<string> mJoystickKeyWasReleased = new List<string>();

        public static JoystickPlugged joystickPluggedCallback = JoystickPluggedDefaultCallback;
        public static JoystickUnPlugged joystickUnPluggedCallback = JoystickUnPluggedDefaultCallback;

        // record down the if the joystick was connected.
        private static bool mIsJoystickConnected = IsJoystickConnected();
        #endregion

        /* Default callback function pointer. */
        private static void JoystickPluggedDefaultCallback() { JCS_Debug.Log("At least one joystick connected!!!"); }
        private static void JoystickUnPluggedDefaultCallback() { JCS_Debug.Log("No joystick connected..."); }

        /// <summary>
        /// Main loop for input.
        /// </summary>
        public static void LateUpdate()
        {
            #region DOUBLE_CLICK
            if (CLICK)
            {
                CLICK_TIMER += Time.deltaTime;

                if (CLICK_TIME < CLICK_TIMER)
                {
                    CLICK = false;
                    CLICK_TIMER = 0.0f;
                }
            }
            #endregion

            DoJoystickCallback();

            ClearJoystickKeymapBuffer();
        }

        /// <summary>
        /// Do one callback, usually for resetting.
        /// </summary>
        public static void InputCallbackOnce()
        {
            if (mIsJoystickConnected)
                joystickPluggedCallback.Invoke();
            else
                joystickUnPluggedCallback.Invoke();
        }

        /// <summary>
        /// Do the joystick callback.
        /// </summary>
        private static void DoJoystickCallback()
        {
            if (mIsJoystickConnected == IsJoystickConnected())
                return;

            mIsJoystickConnected = IsJoystickConnected();

            InputCallbackOnce();
        }

        /// <summary>
        /// Clear the joystick keymap buffer in order to implement
        /// joystick button down and joystick button up.
        /// </summary>
        private static void ClearJoystickKeymapBuffer()
        {
            for (int index = 0;
                index < mJoystickKeyWasPreseed.Count;
                ++index)
            {
                string idString = mJoystickKeyWasPreseed[index];
                mJoystickKeyPressed[idString] = true;
            }

            for (int index = 0;
                index < mJoystickKeyWasReleased.Count;
                ++index)
            {
                string idString = mJoystickKeyWasReleased[index];
                mJoystickKeyReleased[idString] = true;
            }

            // Clear all key action history.
            mJoystickKeyWasPreseed.Clear();
            mJoystickKeyWasReleased.Clear();
        }

        //----------------------------------------------------------------------

        /// <summary>
        /// Check if the user double clicking?
        /// </summary>
        /// <param name="type"> type by JCS_InputType (self-define) </param>
        /// <returns> true: if double click, false nothing happens </returns>
        public static bool OnMouseDoubleClick(JCS_MouseButton type, bool ignorePause = false)
        {
            return OnMouseDoubleClick((int)type, ignorePause);
        }
        /// <summary>
        /// Check if the user double clicking?
        /// </summary>
        /// <param name="button"> type by keycode (Unity built-in) </param>
        /// <returns> true: if double click, false nothing happens </returns>
#if UNITY_5_4_OR_NEWER  // NOTE: Resolve ambiguous issue.
        public
#else
        private
#endif 
        static bool OnMouseDoubleClick(int button, bool ignorePause = false)
        {
            // Check first click
            if (!CLICK)
            {
                if (GetMouseButtonDown(button, ignorePause))
                    CLICK = true;
            }
            // Check double click
            else
            {
                if (GetMouseButtonDown(button, ignorePause))
                {
                    CLICK = false;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Is the mouse dragging right now?
        /// </summary>
        /// <param name="button"></param>
        /// <param name="ignorePause"></param>
        /// <returns></returns>
        public static bool OnMouseDrag(int button, bool ignorePause = false)
        {
            return OnMouseDrag((JCS_MouseButton)button, ignorePause);
        }
        /// <summary>
        /// Is the mouse dragging right now?
        /// </summary>
        /// <param name="button"></param>
        /// <param name="ignorePause"></param>
        /// <returns></returns>
#if UNITY_5_4_OR_NEWER  // NOTE: Resolve ambiguous issue.
        public
#else
        private
#endif
        static bool OnMouseDrag(JCS_MouseButton button, bool ignorePause = false)
        {
            if (START_DRAGGING)
            {
                if (GetMouseButton(button, ignorePause))
                {
                    if (Input.mousePosition != START_DRAG_POINT)
                    {
                        // NOTE(jenchieh): this will prevent if the 
                        // player goes back to the starting point.
                        // 
                        // Once it start dragging, it will always be 
                        // dragging state even when the player make 
                        // the mouse position the same as 'START_DRAG_POINT' 
                        // will still count as dragging!
                        DRAGGING = true;
                    }
                }
                else
                {
                    START_DRAG_POINT = Vector3.zero;
                    START_DRAGGING = false;
                    DRAGGING = false;
                }
            }
            else
            {
                if (GetMouseButtonDown(button, ignorePause))
                {
                    // Record down the mouse position.
                    START_DRAG_POINT = Input.mousePosition;
                    START_DRAGGING = true;
                }
            }

            return DRAGGING;
        }


        // mouse delta position.
        private static Vector3 mMouseDeltaPosition = Vector3.zero;
        // record down last mouse position, in order to
        // get the updated delta mouse position. (variable above.)
        private static Vector3 mLasDeltaPosition = Vector3.zero;

        /// <summary>
        /// Return mouse delta position between frame
        /// to frame.
        /// </summary>
        /// <returns> delta value </returns>
        public static Vector3 MouseDeltaPosition()
        {
            Vector3 currentPosition = Input.mousePosition;
            mMouseDeltaPosition = currentPosition - mLasDeltaPosition;
            mLasDeltaPosition = currentPosition;

            return mMouseDeltaPosition;
        }

        /// <summary>
        /// Get the mouse position on canvas space.
        /// </summary>
        /// <returns> mouse position on canvas </returns>
        public static Vector3 CanvasMousePosition()
        {
            Vector2 pos;
            Canvas myCanvas = JCS_Canvas.GuessCanvas().GetCanvas();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                myCanvas.transform as RectTransform, 
                Input.mousePosition, 
                myCanvas.worldCamera, 
                out pos);

            return myCanvas.transform.TransformPoint(pos);
        }

        /// <summary>
        /// Returns the mouse position on GUI layer.
        /// </summary>
        /// <returns></returns>
        public static Vector3 MousePositionOnGUILayer()
        {
            Vector3 guiMousePosition = Input.mousePosition;

            // original point is at the center of the screen,  
            // so set (0, 0) back to bottom left
            guiMousePosition.x -= JCS_Screen.width / 2;
            guiMousePosition.y -= JCS_Screen.height / 2;

            return guiMousePosition;
        }

        /// <summary>
        /// Return mouse position between 0 to 1.
        /// </summary>
        /// <returns> values </returns>
        public static Vector2 MousePosition0To1()
        {
            float mouseRatioX = Input.mousePosition.x / JCS_Screen.width;
            float mouseRatioY = Input.mousePosition.y / JCS_Screen.height;

            return new Vector2(mouseRatioX, mouseRatioY);
        }

        /// <summary>
        /// Get the mouse state by passing it the action.
        /// </summary>
        /// <param name="act"> key action type. </param>
        /// <param name="button"> button type. </param>
        /// <returns>
        /// true, the mouse state is correct.
        /// false, the mouse state is incorrect.
        /// </returns>
#if UNITY_5_4_OR_NEWER  // NOTE: Resolve ambiguous issue.
        public
#else
        private
#endif
        static bool GetMouseByAction(JCS_KeyActionType act, JCS_MouseButton button, bool ignorePause = false)
        {
            return GetMouseByAction(act, (int)button, ignorePause);
        }
        /// <summary>
        /// Get the mouse state by passing it the action.
        /// </summary>
        /// <param name="act"> key action type. </param>
        /// <param name="button"> button number. </param>
        /// <returns>
        /// true, the mouse state is correct.
        /// false, the mouse state is incorrect.
        /// </returns>
        public static bool GetMouseByAction(JCS_KeyActionType act, int button, bool ignorePause = false)
        {
            switch (act)
            {
                case JCS_KeyActionType.KEY:
                    return GetMouseButton(button, ignorePause);
                case JCS_KeyActionType.KEY_DOWN:
                    return GetMouseButtonDown(button, ignorePause);
                case JCS_KeyActionType.KEY_UP:
                    return GetMouseButtonUp(button, ignorePause);
            }

            JCS_Debug.LogError("This cannot happed");
            return false;
        }
        /// <summary>
        /// Check if the mouse button is down.
        /// </summary>
        /// <param name="button"> button select. </param>
        /// <returns> 
        /// true, button is down.
        /// false, button is not down.
        /// </returns>
        public static bool GetMouseButtonDown(JCS_MouseButton button, bool ignorePause = false)
        {
            return GetMouseButtonDown((int)button, ignorePause);
        }
        /// <summary>
        /// Check if the mouse button is down.
        /// </summary>
        /// <param name="button"> button select. </param>
        /// <returns> 
        /// true, button is down.
        /// false, button is not down.
        /// </returns>
#if UNITY_5_4_OR_NEWER  // NOTE: Resolve ambiguous issue.
        public
#else
        private
#endif
        static bool GetMouseButtonDown(int button, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            if (button == (int)JCS_MouseButton.NONE)
                return false;

            return Input.GetMouseButtonDown(button);
        }
        /// <summary>
        /// Check if the mouse button is pressed.
        /// </summary>
        /// <param name="button"> button select. </param>
        /// <returns> 
        /// true, button is pressed.
        /// false, button is not pressed.
        /// </returns>
        public static bool GetMouseButton(JCS_MouseButton button, bool ignorePause = false)
        {
            return GetMouseButton((int)button, ignorePause);
        }
        /// <summary>
        /// Check if the mouse button is pressed.
        /// </summary>
        /// <param name="button"> button select. </param>
        /// <returns> 
        /// true, button is pressed.
        /// false, button is not pressed.
        /// </returns>
#if UNITY_5_4_OR_NEWER  // NOTE: Resolve ambiguous issue.
        public
#else
        private
#endif
        static bool GetMouseButton(int button, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            if (button == (int)JCS_MouseButton.NONE)
                return false;

            return Input.GetMouseButton(button);
        }
        /// <summary>
        /// Check if the mouse button is up.
        /// </summary>
        /// <param name="button"> button select. </param>
        /// <returns> 
        /// true, button is up.
        /// false, button is not up.
        /// </returns>
        public static bool GetMouseButtonUp(JCS_MouseButton button, bool ignorePause = false)
        {
            return GetMouseButtonUp((int)button, ignorePause);
        }
        /// <summary>
        /// Check if the mouse button is up.
        /// </summary>
        /// <param name="button"> button select. </param>
        /// <returns> 
        /// true, button is up.
        /// false, button is not up.
        /// </returns>
#if UNITY_5_4_OR_NEWER  // NOTE: Resolve ambiguous issue.
        public
#else
        private
#endif
        static bool GetMouseButtonUp(int button, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            if (button == (int)JCS_MouseButton.NONE)
                return false;

            return Input.GetMouseButtonUp(button);
        }

        /// <summary>
        /// Check if the key is down by key action type.
        /// </summary>
        /// <param name="act"> action type. </param>
        /// <param name="key"> key code. </param>
        /// <returns>
        /// true, button with current key action is active.
        /// false, button with current key action is not active.
        /// </returns>
        public static bool GetKeyByAction(JCS_KeyActionType act, KeyCode key, bool ignorePause = false)
        {
            switch (act)
            {
                case JCS_KeyActionType.KEY:
                    return GetKey(key, ignorePause);
                case JCS_KeyActionType.KEY_DOWN:
                    return GetKeyDown(key, ignorePause);
                case JCS_KeyActionType.KEY_UP:
                    return GetKeyUp(key, ignorePause);
            }

            JCS_Debug.LogError("This cannot happed, because all states applied.");
            return false;
        }
        /// <summary>
        /// Is the key down?
        /// </summary>
        /// <param name="key"> key to check if the key is down. </param>
        /// <returns>
        /// true: key is down, false: vice versa.
        /// </returns>
        public static bool GetKeyDown(KeyCode key, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return Input.GetKeyDown(key);
        }
        /// <summary>
        /// Is the key held down?
        /// </summary>
        /// <param name="key"> key to check if the key is held down. </param>
        /// <returns>
        /// true: key is held down, false: vice versa.
        /// </returns>
        public static bool GetKey(KeyCode key, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return Input.GetKey(key);
        }
        /// <summary>
        /// Is the key up?
        /// </summary>
        /// <param name="key"> key to check if is key up. </param>
        /// <returns> true: is key up, false: vice versa. </returns>
        public static bool GetKeyUp(KeyCode key, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return Input.GetKeyUp(key);
        }

        /// <summary>
        /// Get the boolean check depends on the button status.
        /// 
        /// Button status are list here:
        /// 1) Button Down
        /// 2) Button Hold
        /// 3) Button Up
        /// </summary>
        /// <param name="act"> button down, button up, or button pressed. </param>
        /// <param name="buttonName"> name of the button. </param>
        /// <returns> 
        /// Is either pressed, down, up. or not pressed, down, up. 
        /// </returns>
        public static bool GetButtonByAction(JCS_KeyActionType act, string buttonName, bool ignorePause = false)
        {
            switch (act)
            {
                case JCS_KeyActionType.KEY:
                    return GetButton(buttonName, ignorePause);
                case JCS_KeyActionType.KEY_DOWN:
                    return GetButtonDown(buttonName, ignorePause);
                case JCS_KeyActionType.KEY_UP:
                    return GetButtonUp(buttonName, ignorePause);
            }


            JCS_Debug.LogError("This cannot happed.");

            // this cannot happens
            return false;
        }
        /// <summary>
        /// Is the button pressed?
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButton(string buttonName, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return Input.GetButton(buttonName);
        }
        /// <summary>
        /// Is the button down?
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButtonDown(string buttonName, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return Input.GetButtonDown(buttonName);
        }
        /// <summary>
        /// Is the button up?
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButtonUp(string buttonName, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return Input.GetButtonUp(buttonName);
        }

        /// <summary>
        /// Loop through the key and key what key is pressed.
        /// </summary>
        /// <param name="type"> type of the key pressed option. </param>
        /// <returns> key u pressed. </returns>
        public static KeyCode GetAnyKeyByAction(JCS_KeyActionType type, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return KeyCode.None;
            }

            // loop through the key code list
            foreach (KeyCode val in JCS_Util.GetValues<KeyCode>())
            {
                // if the key is pressed, return it.
                if (GetKeyByAction(type, val))
                {
                    return val;
                }
            }

            // default return.
            return KeyCode.None;
        }

        /// <summary>
        /// Check if any key is sent the input signal.
        /// </summary>
        /// <param name="type"> action u want to check. </param>
        /// <returns>
        /// true: key buffer in
        /// false: no key buffer in
        /// </returns>
        public static bool IsAnyKeyBuffer(JCS_KeyActionType type, bool ignorePause = false)
        {
            KeyCode code = GetAnyKeyByAction(type, ignorePause);

            return (code != KeyCode.None);
        }

        /// <summary>
        /// Check if any key is down?
        /// </summary>
        /// <returns>
        /// true: somewhere in the key is down.
        /// false: no key is down.
        /// </returns>
        public static bool GetAnyKeyDown(bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return IsAnyKeyBuffer(JCS_KeyActionType.KEY_DOWN);
        }

        /// <summary>
        /// Check if any key is pressed.
        /// </summary>
        /// <returns>
        /// true: somewhere the key is pressed.
        /// false: no key is pressed.
        /// </returns>
        public static bool GetAnyKey(bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return IsAnyKeyBuffer(JCS_KeyActionType.KEY);
        }

        /// <summary>
        /// Check if any key is up?
        /// </summary>
        /// <returns>
        /// true: somewhere in the key is up.
        /// false: no key is up.
        /// </returns>
        public static bool GetAnyKeyUp(bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return IsAnyKeyBuffer(JCS_KeyActionType.KEY_UP);
        }


        /// <summary>
        /// Return the joystick buffer.
        /// </summary>
        /// <param name="joystickIndex"></param>
        /// <param name="btn"></param>
        /// <returns></returns>
        public static float GetAxis(JCS_JoystickIndex joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            return GetAxis((int)joystickIndex, btn, ignorePause);
        }
        /// <summary>
        /// Return the joystick buffer.
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        public static float GetAxis(int joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            if (btn == JCS_JoystickButton.NONE)
                return 0;

            string idString = JCS_InputSettings.GetJoystickButtonIdName(joystickIndex, btn);

            return GetAxis(idString, ignorePause);
        }
        /// <summary>
        /// Return the joystick buffer.
        /// </summary>
        /// <param name="name"> name of the joystick name. </param>
        /// <returns> value the joystick tilt. </returns>
        public static float GetAxis(string name, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return 0;
            }

            if (name == "")
            {
                JCS_Debug.LogError("InputManager's name variable cannot be empty string...");
                return 0;
            }

            return Input.GetAxis(name);
        }

        /// <summary>
        /// Check if the button have pressed.
        /// </summary>
        /// <param name="joystickIndex"></param>
        /// <param name="btn"></param>
        /// <returns> buffer pressure from hardware. </returns>
        public static bool GetJoystickButton(JCS_JoystickIndex joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            return GetJoystickButton((int)joystickIndex, btn, ignorePause);
        }
        /// <summary>
        /// Check if the button have pressed.
        /// </summary>
        /// <returns></returns>
        public static bool GetJoystickButton(int joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            // check if any joystick connected.
            if (!IsJoystickConnected())
                return false;

            return GetAxis(joystickIndex, btn, ignorePause) > 0;
        }

        /// <summary>
        /// Check if the button have pressed.
        /// </summary>
        /// <param name="idString"> string id </param>
        /// <returns></returns>
        public static bool GetJoystickButton(string idString, bool ignorePause = false)
        {
            return GetAxis(idString, ignorePause) > 0;
        }

        /// <summary>
        /// Check if joystick id, pressed the key?
        /// </summary>
        /// <param name="joystickIndex"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did pressed.
        /// false: not pressed.
        /// </returns>
        public static bool GetJoystickKey(JCS_JoystickIndex joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            return GetJoystickKey((int)joystickIndex, btn, ignorePause);
        }

        /// <summary>
        /// Check if joystick id, pressed the key?
        /// </summary>
        /// <param name="joystickIndex"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did pressed.
        /// false: not pressed.
        /// </returns>
        public static bool GetJoystickKey(int joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            return GetJoystickButton(joystickIndex, btn, ignorePause);
        }


        /// <summary>
        /// Check if joystick id, up the key?
        /// </summary>
        /// <param name="joystickIndex"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did uo.
        /// false: not up.
        /// </returns>
        public static bool GetJoystickKeyUp(int joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            return GetJoystickKeyUp((JCS_JoystickIndex)joystickIndex, btn, ignorePause);
        }

        /// <summary>
        /// Check if joystick id, up the key?
        /// </summary>
        /// <param name="joystickIndex"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did uo.
        /// false: not up.
        /// </returns>
        public static bool GetJoystickKeyUp(JCS_JoystickIndex joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            string idString = JCS_InputSettings.GetJoystickButtonIdName(joystickIndex, btn);

            if (GetJoystickKey(joystickIndex, btn, ignorePause))
            {
                if (mJoystickKeyReleased.ContainsKey(idString))
                    mJoystickKeyReleased[idString] = false;
            }
            else
            {
                if (!mJoystickKeyReleased.ContainsKey(idString))
                {
                    mJoystickKeyReleased.Add(idString, true);
                }
                else
                {
                    if (mJoystickKeyReleased[idString])
                        return false;
                    else
                    {
                        if (!mJoystickKeyWasReleased.Contains(idString))
                            mJoystickKeyWasReleased.Add(idString);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Check if joystick id, down the key?
        /// </summary>
        /// <param name="joystickIndex"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did uo.
        /// false: not up.
        /// </returns>
        public static bool GetJoystickKeyDown(int joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            return GetJoystickKeyDown((JCS_JoystickIndex)joystickIndex, btn, ignorePause);
        }

        /// <summary>
        /// Check if joystick id, down the key?
        /// </summary>
        /// <param name="joystickIndex"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did uo.
        /// false: not up.
        /// </returns>
        public static bool GetJoystickKeyDown(JCS_JoystickIndex joystickIndex, JCS_JoystickButton btn, bool ignorePause = false)
        {
            if (!ignorePause)
            {
                if (JCS_GameManager.instance.GAME_PAUSE)
                    return false;
            }

            string idString = JCS_InputSettings.GetJoystickButtonIdName(joystickIndex, btn);

            if (GetJoystickKey(joystickIndex, btn, ignorePause))
            {
                if (!mJoystickKeyPressed.ContainsKey(idString))
                {
                    mJoystickKeyPressed.Add(idString, false);
                }
                // Key contains!
                else
                {
                    if (mJoystickKeyPressed[idString])
                        return false;
                    else
                    {
                        if (!mJoystickKeyWasPreseed.Contains(idString))
                            mJoystickKeyWasPreseed.Add(idString);
                        return true;
                    }
                }

            }
            else
            {
                if (mJoystickKeyPressed.ContainsKey(idString))
                    mJoystickKeyPressed[idString] = false;
            }

            return false;
        }

        /// <summary>
        /// Check if the joystick key is doing something by these
        /// certain events.
        ///   -> pressed
        ///   -> key down.
        ///   -> key up.
        /// </summary>
        /// <param name="act"></param>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <param name="ignorePause"></param>
        /// <returns></returns>
        public static bool GetJoystickKeyByAction(
            JCS_KeyActionType act, 
            JCS_JoystickIndex id,
            JCS_JoystickButton key, 
            bool ignorePause = false)
        {
            switch (act)
            {
                case JCS_KeyActionType.KEY:
                    return GetJoystickKey(id, key, ignorePause);
                case JCS_KeyActionType.KEY_DOWN:
                    return GetJoystickKeyDown(id, key, ignorePause);
                case JCS_KeyActionType.KEY_UP:
                    return GetJoystickKeyUp(id, key, ignorePause);
            }

            JCS_Debug.LogError("This cannot happens...");
            return false;
        }

        /// <summary>
        /// Check if there is joy stick connect to 
        /// the pc.
        /// 
        /// SOURCE(jenchieh): http://answers.unity3d.com/questions/218477/how-can-i-detect-if-a-gamepad-is-present.html
        /// </summary>
        /// <returns>
        /// true: is connected
        /// false: nothing is detected.
        /// </returns>
        public static bool IsJoystickConnected()
        {
            return (!JCS_Util.IsArrayEmpty(Input.GetJoystickNames()));
        }

        /// <summary>
        /// Check if one specific joystick is connected.
        /// </summary>
        /// <param name="name"> name of the joystick. </param>
        /// <returns>
        /// true, 'name' joystick name is connected.
        /// false, 'name' joystick name is not connected.
        /// </returns>
        public static bool IsJoystickConnected(string name)
        {
            string[] joystickNames = Input.GetJoystickNames();

            for (int index = 0; index < joystickNames.Length; ++index)
            {
                string joystickName = joystickNames[index];
                if (name == joystickName)
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Check either of these key are preseed.
        /// 
        /// If one of the key is pressed, returns true.
        /// else returns false.
        /// </summary>
        /// <param name="keys"> key code array. </param>
        /// <param name="ignorePause"> ignore the pause check? </param>
        /// <returns> 
        /// true, one of the key in the array list is pressed.
        /// false, none of these keys are pressed.
        /// </returns>
        public static bool OneKeys(
            List<KeyCode> keys, 
            bool ignorePause = false)
        {
            return OneKeys(keys.ToArray(), ignorePause);
        }
        public static bool OneKeys(
            KeyCode[] keys, 
            bool ignorePause = false)
        {
            for (int index = 0; index < keys.Length; ++index)
            {
                if (GetKey(keys[index], ignorePause))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Check either of these key are down.
        /// 
        /// If one of the key is down, returns true.
        /// else returns false.
        /// </summary>
        /// <param name="keys"> key code array. </param>
        /// <param name="ignorePause"> ignore the pause check? </param>
        /// <returns> 
        /// true, one of the key in the array list is down.
        /// false, none of these keys are down.
        /// </returns>
        public static bool OneKeysDown(
            List<KeyCode> keys, 
            bool ignorePause = false)
        {
            return OneKeysDown(keys.ToArray(), ignorePause);
        }
        public static bool OneKeysDown(
            KeyCode[] keys, 
            bool ignorePause = false)
        {
            for (int index = 0; index < keys.Length; ++index)
            {
                if (GetKeyDown(keys[index], ignorePause))
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Check either of these key are up.
        /// 
        /// If one of the key is up, returns true.
        /// else returns false.
        /// </summary>
        /// <param name="keys"> key code array. </param>
        /// <param name="ignorePause"> ignore the pause check? </param>
        /// <returns> 
        /// true, one of the key in the array list is up.
        /// false, none of these keys are up.
        /// </returns>
        public static bool OneKeysUp(
            List<KeyCode> keys, 
            bool ignorePause = false)
        {
            return OneKeysUp(keys.ToArray(), ignorePause);
        }
        public static bool OneKeysUp(
            KeyCode[] keys, 
            bool ignorePause = false)
        {
            for (int index = 0; index < keys.Length; ++index)
            {
                if (GetKeyUp(keys[index], ignorePause))
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Check all of these key are preseed.
        /// 
        /// If all of the keys are pressed, returns true.
        /// else returns false.
        /// </summary>
        /// <param name="keys"> key code array. </param>
        /// <param name="ignorePause"> ignore the pause check? </param>
        /// <returns> 
        /// true, all of the keys in the array list are pressed.
        /// false, none of these keys are pressed.
        /// </returns>
        public static bool AllKeys(
            List<KeyCode> keys,
            bool ignorePause = false)
        {
            return AllKeys(keys.ToArray(), ignorePause);
        }
        public static bool AllKeys(
            KeyCode[] keys, 
            bool ignorePause = false)
        {
            for (int index = 0; index < keys.Length; ++index)
            {
                if (!GetKey(keys[index], ignorePause))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check all of these key are down.
        /// 
        /// If all of the keys are down, returns true.
        /// else returns false.
        /// </summary>
        /// <param name="keys"> key code array. </param>
        /// <param name="ignorePause"> ignore the pause check? </param>
        /// <returns> 
        /// true, all of the keys in the array list are down.
        /// false, none of these keys are down.
        /// </returns>
        public static bool AllKeysDown(
            List<KeyCode> keys,
            bool ignorePause = false)
        {
            return AllKeysDown(keys.ToArray(), ignorePause);
        }
        public static bool AllKeysDown(
            KeyCode[] keys,
            bool ignorePause = false)
        {
            for (int index = 0; index < keys.Length; ++index)
            {
                if (!GetKeyDown(keys[index], ignorePause))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check all of these key are up.
        /// 
        /// If all of the keys are up, returns true.
        /// else returns false.
        /// </summary>
        /// <param name="keys"> key code array. </param>
        /// <param name="ignorePause"> ignore the pause check? </param>
        /// <returns> 
        /// true, all of the keys in the array list are up.
        /// false, none of these keys are up.
        /// </returns>
        public static bool AllKeysUp(
            List<KeyCode> keys,
            bool ignorePause = false)
        {
            return AllKeysUp(keys.ToArray(), ignorePause);
        }
        public static bool AllKeysUp(
            KeyCode[] keys,
            bool ignorePause = false)
        {
            for (int index = 0; index < keys.Length; ++index)
            {
                if (!GetKeyUp(keys[index], ignorePause))
                    return false;
            }

            return true;
        }


        /// <summary>
        /// Check either of these key are preseed.
        /// 
        /// If one of the key is pressed, returns true.
        /// else returns false.
        /// </summary>
        /// <param name="keys"> key code array. </param>
        /// <param name="ignorePause"> ignore the pause check? </param>
        /// <returns> 
        /// true, one of the key in the array list is pressed.
        /// false, none of these keys are pressed.
        /// </returns>
        public static bool OneJoystickButtons(
            int joystickIndex,
            List<JCS_JoystickButton> keys,
            bool ignorePause = false)
        {
            return OneJoystickButtons((JCS_JoystickIndex)joystickIndex, keys.ToArray(), ignorePause);
        }
        public static bool OneJoystickButtons(
            JCS_JoystickIndex joystickIndex, 
            List<JCS_JoystickButton> keys,
            bool ignorePause = false)
        {
            return OneJoystickButtons(joystickIndex, keys.ToArray(), ignorePause);
        }
        public static bool OneJoystickButtons(
            int joystickIndex,
            JCS_JoystickButton[] keys,
            bool ignorePause = false)
        {
            return OneJoystickButtons((JCS_JoystickIndex)joystickIndex, keys, ignorePause);
        }
        public static bool OneJoystickButtons(
            JCS_JoystickIndex joystickIndex, 
            JCS_JoystickButton[] keys,
            bool ignorePause = false)
        {
            for (int index = 0; index < keys.Length; ++index)
            {
                if (GetJoystickButton(joystickIndex, keys[index], ignorePause))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Check either of these key are down.
        /// 
        /// If one of the key is down, returns true.
        /// else returns false.
        /// </summary>
        /// <param name="keys"> key code array. </param>
        /// <param name="ignorePause"> ignore the pause check? </param>
        /// <returns> 
        /// true, one of the key in the array list is down.
        /// false, none of these keys are down.
        /// </returns>
        public static bool OneJoystickKeysDown(
            int joystickIndex,
            List<JCS_JoystickButton> keys,
            bool ignorePause = false)
        {
            return OneJoystickKeysDown((JCS_JoystickIndex)joystickIndex, keys.ToArray(), ignorePause);
        }
        public static bool OneJoystickKeysDown(
            JCS_JoystickIndex joystickIndex,
            List<JCS_JoystickButton> keys,
            bool ignorePause = false)
        {
            return OneJoystickKeysDown(joystickIndex, keys.ToArray(), ignorePause);
        }
        public static bool OneJoystickKeysDown(
            int joystickIndex,
            JCS_JoystickButton[] keys,
            bool ignorePause = false)
        {
            return OneJoystickKeysDown((JCS_JoystickIndex)joystickIndex, keys, ignorePause);
        }
        public static bool OneJoystickKeysDown(
            JCS_JoystickIndex joystickIndex,
            JCS_JoystickButton[] keys,
            bool ignorePause = false)
        {
            for (int index = 0; index < keys.Length; ++index)
            {
                if (GetJoystickKeyDown(joystickIndex, keys[index], ignorePause))
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Check either of these key are up.
        /// 
        /// If one of the key is up, returns true.
        /// else returns false.
        /// </summary>
        /// <param name="keys"> key code array. </param>
        /// <param name="ignorePause"> ignore the pause check? </param>
        /// <returns> 
        /// true, one of the key in the array list is up.
        /// false, none of these keys are up.
        /// </returns>
        public static bool OneJoystickKeysUp(
            int joystickIndex,
            List<JCS_JoystickButton> keys,
            bool ignorePause = false)
        {
            return OneJoystickKeysUp((JCS_JoystickIndex)joystickIndex, keys.ToArray(), ignorePause);
        }
        public static bool OneJoystickKeysUp(
            JCS_JoystickIndex joystickIndex,
            List<JCS_JoystickButton> keys,
            bool ignorePause = false)
        {
            return OneJoystickKeysUp(joystickIndex, keys.ToArray(), ignorePause);
        }
        public static bool OneJoystickKeysUp(
            int joystickIndex,
            JCS_JoystickButton[] keys,
            bool ignorePause = false)
        {
            return OneJoystickKeysUp((JCS_JoystickIndex)joystickIndex, keys, ignorePause);
        }
        public static bool OneJoystickKeysUp(
            JCS_JoystickIndex joystickIndex,
            JCS_JoystickButton[] keys,
            bool ignorePause = false)
        {
            for (int index = 0; index < keys.Length; ++index)
            {
                if (GetJoystickKeyUp(joystickIndex, keys[index], ignorePause))
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Check all of these key are preseed.
        /// 
        /// If all of the keys are pressed, returns true.
        /// else returns false.
        /// </summary>
        /// <param name="keys"> key code array. </param>
        /// <param name="ignorePause"> ignore the pause check? </param>
        /// <returns> 
        /// true, all of the keys in the array list are pressed.
        /// false, none of these keys are pressed.
        /// </returns>
        public static bool AllJoystickButtons(
            int joystickIndex,
            List<JCS_JoystickButton> keys,
            bool ignorePause = false)
        {
            return AllJoystickButtons((JCS_JoystickIndex)joystickIndex, keys.ToArray(), ignorePause);
        }
        public static bool AllJoystickButtons(
            JCS_JoystickIndex joystickIndex,
            List<JCS_JoystickButton> keys,
            bool ignorePause = false)
        {
            return AllJoystickButtons(joystickIndex, keys.ToArray(), ignorePause);
        }
        public static bool AllJoystickButtons(
            int joystickIndex,
            JCS_JoystickButton[] keys,
            bool ignorePause = false)
        {
            return AllJoystickButtons((JCS_JoystickIndex)joystickIndex, keys, ignorePause);
        }
        public static bool AllJoystickButtons(
            JCS_JoystickIndex joystickIndex,
            JCS_JoystickButton[] keys,
            bool ignorePause = false)
        {
            for (int index = 0; index < keys.Length; ++index)
            {
                if (!GetJoystickButton(joystickIndex, keys[index], ignorePause))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check all of these key are down.
        /// 
        /// If all of the keys are down, returns true.
        /// else returns false.
        /// </summary>
        /// <param name="keys"> key code array. </param>
        /// <param name="ignorePause"> ignore the pause check? </param>
        /// <returns> 
        /// true, all of the keys in the array list are down.
        /// false, none of these keys are down.
        /// </returns>
        public static bool AllJoystickKeysDown(
            int joystickIndex,
            List<JCS_JoystickButton> keys,
            bool ignorePause = false)
        {
            return AllJoystickKeysDown((JCS_JoystickIndex)joystickIndex, keys.ToArray(), ignorePause);
        }
        public static bool AllJoystickKeysDown(
            JCS_JoystickIndex joystickIndex,
            List<JCS_JoystickButton> keys,
            bool ignorePause = false)
        {
            return AllJoystickKeysDown(joystickIndex, keys.ToArray(), ignorePause);
        }
        public static bool AllJoystickKeysDown(
            int joystickIndex,
            JCS_JoystickButton[] keys,
            bool ignorePause = false)
        {
            return AllJoystickKeysDown((JCS_JoystickIndex)joystickIndex, keys, ignorePause);
        }
        public static bool AllJoystickKeysDown(
            JCS_JoystickIndex joystickIndex,
            JCS_JoystickButton[] keys,
            bool ignorePause = false)
        {
            for (int index = 0; index < keys.Length; ++index)
            {
                if (!GetJoystickKeyDown(joystickIndex, keys[index], ignorePause))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check all of these key are up.
        /// 
        /// If all of the keys are up, returns true.
        /// else returns false.
        /// </summary>
        /// <param name="keys"> key code array. </param>
        /// <param name="ignorePause"> ignore the pause check? </param>
        /// <returns> 
        /// true, all of the keys in the array list are up.
        /// false, none of these keys are up.
        /// </returns>
        public static bool AllJoystickKeysUp(
            int joystickIndex,
            List<JCS_JoystickButton> keys,
            bool ignorePause = false)
        {
            return AllJoystickKeysUp((JCS_JoystickIndex)joystickIndex, keys.ToArray(), ignorePause);
        }
        public static bool AllJoystickKeysUp(
            JCS_JoystickIndex joystickIndex,
            List<JCS_JoystickButton> keys,
            bool ignorePause = false)
        {
            return AllJoystickKeysUp(joystickIndex, keys.ToArray(), ignorePause);
        }
        public static bool AllJoystickKeysUp(
            int joystickIndex,
            JCS_JoystickButton[] keys,
            bool ignorePause = false)
        {
            return AllJoystickKeysUp((JCS_JoystickIndex)joystickIndex, keys, ignorePause);
        }
        public static bool AllJoystickKeysUp(
            JCS_JoystickIndex joystickIndex,
            JCS_JoystickButton[] keys,
            bool ignorePause = false)
        {
            for (int index = 0; index < keys.Length; ++index)
            {
                if (!GetJoystickKeyUp(joystickIndex, keys[index], ignorePause))
                    return false;
            }

            return true;
        }


#region CTRL

        /// <summary>
        /// Is one the ctrl key pressed?
        /// </summary>
        /// <returns>
        /// One the ctrl key is pressed.
        /// </returns>
        public static bool OneCtrlKey()
        {
            return (GetKey(KeyCode.LeftControl) || GetKey(KeyCode.RightControl));
        }

        /// <summary>
        /// Is one the ctrl key down?
        /// </summary>
        /// <returns>
        /// One the ctrl key is down.
        /// </returns>
        public static bool OneCtrlKeyDown()
        {
            return (GetKeyDown(KeyCode.LeftControl) || GetKeyDown(KeyCode.RightControl));
        }

        /// <summary>
        /// Is one the ctrl key up?
        /// </summary>
        /// <returns>
        /// One the ctrl key is up.
        /// </returns>
        public static bool OneCtrlKeyUp()
        {
            return (GetKeyUp(KeyCode.LeftControl) || GetKeyUp(KeyCode.RightControl));
        }

        /// <summary>
        /// Is key pressed with the control key?
        /// </summary>
        /// <param name="key"> key code. </param>
        /// <returns>
        /// 
        /// </returns>
        public static bool GetKeyWithCtrl(KeyCode key)
        {
            if (!OneCtrlKey())
                return false;

            return GetKey(key);
        }

        /// <summary>
        /// Is key down with the control key?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKeyDownWithCtrl(KeyCode key)
        {
            if (!OneCtrlKey())
                return false;

            return GetKeyDown(key);
        }

        /// <summary>
        /// Is key up with the control key?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKeyUpWithCtrl(KeyCode key)
        {
            if (!OneCtrlKey())
                return false;

            return GetKeyUp(key);
        }
#endregion

#region ALT

        /// <summary>
        /// Is one the alt key pressed?
        /// </summary>
        /// <returns>
        /// One the ctrl key is pressed.
        /// </returns>
        public static bool OneAltKey()
        {
            return (GetKey(KeyCode.LeftAlt) || GetKey(KeyCode.RightAlt));
        }

        /// <summary>
        /// Is one the alt key down?
        /// </summary>
        /// <returns>
        /// One the ctrl key is down.
        /// </returns>
        public static bool OneAltKeyDown()
        {
            return (GetKeyDown(KeyCode.LeftAlt) || GetKeyDown(KeyCode.RightAlt));
        }

        /// <summary>
        /// Is one the alt key up?
        /// </summary>
        /// <returns>
        /// One the ctrl key is up.
        /// </returns>
        public static bool OneAltKeyUp()
        {
            return (GetKeyUp(KeyCode.LeftAlt) || GetKeyUp(KeyCode.RightAlt));
        }

        /// <summary>
        /// Is key pressed with the alt key?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKeyWithAlt(KeyCode key)
        {
            if (!OneAltKey())
                return false;

            return GetKey(key);
        }

        /// <summary>
        /// Is key down with the alt key?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKeyDownWithAlt(KeyCode key)
        {
            if (!OneAltKey())
                return false;

            return GetKeyDown(key);
        }

        /// <summary>
        /// Is key up with the alt key?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKeyUpWithAlt(KeyCode key)
        {
            if (!OneAltKey())
                return false;

            return GetKeyUp(key);
        }
#endregion

#region SHIFT

        /// <summary>
        /// Is one the shift key pressed?
        /// </summary>
        /// <returns>
        /// One the ctrl key is pressed.
        /// </returns>
        public static bool OneShiftKey()
        {
            return (GetKey(KeyCode.LeftShift) || GetKey(KeyCode.RightShift));
        }

        /// <summary>
        /// Is one the shift key down?
        /// </summary>
        /// <returns>
        /// One the ctrl key is down.
        /// </returns>
        public static bool OneShiftKeyDown()
        {
            return (GetKeyDown(KeyCode.LeftShift) || GetKeyDown(KeyCode.RightShift));
        }

        /// <summary>
        /// Is one the shift key up?
        /// </summary>
        /// <returns>
        /// One the ctrl key is up.
        /// </returns>
        public static bool OneShiftKeyUp()
        {
            return (GetKeyUp(KeyCode.LeftShift) || GetKeyUp(KeyCode.RightShift));
        }

        /// <summary>
        /// Is key pressed with the shift key?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKeyWithShift(KeyCode key)
        {
            if (!OneShiftKey())
                return false;

            return GetKey(key);
        }

        /// <summary>
        /// Is key down with the shift key?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKeyDownWithShift(KeyCode key)
        {
            if (!OneShiftKey())
                return false;

            return GetKeyDown(key);
        }

        /// <summary>
        /// Is key up with the shift key?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetKeyUpWithShift(KeyCode key)
        {
            if (!OneShiftKey())
                return false;

            return GetKeyUp(key);
        }
#endregion

#region CTRL_SHIFT
        /// <summary>
        /// Check if the 'key' and the ctrl and shift key is pressed.
        /// </summary>
        /// <param name="key"> key to check if pressed together. </param>
        /// <returns>
        /// true, key, control and shift key is all pressed.
        /// false, either control, shift, or key is not pressed.
        /// </returns>
        public static bool GetKeyWithCtrlShift(KeyCode key)
        {
            if (!OneCtrlKey() || !OneShiftKey())
                return false;

            return GetKey(key);
        }

        /// <summary>
        /// Check if the 'key' and the ctrl and shift key is down.
        /// </summary>
        /// <param name="key"> key to check if down together. </param>
        /// <returns>
        /// true, key, control and shift key is all down.
        /// false, either control, shift, or key is not down.
        /// </returns>
        public static bool GetKeyDownWithCtrlShift(KeyCode key)
        {
            if (!OneCtrlKey() || !OneShiftKey())
                return false;

            return GetKeyDown(key);
        }

        /// <summary>
        /// Check if the 'key' and the ctrl and shift key is up.
        /// </summary>
        /// <param name="key"> key to check if down together. </param>
        /// <returns>
        /// true, key, control and shift key is all up.
        /// false, either control, shift, or key is not up.
        /// </returns>
        public static bool GetKeyUpWithCtrlShift(KeyCode key)
        {
            if (!OneCtrlKey() || !OneShiftKey())
                return false;

            return GetKeyUp(key);
        }
#endregion

#region ALT_CTRL
        /// <summary>
        /// Check if the 'key' and the ctrl and alt key is pressed.
        /// </summary>
        /// <param name="key"> key to check if pressed together. </param>
        /// <returns>
        /// true, key, control and alt key is all pressed.
        /// false, either control, alt, or key is not pressed.
        /// </returns>
        public static bool GetKeyWithAltCtrl(KeyCode key)
        {
            if (!OneAltKey() || !OneCtrlKey())
                return false;

            return GetKey(key);
        }

        /// <summary>
        /// Check if the 'key' and the ctrl and alt key is down.
        /// </summary>
        /// <param name="key"> key to check if down together. </param>
        /// <returns>
        /// true, key, control and alt key is all down.
        /// false, either control, alt, or key is not down.
        /// </returns>
        public static bool GetKeyDownWithAltCtrl(KeyCode key)
        {
            if (!OneAltKey() || !OneCtrlKey())
                return false;

            return GetKeyDown(key);
        }

        /// <summary>
        /// Check if the 'key' and the ctrl and alt key is up.
        /// </summary>
        /// <param name="key"> key to check if down together. </param>
        /// <returns>
        /// true, key, control and alt key is all up.
        /// false, either control, alt, or key is not up.
        /// </returns>
        public static bool GetKeyUpWithAltCtrl(KeyCode key)
        {
            if (!OneAltKey() || !OneCtrlKey())
                return false;

            return GetKeyUp(key);
        }
#endregion

#region ALT_SHIFT
        /// <summary>
        /// Check if the 'key' and the alt and shift key is pressed.
        /// </summary>
        /// <param name="key"> key to check if pressed together. </param>
        /// <returns>
        /// true, key, alt and shift key is all pressed.
        /// false, either alt, shift, or key is not pressed.
        /// </returns>
        public static bool GetKeyWithAltShift(KeyCode key)
        {
            if (!OneAltKey() || !OneShiftKey())
                return false;

            return GetKey(key);
        }

        /// <summary>
        /// Check if the 'key' and the alt and shift key is down.
        /// </summary>
        /// <param name="key"> key to check if down together. </param>
        /// <returns>
        /// true, key, alt and shift key is all down.
        /// false, either alt, shift, or key is not down.
        /// </returns>
        public static bool GetKeyDownWithAltShift(KeyCode key)
        {
            if (!OneAltKey() || !OneShiftKey())
                return false;

            return GetKeyDown(key);
        }

        /// <summary>
        /// Check if the 'key' and the alt and shift key is up.
        /// </summary>
        /// <param name="key"> key to check if down together. </param>
        /// <returns>
        /// true, key, alt and shift key is all up.
        /// false, either alt, shift, or key is not up.
        /// </returns>
        public static bool GetKeyUpWithAltShift(KeyCode key)
        {
            if (!OneAltKey() || !OneShiftKey())
                return false;

            return GetKeyUp(key);
        }
#endregion

#region ALT_CTRL_SHIFT

        /// <summary>
        /// Check if the 'key', alt, shift and ctrl key is pressed.
        /// </summary>
        /// <param name="key"> key to check if pressed together. </param>
        /// <returns>
        /// true, key, alt, shift and ctrl key is all pressed.
        /// false, either alt, shift, ctrl, or key is not pressed.
        /// </returns>
        public static bool GetKeyWithAltCtrlShift(KeyCode key)
        {
            if (!OneAltKey() || !OneShiftKey() || !OneCtrlKey())
                return false;

            return GetKey(key);
        }

        /// <summary>
        /// Check if the 'key', alt, shift and ctrl key is down.
        /// </summary>
        /// <param name="key"> key to check if down together. </param>
        /// <returns>
        /// true, key, alt, shift and ctrl key is all down.
        /// false, either alt, shift, ctrl, or key is not down.
        /// </returns>
        public static bool GetKeyDownWithAltCtrlShift(KeyCode key)
        {
            if (!OneAltKey() || !OneShiftKey() || !OneCtrlKey())
                return false;

            return GetKeyDown(key);
        }

        /// <summary>
        /// Check if the 'key', alt, shift and ctrl key is up.
        /// </summary>
        /// <param name="key"> key to check if down together. </param>
        /// <returns>
        /// true, key, alt, shift and ctrl key is all up.
        /// false, either alt, shift, ctrl, or key is not up.
        /// </returns>
        public static bool GetKeyUpWithAltCtrlShift(KeyCode key)
        {
            if (!OneAltKey() || !OneShiftKey() || !OneCtrlKey())
                return false;

            return GetKeyUp(key);
        }

#endregion

        /// <summary>
        /// Get key with certain combination.
        /// </summary>
        /// <param name="comb"> combination type. </param>
        /// <param name="key"> key code. </param>
        /// <returns>
        /// true, key with this combination is pressed.
        /// false, key with this combination is not pressed.
        /// </returns>
        public static bool GetKeyWith(JCS_KeyCombination comb, KeyCode key)
        {
            switch (comb)
            {
                case JCS_KeyCombination.ALT:
                    return GetKeyWithAlt(key);
                case JCS_KeyCombination.CTRL:
                    return GetKeyWithCtrl(key);
                case JCS_KeyCombination.SHIFT:
                    return GetKeyWithShift(key);
                case JCS_KeyCombination.ALT_CTRL:
                    return GetKeyWithAltCtrl(key);
                case JCS_KeyCombination.ALT_SHIFT:
                    return GetKeyWithAltShift(key);
                case JCS_KeyCombination.CTRL_SHIFT:
                    return GetKeyWithCtrlShift(key);
                case JCS_KeyCombination.ALT_CTRL_SHIFT:
                    return GetKeyWithAltCtrlShift(key);
            }

            // Just return normal get key.
            return GetKey(key);
        }

        /// <summary>
        /// Get key pressed with certain combination.
        /// </summary>
        /// <param name="with"> combination of key info. </param>
        /// <returns>
        /// true, key with this combination is pressed.
        /// false, key with this combination is not pressed.
        /// </returns>
        public static bool GetKeyWith(JCS_KeyWith with)
        {
            return GetKeyWith(with.comb, with.key);
        }

        /// <summary>
        /// Get key down with certain combination.
        /// </summary>
        /// <param name="comb"> combination type. </param>
        /// <param name="key"> key code. </param>
        /// <returns>
        /// true, key with this combination is down.
        /// false, key with this combination is not down.
        /// </returns>
        public static bool GetKeyDownWith(JCS_KeyCombination comb, KeyCode key)
        {
            switch (comb)
            {
                case JCS_KeyCombination.ALT:
                    return GetKeyDownWithAlt(key);
                case JCS_KeyCombination.CTRL:
                    return GetKeyDownWithCtrl(key);
                case JCS_KeyCombination.SHIFT:
                    return GetKeyDownWithShift(key);
                case JCS_KeyCombination.ALT_CTRL:
                    return GetKeyDownWithAltCtrl(key);
                case JCS_KeyCombination.ALT_SHIFT:
                    return GetKeyDownWithAltShift(key);
                case JCS_KeyCombination.CTRL_SHIFT:
                    return GetKeyDownWithCtrlShift(key);
                case JCS_KeyCombination.ALT_CTRL_SHIFT:
                    return GetKeyDownWithAltCtrlShift(key);
            }

            // Just return normal get key.
            return GetKeyDown(key);
        }

        /// <summary>
        /// Get key down with certain combination.
        /// </summary>
        /// <param name="with">combination of key info.</param>
        /// <returns>
        /// true, key with this combination is down.
        /// false, key with this combination is not down.
        /// </returns>
        public static bool GetKeyDownWith(JCS_KeyWith with)
        {
            return GetKeyDownWith(with.comb, with.key);
        }

        /// <summary>
        /// Get key up with certain combination.
        /// </summary>
        /// <param name="comb"> combination type. </param>
        /// <param name="key"> key code. </param>
        /// <returns>
        /// true, key with this combination is up.
        /// false, key with this combination is not up.
        /// </returns>
        public static bool GetKeyUpWith(JCS_KeyCombination comb, KeyCode key)
        {
            switch (comb)
            {
                case JCS_KeyCombination.ALT:
                    return GetKeyUpWithAlt(key);
                case JCS_KeyCombination.CTRL:
                    return GetKeyUpWithCtrl(key);
                case JCS_KeyCombination.SHIFT:
                    return GetKeyUpWithShift(key);
                case JCS_KeyCombination.ALT_CTRL:
                    return GetKeyUpWithAltCtrl(key);
                case JCS_KeyCombination.ALT_SHIFT:
                    return GetKeyUpWithAltShift(key);
                case JCS_KeyCombination.CTRL_SHIFT:
                    return GetKeyUpWithCtrlShift(key);
                case JCS_KeyCombination.ALT_CTRL_SHIFT:
                    return GetKeyUpWithAltCtrlShift(key);
            }

            // Just return normal get key.
            return GetKeyUp(key);
        }

        /// <summary>
        /// Get key up with certain combination.
        /// </summary>
        /// <param name="with">combination of key info.</param>
        /// <returns>
        /// true, key with this combination is up.
        /// false, key with this combination is not up.
        /// </returns>
        public static bool GetKeyUpWith(JCS_KeyWith with)
        {
            return GetKeyUpWith(with.comb, with.key);
        }

    }
}
