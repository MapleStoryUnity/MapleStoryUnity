/**
 * $File: JCS_TextTimer.cs $
 * $Date: 2019-07-12 16:58:35 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright © 2019 by Shen, Jen-Chieh $
 */

/* NOTE: If you are using `TextMesh Pro` uncomment this line.
 */
#define TMP_PRO

using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Render timer in the text.
    /// </summary>
    public class JCS_TextTimer : JCS_TextObject
    {
        /* Variables */

        public Action onTimeUp = null;

        private const float MAX_HOUR_TIME = 23.0f;
        private const float MAX_MINUTE_TIME = 59.0f;
        private const float MAX_SECOND_TIME = 60.0f;

        private const float MIN_HOUR_TIME = 0.0f;
        private const float MIN_MINUTE_TIME = 0.0f;
        private const float MIN_SECOND_TIME = 0.0f;

        [Separator("Check Variables (JCS_TextTimer)")]

        [SerializeField]
        private bool mDoTimeUpCallback = false;

        [SerializeField]
        private string mHoursText = "";

        [SerializeField]
        private string mMinutesText = "";

        [SerializeField]
        private string mSecondsText = "";

        [Separator("Runtime Variables (JCS_TextTimer)")]

        [Tooltip("Timer active or not active.")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Current hours in the game.")]
        [SerializeField]
        [Range(0, 23)]
        private float mCurrentHours = 0;

        [Tooltip("Current minutes in the game.")]
        [SerializeField]
        [Range(0, 59)]
        private float mCurrentMinutes = 0;

        [Tooltip("Current seconds in the game.")]
        [SerializeField]
        [Range(0, 59)]
        private float mCurrentSeconds = 0;

        [Tooltip("Text add between each unit.")]
        [SerializeField]
        private string mDelimiterText = " ";

        [Tooltip("Do round up instead of round down.")]
        [SerializeField]
        private bool mRoundUp = false;

        [Tooltip("Hide the number when is zero.")]
        [SerializeField]
        private bool mHideWhenZero = false;

        // -- flags --
        private bool mMinusMinute = false;
        private bool mMinusHour = false;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("- Sound")]

        [Tooltip("Sound played when hours get reduced.")]
        [SerializeField]
        private AudioClip mHourSound = null;

        [Tooltip("Sound played when minutes get reduced.")]
        [SerializeField]
        private AudioClip mMinuteSound = null;

        [Tooltip("Sound played when seconds get reduced.")]
        [SerializeField]
        private AudioClip mSecondSound = null;

        // Track the second changes, so we are able to play the
        // second sound.
        private int mTrackSecond = 0;

        /* Setter & Getter */

        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public bool RoundUp { get { return this.mRoundUp; } set { this.mRoundUp = value; } }
        public bool HideWhenZero { get { return this.mHideWhenZero; } set { this.mHideWhenZero = value; } }
        public string DelimiterText { get { return this.mDelimiterText; } set { this.mDelimiterText = value; } }
        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        public AudioClip HourSound { get { return this.mHourSound; } set { this.mHourSound = value; } }
        public AudioClip MinuteSound { get { return this.mMinuteSound; } set { this.mMinuteSound = value; } }
        public AudioClip SecondSound { get { return this.mSecondSound; } set { this.mSecondSound = value; } }

        /* Functions */

        private void Start()
        {
            UpdateTimeUI();
        }

        private void Update()
        {
            DoTimer();

            DoTimeIsUpCallback();
        }

        /// <summary>
        /// Set the current time.
        /// </summary>
        /// <param name="hour"> hour value. </param>
        /// <param name="minute"> minute value. </param>
        /// <param name="second"> second value. </param>
        public void SetCurrentTime(float hour, float minute, float second)
        {
            this.mCurrentHours = hour;
            this.mCurrentMinutes = minute;
            this.mCurrentSeconds = second;

            /* Set the time in proper range. */

            if (mCurrentHours > MAX_HOUR_TIME)
                mCurrentHours = MAX_HOUR_TIME;
            else if (mCurrentHours < MIN_HOUR_TIME)
                mCurrentHours = MIN_HOUR_TIME;

            if (mCurrentMinutes > MAX_MINUTE_TIME)
                mCurrentMinutes = MAX_MINUTE_TIME;
            else if (mCurrentMinutes < MIN_MINUTE_TIME)
                mCurrentMinutes = MIN_MINUTE_TIME;

            if (mCurrentSeconds > MAX_SECOND_TIME)
                mCurrentSeconds = MAX_SECOND_TIME;
            else if (mCurrentSeconds < MIN_SECOND_TIME)
                mCurrentSeconds = MIN_SECOND_TIME;

            // update GUI
            UpdateTimeUI();

            // reset callback everytime we set to a new time.
            this.mDoTimeUpCallback = false;
        }

        /// <summary>
        /// Check if the time is up?
        /// </summary>
        /// <returns>
        /// true: is time up.
        /// false : have not reach the target time.
        /// </returns>
        public bool IsTimeUp()
        {
            return (mCurrentHours == MIN_HOUR_TIME &&
                mCurrentMinutes == MIN_MINUTE_TIME &&
                mCurrentSeconds == MIN_SECOND_TIME);
        }

        /// <summary>
        /// Update the time UI.
        /// </summary>
        public void UpdateTimeUI()
        {
            UpdateTimeUI(mCurrentHours, mCurrentMinutes, mCurrentSeconds);
        }

        /// <summary>
        /// Update the time UI
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="seocond"></param>
        public void UpdateTimeUI(float hour, float minute, float second)
        {
#if TMP_PRO
            if (mTextLegacy == null && mTextTMP == null)
#else
            if (mTextLegacy == null)
#endif
            {
                JCS_Debug.LogError("Text slot cannot be null references");
                return;
            }

            DoHourUI(hour);
            DoMinuteUI(minute);
            DoSecondUI(second);

            text = mHoursText + mMinutesText + mSecondsText;
        }

        /// <summary>
        /// Do the Hour GUI.
        /// </summary>
        /// <param name="hour"> time to apply. </param>
        private void DoHourUI(float hour)
        {
            if (RoundUp && (hour % 10.0f != 0.0f))
                ++hour;

            if (mHideWhenZero && hour == 0.0f)
                mHoursText = "";
            else
                mHoursText = ((int)hour).ToString() + mDelimiterText;
        }

        /// <summary>
        /// Do the Hour GUI.
        /// </summary>
        private void DoHourUI()
        {
            DoHourUI(mCurrentHours);
        }

        /// <summary>
        /// Do the Minute GUI.
        /// </summary>
        /// <param name="minute"> time to apply. </param>
        private void DoMinuteUI(float minute)
        {
            if (RoundUp && (minute % 10.0f != 0.0f))
                ++minute;

            if (mHideWhenZero && minute == 0.0f)
                mMinutesText = "";
            else
                mMinutesText = ((int)minute).ToString() + mDelimiterText;
        }

        /// <summary>
        /// Do the Minute GUI.
        /// </summary>
        private void DoMinuteUI()
        {
            DoMinuteUI(mCurrentMinutes);
        }

        /// <summary>
        /// Do the Seconds GUI.
        /// </summary>
        /// <param name="second"> time to apply. </param>
        private void DoSecondUI(float second)
        {
            if (RoundUp && (second % 10.0f != 0.0f))
                ++second;

            if (mHideWhenZero && second == 0.0f)
                mSecondsText = "";
            else
                mSecondsText = ((int)second).ToString();
        }

        /// <summary>
        /// Do the Seconds GUI.
        /// </summary>
        private void DoSecondUI()
        {
            DoSecondUI(mCurrentSeconds);
        }

        /// <summary>
        /// Do the timer algorithm
        /// </summary>
        private void DoTimer()
        {
            // check if timer active?
            if (!mActive)
                return;

            mCurrentSeconds -= JCS_Time.ItTime(mTimeType);

            int currentSecond = (int)mCurrentSeconds;
            if (mTrackSecond != currentSecond)
            {
                PlayTimerSound(mSecondSound);
                mTrackSecond = currentSecond;
            }

            if (mCurrentSeconds < MIN_SECOND_TIME)
            {
                // 檢查上面是否還有剩.
                if (mCurrentMinutes == MIN_MINUTE_TIME &&
                    mCurrentHours == MIN_HOUR_TIME)
                {
                    // time is up!
                    mCurrentSeconds = MIN_SECOND_TIME;
                }
                else
                {
                    // set to max time.
                    mCurrentSeconds = MAX_SECOND_TIME;

                    // ready to minus a minute
                    mMinusMinute = true;
                }
            }


            if (mMinusMinute)
            {
                PlayTimerSound(mMinuteSound);
                --mCurrentMinutes;

                if (mCurrentMinutes < MIN_MINUTE_TIME)
                {
                    // 檢查上面是否還有剩.
                    if (mCurrentHours <= MIN_HOUR_TIME)
                        mCurrentMinutes = MIN_MINUTE_TIME;
                    else
                    {
                        mCurrentMinutes = MAX_MINUTE_TIME;

                        // ready to minus a hour
                        mMinusHour = true;
                    }
                }

                // done minus a minute
                mMinusMinute = false;
            }

            if (mMinusHour)
            {
                PlayTimerSound(mHourSound);
                --mCurrentHours;

                if (mCurrentHours <= MIN_HOUR_TIME)
                {
                    mCurrentHours = MIN_HOUR_TIME;
                }

                // done minus a hour
                mMinusHour = false;
            }

            UpdateTimeUI();
        }

        /// <summary>
        /// Do is time up callback.
        /// </summary>
        private void DoTimeIsUpCallback()
        {
            if (!IsTimeUp() || mDoTimeUpCallback)
                return;

            // make sure we only do one time the callback.
            mDoTimeUpCallback = true;

            onTimeUp?.Invoke();
        }

        /// <summary>
        /// Play the tick sound.
        /// </summary>
        /// <param name="clip"></param>
        private void PlayTimerSound(AudioClip clip)
        {
            if (clip == null)
                return;

            JCS_SoundManager sm = JCS_SoundManager.instance;
            sm.GlobalSoundPlayer().PlayOneShot(clip);
        }
    }
}
