/**
 * $File: JCS_SpriteTimer.cs $
 * $Date: 2017-03-10 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System;
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Timer system using sprite 0 to 9.
    /// </summary>
    public class JCS_SpriteTimer : MonoBehaviour
    {
        /* Variables */

        public Action onTimeUp = null;

        private const float MAX_HOUR_TIME = 23.0f;
        private const float MAX_MINUTE_TIME = 59.0f;
        private const float MAX_SECOND_TIME = 60.0f;

        private const float MIN_HOUR_TIME = 0.0f;
        private const float MIN_MINUTE_TIME = 0.0f;
        private const float MIN_SECOND_TIME = 0.0f;

        [Separator("Check Variables (JCS_SpriteTimer)")]

        [SerializeField]
        [ReadOnly]
        private bool mDoTimeUpCallback = false;

        [Separator("Runtime Variables (JCS_SpriteTimer)")]

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

        [Tooltip("Do round up instead of round down.")]
        [SerializeField]
        private bool mRoundUp = false;

        [Tooltip("Type of the delta time.")]
        [SerializeField]
        private JCS_TimeType mTimeType = JCS_TimeType.DELTA_TIME;

        [Header("- Sprite Slots")]

        [Tooltip("Time text sprite 0.")]
        [SerializeField]
        private Sprite mTimeText0 = null;
        [Tooltip("Time text sprite 1.")]
        [SerializeField]
        private Sprite mTimeText1 = null;
        [Tooltip("Time text sprite 2.")]
        [SerializeField]
        private Sprite mTimeText2 = null;
        [Tooltip("Time text sprite 3.")]
        [SerializeField]
        private Sprite mTimeText3 = null;
        [Tooltip("Time text sprite 4.")]
        [SerializeField]
        private Sprite mTimeText4 = null;
        [Tooltip("Time text sprite 5.")]
        [SerializeField]
        private Sprite mTimeText5 = null;
        [Tooltip("Time text sprite 6.")]
        [SerializeField]
        private Sprite mTimeText6 = null;
        [Tooltip("Time text sprite 7.")]
        [SerializeField]
        private Sprite mTimeText7 = null;
        [Tooltip("Time text sprite 8.")]
        [SerializeField]
        private Sprite mTimeText8 = null;
        [Tooltip("Time text sprite 9.")]
        [SerializeField]
        private Sprite mTimeText9 = null;

        [Header("- Sprite")]

        [Tooltip("Each digit for hour.")]
        [SerializeField]
        private JCS_UnityObject mDigitHour1 = null;

        [Tooltip("Each digit for hour.")]
        [SerializeField]
        private JCS_UnityObject mDigitHour2 = null;

        [Tooltip("Each digit for minute.")]
        [SerializeField]
        private JCS_UnityObject mDigitMinute1 = null;

        [Tooltip("Each digit for minute.")]
        [SerializeField]
        private JCS_UnityObject mDigitMinute2 = null;

        [Tooltip("Each digit for second.")]
        [SerializeField]
        private JCS_UnityObject mDigitSecond1 = null;

        [Tooltip("Each digit for second.")]
        [SerializeField]
        private JCS_UnityObject mDigitSecond2 = null;

        [Header("- Spacing")]

        [Tooltip("Interval between each digit.")]
        [SerializeField]
        [Range(0.1f, 5000.0f)]
        private float mDigitInterval = 0.5f;

        [Tooltip("Interval between each unit digit.")]
        [SerializeField]
        [Range(0.1f, 5000.0f)]
        private float mDigitUnitInterval = 0.5f;

        // last interval we updated.
        private float mLastInterval = 0;

        // -- flags --
        private bool mMinusMinute = false;
        private bool mMinusHour = false;

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
        public JCS_TimeType DeltaTimeType { get { return this.mTimeType; } set { this.mTimeType = value; } }

        public AudioClip HourSound { get { return this.mHourSound; } set { this.mHourSound = value; } }
        public AudioClip MinuteSound { get { return this.mMinuteSound; } set { this.mMinuteSound = value; } }
        public AudioClip SecondSound { get { return this.mSecondSound; } set { this.mSecondSound = value; } }

        /* Functions */

        private void Start()
        {
            UpdateTimeInterval();

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
        /// Update the Time GUI base on this particular order.
        /// </summary>
        public void UpdateTimeInterval()
        {
            UpdateSecondInterval();
            UpdateMinuteInterval();
            UpdateHourInterval();
        }

        /// <summary>
        /// Update hour interval.
        /// </summary>
        public void UpdateHourInterval()
        {
            if (mDigitHour1 == null || mDigitHour2 == null)
            {
                JCS_Debug.LogError("Digit slot cannot be null references");
                return;
            }

            float unitInterval = mDigitUnitInterval - mLastInterval;

            Vector3 newPos = mDigitHour1.transform.localPosition;
            newPos.x += -(mDigitInterval * 0) - unitInterval;
            mDigitHour1.transform.localPosition = newPos;

            newPos = mDigitHour2.transform.localPosition;
            newPos.x += -(mDigitInterval * 1) - unitInterval;
            mDigitHour2.transform.localPosition = newPos;

            // record down the last digit's localPosition.
            mLastInterval = newPos.x;
        }

        /// <summary>
        /// Update minute interval.
        /// </summary>
        public void UpdateMinuteInterval()
        {
            if (mDigitMinute1 == null || mDigitMinute2 == null)
            {
                JCS_Debug.LogError("Digit slot cannot be null references");
                return;
            }

            float unitInterval = mDigitUnitInterval - mLastInterval;

            Vector3 newPos = mDigitMinute1.transform.localPosition;
            newPos.x += -(mDigitInterval * 0) - unitInterval;
            mDigitMinute1.transform.localPosition = newPos;

            newPos = mDigitMinute2.transform.localPosition;
            newPos.x += -(mDigitInterval * 1) - unitInterval;
            mDigitMinute2.transform.localPosition = newPos;

            // record down the last digit's localPosition.
            mLastInterval = newPos.x;
        }

        /// <summary>
        /// Update second interval.
        /// </summary>
        public void UpdateSecondInterval()
        {
            if (mDigitSecond1 == null || mDigitSecond2 == null)
            {
                JCS_Debug.LogError("Digit slot cannot be null references");
                return;
            }

            float unitInterval = (mDigitUnitInterval * 0);

            Vector3 newPos = mDigitSecond1.transform.localPosition;
            newPos.x += -(mDigitInterval * 0) - unitInterval;
            mDigitSecond1.transform.localPosition = newPos;

            newPos = mDigitSecond2.transform.localPosition;
            newPos.x += -(mDigitInterval * 1) - unitInterval;
            mDigitSecond2.transform.localPosition = newPos;

            // record down the last digit's localPosition.
            mLastInterval = newPos.x;
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
            DoHourUI(hour);
            DoMinuteUI(minute);
            DoSecondUI(second);
        }

        /// <summary>
        /// Do the Hour GUI.
        /// </summary>
        /// <param name="hour"> time to apply. </param>
        private void DoHourUI(float hour)
        {
            if (mDigitHour1 == null || mDigitHour2 == null)
            {
                JCS_Debug.LogError("Digit slot cannot be null references");
                return;
            }

            if (RoundUp && (hour % 10.0f != 0.0f))
                ++hour;

            int valDigit = JCS_Mathf.GetSingleDigit(1, (int)hour);
            mDigitHour1.LocalSprite = GetSingleDigitSprite(valDigit);

            valDigit = JCS_Mathf.GetSingleDigit(2, (int)hour);
            mDigitHour2.LocalSprite = GetSingleDigitSprite(valDigit);
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
            if (mDigitMinute1 == null || mDigitMinute2 == null)
            {
                JCS_Debug.LogError("Digit slot cannot be null references");
                return;
            }

            if (RoundUp && (minute % 10.0f != 0.0f))
                ++minute;

            int valDigit = JCS_Mathf.GetSingleDigit(1, (int)minute);
            mDigitMinute1.LocalSprite = GetSingleDigitSprite(valDigit);

            valDigit = JCS_Mathf.GetSingleDigit(2, (int)minute);
            mDigitMinute2.LocalSprite = GetSingleDigitSprite(valDigit);
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
            if (mDigitSecond1 == null || mDigitSecond2 == null)
            {
                JCS_Debug.LogError("Digit slot cannot be null references");
                return;
            }

            if (RoundUp && (second % 10.0f != 0.0f))
                ++second;

            int valDigit = JCS_Mathf.GetSingleDigit(1, (int)second);
            mDigitSecond1.LocalSprite = GetSingleDigitSprite(valDigit);

            valDigit = JCS_Mathf.GetSingleDigit(2, (int)second);
            mDigitSecond2.LocalSprite = GetSingleDigitSprite(valDigit);
        }

        /// <summary>
        /// Do the Seconds GUI.
        /// </summary>
        private void DoSecondUI()
        {
            DoSecondUI(mCurrentSeconds);
        }

        // <summary>
        /// Return a digit sprite.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private Sprite GetSingleDigitSprite(int num)
        {
            switch (num)
            {
                case 0: return mTimeText0;
                case 1: return mTimeText1;
                case 2: return mTimeText2;
                case 3: return mTimeText3;
                case 4: return mTimeText4;
                case 5: return mTimeText5;
                case 6: return mTimeText6;
                case 7: return mTimeText7;
                case 8: return mTimeText8;
                case 9: return mTimeText9;
            }

            // default return 'zero' sprite.
            return mTimeText0;
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

            DoSecondUI();


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

                DoMinuteUI();

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

                DoHourUI();

                // done minus a hour
                mMinusHour = false;
            }

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
