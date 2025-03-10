/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using FlxCs;

namespace Mx
{
    public delegate void CompletingReadCallback(string answer, MxItem item);

    public class MxCompletionWindow : EditorWindow
    {
        /* Variables */

        public static MxCompletionWindow instance = null;

        private const string DEFAULT_PROMPT = "M-x ";

        public static bool INHIBIT_CLOSE = false;

#if UNITY_2022_3_OR_NEWER
        private const string mToolbarSearchTextFieldStyleName = "ToolbarSearchTextField";
        private const string mToolbarSearchCancelButtonStyleName = "ToolbarSearchCancelButton";
#else
        private const string mToolbarSearchTextFieldStyleName = "ToolbarSeachTextField";
        private const string mToolbarSearchCancelButtonStyleName = "ToolbarSeachCancelButton";
#endif

        private const string FIND_SEARCH_FIELD_CTRL_NAME = "FindEditorToolsSearchField";

        public static string OVERRIDE_PROMPT = null;
        public static Dictionary<string, MxItem> OVERRIDE_COLLECTION = null;
        public static CompletingReadCallback OVERRIDE_EXECUTE = null;
        public static CompletingReadCallback OVERRIDE_HOVER = null;
        public static bool REQUIRED_MATCH = true;

        private List<Mx> mTypes = null;
        private List<MethodInfo> mMethods = null;

        private Dictionary<string, MethodInfo> mMethodsIndex = new();

        private string mSearchString = String.Empty;
        private int mSelected = 0;
        private float mScrollBar = 0.0f;

        private int mCommandsFilteredCount = 0;
        private List<string> mCommands = new();
        private List<string> mCommandsFiltered = new();

        private static List<string> mHistory = null;

        private enum InputType
        {
            None,
            Clear,
            Close,
            Execute,
            SelectionDown,
            SelectionUp,
        };

        private const float mButtonStartPosition = 24.0f;
        private const float mButtonHeight = 16.0f;
        private const float mSrollbarWidth = 15.0f;
        private const float mIconWidth = 20.0f;
        private static readonly Color mHover =
            (EditorGUIUtility.isProSkin) ?
            new Color32(38, 79, 120, 255) :
            new Color32(153, 201, 239, 255);
        private static readonly Color mDefault =
            (EditorGUIUtility.isProSkin) ?
            new Color32(46, 46, 46, 255) :
            new Color32(180, 180, 180, 255);
        private GUIStyle mGuiStyleHover = new GUIStyle();
        private GUIStyle mGuiStyleDefault = new GUIStyle();

        private static readonly Color mDefaultText =
            (EditorGUIUtility.isProSkin) ?
            new Color(46, 46, 46) :  // #2E2E2E
            new Color(0, 0, 0);

        /* Setter & Getter */

        /* Functions */

        [MenuItem("Tools/Mx/Window/Completion &x", false, -1000)]
        public static void ShowWindow()
        {
            var data = MxSettings.data;

            EditorWindow window = GetWindow<MxCompletionWindow>("Mx Completion");
            Resolution res = Screen.currentResolution;
            int width = res.width * data.MinWindowWidthRatio / 100;
            int height = res.height * data.MinWindowHeightRatio / 100;
            window.minSize = new Vector2(width, height);
        }

        private void OnEnable()
        {
            instance = this;

            mHistory = GetHistory();

            EditorApplication.quitting += OnQuitting;

            /* Initialize some UI components! */
            {
                mGuiStyleHover.normal.textColor = mDefaultText;
                mGuiStyleDefault.normal.textColor = mDefaultText;
            }

            Refresh();
        }

        private void OnDisable()
        {
            OVERRIDE_PROMPT = null;
            OVERRIDE_COLLECTION = null;
            OVERRIDE_EXECUTE = null;
            OVERRIDE_HOVER = null;
            REQUIRED_MATCH = true;
            INHIBIT_CLOSE = false;
        }

        private void OnQuitting()
        {
            ClearHistory();
        }

        private void OnGUI()
        {
            InputType input = this.UpdateEventBeforeDraw(Event.current);

            DrawSearchBar(input);
            DrawCompletion(input);
        }

        public void Refresh()
        {
            this.mTypes = GetMx();
            this.mMethods = GetMethods();

            RecreateCommandList();
        }

        /// <summary>
        /// Return ture when this editor window is focused.
        /// </summary>
        public bool IsFocused()
        {
            return focusedWindow == this;
        }

        /// <summary>
        /// Return a list of Mx subclasses.
        /// </summary>
        private List<Mx> GetMx()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(Mx)))
                .Select(type => Activator.CreateInstance(type) as Mx)
                .Where(inst => inst.Enable())
                .ToList();
        }

        /// <summary>
        /// Return a list of methods from Mx subclasses.
        /// </summary>
        private List<MethodInfo> GetMethods()
        {
            var bindings = BindingFlags.Static
                | BindingFlags.Public
                | BindingFlags.NonPublic;

            return mTypes.SelectMany(t => t.GetType().GetMethods(bindings))
                .Where(m => m.GetCustomAttributes(typeof(InteractiveAttribute), false).Length > 0)
                .Where(m => (m.GetCustomAttribute(typeof(InteractiveAttribute)) as InteractiveAttribute).Enabled)
                .ToList();
        }

        private void DrawSearchBar(InputType input)
        {
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    string prompt = OVERRIDE_PROMPT ?? DEFAULT_PROMPT;

                    if (MxSettings.data.ShowCommandCount)
                    {
                        int selected = mSelected + 1;
                        selected = Mathf.Min(selected, mCommandsFilteredCount);

                        prompt = "[" + selected + "/" + mCommandsFilteredCount + "] " + prompt;
                    }

                    MxEditorUtil.LabelField(prompt);

                    GUI.SetNextControlName(FIND_SEARCH_FIELD_CTRL_NAME);

                    mSearchString = EditorGUILayout.TextField(mSearchString, GUI.skin.FindStyle(mToolbarSearchTextFieldStyleName));

                    if (GUILayout.Button(string.Empty, GUI.skin.FindStyle(mToolbarSearchCancelButtonStyleName)) && mSearchString != string.Empty)
                    {
                        Event evt = new Event();
                        evt.type = EventType.KeyDown;
                        evt.keyCode = KeyCode.Escape;
                        this.SendEvent(evt);
                    }

                    if (String.IsNullOrEmpty(mSearchString) ||
                        input == InputType.Clear ||
                        mCommandsFilteredCount == 0)
                    {
                        EditorGUI.FocusTextInControl(FIND_SEARCH_FIELD_CTRL_NAME);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            if (EditorGUI.EndChangeCheck())
            {
                this.RecreateFilteredList();
                mSelected = Mathf.Clamp(mSelected, 0, mCommandsFilteredCount - 1);

                CheckScrollToSelected();
            }
        }

        private void DrawCompletion(InputType input)
        {
            Rect winRect = this.position;
            float fButtonAreaHeight = winRect.height - mButtonStartPosition;
            float fButtonCount = fButtonAreaHeight / mButtonHeight;
            int iButtonCountFloor = Mathf.Min(Mathf.FloorToInt(fButtonCount), mCommandsFilteredCount); // used for showing the scrollbar
            int iButtonCountCeil = Mathf.Min(Mathf.CeilToInt(fButtonCount), mCommandsFilteredCount);   // used to additionally show the last button (even if visible only in half)
            bool bScrollbar = iButtonCountCeil < mCommandsFilteredCount;

            //

            if (bScrollbar)
            {
                mScrollBar = GUI.VerticalScrollbar(
                    new Rect(
                        winRect.width - mSrollbarWidth,
                        mButtonStartPosition,
                        mSrollbarWidth,
                        winRect.height - mButtonStartPosition
                    ),
                    mScrollBar,
                    iButtonCountFloor * mButtonHeight,
                    0.0f,
                    mCommandsFilteredCount * mButtonHeight
                );
            }

            int _base = bScrollbar ? Mathf.RoundToInt(mScrollBar / mButtonHeight) : 0;

            float summaryWidth = winRect.width * MxSettings.data.SummaryRatio / 100.0f;

            float sbWidth = (bScrollbar ? mSrollbarWidth : 0.0f);

            float width = winRect.width - sbWidth - mIconWidth;
            float height = mButtonHeight - 1.0f;

            float tooltipDisplayWidth = winRect.width - summaryWidth - sbWidth;

            for (int i = _base, j = 0, imax = Mathf.Min(_base + iButtonCountCeil, mCommandsFilteredCount); i < imax; ++i, ++j)
            {
                string name = mCommandsFiltered[i];
                bool selected = (i == mSelected);

                float y = mButtonStartPosition + j * mButtonHeight;
                var rMain = new Rect(mIconWidth, y, width, height);

                EditorGUI.DrawRect(rMain, selected ? mHover : mDefault);

                float indentation = EditorGUI.IndentedRect(rMain).x - rMain.x;
                mGuiStyleHover.fixedWidth = rMain.width - indentation;
                mGuiStyleDefault.fixedWidth = rMain.width - indentation;

                GUIStyle style = selected ? mGuiStyleHover : mGuiStyleDefault;
                float nameWidth = new GUIStyle().CalcSize(new GUIContent(name)).x;
                nameWidth += mIconWidth;

                float summaryStart = Mathf.Max(summaryWidth, nameWidth);

                if (IsCompletingRead())
                {
                    MxItem item = GetItem(name);

                    if (item == null)
                        continue;

                    EditorGUI.LabelField(rMain, name, style);

                    // Draw icon and insert tooltip
                    var rIcon = new Rect(0.0f, mButtonStartPosition + j * mButtonHeight, mIconWidth, mButtonHeight - 1.0f);
                    EditorGUI.DrawRect(rIcon, selected ? mHover : mDefault);
                    EditorGUI.LabelField(rIcon, new GUIContent(item.texture, item.tooltip));

                    // Draw summary
                    var rSummary = new Rect(summaryStart, y, tooltipDisplayWidth, height);
                    EditorGUI.LabelField(rSummary, item.summary);
                }
                else
                {
                    InteractiveAttribute attr = GetAttribute(name);

                    if (attr == null)
                        continue;

                    EditorGUI.LabelField(rMain, new GUIContent(name, null, attr.tooltip), style);

                    // Draw icon and insert tooltip
                    var rIcon = new Rect(0.0f, mButtonStartPosition + j * mButtonHeight, mIconWidth, mButtonHeight - 1.0f);
                    EditorGUI.DrawRect(rIcon, selected ? mHover : mDefault);
                    EditorGUI.LabelField(rIcon, new GUIContent(attr.texture, attr.tooltip));

                    // Draw summary
                    var rSummary = new Rect(summaryStart, y, tooltipDisplayWidth, height);
                    EditorGUI.LabelField(rSummary, attr.summary);
                }
            }

            this.UpdateEventAfterDraw(Event.current, input, _base, bScrollbar);
        }

        private InputType UpdateEventBeforeDraw(Event evt)
        {
            if (evt == null)
                return InputType.None;

            switch (evt.type)
            {
                case EventType.KeyDown:

                    switch (evt.keyCode)
                    {
                        case KeyCode.KeypadEnter:
                        case KeyCode.Return:
                            evt.Use();
                            return InputType.Execute;

                        case KeyCode.DownArrow:
                            evt.Use();
                            return InputType.SelectionDown;

                        case KeyCode.UpArrow:
                            evt.Use();
                            return InputType.SelectionUp;

                        case KeyCode.Escape:
                            {
                                if (mSearchString == string.Empty)
                                {
                                    evt.Use();
                                    this.Close();
                                    return InputType.Close;
                                }
                                else
                                {
                                    return InputType.Clear;
                                }
                            }
                    }
                    break;
                default:
                    break;
            }

            return InputType.None;
        }

        private void UpdateEventAfterDraw(Event evt, InputType input, int currentBase, bool scrollbar)
        {
            if (evt == null)
                return;

            switch (input)
            {
                case InputType.None:
                    {
                        if (IsFocused() && IsCompletingRead())
                            InvokeHover();
                    }
                    break;

                case InputType.Close:
                    return;

                case InputType.Execute:
                    {
                        string asnwer = mSearchString;

                        if (REQUIRED_MATCH)
                        {
                            if (mCommandsFilteredCount <= 0)
                                return;

                            asnwer = mCommandsFiltered[mSelected];
                        }
                        else
                        {
                            if (mCommandsFilteredCount != 0)
                                asnwer = mCommandsFiltered[mSelected];
                        }

                        this.ExecuteCommand(asnwer);
                    }
                    return;

                case InputType.SelectionDown:
                    {
                        ++mSelected;

                        if (MxSettings.data.Cycle)
                        {
                            if (mSelected >= mCommandsFilteredCount)
                                mSelected = 0;
                        }
                        else
                        {
                            mSelected = Mathf.Clamp(mSelected, 0, mCommandsFilteredCount - 1);
                        }

                        if (scrollbar)
                            this.CheckScrollToSelected();

                        InvokeHover();

                        this.Repaint();
                    }
                    return;

                case InputType.SelectionUp:
                    {
                        --mSelected;

                        if (MxSettings.data.Cycle)
                        {
                            if (mSelected < 0)
                                mSelected = mCommandsFilteredCount - 1;
                        }
                        else
                        {
                            mSelected = Mathf.Clamp(mSelected, 0, mCommandsFilteredCount - 1);
                        }

                        if (scrollbar)
                            this.CheckScrollToSelected();

                        InvokeHover();

                        this.Repaint();
                    }
                    return;

                default:
                    break;
            }

            switch (evt.type)
            {
                case EventType.MouseDown:
                    {
                        int iIndex = currentBase + Mathf.FloorToInt((evt.mousePosition.y - mButtonStartPosition) / mButtonHeight);
                        if (iIndex >= 0 && iIndex < mCommandsFilteredCount && (!scrollbar || evt.mousePosition.x < this.position.width - mSrollbarWidth))
                        {
                            this.ExecuteCommand(mCommandsFiltered[iIndex]);
                        }
                    }
                    break;

                case EventType.ScrollWheel:
                    {
                        if (scrollbar)
                        {
                            mScrollBar += evt.delta.y * mButtonHeight;
                            this.Repaint();
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private void CheckScrollToSelected()
        {
            float fButtonAreaHeight = this.position.height - mButtonStartPosition;
            float fButtonCount = fButtonAreaHeight / mButtonHeight;
            int iBase = Mathf.RoundToInt(mScrollBar / mButtonHeight);
            int iButtonCountFloor = Mathf.Min(Mathf.FloorToInt(fButtonCount), mCommandsFilteredCount);

            if (mSelected >= Mathf.Min(iBase + iButtonCountFloor, mCommandsFilteredCount))
            {
                mScrollBar = mButtonHeight * (mSelected - iButtonCountFloor + 1);
            }
            else if (mSelected < iBase)
            {
                mScrollBar = mButtonHeight * mSelected;
            }
        }

        private void InvokeHover()
        {
            if (!MxSettings.data.AutoPreview)
                return;

            if (mCommandsFilteredCount <= 0)
                return;

            string name = mCommandsFiltered[mSelected];

            if (OVERRIDE_HOVER != null)
                OVERRIDE_HOVER(name, GetItem(name));
        }

        /// <summary>
        /// Call this to continue the completing read process.
        /// </summary>
        private void Continue()
        {
            if (!IsCompletingRead())
                return;

            mSearchString = "";
            RecreateCommandList();

            INHIBIT_CLOSE = true;
        }

        private void ExecuteCommand(string name)
        {
            if (IsCompletingRead())
                ExecCommand_Completing(name);
            else
                ExecCommand_Root(name);

            INHIBIT_CLOSE = false;
        }

        private void ExecCommand_Root(string name)
        {
            MxUtil.MoveToLast(mHistory, name);
            UpdateHistory();

            MethodInfo method = GetMethod(name);
            method.Invoke(null, null);

            if (IsCompletingRead())
            {
                Continue();
                return;
            }

            this.Close();
        }

        private void ExecCommand_Completing(string name)
        {
            if (OVERRIDE_EXECUTE != null)
                OVERRIDE_EXECUTE.Invoke(name, GetItem(name));

            if (!INHIBIT_CLOSE)
                this.Close();
        }

        private MxItem GetItem(string name)
        {
            if (OVERRIDE_COLLECTION.ContainsKey(name))
                return OVERRIDE_COLLECTION[name];
            return null;
        }

        private string FormName(MethodInfo info, bool full = false)
        {
            if (full)
                return info.DeclaringType + "." + info.Name;
            return info.Name;
        }

        private void RecreateCommandList()
        {
            if (IsCompletingRead())
            {
                mCommands = OVERRIDE_COLLECTION.Keys.ToList();
                RecreateFilteredList();
                return;
            }

            mCommands.Clear();
            mMethodsIndex.Clear();

            HashSet<string> repeated = new();

            foreach (MethodInfo method in mMethods)
            {
                string name = FormName(method);
                string nameFull = FormName(method, true);

                if (repeated.Contains(name))
                {
                    if (mMethodsIndex.ContainsKey(name))
                    {
                        MethodInfo temp = mMethodsIndex[name];

                        mMethodsIndex.Remove(name);
                        mCommands.Remove(name);

                        //string _name = FormName(temp);
                        string _nameFull = FormName(temp, true);

                        mMethodsIndex.Add(_nameFull, temp);
                        mCommands.Add(_nameFull);
                    }

                    name = nameFull;
                }
                else
                {
                    repeated.Add(name);
                }

                mMethodsIndex.Add(name, method);
                mCommands.Add(name);
            }

            // Initial sort
            switch (MxSettings.data.InitialSortingOrder)
            {
                case SortType.Alphabetic:
                    mCommands = mCommands.OrderBy(i => i).ToList();
                    break;

                case SortType.Length:
                    mCommands = mCommands.OrderBy(i => i.Length).ToList();
                    break;
            }

            // Sort history in the correct order!
            if (MxSettings.data.History)
                mCommands = mCommands.OrderByDescending(d => mHistory.IndexOf(d)).ToList();

            RecreateFilteredList();
        }

        private void RecreateFilteredList()
        {
            if (mCommands == null)
            {
                mCommandsFiltered = new List<string>();
            }
            else if (String.IsNullOrEmpty(mSearchString))
            {
                mCommandsFiltered = new List<string>(mCommands);
            }
            else
            {
                mCommandsFiltered.Clear();

                SortedDictionary<int, List<string>> scores = new();

                for (int i = 0, imax = mCommands.Count; i < imax; ++i)
                {
                    string cmd = mCommands[i];

                    FlxCs.Score score = Flx.Score(cmd, mSearchString);

                    // First filtered out low score!
                    if (score != null && score.score > 0)
                    {
                        if (!scores.ContainsKey(score.score))
                            scores.Add(score.score, new List<string>());

                        scores[score.score].Add(cmd);
                    }
                }

                // The sort it by the score!
                foreach (int key in scores.Keys.Reverse())
                {
                    scores[key] = scores[key].OrderBy(i => i).ToList();

                    foreach (string cmd in scores[key])
                    {
                        mCommandsFiltered.Add(cmd);
                    }
                }
            }

            mSelected = 0;
            mCommandsFilteredCount = mCommandsFiltered.Count;
        }

        public MethodInfo GetMethod(string candidate)
        {
            return mMethodsIndex[candidate];
        }

        public InteractiveAttribute GetAttribute(string candidate)
        {
            if (mMethodsIndex.ContainsKey(candidate))
                return mMethodsIndex[candidate].GetCustomAttribute(typeof(InteractiveAttribute), false) as InteractiveAttribute;
            return null;
        }

        #region History
        public static readonly string PK_HISTORY = MxEditorUtil.FormKey("History");

        public static void UpdateHistory()
        {
            MxEditorUtil.SetList(PK_HISTORY, mHistory);
        }

        public static List<string> GetHistory()
        {
            return MxEditorUtil.GetList(PK_HISTORY);
        }

        public static void ClearHistory()
        {
            mHistory.Clear();
            UpdateHistory();
        }
        #endregion

        public static void OverrideIt(
            string prompt,
            Dictionary<string, MxItem> collection,
            CompletingReadCallback callback,
            CompletingReadCallback hover,
            bool requiredMatch = true)
        {
            bool already = IsCompletingRead();

            OVERRIDE_PROMPT = prompt;
            OVERRIDE_COLLECTION = collection;
            OVERRIDE_EXECUTE = callback;
            OVERRIDE_HOVER = hover;
            REQUIRED_MATCH = requiredMatch;

            if (already)
                instance.Continue();
        }

        public static bool IsCompletingRead()
        {
            return OVERRIDE_PROMPT != null
                || OVERRIDE_COLLECTION != null
                || OVERRIDE_EXECUTE != null
                || OVERRIDE_HOVER != null;
        }
    }
}
