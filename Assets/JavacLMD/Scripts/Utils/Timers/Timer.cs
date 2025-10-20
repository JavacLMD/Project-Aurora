using System;

namespace JavacLMD.Utils.Timers
{
    public abstract class Timer : IDisposable
    {
        public Action OnStart, OnStop, OnReset;

        public float TimeScale = 1;
        private float _elapsedTime;
        private bool _isActive;
        
        public bool IsRunning => _isActive;
        public float ElapsedTime => _elapsedTime;

        public Timer()
        {
            _isActive = false;
            _elapsedTime = 0;
            TimerManager.Instance.Register(this);
        }
        
        public void Start()
        {
            if (_isActive) return;
            _isActive = true;

            OnStart?.Invoke();
        }

        public void Stop(bool reset = false)
        {
            if (!_isActive) return;
            _isActive = false;
            OnStop?.Invoke();
            if (reset)
            {
                Reset();
            }
        }

        public void Reset()
        {
            _elapsedTime = 0;
            OnReset?.Invoke();
        }
        
        public void Pause() => _isActive = false;
        public void Resume() => _isActive = true;
        

        internal void Tick(float deltaTime)
        {
            if (!_isActive) return;
            deltaTime *= TimeScale;
            
            _elapsedTime += deltaTime;
            TickInternal(deltaTime);
        }
        

        protected virtual void TickInternal(float deltaTime)
        {
            
        }


        ~Timer()
        {
            Dispose(false);
        }

        bool disposed = false;
        
        // Call Dispose to ensure deregistration of the timer from the TimerManager
        // when the consumer is done with the timer or being destroyed
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposed) return;

            if (disposing)
            {
                TimerManager.Instance.Unregister(this);
            }

            disposed = true;
        }
    }
}