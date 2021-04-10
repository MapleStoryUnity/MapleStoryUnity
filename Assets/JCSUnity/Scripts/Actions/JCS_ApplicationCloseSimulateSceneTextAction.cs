/**
 * $File: JCS_ApplicationCloseSimulateSceneTextAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace JCSUnity
{
    /// <summary>
    /// Text shown in the application close simluate scene.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class JCS_ApplicationCloseSimulateSceneTextAction
        : MonoBehaviour
    {
        /* Variables */

        private Text mText = null;

#if (UNITY_EDITOR)
        [Header("** Helper Variables (JCS_ApplicationCloseSimulateSceneTextAction) **")]

        [SerializeField]
        private string mTextShowInEditMode = "";
#endif

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            mText = this.GetComponent<Text>();

#if (UNITY_EDITOR)
            mText.text = mTextShowInEditMode;
#else
            mText.text = "";
#endif
        }
    }
}
