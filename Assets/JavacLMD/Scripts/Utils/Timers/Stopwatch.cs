using System;

namespace JavacLMD.Utils.Timers
{
    public class StopwatchTimer : Timer
    {
        public StopwatchTimer() : base() { }

        public Action<float> OnStopwatchTick;
        
        protected override void TickInternal(float deltaTime)
        {
            OnStopwatchTick?.Invoke(ElapsedTime);
        }

        public void ResetStopwatch(bool stop = false)
        {
            Reset();
            if (stop) Stop();
        }
    }

}