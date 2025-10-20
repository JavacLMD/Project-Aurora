using System;
using UnityEngine;

namespace JavacLMD.Utils.Timers
{
    public class IntervalTimer : Timer
    {
        [field: SerializeField]
        public float Interval { get; private set; }

        public Action OnIntervalElapsed;

        private float _nextTriggerTime;

        public IntervalTimer(float interval)
        {
            Interval = Mathf.Max(0.01f, interval); // prevent 0 interval
        }

        protected override void TickInternal(float deltaTime)
        {
            if (ElapsedTime >= _nextTriggerTime)
            {
                _nextTriggerTime += Interval;
                OnIntervalElapsed?.Invoke();
            }
        }

        public void Restart(float? newInterval = null)
        {
            if (newInterval.HasValue)
                Interval = Mathf.Max(0.01f, newInterval.Value);

            _nextTriggerTime = 0f;
            Reset();
            Start();
        }

        public override string ToString() => $"IntervalTimer(every {Interval:0.00}s)";
    }
}