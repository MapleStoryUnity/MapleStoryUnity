/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using System;
using UnityEditor;
using UnityEngine;

namespace Mx
{
    public enum SortType
    {
        None,

        Alphabetic,
        Length,
    }

    [Serializable]
    public class MxData
    {
        /* Variables */

        private float mSummaryRatio = 40.0f;
        private SortType mInitialSortingOrder = SortType.Alphabetic;
        private bool mShowCommandCount = true;
        private bool mCycle = true;
        private bool mHistory = true;
        private bool mAutoPreview = true;

        private int mMinWindowWidthRatio = 50;
        private int mMinWindowHeightRatio = 25;

        /* Setter & Getter */

        public float SummaryRatio { get { return mSummaryRatio; } }
        public SortType InitialSortingOrder { get { return mInitialSortingOrder; } }
        public bool ShowCommandCount { get { return mShowCommandCount; } }
        public bool Cycle { get { return mCycle; } }
        public bool History { get { return mHistory; } }
        public bool AutoPreview { get { return mAutoPreview; } }
        public int MinWindowWidthRatio { get { return mMinWindowWidthRatio; } }
        public int MinWindowHeightRatio { get { return mMinWindowHeightRatio; } }

        /* Functions */

        public string FormKey(string name)
        {
            return MxUtil.FormKey("Data.") + name;
        }

        public void Init()
        {
            mSummaryRatio = EditorPrefs.GetFloat(FormKey("mSummaryRatio"), mSummaryRatio);
            mInitialSortingOrder = (SortType)EditorPrefs.GetInt(FormKey("mInitialSortingOrder"), (int)mInitialSortingOrder);
            mShowCommandCount = EditorPrefs.GetBool(FormKey("mShowCommandCount"), mShowCommandCount);
            mCycle = EditorPrefs.GetBool(FormKey("mCycle"), mCycle);
            mHistory = EditorPrefs.GetBool(FormKey("mHistory"), mHistory);
            mAutoPreview = EditorPrefs.GetBool(FormKey("mAutoPreview"), mAutoPreview);
            mMinWindowWidthRatio = EditorPrefs.GetInt(FormKey("mMinWindowWidthRatio"), mMinWindowWidthRatio);
            mMinWindowHeightRatio = EditorPrefs.GetInt(FormKey("mMinWindowHeightRatio"), mMinWindowHeightRatio);
        }

        public void Draw()
        {
            GUILayoutOption[] options = {
                GUILayout.MaxWidth(150.0f),
                GUILayout.ExpandWidth(false),
            };

            MxEditorUtil.BeginHorizontal(() =>
            {
                EditorGUILayout.LabelField("Summary Ratio", options);
                mSummaryRatio = EditorGUILayout.Slider(mSummaryRatio, 20.0f, 80.0f);

                MxEditorUtil.ResetButton(() => mSummaryRatio = 40.0f);
                GUILayout.FlexibleSpace();
            });

            MxEditorUtil.BeginHorizontal(() =>
            {
                EditorGUILayout.LabelField("Initial Sorting Order", options);
                mInitialSortingOrder = (SortType)EditorGUILayout.EnumPopup(mInitialSortingOrder);
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(102));

                MxEditorUtil.ResetButton(() => mInitialSortingOrder = SortType.Alphabetic);
                GUILayout.FlexibleSpace();
            });

            MxEditorUtil.BeginHorizontal(() =>
            {
                EditorGUILayout.LabelField("Show Command Count", options);
                mShowCommandCount = EditorGUILayout.Toggle(mShowCommandCount);
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(138));

                MxEditorUtil.ResetButton(() => mShowCommandCount = true);
                GUILayout.FlexibleSpace();
            });

            MxEditorUtil.BeginHorizontal(() =>
            {
                EditorGUILayout.LabelField("Cycle", options);
                mCycle = EditorGUILayout.Toggle(mCycle);
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(138));

                MxEditorUtil.ResetButton(() => mCycle = true);
                GUILayout.FlexibleSpace();
            });

            MxEditorUtil.BeginHorizontal(() =>
            {
                EditorGUILayout.LabelField("History", options);
                mHistory = EditorGUILayout.Toggle(mHistory);
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(138));

                MxEditorUtil.ResetButton(() => mHistory = true);
                GUILayout.FlexibleSpace();
            });

            MxEditorUtil.BeginHorizontal(() =>
            {
                EditorGUILayout.LabelField("Auto Preview", options);
                mAutoPreview = EditorGUILayout.Toggle(mAutoPreview);
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(138));

                MxEditorUtil.ResetButton(() => mAutoPreview = true);
                GUILayout.FlexibleSpace();
            });

            EditorGUILayout.LabelField("Window", EditorStyles.boldLabel);

            MxEditorUtil.Indent(() =>
            {
                MxEditorUtil.BeginHorizontal(() =>
                {
                    EditorGUILayout.LabelField("Minimum Width Ratio", options);
                    mMinWindowWidthRatio = EditorGUILayout.IntSlider(mMinWindowWidthRatio, 10, 80);

                    MxEditorUtil.ResetButton(() => mMinWindowWidthRatio = 50);
                    GUILayout.FlexibleSpace();
                });

                MxEditorUtil.BeginHorizontal(() =>
                {
                    EditorGUILayout.LabelField("Minimum Height Ratio", options);
                    mMinWindowHeightRatio = EditorGUILayout.IntSlider(mMinWindowHeightRatio, 10, 80);

                    MxEditorUtil.ResetButton(() => mMinWindowHeightRatio = 25);
                    GUILayout.FlexibleSpace();
                });
            });
        }

        public void SavePref()
        {
            EditorPrefs.SetFloat(FormKey("mSummaryRatio"), mSummaryRatio);
            EditorPrefs.SetInt(FormKey("mInitialSortingOrder"), (int)mInitialSortingOrder);
            EditorPrefs.SetBool(FormKey("mShowCommandCount"), mShowCommandCount);
            EditorPrefs.SetBool(FormKey("mCycle"), mCycle);
            EditorPrefs.SetBool(FormKey("mHistory"), mHistory);
            EditorPrefs.SetBool(FormKey("mAutoPreview"), mAutoPreview);
            EditorPrefs.SetInt(FormKey("mMinWindowWidthRatio"), mMinWindowWidthRatio);
            EditorPrefs.SetInt(FormKey("mMinWindowHeightRatio"), mMinWindowHeightRatio);
        }
    }
}
