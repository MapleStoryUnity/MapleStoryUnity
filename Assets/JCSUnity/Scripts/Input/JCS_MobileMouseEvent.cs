/**
 * $File: JCS_MobileMouseEvent.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Linq;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Use this to receiving the single from mobile touch input/bufffer.
    /// </summary>
    public class JCS_MobileMouseEvent : MonoBehaviour
    {
        /* Variables */

        [Header("** Check Variables (JCS_MobileMouseEvent) **")]

        [Tooltip("")]
        [SerializeField]
        private Camera mCamera = null;

        [SerializeField]
        private RaycastHit[] mHits = new RaycastHit[0];

        [Tooltip("Check if touched last frame.")]
        [SerializeField]
        private bool mTouchedLastFrame = false;

        [Header("** Runtime Variables (JCS_MobileMouseEvent) **")]

        [Tooltip("Distance Raycast shoot.")]
        [SerializeField]
        [Range(0.0f, 5000.0f)]
        private float mRaycastDistance = 100.0f;

        /* Setter & Getter */

        public float RaycastDistance { get { return this.mRaycastDistance; } set { this.mRaycastDistance = value; } }

        /* Functions */

        private void Awake()
        {
            JCS_InputManager.instance.SetGlobalMobileMouseEvent(this);
        }

        private void Start()
        {
            if (mCamera == null)
                this.mCamera = JCS_Camera.main.GetCamera();
        }

        private void Update()
        {
            Handle_EnterExit();
            Handle_UpOver();
            Handle_DownDrag();
        }

        /// <summary>
        /// Send OBJ the message NAME.
        /// </summary>
        /// <param name="obj"> Game object you want to send. </param>
        /// <param name="name"> Event name. </param>
        private void _SendMessage(GameObject obj, string name)
        {
            obj.SendMessage(name, SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// Debug print name.
        /// </summary>
        /// <param name="trans"></param>
        private void PrintName(Transform trans)
        {
#if (UNITY_EDITOR)
            if (!JCS_GameSettings.instance.DEBUG_MODE)
                return;

            // print the name of the detected transform.
            JCS_Debug.PrintName(trans);
#endif
        }

        /// <summary>
        /// Handle these events.
        ///   -> OnMouseEnter
        ///   -> OnMouseExit
        /// </summary>
        private void Handle_EnterExit()
        {
            JCS_InputManager im = JCS_InputManager.instance;

            // A ray is an infinite line starting at an origin and going into a direction
            // For this we will use our mouse position
            Ray ray = mCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, mRaycastDistance);

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform == null)
                    continue;

                GameObject obj = hit.transform.gameObject;

                if (mHits.Contains(hit))
                    continue;

                if (im.Support_OnMouseEnter)
                    _SendMessage(obj, "OnMouseEnter");
            }

            foreach (RaycastHit hit in mHits)
            {
                if (hit.transform == null)
                    continue;

                GameObject obj = hit.transform.gameObject;

                if (hits.Contains(hit))
                    continue;

                if (im.Support_OnMouseExit)
                    _SendMessage(obj, "OnMouseExit");

                PrintName(hit.transform);
            }

            mHits = hits;
        }

        /// <summary>
        /// Handle these events.
        ///   -> OnMouseUp
        ///   -> OnMouseOver
        /// </summary>
        private void Handle_UpOver()
        {
            JCS_InputManager im = JCS_InputManager.instance;

            JCS_SlideInput slideInput = im.GetGlobalSlideInput();

            foreach (RaycastHit hit in mHits)
            {
                if (hit.transform == null)
                    continue;

                GameObject obj = hit.transform.gameObject;

                if (im.Support_OnMouseOver)
                    _SendMessage(obj, "OnMouseOver");

                if (im.Support_OnMouseUp)
                {
                    if (mTouchedLastFrame && !slideInput.Touched)
                        _SendMessage(obj, "OnMouseUp");
                }

                PrintName(hit.transform);
            }
        }

        /// <summary>
        /// Handle these events.
        ///   -> OnMouseDown
        ///   -> OnMouseDrag
        /// </summary>
        private void Handle_DownDrag()
        {
            JCS_InputManager im = JCS_InputManager.instance;

            JCS_SlideInput slideInput = im.GetGlobalSlideInput();

            if (!slideInput.Touched)
            {
                this.mTouchedLastFrame = false;
                return;
            }

            // A ray is an infinite line starting at an origin and going into a direction
            // For this we will use our mouse position
            Ray ray = mCamera.ScreenPointToRay(Input.mousePosition);

            mHits = Physics.RaycastAll(ray, mRaycastDistance);

            foreach (RaycastHit hit in mHits)
            {
                if (hit.transform == null)
                    continue;

                GameObject obj = hit.transform.gameObject;

                if (slideInput.Touched)
                {
                    if (im.Support_OnMouseDown)
                    {
                        if (!mTouchedLastFrame)
                            _SendMessage(obj, "OnMouseDown");
                    }

                    if (im.Support_OnMouseDrag)
                    {
                        if (slideInput.DeltaPos != Vector2.zero)
                            _SendMessage(obj, "OnMouseDrag");
                    }
                }

                PrintName(hit.transform);
            }

            this.mTouchedLastFrame = true;
        }
    }
}
