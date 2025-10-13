/**
 * $File: JCS_BlackScreen.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Black screen game object for JCSUnity usage.
    /// </summary>
    [RequireComponent(typeof(JCS_FadeObject))]
    public class JCS_BlackScreen : MonoBehaviour
    {
        /* Variables */

        private JCS_FadeObject mFadeObject = null;

        /* Setter & Getter */

        public JCS_FadeObject fadeObject { get { return mFadeObject; } }
        public Color localColor { get { return mFadeObject.localColor; } set { mFadeObject.localColor = value; } }

        /* Functions */

        private void Awake()
        {
            mFadeObject = GetComponent<JCS_FadeObject>();
        }

        private void Start()
        {
            // everytime it reload the scene.
            // move to the last child make sure everything get cover by 
            JCS_Util.MoveToTheLastChild(transform);
        }

        /// <summary>
        /// Fade in black screen.
        /// </summary>
        /// <param name="time"></param>
        public void FadeIn(float time)
        {
            mFadeObject.FadeIn(time);
        }

        /// <summary>
        /// Fade out black screen.
        /// </summary>
        /// <param name="time"></param>
        public void FadeOut(float time)
        {
            mFadeObject.FadeOut(time);
        }

        /// <summary>
        /// Return true if the black screen is fade in.
        /// </summary>
        public bool IsFadeIn()
        {
            return mFadeObject.IsFadeIn();
        }

        /// <summary>
        /// Return true if the black screen is fade out.
        /// </summary>
        public bool IsFadeOut()
        {
            return mFadeObject.IsFadeOut();
        }

        /// <summary>
        /// Move the panel to front
        /// </summary>
        public void MoveToTheLastChild()
        {
            Transform parent = transform.parent;

            Vector3 recordPos = transform.localPosition;
            Vector3 recordScale = transform.localScale;
            Quaternion recordRot = transform.localRotation;

            // this part will mess up the transform
            // so we record all we need and set it back
            {
                transform.SetParent(null);
                transform.SetParent(parent);
            }

            // here we set it back!
            transform.localPosition = recordPos;
            transform.localScale = recordScale;
            transform.localRotation = recordRot;
        }
    }
}
