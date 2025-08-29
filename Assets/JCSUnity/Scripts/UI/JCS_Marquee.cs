/**
 * $File: JCS_Marquee.cs $
 * $Date: 2019-07-22 11:59:29 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2019 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Marquee, for announcing, display highlighted text, etc.
    /// </summary>
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Mask))]
    public class JCS_Marquee : MonoBehaviour
    {
        /* Variables */

#if UNITY_EDITOR
        [Separator("Helper Variables (JCS_Marquee)")]

        [Tooltip("Test this component with key event.")]
        public bool testWithKey = false;

        [Tooltip("Key to send message A.")]
        public KeyCode sendMsgAKey = KeyCode.H;

        [Tooltip("Text message A ready to be test display.")]
        public string msgA = "爽拉~";

        [Tooltip("Key to send message B.")]
        public KeyCode sendMsgBKey = KeyCode.J;

        [Tooltip("Text message B ready to be test display.")]
        public string msgB = "又是一個廣播~";
#endif

        [Separator("Check Variables (JCS_Marquee)")]

        [SerializeField]
        [ReadOnly]
        private JCS_3DDistanceTileAction mDistanceTileAction = null;

        [Separator("Initialize Variables (JCS_Marquee)")]

        [Tooltip("Text object that is hiddent under mask.")]
        [SerializeField]
        private Text mTextContainer = null;

        /* Setter & Getter */

        public Text textContainer { get { return mTextContainer; } set { this.mTextContainer = value; } }

        /* Functions */

        private void Awake()
        {
            if (mTextContainer == null)
                mTextContainer = GetComponentInChildren<Text>();

            mDistanceTileAction = JCS_Util.ForceGetComponent<JCS_3DDistanceTileAction>(mTextContainer);
        }

#if UNITY_EDITOR
        private void Update()
        {
            Test();
        }

        private void Test()
        {
            if (!testWithKey)
                return;

            if (Input.GetKeyDown(sendMsgAKey))
                SetText(msgA, true);

            if (Input.GetKeyDown(sendMsgBKey))
                SetText(msgB, false);
        }
#endif

        /// <summary>
        /// Set the marquee text and replay from start.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="loop"></param>
        public void SetText(string msg, bool loop = true)
        {
            if (mTextContainer == null)
            {
                Debug.Log("Can't display the marquee text without text container");
                return;
            }

            this.mTextContainer.text = msg;

            if (!loop)
            {
                mDistanceTileAction.afterResetCallback = () =>
                {
                    mDistanceTileAction.enabled = false;
                };
            }
            else
            {
                mDistanceTileAction.afterResetCallback = null;
                mDistanceTileAction.enabled = true;
            }

            mDistanceTileAction.ResetPosition();
        }
    }
}
