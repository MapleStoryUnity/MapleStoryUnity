/**
 * $File: JCS_Input.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using System.Collections.Generic;
using UnityEngine;

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
    public enum JCS_JoystickId
    {
        // Joystick 0 - 10 (1 ~ 11)
        ANY = 0x00,
        ID_01 = 0x01,
        ID_02 = 0x02,
        ID_03 = 0x03,
        ID_04 = 0x04,
        ID_05 = 0x05,
        ID_06 = 0x06,
        ID_07 = 0x07,
        ID_08 = 0x08,
        ID_09 = 0x09,
        ID_10 = 0x0A,
        ID_11 = 0x0B,
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

        private static Dictionary<string, bool> mJoystickKeyPressed = new();
        private static Dictionary<string, bool> mJoystickKeyReleased = new();

        private static List<string> mJoystickKeyWasPreseed = new();
        private static List<string> mJoystickKeyWasReleased = new();

        public static Action onJoystickPlugged = DefaultJoystickPlugged;
        public static Action onJoystickUnplugged = DefaultJoystickUnplugged;

        // record down the if the joystick was connected.
        private static bool mIsJoystickConnected = IsJoystickConnected();

        #endregion

        /* Default callback function pointer. */

        private static void DefaultJoystickPlugged()
        {
            Debug.Log("At least one joystick connected!!!");
        }
        private static void DefaultJoystickUnplugged()
        {
            Debug.Log("No joystick connected...");
        }

        /// <summary>
        /// Main loop for input.
        /// </summary>
        public static void LateUpdate()
        {
            #region DOUBLE_CLICK

            if (CLICK)
            {
                CLICK_TIMER += Time.unscaledDeltaTime;

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
        /// Execute operations with key state pressed/up/down.
        /// </summary>
        /// <param name="key"> Key to press. </param>
        /// <param name="callback"> Callback after key being pressed. </param>
        /// <returns> Result key state. </returns>
        public static bool WithKey(KeyCode key, Action callback)
        {
            return WithKeyAction(JCS_KeyActionType.KEY, key, callback);
        }
        public static bool WithKeyUp(KeyCode key, Action callback)
        {
            return WithKeyAction(JCS_KeyActionType.KEY_UP, key, callback);
        }
        public static bool WithKeyDown(KeyCode key, Action callback)
        {
            return WithKeyAction(JCS_KeyActionType.KEY_DOWN, key, callback);
        }
        private static bool WithKeyAction(JCS_KeyActionType action, KeyCode key, Action callback)
        {
            if (GetKeyByAction(action, key))
            {
                callback?.Invoke();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Do one callback, usually for resetting.
        /// </summary>
        public static void InputCallbackOnce()
        {
            if (mIsJoystickConnected)
                onJoystickPlugged?.Invoke();
            else
                onJoystickUnplugged?.Invoke();
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
            for (int index = 0; index < mJoystickKeyWasPreseed.Count; ++index)
            {
                string idString = mJoystickKeyWasPreseed[index];
                mJoystickKeyPressed[idString] = true;
            }

            for (int index = 0; index < mJoystickKeyWasReleased.Count; ++index)
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
        public static bool OnMouseDoubleClick(JCS_MouseButton type)
        {
            return OnMouseDoubleClick((int)type);
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
        static bool OnMouseDoubleClick(int button)
        {
            // Check first click
            if (!CLICK)
            {
                if (GetMouseButtonDown(button))
                    CLICK = true;
            }
            // Check double click
            else
            {
                if (GetMouseButtonDown(button))
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
        /// <returns></returns>
        public static bool OnMouseDrag(int button)
        {
            return OnMouseDrag((JCS_MouseButton)button);
        }
        /// <summary>
        /// Is the mouse dragging right now?
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
#if UNITY_5_4_OR_NEWER  // NOTE: Resolve ambiguous issue.
        public
#else
        private
#endif
        static bool OnMouseDrag(JCS_MouseButton button)
        {
            if (START_DRAGGING)
            {
                if (GetMouseButton(button))
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
                if (GetMouseButtonDown(button))
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
            Canvas myCanvas = JCS_Canvas.GuessCanvas().canvas;

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
        static bool GetMouseByAction(JCS_KeyActionType act, JCS_MouseButton button)
        {
            return GetMouseByAction(act, (int)button);
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
        public static bool GetMouseByAction(JCS_KeyActionType act, int button)
        {
            switch (act)
            {
                case JCS_KeyActionType.KEY:
                    return GetMouseButton(button);
                case JCS_KeyActionType.KEY_DOWN:
                    return GetMouseButtonDown(button);
                case JCS_KeyActionType.KEY_UP:
                    return GetMouseButtonUp(button);
            }

            return false;  // This cannot happed
        }
        /// <summary>
        /// Check if the mouse button is down.
        /// </summary>
        /// <param name="button"> button select. </param>
        /// <returns> 
        /// true, button is down.
        /// false, button is not down.
        /// </returns>
        public static bool GetMouseButtonDown(JCS_MouseButton button)
        {
            return GetMouseButtonDown((int)button);
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
        static bool GetMouseButtonDown(int button)
        {
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
        public static bool GetMouseButton(JCS_MouseButton button)
        {
            return GetMouseButton((int)button);
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
        static bool GetMouseButton(int button)
        {
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
        public static bool GetMouseButtonUp(JCS_MouseButton button)
        {
            return GetMouseButtonUp((int)button);
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
        static bool GetMouseButtonUp(int button)
        {
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
        public static bool GetKeyByAction(JCS_KeyActionType act, params KeyCode[] keys)
        {
            switch (act)
            {
                case JCS_KeyActionType.KEY:
                    return GetKey(keys);
                case JCS_KeyActionType.KEY_DOWN:
                    return GetKeyDown(keys);
                case JCS_KeyActionType.KEY_UP:
                    return GetKeyUp(keys);
            }

            return false;  // This cannot happens!
        }

        /// <summary>
        /// Is the key held down?
        /// </summary>
        /// <param name="key"> key to check if the key is held down. </param>
        /// <returns>
        /// true: key is held down, false: vice versa.
        /// </returns>
        public static bool GetKey(params KeyCode[] keys)
        {
            foreach (KeyCode key in keys)
            {
                if (Input.GetKey(key))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Is the key down?
        /// </summary>
        /// <param name="key"> key to check if the key is down. </param>
        /// <returns>
        /// true: key is down, false: vice versa.
        /// </returns>
        public static bool GetKeyDown(params KeyCode[] keys)
        {
            foreach (KeyCode key in keys)
            {
                if (Input.GetKeyDown(key))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Is the key up?
        /// </summary>
        /// <param name="key"> key to check if is key up. </param>
        /// <returns> true: is key up, false: vice versa. </returns>
        public static bool GetKeyUp(params KeyCode[] keys)
        {
            foreach (KeyCode key in keys)
            {
                if (Input.GetKeyUp(key))
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
        /// <returns> 
        /// true, all of the keys in the array list are pressed.
        /// false, none of these keys are pressed.
        /// </returns>
        public static bool AllKeys(params KeyCode[] keys)
        {
            foreach (KeyCode key in keys)
            {
                if (!GetKey(key))
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
        /// <returns> 
        /// true, all of the keys in the array list are down.
        /// false, none of these keys are down.
        /// </returns>
        public static bool AllKeysDown(params KeyCode[] keys)
        {
            foreach (KeyCode key in keys)
            {
                if (!GetKeyDown(key))
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
        /// <returns> 
        /// true, all of the keys in the array list are up.
        /// false, none of these keys are up.
        /// </returns>
        public static bool AllKeysUp(params KeyCode[] keys)
        {
            foreach (KeyCode key in keys)
            {
                if (!GetKeyUp(key))
                    return false;
            }

            return true;
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
        public static bool GetButtonByAction(JCS_KeyActionType act, params string[] names)
        {
            switch (act)
            {
                case JCS_KeyActionType.KEY:
                    return GetButton(names);
                case JCS_KeyActionType.KEY_DOWN:
                    return GetButtonDown(names);
                case JCS_KeyActionType.KEY_UP:
                    return GetButtonUp(names);
            }

            return false;  // this cannot happens
        }

        /// <summary>
        /// Is the button pressed?
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButton(params string[] names)
        {
            foreach (string name in names)
            {
                if (Input.GetButton(name))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Is the button down?
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButtonDown(params string[] names)
        {
            foreach (string name in names)
            {
                if (Input.GetButtonDown(name))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Is the button up?
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public static bool GetButtonUp(params string[] names)
        {
            foreach (string name in names)
            {
                if (Input.GetButtonUp(name))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Loop through the key and key what key is pressed.
        /// </summary>
        /// <param name="type"> type of the key pressed option. </param>
        /// <returns> key u pressed. </returns>
        public static KeyCode GetAnyKeyByAction(JCS_KeyActionType type)
        {
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
        public static bool IsAnyKeyBuffer(JCS_KeyActionType type)
        {
            KeyCode code = GetAnyKeyByAction(type);

            return (code != KeyCode.None);
        }

        /// <summary>
        /// Check if any key is down?
        /// </summary>
        /// <returns>
        /// true: somewhere in the key is down.
        /// false: no key is down.
        /// </returns>
        public static bool GetAnyKeyDown()
        {
            return IsAnyKeyBuffer(JCS_KeyActionType.KEY_DOWN);
        }

        /// <summary>
        /// Check if any key is pressed.
        /// </summary>
        /// <returns>
        /// true: somewhere the key is pressed.
        /// false: no key is pressed.
        /// </returns>
        public static bool GetAnyKey()
        {
            return IsAnyKeyBuffer(JCS_KeyActionType.KEY);
        }

        /// <summary>
        /// Check if any key is up?
        /// </summary>
        /// <returns>
        /// true: somewhere in the key is up.
        /// false: no key is up.
        /// </returns>
        public static bool GetAnyKeyUp()
        {
            return IsAnyKeyBuffer(JCS_KeyActionType.KEY_UP);
        }

        /// <summary>
        /// Return the joystick buffer.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="btn"></param>
        /// <returns></returns>
        public static float GetAxis(JCS_JoystickId index, JCS_JoystickButton btn)
        {
            return GetAxis((int)index, btn);
        }
        public static float GetAxis(int index, JCS_JoystickButton btn)
        {
            if (btn == JCS_JoystickButton.NONE)
                return 0;

            string id = JCS_InputSettings.GetJoystickButtonIdName(index, btn);

            return GetAxis(id);
        }
        public static float GetAxis(string name)
        {
            if (name == "")
            {
                Debug.LogError("InputManager's name variable can't be an empty string");
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
        public static bool GetJoystickButton(JCS_JoystickId index, params JCS_JoystickButton[] btns)
        {
            return GetJoystickButton((int)index, btns);
        }
        public static bool GetJoystickButton(int index, params JCS_JoystickButton[] btns)
        {
            // check if any joystick connected.
            if (!IsJoystickConnected())
                return false;

            foreach (JCS_JoystickButton btn in btns)
            {
                if (GetAxis(index, btn) > 0)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Check if the button have pressed.
        /// </summary>
        /// <param name="id"> string id </param>
        /// <returns></returns>
        public static bool GetJoystickButton(string id)
        {
            return GetAxis(id) > 0;
        }

        /// <summary>
        /// Check if joystick id, pressed the key?
        /// </summary>
        /// <param name="index"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did pressed.
        /// false: not pressed.
        /// </returns>
        public static bool GetJoystickKey(JCS_JoystickId index, JCS_JoystickButton btn)
        {
            return GetJoystickKey((int)index, btn);
        }

        /// <summary>
        /// Check if joystick id, pressed the key?
        /// </summary>
        /// <param name="index"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did pressed.
        /// false: not pressed.
        /// </returns>
        public static bool GetJoystickKey(int index, JCS_JoystickButton btn)
        {
            return GetJoystickButton(index, btn);
        }

        /// <summary>
        /// Check if joystick id, up the key?
        /// </summary>
        /// <param name="index"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did uo.
        /// false: not up.
        /// </returns>
        public static bool GetJoystickKeyUp(int index, JCS_JoystickButton btn)
        {
            return GetJoystickKeyUp((JCS_JoystickId)index, btn);
        }

        /// <summary>
        /// Check if joystick id, up the key?
        /// </summary>
        /// <param name="index"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did uo.
        /// false: not up.
        /// </returns>
        public static bool GetJoystickKeyUp(JCS_JoystickId index, JCS_JoystickButton btn)
        {
            string id = JCS_InputSettings.GetJoystickButtonIdName(index, btn);

            if (GetJoystickKey(index, btn))
            {
                if (mJoystickKeyReleased.ContainsKey(id))
                    mJoystickKeyReleased[id] = false;
            }
            else
            {
                if (!mJoystickKeyReleased.ContainsKey(id))
                {
                    mJoystickKeyReleased.Add(id, true);
                }
                else
                {
                    if (mJoystickKeyReleased[id])
                        return false;
                    else
                    {
                        if (!mJoystickKeyWasReleased.Contains(id))
                            mJoystickKeyWasReleased.Add(id);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Check if joystick id, down the key?
        /// </summary>
        /// <param name="index"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did uo.
        /// false: not up.
        /// </returns>
        public static bool GetJoystickKeyDown(int index, JCS_JoystickButton btn)
        {
            return GetJoystickKeyDown((JCS_JoystickId)index, btn);
        }

        /// <summary>
        /// Check if joystick id, down the key?
        /// </summary>
        /// <param name="index"> id. </param>
        /// <param name="btn"> target button. </param>
        /// <returns>
        /// true: did uo.
        /// false: not up.
        /// </returns>
        public static bool GetJoystickKeyDown(JCS_JoystickId index, JCS_JoystickButton btn)
        {
            string id = JCS_InputSettings.GetJoystickButtonIdName(index, btn);

            if (GetJoystickKey(index, btn))
            {
                if (!mJoystickKeyPressed.ContainsKey(id))
                {
                    mJoystickKeyPressed.Add(id, false);
                }
                // Key contains!
                else
                {
                    if (mJoystickKeyPressed[id])
                        return false;
                    else
                    {
                        if (!mJoystickKeyWasPreseed.Contains(id))
                            mJoystickKeyWasPreseed.Add(id);
                        return true;
                    }
                }

            }
            else
            {
                if (mJoystickKeyPressed.ContainsKey(id))
                    mJoystickKeyPressed[id] = false;
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
        /// <returns></returns>
        public static bool GetJoystickKeyByAction(
            JCS_KeyActionType act,
            JCS_JoystickId id,
            JCS_JoystickButton key)
        {
            switch (act)
            {
                case JCS_KeyActionType.KEY:
                    return GetJoystickKey(id, key);
                case JCS_KeyActionType.KEY_DOWN:
                    return GetJoystickKeyDown(id, key);
                case JCS_KeyActionType.KEY_UP:
                    return GetJoystickKeyUp(id, key);
            }

            return false;  // This can't happen!
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

            foreach (string joystickName in joystickNames)
            {
                if (name == joystickName)
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
        /// <returns> 
        /// true, all of the keys in the array list are pressed.
        /// false, none of these keys are pressed.
        /// </returns>
        public static bool AllJoystickButtons(int index, params JCS_JoystickButton[] keys)
        {
            return AllJoystickButtons((JCS_JoystickId)index, keys);
        }
        public static bool AllJoystickButtons(JCS_JoystickId index, params JCS_JoystickButton[] keys)
        {
            foreach (JCS_JoystickButton key in keys)
            {
                if (!GetJoystickButton(index, key))
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
        /// <returns> 
        /// true, all of the keys in the array list are down.
        /// false, none of these keys are down.
        /// </returns>
        public static bool AllJoystickKeysDown(int index, params JCS_JoystickButton[] keys)
        {
            return AllJoystickKeysDown((JCS_JoystickId)index, keys);
        }
        public static bool AllJoystickKeysDown(JCS_JoystickId index, params JCS_JoystickButton[] keys)
        {
            foreach (JCS_JoystickButton key in keys)
            {
                if (!GetJoystickKeyDown(index, key))
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
        /// <returns> 
        /// true, all of the keys in the array list are up.
        /// false, none of these keys are up.
        /// </returns>
        public static bool AllJoystickKeysUp(int index, params JCS_JoystickButton[] keys)
        {
            return AllJoystickKeysUp((JCS_JoystickId)index, keys);
        }
        public static bool AllJoystickKeysUp(JCS_JoystickId index, params JCS_JoystickButton[] keys)
        {
            foreach (JCS_JoystickButton key in keys)
            {
                if (!GetJoystickKeyUp(index, key))
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
