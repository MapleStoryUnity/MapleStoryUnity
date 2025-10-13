/**
 * $File: JCS_AppCloseSimulateSceneTextAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Text shown in the application close simluate scene.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class JCS_AppCloseSimulateSceneTextAction : MonoBehaviour
    {
        /* Variables */

        private Text mText = null;

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_AppCloseSimulateSceneTextAction)")]

        [SerializeField]
        private string mTextShowInEditMode = "";
#endif

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            mText = GetComponent<Text>();

#if UNITY_EDITOR
            mText.text = mTextShowInEditMode;
#else
            mText.text = "";
#endif
        }
    }
}
