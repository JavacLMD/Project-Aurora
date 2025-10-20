using System;
using UnityEngine;

namespace JavacLMD.Utils.Timers
{
    #region Base Timer

    [Serializable]
    public abstract class BaseTimer : ITimer
    {
        [SerializeField] protected float? _duration;
        [SerializeField] protected float _elapsedTimeInSeconds;
        [SerializeField] protected bool _isRunning;
        [SerializeField] protected bool _isFinished;

        public float? Duration => _duration;
        public float ElapsedTimeInSeconds => _elapsedTimeInSeconds;
        public bool IsRunning => _isRunning;
        public bool IsFinished => _isFinished;

        public event Action OnStart = () => { };
        public event Action OnStop = () => { };
        public event Action OnReset = () => { };
        public event Action<float, float?> OnTick = (_, __) => { };
        public event Action OnComplete = () => { };

        public virtual void Start()
        {
            if (_isRunning || _isFinished) return;
            _isRunning = true;
            TimerManager.Instance.Register(this);
            OnStart.Invoke();
        }

        public virtual void Stop()
        {
            if (!_isRunning) return;
            _isRunning = false;
            TimerManager.Instance.Unregister(this);
            OnStop.Invoke();
        }

        public virtual void Reset()
        {
            _elapsedTimeInSeconds = 0f;
            _isRunning = false;
            _isFinished = false;
            OnReset.Invoke();
        }

        public abstract void Tick(float deltaTime);
    }

    [Serializable]
    public class CountdownTimer : BaseTimer
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
