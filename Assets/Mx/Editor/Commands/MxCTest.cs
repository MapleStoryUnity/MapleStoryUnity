/**
 * Copyright (c) Jen-Chieh Shen. All rights reserved.
 * 
 * jcs090218@gmail.com
 */
using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace Mx
{
    public class MxCTest : Mx
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        public override bool Enable() { return true; }

        /// <summary>
        /// Copied from https://forum.unity.com/threads/run-tests-from-editor-script.677221/.
        /// </summary>
        private static void RunTests(TestMode mode)
        {
            var testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
            var filter = new Filter() { testMode = mode };

            testRunnerApi.Execute(new ExecutionSettings(filter));

            EditorApplication.ExecuteMenuItem("Window/General/Test Runner");

            // TODO: Switch tab to corresponding test mode!
        }

        [Interactive(icon: "TestPassed", summary: "Run all tests")]
        public static void RunAllTests()
        {
            var modes = MxUtil.EnumTuple(typeof(TestMode));

            CompletingRead("Test mode: ", modes, (mode, _) =>
            {
                switch (mode)
                {
                    case "EditMode":
                        RunTests(TestMode.EditMode);
                        break;
                    case "PlayMode":
                        RunTests(TestMode.PlayMode);
                        break;
                    default:
                        Debug.LogError("Unknown test mode: " + mode);
                        break;
                }
            });
        }
    }
}
