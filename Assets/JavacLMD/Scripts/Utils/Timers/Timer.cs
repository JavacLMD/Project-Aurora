using System;
using System.Collections.Generic;
using JavacLMD.Utils.Singletons;

namespace JavacLMD.Utils.Timers
{

    #region Interval Timer

    #endregion

    #region Stopwatch Timer

    [Serializable]
    public class StopwatchTimer : BaseTimer
    {
        public StopwatchTimer(float? duration = null)
        {
            _duration = duration;
        }

        public override void Tick(float deltaTime)
        {
            if (!_isRunning || _isFinished) return;

            _elapsedTimeInSeconds += deltaTime;
            OnTick.Invoke(_elapsedTimeInSeconds, _duration);

            if (_duration.HasValue && _elapsedTimeInSeconds >= _duration.Value)
            {
                _isFinished = true;
                Stop();
                OnComplete.Invoke();
            }
        }
    }

    #endregion

   
}
