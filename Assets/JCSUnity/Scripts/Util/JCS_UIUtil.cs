/**
 * $File: JCS_UIUtil.cs $
 * $Date: 2018-07-16 13:28:22 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2018 by Shen, Jen-Chieh $
 */

/* NOTE: If you are using `TextMesh Pro` uncomment this line.
 */
#define TMP_PRO

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if TMP_PRO
using TMPro;
#endif

namespace JCSUnity
{
    public delegate void EventTriggerEvent(PointerEventData data);
    public delegate void EventTriggerEventButtonSelection(PointerEventData data, JCS_ButtonSelection selection);

    /// <summary>
    /// User interface related utilities functions.
    /// </summary>
    public static class JCS_UIUtil
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Return true if GameObject contains built-in UI components.
        /// </summary>
        public static bool IsUnityDefinedUI(Component comp)
        {
            return (comp.GetComponent<RawImage>() ||
                comp.GetComponent<Image>() ||
                comp.GetComponent<Button>() ||
                comp.GetComponent<Dropdown>() ||
                comp.GetComponent<TMP_Dropdown>() ||
                comp.GetComponent<Slider>() ||
                comp.GetComponent<Scrollbar>() ||
                comp.GetComponent<Text>() ||
                comp.GetComponent<TMP_Text>() ||
                comp.GetComponent<Toggle>() ||
                comp.GetComponent<InputField>() ||
                comp.GetComponent<TMP_InputField>());
        }

        #region EVENT
        /// <summary>
        /// Add Event to Unity's Event Trigger(Script)
        /// </summary>
        /// <param name="te"></param>
        /// <param name="type"></param>
        /// <param name="func"></param>
        public static void AddEventTriggerEvent(EventTrigger te, EventTriggerType type, EventTriggerEvent func)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.AddListener((data) => { func((PointerEventData)data); });
            te.triggers.Add(entry);
        }

        /// <summary>
        /// Add Event to Unity's Event Trigger(Script)
        /// </summary>
        /// <param name="te"></param>
        /// <param name="type"></param>
        /// <param name="func"></param>
        public static void AddEventTriggerEvent(EventTrigger te, EventTriggerType type, EventTriggerEventButtonSelection func, JCS_ButtonSelection selection)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.AddListener((data) => { func((PointerEventData)data, selection); });
            te.triggers.Add(entry);
        }
        #endregion

        #region LANGUAGE
        /// <summary>
        /// Set text by system language and language data.
        /// </summary>
        /// <param name="data"> Language data list. </param>
        /// <param name="txt"> Text container to display language data. </param>
        public static void SetLangText(JCS_LangDataList data, Text txt)
        {
            SystemLanguage lang = JCS_AppManager.instance.systemLanguage;

            foreach (JCS_LangData langData in data.LangData)
            {
                if (lang == langData.Language)
                {
                    SetText(txt, langData.Data);
                    break;
                }
            }
        }

#if TMP_PRO
        /// <summary>
        /// Set text by system language and language data.
        /// </summary>
        /// <param name="data"> Language data list. </param>
        /// <param name="txt"> Text container to display language data. </param>
        public static void SetLangText(JCS_LangDataList data, TMP_Text txt)
        {
            SystemLanguage lang = JCS_AppManager.instance.systemLanguage;

            foreach (JCS_LangData langData in data.LangData)
            {
                if (lang == langData.Language)
                {
                    SetText(txt, langData.Data);
                    break;
                }
            }
        }
#endif
        #endregion

        #region TEXT
        /// <summary>
        /// Set the text with data.
        /// </summary>
        /// <param name="txt"> Target text object to be set. </param>
        /// <param name="data"> Text value to display inside text object. </param>
        public static void SetText(Text txt, string data)
        {
            if (txt == null) return;
            txt.text = data;
        }

#if TMP_PRO
        /// <summary>
        /// Set the text with data.
        /// </summary>
        /// <param name="txt"> Target text mesh to be set. </param>
        /// <param name="data"> Text value to display inside text mesh. </param>
        public static void SetText(TMP_Text txt, string data)
        {
            if (txt == null) return;
            txt.text = data;
        }
#endif
        #endregion

        #region DROPDOWN

        /*************************************************************************/
        /*   Universal Version                                                    */
        /*************************************************************************/

        /// <summary>
        /// Returns item vlaue by index.
        /// </summary>
        /// <param name="dd"> Dropdown object. </param>
        /// <param name="index"> item name. </param>
        /// <returns> Current selected text value. </returns>
        public static string Dropdown_GetItemValue(JCS_DropdownObject dd, int index)
        {
            if (dd.DropdownLegacy != null)
                return Dropdown_GetItemValue(dd.DropdownLegacy, index);

            return Dropdown_GetItemValue(dd.DropdownTMP, index);
        }

        /// <summary>
        /// Get the current selected value of the Dropdown object.
        /// </summary>
        /// <param name="dd"> drop down object. </param>
        /// <returns> Current selected text value. </returns>
        public static string Dropdown_GetSelectedValue(JCS_DropdownObject dd)
        {
            if (dd.DropdownLegacy != null)
                return Dropdown_GetSelectedValue(dd.DropdownLegacy);

            return Dropdown_GetSelectedValue(dd.DropdownTMP);
        }

        /// <summary>
        /// Return the index of item in the dropdown's item.
        /// If not found, return -1.
        /// </summary>
        /// <param name="dd"> Dropdown object. </param>
        /// <param name="itemName"> item name. </param>
        /// <returns>
        /// Index of the item value found.
        /// If not found, will returns -1.
        /// </returns>
        public static int Dropdown_GetItemIndex(JCS_DropdownObject dd, string itemName)
        {
            if (dd.DropdownLegacy != null)
                return Dropdown_GetItemIndex(dd.DropdownLegacy, itemName);

            return Dropdown_GetItemIndex(dd.DropdownTMP, itemName);
        }

        /// <summary>
        /// Set the value to the target item.
        /// </summary>
        /// <param name="dd"> dropdown object. </param>
        /// <param name="itemName"> name of the item. </param>
        public static void Dropdown_SetSelection(JCS_DropdownObject dd, string itemName)
        {
            if (dd.DropdownLegacy != null)
                Dropdown_SetSelection(dd.DropdownLegacy, itemName);

            if (dd.DropdownTMP != null)
                Dropdown_SetSelection(dd.DropdownTMP, itemName);
        }

        /// <summary>
        /// Set the selection to target index.
        /// </summary>
        /// <param name="dd"> dropdown object. </param>
        /// <param name="selection"> selection id. </param>
        public static void Dropdown_SetSelection(JCS_DropdownObject dd, int selection)
        {
            if (dd.DropdownLegacy != null)
                Dropdown_SetSelection(dd.DropdownLegacy, selection);

            if (dd.DropdownTMP != null)
                Dropdown_SetSelection(dd.DropdownTMP, selection);
        }

        /// <summary>
        /// Refresh the current selection.
        /// </summary>
        /// <param name="dd"> dropdown object. </param>
        public static void Dropdown_RefreshSelection(JCS_DropdownObject dd)
        {
            if (dd.DropdownLegacy != null)
                Dropdown_RefreshSelection(dd.DropdownLegacy);

            if (dd.DropdownTMP != null)
                Dropdown_RefreshSelection(dd.DropdownTMP);
        }

        /// <summary>
        /// Add an option to dropdown.
        /// </summary>
        /// <param name="dd"> dropdown object. </param>
        /// <param name="inText"> input text. </param>
        public static void Dropdown_AddOption(JCS_DropdownObject dd, string inText)
        {
            if (dd.DropdownLegacy != null)
                Dropdown_AddOption(dd.DropdownLegacy, inText);

            if (dd.DropdownTMP != null)
                Dropdown_AddOption(dd.DropdownTMP, inText);
        }

        /*************************************************************************/
        /*   Legacy Version                                                    */
        /*************************************************************************/

        /// <summary>
        /// Returns item vlaue by index.
        /// </summary>
        /// <param name="dd"> Dropdown object. </param>
        /// <param name="index"> item name. </param>
        /// <returns> Current selected text value. </returns>
        public static string Dropdown_GetItemValue(Dropdown dd, int index)
        {
            return dd.options[index].text;
        }

        /// <summary>
        /// Get the current selected value of the Dropdown object.
        /// </summary>
        /// <param name="dd"> drop down object. </param>
        /// <returns> Current selected text value. </returns>
        public static string Dropdown_GetSelectedValue(Dropdown dd)
        {
            return Dropdown_GetItemValue(dd, dd.value);
        }

        /// <summary>
        /// Return the index of item in the dropdown's item.
        /// If not found, return -1.
        /// </summary>
        /// <param name="dd"> Dropdown object. </param>
        /// <param name="itemName"> item name. </param>
        /// <returns>
        /// Index of the item value found.
        /// If not found, will returns -1.
        /// </returns>
        public static int Dropdown_GetItemIndex(Dropdown dd, string itemName)
        {
            for (int index = 0; index < dd.options.Count; ++index)
            {
                if (itemName == Dropdown_GetItemValue(dd, index))
                    return index;
            }

            return -1;
        }

        /// <summary>
        /// Set the value to the target item.
        /// </summary>
        /// <param name="dd"> dropdown object. </param>
        /// <param name="itemName"> name of the item. </param>
        /// <returns>
        /// true : found the item and set it succesfully.
        /// false : did not find the item, failed to set.
        /// </returns>
        public static bool Dropdown_SetSelection(Dropdown dd, string itemName)
        {
            int index = Dropdown_GetItemIndex(dd, itemName);

            // If not found, will return -1.
            if (index == -1)
                return false;

            dd.value = index;

            return true;
        }

        /// <summary>
        /// Set the selection to target index.
        /// </summary>
        /// <param name="dd"> dropdown object. </param>
        /// <param name="selection"> selection id. </param>
        public static void Dropdown_SetSelection(Dropdown dd, int selection)
        {
            dd.value = selection;
        }

        /// <summary>
        /// Refresh the current selection.
        /// </summary>
        /// <param name="dd"> dropdown object. </param>
        public static void Dropdown_RefreshSelection(Dropdown dd)
        {
            int currentSelectionId = dd.value;

            if (currentSelectionId != 0)
                Dropdown_SetSelection(dd, 0);
            else
            {
                // We use something else.
                //
                // NOTE(jenchieh): Glady, negative one does not
                // occurs error.
                Dropdown_SetSelection(dd, -1);
            }
            Dropdown_SetSelection(dd, currentSelectionId);
        }

        /// <summary>
        /// Add an option to dropdown.
        /// </summary>
        /// <param name="dd"> dropdown object. </param>
        /// <param name="inText"> input text. </param>
        /// <returns> Dropdown pointer. </returns>
        public static Dropdown Dropdown_AddOption(Dropdown dd, string inText)
        {
            dd.options.Add(new Dropdown.OptionData() { text = inText });

            return dd;
        }

        /*************************************************************************/
        /*   TMP Version                                                    */
        /*************************************************************************/

        /// <summary>
        /// Returns item vlaue by index.
        /// </summary>
        /// <param name="dd"> Dropdown object. </param>
        /// <param name="index"> item name. </param>
        /// <returns> Current selected text value. </returns>
        public static string Dropdown_GetItemValue(TMP_Dropdown dd, int index)
        {
            return dd.options[index].text;
        }

        /// <summary>
        /// Get the current selected value of the Dropdown object.
        /// </summary>
        /// <param name="dd"> drop down object. </param>
        /// <returns> Current selected text value. </returns>
        public static string Dropdown_GetSelectedValue(TMP_Dropdown dd)
        {
            return Dropdown_GetItemValue(dd, dd.value);
        }

        /// <summary>
        /// Return the index of item in the dropdown's item.
        /// If not found, return -1.
        /// </summary>
        /// <param name="dd"> Dropdown object. </param>
        /// <param name="itemName"> item name. </param>
        /// <returns>
        /// Index of the item value found.
        /// If not found, will returns -1.
        /// </returns>
        public static int Dropdown_GetItemIndex(TMP_Dropdown dd, string itemName)
        {
            for (int index = 0; index < dd.options.Count; ++index)
            {
                if (itemName == Dropdown_GetItemValue(dd, index))
                    return index;
            }

            return -1;
        }

        /// <summary>
        /// Set the value to the target item.
        /// </summary>
        /// <param name="dd"> dropdown object. </param>
        /// <param name="itemName"> name of the item. </param>
        /// <returns>
        /// true : found the item and set it succesfully.
        /// false : did not find the item, failed to set.
        /// </returns>
        public static bool Dropdown_SetSelection(TMP_Dropdown dd, string itemName)
        {
            int index = Dropdown_GetItemIndex(dd, itemName);

            // If not found, will return -1.
            if (index == -1)
                return false;

            dd.value = index;

            return true;
        }

        /// <summary>
        /// Set the selection to target index.
        /// </summary>
        /// <param name="dd"> dropdown object. </param>
        /// <param name="selection"> selection id. </param>
        public static void Dropdown_SetSelection(TMP_Dropdown dd, int selection)
        {
            dd.value = selection;
        }

        /// <summary>
        /// Refresh the current selection.
        /// </summary>
        /// <param name="dd"> dropdown object. </param>
        public static void Dropdown_RefreshSelection(TMP_Dropdown dd)
        {
            int currentSelectionId = dd.value;

            if (currentSelectionId != 0)
                Dropdown_SetSelection(dd, 0);
            else
            {
                // We use something else.
                //
                // NOTE(jenchieh): Glady, negative one does not
                // occurs error.
                Dropdown_SetSelection(dd, -1);
            }
            Dropdown_SetSelection(dd, currentSelectionId);
        }

        /// <summary>
        /// Add an option to dropdown.
        /// </summary>
        /// <param name="dd"> dropdown object. </param>
        /// <param name="inText"> input text. </param>
        /// <returns> Dropdown pointer. </returns>
        public static TMP_Dropdown Dropdown_AddOption(TMP_Dropdown dd, string inText)
        {
            dd.options.Add(new TMP_Dropdown.OptionData() { text = inText });

            return dd;
        }

        #endregion

        #region ANCHOR
        /// <summary>
        /// Check the anchor presets type.
        /// </summary>
        /// <param name="rt"> recttransform </param>
        /// <param name="type"> type of the anchor presets. </param>
        /// <returns>
        /// true, is anchor presets type.
        /// false, not this anchor presets type.
        /// </returns>
        public static bool IsAchorPresets(RectTransform rt, JCS_AnchorPresetsType type)
        {
            Vector2 min = rt.anchorMin;
            Vector2 max = rt.anchorMax;

            bool left = IsAnchorLeft(min, max);
            bool center = IsAnchorCenter(min, max);
            bool right = IsAnchorRight(min, max);
            bool stretchX = IsAnchorStretchX(min, max);

            bool top = IsAnchorTop(min, max);
            bool middle = IsAnchorMiddle(min, max);
            bool bottom = IsAnchorBottom(min, max);
            bool stretchY = IsAnchorStretchY(min, max);

            switch (type)
            {
                case JCS_AnchorPresetsType.LEFT_TOP: return (left && top);
                case JCS_AnchorPresetsType.LEFT_MIDDLE: return (left && middle);
                case JCS_AnchorPresetsType.LEFT_BOTTOM: return (left && bottom);
                case JCS_AnchorPresetsType.LEFT_STRETCH: return (left && stretchY);

                case JCS_AnchorPresetsType.CENTER_TOP: return (center && top);
                case JCS_AnchorPresetsType.CENTER_MIDDLE: return (center && middle);
                case JCS_AnchorPresetsType.CENTER_BOTTOM: return (center && bottom);
                case JCS_AnchorPresetsType.CENTER_STRETCH: return (center && stretchY);

                case JCS_AnchorPresetsType.RIGHT_TOP: return (right && top);
                case JCS_AnchorPresetsType.RIGHT_MIDDLE: return (right && middle);
                case JCS_AnchorPresetsType.RIGHT_BOTTOM: return (right && bottom);
                case JCS_AnchorPresetsType.RIGHT_STRETCH: return (right && stretchY);

                case JCS_AnchorPresetsType.STRETCH_TOP: return (stretchX && top);
                case JCS_AnchorPresetsType.STRETCH_MIDDLE: return (stretchX && middle);
                case JCS_AnchorPresetsType.STRETCH_BOTTOM: return (stretchX && bottom);
                case JCS_AnchorPresetsType.STRETCH_STRETCH: return (stretchX && stretchY);
            }

            // default.
            return false;
        }

        /// <summary>
        /// Check the anchor presets type.
        /// </summary>
        /// <param name="rt"> recttransform </param>
        /// <returns>
        /// Type of the current recttransform is.
        /// </returns>
        public static JCS_AnchorPresetsType IsAchorPresets(RectTransform rt)
        {
            if (IsAchorPresets(rt, JCS_AnchorPresetsType.LEFT_TOP))
                return JCS_AnchorPresetsType.LEFT_TOP;
            else if (IsAchorPresets(rt, JCS_AnchorPresetsType.LEFT_MIDDLE))
                return JCS_AnchorPresetsType.LEFT_MIDDLE;
            else if (IsAchorPresets(rt, JCS_AnchorPresetsType.LEFT_BOTTOM))
                return JCS_AnchorPresetsType.LEFT_BOTTOM;
            else if (IsAchorPresets(rt, JCS_AnchorPresetsType.LEFT_STRETCH))
                return JCS_AnchorPresetsType.LEFT_STRETCH;

            else if (IsAchorPresets(rt, JCS_AnchorPresetsType.CENTER_TOP))
                return JCS_AnchorPresetsType.CENTER_TOP;
            else if (IsAchorPresets(rt, JCS_AnchorPresetsType.CENTER_MIDDLE))
                return JCS_AnchorPresetsType.CENTER_MIDDLE;
            else if (IsAchorPresets(rt, JCS_AnchorPresetsType.CENTER_BOTTOM))
                return JCS_AnchorPresetsType.CENTER_BOTTOM;
            else if (IsAchorPresets(rt, JCS_AnchorPresetsType.CENTER_STRETCH))
                return JCS_AnchorPresetsType.CENTER_STRETCH;

            else if (IsAchorPresets(rt, JCS_AnchorPresetsType.RIGHT_TOP))
                return JCS_AnchorPresetsType.RIGHT_TOP;
            else if (IsAchorPresets(rt, JCS_AnchorPresetsType.RIGHT_MIDDLE))
                return JCS_AnchorPresetsType.RIGHT_MIDDLE;
            else if (IsAchorPresets(rt, JCS_AnchorPresetsType.RIGHT_BOTTOM))
                return JCS_AnchorPresetsType.RIGHT_BOTTOM;
            else if (IsAchorPresets(rt, JCS_AnchorPresetsType.RIGHT_STRETCH))
                return JCS_AnchorPresetsType.RIGHT_STRETCH;

            else if (IsAchorPresets(rt, JCS_AnchorPresetsType.STRETCH_TOP))
                return JCS_AnchorPresetsType.STRETCH_TOP;
            else if (IsAchorPresets(rt, JCS_AnchorPresetsType.STRETCH_MIDDLE))
                return JCS_AnchorPresetsType.STRETCH_MIDDLE;
            else if (IsAchorPresets(rt, JCS_AnchorPresetsType.STRETCH_BOTTOM))
                return JCS_AnchorPresetsType.STRETCH_BOTTOM;
            else if (IsAchorPresets(rt, JCS_AnchorPresetsType.STRETCH_STRETCH))
                return JCS_AnchorPresetsType.STRETCH_STRETCH;

            // default
            return JCS_AnchorPresetsType.NONE;
        }

        /// <summary>
        /// Check if the anchor point currently at the left.
        /// </summary>
        /// <param name="min"> achorMin </param>
        /// <param name="max"> achorMax </param>
        /// <returns>
        /// True, anchor is on the left.
        /// false, vice versa.
        /// </returns>
        public static bool IsAnchorLeft(Vector2 min, Vector2 max)
        {
            return (min.x == 0.0f && max.x == 0.0f);
        }

        /// <summary>
        /// Check if the anchor point currently at the center.
        /// </summary>
        /// <param name="min"> achorMin </param>
        /// <param name="max"> achorMax </param>
        /// <returns>
        /// True, anchor is on the center.
        /// false, vice versa.
        /// </returns>
        public static bool IsAnchorCenter(Vector2 min, Vector2 max)
        {
            return (min.x == 0.5f && max.x == 0.5f);
        }

        /// <summary>
        /// Check if the anchor point currently at the right.
        /// </summary>
        /// <param name="min"> achorMin </param>
        /// <param name="max"> achorMax </param>
        /// <returns>
        /// True, anchor is on the right.
        /// false, vice versa.
        /// </returns>
        public static bool IsAnchorRight(Vector2 min, Vector2 max)
        {
            return (min.x == 1.0f && max.x == 1.0f);
        }

        /// <summary>
        /// Check if the anchor point currently at the stretch x.
        /// </summary>
        /// <param name="min"> achorMin </param>
        /// <param name="max"> achorMax </param>
        /// <returns>
        /// True, anchor is on the stretch x.
        /// false, vice versa.
        /// </returns>
        public static bool IsAnchorStretchX(Vector2 min, Vector2 max)
        {
            return (min.x == 0.0f && max.x == 1.0f);
        }

        /// <summary>
        /// Check if the anchor point currently at the top.
        /// </summary>
        /// <param name="min"> achorMin </param>
        /// <param name="max"> achorMax </param>
        /// <returns>
        /// True, anchor is on the top.
        /// false, vice versa.
        /// </returns>
        public static bool IsAnchorTop(Vector2 min, Vector2 max)
        {
            return (min.y == 1.0f && max.y == 1.0f);
        }

        /// <summary>
        /// Check if the anchor point currently at the middle.
        /// </summary>
        /// <param name="min"> achorMin </param>
        /// <param name="max"> achorMax </param>
        /// <returns>
        /// True, anchor is on the middle.
        /// false, vice versa.
        /// </returns>
        public static bool IsAnchorMiddle(Vector2 min, Vector2 max)
        {
            return (min.y == 0.5f && max.y == 0.5f);
        }

        /// <summary>
        /// Check if the anchor point currently at the bottom.
        /// </summary>
        /// <param name="min"> achorMin </param>
        /// <param name="max"> achorMax </param>
        /// <returns>
        /// True, anchor is on the bottom.
        /// false, vice versa.
        /// </returns>
        public static bool IsAnchorBottom(Vector2 min, Vector2 max)
        {
            return (min.y == 0.0f && max.y == 0.0f);
        }

        /// <summary>
        /// Check if the anchor point currently at the stretch y.
        /// </summary>
        /// <param name="min"> achorMin </param>
        /// <param name="max"> achorMax </param>
        /// <returns>
        /// True, anchor is on the stretch Y.
        /// false, vice versa.
        /// </returns>
        public static bool IsAnchorStretchY(Vector2 min, Vector2 max)
        {
            return (min.y == 0.0f && max.y == 1.0f);
        }
        #endregion

        #region CANVAS
        /// <summary>
        /// Show the canvas so it's visible on the screen.
        /// </summary>
        public static void ShowCanvas(JCS_Canvas[] canvas)
        {
            foreach (var cvs in canvas)
            {
                if (cvs != null)
                    cvs.Show();
            }
        }

        /// <summary>
        /// Hide the canvas so it's invisible on the screen.
        /// </summary>
        public static void HideCanvas(JCS_Canvas[] canvas)
        {
            foreach (var cvs in canvas)
            {
                if (cvs != null)
                    cvs.Hide();
            }
        }
        #endregion

        #region PANELS
        /// <summary>
        /// Active panels in array.
        /// </summary>
        public static void ActivePanels(JCS_PanelRoot[] dos, bool sound)
        {
            foreach (JCS_PanelRoot panel in dos)
            {
                if (panel != null)
                {
                    panel.Show(!sound);
                }
            }
        }
        public static void ActivePanels(JCS_TweenPanel[] tps)
        {
            foreach (JCS_TweenPanel panel in tps)
            {
                if (panel != null)
                    panel.Active();
            }
        }

        /// <summary>
        /// Deactive panels in array.
        /// </summary>
        public static void DeactivePanels(JCS_PanelRoot[] dos, bool sound)
        {
            foreach (JCS_PanelRoot panel in dos)
            {
                if (panel != null)
                {
                    panel.Hide(!sound);
                }
            }
        }
        public static void DeactivePanels(JCS_TweenPanel[] tps)
        {
            foreach (JCS_TweenPanel panel in tps)
            {
                if (panel != null)
                    panel.Deactive();
            }
        }
        #endregion

        #region IMAGE
        /// <summary>
        /// Returns the size of the image.
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Vector2 GetImageRect(Image img)
        {
            var rt = img.transform.GetComponent<RectTransform>();

            if (rt == null)
            {
                Debug.LogError("No `RectTransform` on your image!");
                return Vector2.one;
            }

            float width = rt.sizeDelta.x * rt.localPosition.x;
            float height = rt.sizeDelta.y * rt.localPosition.y;

            return new Vector2(width, height);
        }
        #endregion

        #region SPRITE
        /// <summary>
        /// Return transparent sprite.
        /// </summary>
        public static Sprite SpriteTransparent()
        {
            return JCS_UtilManager.instance.SpriteTransparent;
        }

        /// <summary>
        /// Returns the size of the sprite renderer without the scale value multiply.
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static Vector2 GetSpriteRendererRectWithNoScale(SpriteRenderer sr)
        {
            float width = sr.bounds.extents[0] * 2;
            float height = sr.bounds.extents[1] * 2;

            return new Vector2(width, height);
        }

        /// <summary>
        /// Returns the size of the sprite renderer.
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static Vector2 GetSpriteRendererRect(SpriteRenderer sr)
        {
            float width = sr.bounds.extents[0] * 2 * sr.transform.localScale.x;
            float height = sr.bounds.extents[1] * 2 * sr.transform.localScale.y;

            return new Vector2(width, height);
        }
        #endregion

        /// <summary>
        /// Solve the flash problem! (JCS_CheckableObject)
        /// 
        /// Check if the mouse still on top of the image!
        /// 
        /// ATTENTIOIN(jenchieh): this will not work on the resizable window.
        /// </summary>
        /// <returns></returns>
        public static bool MouseOverGUI(RectTransform imageRect, RectTransform rootPanel = null)
        {
            Vector2 mousePos = JCS_Input.MousePositionOnGUILayer();
            Vector2 checkPos = imageRect.localPosition;

            if (rootPanel != null)
                checkPos += new Vector2(rootPanel.localPosition.x, rootPanel.localPosition.y);

            // this item image size
            Vector2 slotRect = imageRect.sizeDelta;

            float halfSlotWidth = slotRect.x / 2 * imageRect.localScale.x;
            float halfSlotHeight = slotRect.y / 2 * imageRect.localScale.y;

            float leftBorder = checkPos.x - halfSlotWidth;
            float rightBorder = checkPos.x + halfSlotWidth;
            float topBorder = checkPos.y + halfSlotHeight;
            float bottomBorder = checkPos.y - halfSlotHeight;

            // Basic AABB collide math
            if (mousePos.x <= rightBorder &&
                mousePos.x >= leftBorder &&
                mousePos.y <= topBorder &&
                mousePos.y >= bottomBorder)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if mosue is over any UI element.
        /// </summary>
        /// <returns>
        /// Return true, if there are UI element infront.
        /// Return false, if there are NO UI element infront.
        /// </returns>
        public static bool IsOverGUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}
