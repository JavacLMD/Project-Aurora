using System;
using UnityEngine;

namespace JavacLMD.Utils.Timers
{
    public class CountdownTimer : Timer
    {
        public float Duration { get; private set; }
        public Action<float, float> OnCountdownTick;
        public Action OnComplete;

        private bool _hasCompleted;

        public CountdownTimer(float duration) : base()
        {
            Duration = duration;
        }
        
        
        protected override void TickInternal(float deltaTime)
        {
            float remaining = Duration - ElapsedTime;
            OnCountdownTick?.Invoke(Mathf.Max(remaining, 0f), Duration);

            if (!_hasCompleted && ElapsedTime >= Duration)
            {
                _hasCompleted = true;
                OnComplete?.Invoke();
                Stop();
            }
        }

        public void Restart(float? newDuration = null)
        {
            if (newDuration.HasValue)
                Duration = newDuration.Value;
            _hasCompleted = false;
            Reset();
            Start();
        }

    }
}