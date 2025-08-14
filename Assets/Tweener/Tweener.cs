using System;
using JCSUnity;

namespace PeterVuorela.Tweener
{
    /// <summary>
    /// Tweening, handle progression.
    /// </summary>
    public class Tweener
    {
        /* Variables */

        public Action onDone = null;

        private TweenDelegate _Easing = null;

        private float _From = 0.0f;
        private float _To = 0.0f;

        private float _ProgressPct = 0.0f;

        private bool _Animating = false;
        private float _TimeElapsed = 0.0f;
        private float _Duration = 1.0f;
        private float _Progression = 0.0f;

        private JCS_TimeType _DeltaTimeType = JCS_TimeType.DELTA_TIME;

        /* Setter & Getter */

        public bool animating { get { return _Animating; } }
        public float progression { get { return _Progression; } }
        public float from { get { return _From; } set { _From = value; } }
        public float to { get { return _To; } set { _To = value; } }
        public float progressPct { get { return _ProgressPct; } }
        public JCS_TimeType timeType { get { return this._DeltaTimeType; } }

        /* Functions */

        public Tweener() { }

        /// <summary>
        /// Eases from value to value.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="easing">Easing.</param>
        public void easeFromTo(float from, float to,
            bool resetElapsedTime = true,
            float duration = 1.0f,
            TweenDelegate easing = null, Action callback = null,
            JCS_TimeType deltaTimeType = JCS_TimeType.DELTA_TIME)
        {
            if (easing == null)
                easing = Easing.Linear;

            _Easing = easing;
            onDone = callback;

            _From = from;
            _To = to;

            _Duration = duration;
            _ProgressPct = 0.0f;
            _Animating = true;

            if (resetElapsedTime)
                _TimeElapsed = 0.0f;

            _DeltaTimeType = deltaTimeType;
        }

        public void update(bool callCallBack = true)
        {
            if (!_Animating)
                return;

            if (_TimeElapsed < _Duration)
            {
                if (_Easing == null)
                    return;

                _Progression = _Easing.Invoke(_TimeElapsed, _From, (_To - _From), _Duration);

                _ProgressPct = _TimeElapsed / _Duration;

                _TimeElapsed += JCS_Time.ItTime(_DeltaTimeType);
            }
            else
            {
                _Progression = _To;

                _Animating = false;
                _TimeElapsed = 0.0f;
                _ProgressPct = 1.0f;

                if (callCallBack)
                    onDone?.Invoke();
            }
        }

        public void update(ref float whatToTween)
        {
            bool wasAnimating = _Animating;
            update(false);
            whatToTween = _Progression;

            if (wasAnimating && !_Animating)
            {
                onDone?.Invoke();
            }
        }

        /// <summary>
        /// Reset tweener effect setting.
        /// </summary>
        public void ResetTweener()
        {
            _Progression = _To;
            _Animating = false;
            _TimeElapsed = 0.0f;
            _ProgressPct = 1.0f;
        }

        /// <summary>
        /// Invoke the callback.
        /// </summary>
        public void DoCallback()
        {
            onDone?.Invoke();
        }
    }
}
