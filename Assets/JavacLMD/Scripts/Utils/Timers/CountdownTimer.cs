using System;

namespace JavacLMD.Utils.Timers
{

    #region Countdown Timer

    [Serializable]
    public class CountdownTimer : ITimer
    {
        public CountdownTimer(float? duration)
        {
            _duration = duration;
            _elapsedTimeInSeconds = duration ?? 0f;
        }

        public override void Tick(float deltaTime)
        {
            if (!_isRunning || _isFinished) return;

            _elapsedTimeInSeconds -= deltaTime;
            OnTick.Invoke(_elapsedTimeInSeconds, _duration);

            if (_duration.HasValue && _elapsedTimeInSeconds <= 0f)
            {
                _elapsedTimeInSeconds = 0f;
                _isFinished = true;
                Stop();
                OnComplete.Invoke();
            }
        }

        public override void Reset()
        {
            _elapsedTimeInSeconds = _duration ?? 0f;
            base.Reset();
        }
    }

    #endregion
}
