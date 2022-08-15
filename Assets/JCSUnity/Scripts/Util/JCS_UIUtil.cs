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

#if TMP_PRO
using TMPro;
#endif

namespace JCSUnity
{
    /// <summary>
    /// User interface related utilities functions.
    /// </summary>
    public static class JCS_UIUtil
    {
        /// <summary>
        /// Return true if GameObject contains built-in UI components.
        /// </summary>
        /// <returns></returns>
        public static bool IsUnityDefinedUI(Component comp)
        {
            return (comp.GetComponent<RawImage>() ||
                comp.GetComponent<Image>() ||
                comp.GetComponent<Button>() ||
                comp.GetComponent<Dropdown>() ||
                comp.GetComponent<Slider>() ||
                comp.GetComponent<Scrollbar>() ||
                comp.GetComponent<Text>() ||
                comp.GetComponent<Toggle>() ||
                comp.GetComponent<InputField>());
        }

        #region LANGUAGE
        /// <summary>
        /// Set text by system language and language data.
        /// </summary>
        /// <param name="data"> Language data list. </param>
        /// <param name="txt"> Text container to display language data. </param>
        public static void SetLangText(JCS_LangDataList data, Text txt)
        {
            SystemLanguage lang = JCS_ApplicationManager.instance.systemLanguage;

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
        public static void SetLangText(JCS_LangDataList data, TextMeshPro txt)
        {
            SystemLanguage lang = JCS_ApplicationManager.instance.systemLanguage;

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
        public static void SetText(TextMeshPro txt, string data)
        {
            if (txt == null) return;
            txt.text = data;
        }
#endif
        #endregion

        #region DROPDOWN
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
            dd.options.Add(
                new Dropdown.OptionData() { text = inText });

            return dd;
        }

        /*************************************************************************/
        /*   JCSUnity Version                                                    */
        /*************************************************************************/

        /// <summary>
        /// Returns item vlaue by index.
        /// </summary>
        /// <param name="dd"> JCSUnity dropdown object. </param>
        /// <param name="index"> item name. </param>
        /// <returns></returns>
        public static string Dropdown_GetItemValue(JCS_Dropdown dd, int index)
        {
            /* NOTE(jenchieh): this is all the key for JCS_Dropdown
             * object. We use 'DropdownRealTexts' as our option values.
             */
            return dd.DropdownRealTexts[index];
        }

        /// <summary>
        /// Get the current selected value of the Dropdown object.
        /// </summary>
        /// <param name="dd"> drop down object. </param>
        /// <returns> current selected text value. </returns>
        public static string Dropdown_GetSelectedValue(JCS_Dropdown dd)
        {
            return Dropdown_GetItemValue(dd, dd.dropdown.value);
        }

        /// <summary>
        /// Return the index of item in the dropdown's item.
        /// If not found, return -1.
        /// </summary>
        /// <param name="dd"> JCSUniy dropdown object. </param>
        /// <param name="itemName"> item name. </param>
        /// <returns>
        /// Index of the item value found.
        /// If not found, will returns -1.
        /// </returns>
        public static int Dropdown_GetItemIndex(JCS_Dropdown dd, string itemName)
        {
            for (int index = 0; index < dd.DropdownRealTexts.Count; ++index)
            {
                if (itemName == Dropdown_GetItemValue(dd, index))
                    return index;
            }

            return -1;
        }

        /// <summary>
        /// Set selection when using JCS_Dropdown instead of
        /// UnityEngine.UI.Dropdown object.
        /// </summary>
        /// <param name="dd"> JCSUnity dropdown object. </param>
        /// <param name="itemName"> selection item name. </param>
        /// <returns>
        /// true : found the item and set it succesfully.
        /// false : did not find the item, failed to set.
        /// </returns>
        public static bool Dropdown_SetSelection(JCS_Dropdown dd, string itemName)
        {
            int index = Dropdown_GetItemIndex(dd, itemName);

            // If not found, will return -1.
            if (index == -1)
                return false;

            dd.dropdown.value = index;

            return true;
        }

        /// <summary>
        /// Add an option to dropdown.
        /// </summary>
        /// <param name="dd"> dropdown object. </param>
        /// <param name="inText"> input text. </param>
        /// <returns> JCSUnity's dropdown object. </returns>
        public static JCS_Dropdown Dropdown_AddOption(JCS_Dropdown dd, string inText)
        {
            dd.dropdown.options.Add(
                new Dropdown.OptionData() { text = inText });

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
        public static void ActivePanels(JCS_DialogueObject[] dos, bool sound)
        {
            foreach (JCS_DialogueObject panel in dos)
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
        public static void DeactivePanels(JCS_DialogueObject[] dos, bool sound)
        {
            foreach (JCS_DialogueObject panel in dos)
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
    }
}
