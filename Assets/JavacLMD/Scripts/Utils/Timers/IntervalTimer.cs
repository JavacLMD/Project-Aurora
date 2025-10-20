using System;
using UnityEngine;

namespace JavacLMD.Utils.Timers
{

    #region Interval Timer

    [Serializable]
    public class IntervalTimer : BaseTimer
    {
        [SerializeField] private float _interval = 1f;
        private float _intervalElapsed;

        public float Interval
        {
            get => _interval;
            set => _interval = Mathf.Max(0.001f, value);
        }

        public IntervalTimer(float interval, float? duration = null)
        {
            _interval = interval;
            _duration = duration;
        }

        public override void Tick(float deltaTime)
        {
            if (!_isRunning || _isFinished) return;

            _elapsedTimeInSeconds += deltaTime;
            _intervalElapsed += deltaTime;

            while (_intervalElapsed >= _interval)
            {
                _intervalElapsed -= _interval;
                OnTick.Invoke(_elapsedTimeInSeconds, _duration);
            }

            if (_duration.HasValue && _elapsedTimeInSeconds >= _duration.Value)
            {
                _isFinished = true;
                Stop();
                OnComplete.Invoke();
            }
        }

        public override void Reset()
        {
            _intervalElapsed = 0f;
            base.Reset();
        }
    }

    #endregion
}
