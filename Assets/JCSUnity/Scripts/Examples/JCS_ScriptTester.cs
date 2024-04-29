#if UNITY_EDITOR
/**
 * $File: JCS_ScriptTester.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Tester to test the script system/dialgoue system.
    /// </summary>
    public class JCS_ScriptTester : MonoBehaviour
    {
        /* Variables */

        [Separator("Test Variables (JCS_ScriptTester)")]

        [Tooltip("Dialogue system use to test. If null will use the default from 'JCS_UtilitiesManager'.")]
        [SerializeField]
        private JCS_DialogueSystem mDialogueSystem = null;

        [Tooltip("Script to run the current text box. (test)")]
        [SerializeField]
        private JCS_DialogueScript mTestDialogueScript = null;

        public KeyCode DisposeKey = KeyCode.Q;
        public KeyCode RunScriptKey = KeyCode.W;


        /* Setter & Getter */

        /* Functions */

        private void Start()
        {
            // use default.
            if (mDialogueSystem == null)
                mDialogueSystem = JCS_UtilitiesManager.instance.GetDialogueSystem();
        }

        private void Update()
        {
            if (mTestDialogueScript == null)
                return;

            if (JCS_Input.GetKeyDown(DisposeKey))
                mDialogueSystem.Dispose();
            if (JCS_Input.GetKeyDown(RunScriptKey))
                mDialogueSystem.ActiveDialogue(mTestDialogueScript);
        }
    }
}
#endif
