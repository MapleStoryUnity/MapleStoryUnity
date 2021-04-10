/**
 * $File: JCS_RollBtnSelector.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    /// <summary>
    /// A bunch of buttons do the roll effect.
    /// 
    /// Plz use this transform as the standard.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]       // 不是一定需要的, 不過強制一下好了.
    public class JCS_RollBtnSelector
        : MonoBehaviour
    {
        /* Variables */

        // the button has been focusing on.
        private JCS_RollSelectorButton mFocusBtn = null;

        [Header("** Check Variables (JCS_RollBtnSelector) **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_PanelRoot mPanelRoot = null;

        [Header("** Initialize Variables (JCS_RollBtnSelector) **")]

        [Tooltip("Array of buttons you want to do in sequence.")]
        [SerializeField]
        private JCS_RollSelectorButton[] mButtons = null;

        [Tooltip("Space to each buttons.")]
        [SerializeField]
        private float mSpacing = 60;

        [Tooltip("Dimension the effect.")]
        [SerializeField]
        private JCS_2DDimensions mDimension = JCS_2DDimensions.VERTICAL;

        [Header("- Asymptotic Order")]

        [Tooltip("Enable the asymptotic order effect?")]
        [SerializeField]
        private bool mAsympEffect = false;

        [Tooltip("Asymptotic origin offset.")]
        [SerializeField]
        private Vector3 mAsympDiffScale = new Vector3(0.5f, 0.5f, 0.5f);

        [Header("- Scroll")]

        [Tooltip("How fast the buttons move?")]
        [SerializeField]
        private float mScrollFriction = 0.2f;

        private bool mAnimating = false;

        private int mLastScrollIndex = 0;

        [Tooltip("How long to scroll one button per times?")]
        [SerializeField]
        private float mScrollSpacingTime = 0.2f;

        private float mScrollSpacingTimer = 0.0f;

        // target we want to scroll to one direction
        private int mTargetScrollIndex = 0;

        // counter in order to approach to "mTargetScrollIndex" about
        private int mScrollIndexCounter = 0;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            this.mPanelRoot = this.GetComponentInParent<JCS_PanelRoot>();
        }

        private void Start()
        {
            InitButton();
            InitAsympScale();
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            DoAnim();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (JCS_Input.GetKeyDown(KeyCode.H))
                ApplyTargetForAnim(-1);
        }
#endif

        /// <summary>
        /// Initialize the focus selector.
        /// </summary>
        /// <param name="rbs"></param>
        public void SetFocusSelector(JCS_RollSelectorButton rbs)
        {
            // if still animating disabled.
            if (mAnimating)
                return;

            // record down the last focus buttons index.
            mLastScrollIndex = mFocusBtn.ScrollIndex;

            // assign new focus button!
            this.mFocusBtn = rbs;

            foreach (JCS_RollSelectorButton btn in mButtons)
            {
                btn.SetInteractable(false);
            }

            // only enable this
            mFocusBtn.SetInteractable(true);

            // show in front.
            JCS_Utility.MoveToTheLastChild(mFocusBtn.transform);

            // Active anim, so can set the focused button to center.
            FindScrollIndex();
        }

        /// <summary>
        /// Check if the object is the same object.
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        public bool IsFoucsed(JCS_RollSelectorButton btn)
        {
            return (this.mFocusBtn == btn);
        }

        /// <summary>
        /// Initilaize the buttons in the array.
        /// </summary>
        private void InitButton()
        {
            JCS_RollSelectorButton currentBtn = null;
            int indexCounter = 0;

            JCS_RollSelectorButton[] buttons = new JCS_RollSelectorButton[mButtons.Length];

            for (int index = 0; index < mButtons.Length; ++index)
            {
                currentBtn = mButtons[index];

                if (currentBtn == null)
                {
                    JCS_Debug.LogError("Missing jcs_button assign in the inspector");
                    continue;
                }

                // set to array object to know this handler.
                currentBtn.SetRollSelector(this);


                Vector3 newPos = this.transform.localPosition;

                bool isEven = JCS_Mathf.IsEven(index);

                if (index != 0)
                    currentBtn.SetInteractable(false);
                else
                {
                    // the first one (center)
                    mFocusBtn = currentBtn;
                    JCS_Utility.MoveToTheLastChild(mFocusBtn.transform);
                    currentBtn.SetInteractable(true);
                    mLastScrollIndex = currentBtn.ScrollIndex;
                }

                if (!isEven)
                    ++indexCounter;

                float intervalDistance = mSpacing * indexCounter;

                switch (mDimension)
                {
                    case JCS_2DDimensions.VERTICAL:
                        {
                            if (mPanelRoot != null)
                                intervalDistance /= mPanelRoot.PanelDeltaWidthRatio;

                            if (isEven)
                                newPos.y += intervalDistance;
                            else
                                newPos.y -= intervalDistance;
                        }
                        break;
                    case JCS_2DDimensions.HORIZONTAL:
                        {
                            if (mPanelRoot != null)
                                intervalDistance /= mPanelRoot.PanelDeltaHeightRatio;

                            if (isEven)
                                newPos.x += intervalDistance;
                            else
                                newPos.x -= intervalDistance;
                        }
                        break;
                }

                currentBtn.GetRectTransfom().localPosition = newPos;

                // set friction.
                currentBtn.SimpleTrackAction.Friction = mScrollFriction;
                // set tracking
                currentBtn.SetTrackPosition();

                // get the correct order
                int correctIndex = 0;

                if (isEven)
                    correctIndex = ((mButtons.Length - 1) - index) / 2;
                else
                    correctIndex = ((mButtons.Length) + index) / 2;

                buttons[correctIndex] = currentBtn;
            }

            mButtons = buttons;

            // initialzie the scroll index.
            for (int index = 0;
                index < mButtons.Length;
                ++index)
            {
                currentBtn = mButtons[index];

                // set index.
                currentBtn.ScrollIndex = index;
            }
        }

        /// <summary>
        /// Initialize all the buttons' scale size.
        /// </summary>
        private void InitAsympScale()
        {
            // if not this effect, return it.
            if (!mAsympEffect)
                return;

            int centerIndex = JCS_Mathf.FindMiddleIndex(mButtons.Length);

            // initialzie the scroll index.
            for (int index = 0; index < mButtons.Length; ++index)
            {
                JCS_RollSelectorButton currentBtn = mButtons[index];

                Vector3 scale = Vector3.zero;
                if (index <= centerIndex)
                    scale = (mAsympDiffScale * index) + mAsympDiffScale;
                else
                    scale = (mAsympDiffScale * (index - ((index - centerIndex) * 2))) + mAsympDiffScale;

                if (mPanelRoot != null)
                {
                    scale.x /= mPanelRoot.PanelDeltaWidthRatio;
                    scale.y /= mPanelRoot.PanelDeltaHeightRatio;
                }

                JCS_ScaleEffect se = currentBtn.GetScaleEffect();

                if (se == null)
                {
                    JCS_Debug.LogError("JCS_ScaleEffect are null but we still want the effect. Please make sure all the button have JCS_ScaleEffet component!");

                    // close the effect.
                    mAsympEffect = false;

                    return;
                }

                se.RecordScale += scale;

                // the change value plus the original scale.
                // so it will keep the original setting form the
                // level designer.
                se.TowardScale += scale + se.GetScaleValue();

                se.Active();
            }

        }

        /// <summary>
        /// Figure out the scroll index and start animating.
        /// </summary>
        private void FindScrollIndex()
        {
            if (mFocusBtn == null)
            {
                JCS_Debug.LogError("Can't do the movement without focus button");
                return;
            }

            // get the current scroll index.
            int currentScrollIndex = mFocusBtn.ScrollIndex;

            if (mLastScrollIndex == currentScrollIndex)
            {
                JCS_Debug.LogError("Last Scroll Index and Current Scroll Index are the same");
                return;
            }

            // 如果 現在是8(last), 我點5(current),
            // last - current = 3
            // 也就是說 往上移動三格
            int diffIndex = mLastScrollIndex - currentScrollIndex;

            mTargetScrollIndex = diffIndex;

            mScrollIndexCounter = 0;

            mAnimating = true;
        }

        /// <summary>
        /// Do the scroll animation.
        /// </summary>
        private void DoAnim()
        {
            if (!mAnimating)
                return;

            // don't block the first time.
            if (mScrollIndexCounter != 0)
            {
                mScrollSpacingTimer += Time.deltaTime;

                if (mScrollSpacingTimer < mScrollSpacingTime)
                    return;
            }

            int direction = 1;

            if (mTargetScrollIndex < mScrollIndexCounter)
            {
                direction = -1;
                --mScrollIndexCounter;
            }
            else
                ++mScrollIndexCounter;

            // set target in order to trigger anim
            ApplyTargetForAnim(direction);

            // do once reset once timer
            mScrollSpacingTimer = 0;

            if (mScrollIndexCounter == mTargetScrollIndex)
            {
                mAnimating = false;  // disable the anim
                mScrollIndexCounter = 0;  // reset counter
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diffIndex"></param>
        private void ApplyTargetForAnim(int diffIndex)
        {
            JCS_RollSelectorButton currentBtn = null;
            JCS_RollSelectorButton targetBtn = null;

            Vector3[] newTargetPosHolder = new Vector3[mButtons.Length];
            int[] scrollIndexHolder = new int[mButtons.Length];

            // asymp scale effect
            Vector3[] newRecordScaleHolder = new Vector3[mButtons.Length];
            Vector3[] newTowardScaleHolder = new Vector3[mButtons.Length];

            for (int index = 0; index < mButtons.Length; ++index)
            {
                int overflowIndex = JCS_Mathf.OverFlowIndex(index + diffIndex, mButtons.Length);

                currentBtn = mButtons[index];
                targetBtn = mButtons[overflowIndex];

                if (currentBtn == null || targetBtn == null)
                {
                    JCS_Debug.LogError("Missing `JCS_Button` assign in the inspector...");
                    continue;
                }

                Vector3 newTargetPos = currentBtn.SimpleTrackAction.TargetPosition;

                newTargetPos = targetBtn.SimpleTrackAction.TargetPosition;

                newTargetPosHolder[index] = newTargetPos;
                scrollIndexHolder[index] = targetBtn.ScrollIndex;

                if (mAsympEffect)
                {
                    newRecordScaleHolder[index] = targetBtn.GetScaleEffect().RecordScale;
                    newTowardScaleHolder[index] = targetBtn.GetScaleEffect().TowardScale;
                }
            }

            for (int index = 0; index < mButtons.Length; ++index)
            {
                currentBtn = mButtons[index];

                currentBtn.SimpleTrackAction.TargetPosition = newTargetPosHolder[index];

                currentBtn.ScrollIndex = scrollIndexHolder[index];

                if (mAsympEffect)
                {
                    JCS_ScaleEffect se = currentBtn.GetScaleEffect();

                    se.RecordScale = newRecordScaleHolder[index];
                    se.TowardScale = newTowardScaleHolder[index];
                    se.Active();
                }
            }
        }
    }
}
