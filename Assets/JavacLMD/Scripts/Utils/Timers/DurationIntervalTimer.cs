using System;
using UnityEngine;

namespace JavacLMD.Utils.Timers
{
    [Serializable]
    public class DurationIntervalTimer : IntervalTimer
    {
        [field: SerializeField]
        public float Duration { get; private set; }

        public Action OnComplete;

        private bool _hasCompleted;

        public DurationIntervalTimer(float interval, float duration) 
            : base(interval)
        {
            Duration = Mathf.Max(0.01f, duration);
        }

        protected override void TickInternal(float deltaTime)
        {
            base.TickInternal(deltaTime); // still triggers intervals

            if (!_hasCompleted && ElapsedTime >= Duration)
            {
                _hasCompleted = true;
                OnComplete?.Invoke();
                Stop();
            }
        }

        public void Restart(float? newInterval = null, float? newDuration = null)
        {
            if (newInterval.HasValue)
                base.Restart(newInterval);
            if (newDuration.HasValue)
                Duration = Mathf.Max(0.01f, newDuration.Value);

            _hasCompleted = false;
            Reset();
            Start();
        }

        public override string ToString() => $"DurationIntervalTimer({Interval:0.00}s interval, {Duration:0.00}s duration)";
    }
}