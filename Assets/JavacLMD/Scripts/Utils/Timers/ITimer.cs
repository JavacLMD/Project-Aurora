using System;

namespace JavacLMD.Utils.Timers
{
    public interface ITimer
    {
        event Action OnStart;
        event Action OnStop;
        event Action OnReset;
        event Action<float, float?> OnTick; // elapsedTime, optional duration
        event Action OnComplete;

        float ElapsedTimeInSeconds { get; }
        float? Duration { get; } // nullable
        bool IsRunning { get; }
        bool IsFinished { get; }

        void Start();
        void Stop();
        void Reset();
        void Tick(float deltaTime);
    }



}