using System.Collections.Generic;
using JavacLMD.Utils.Attributes;
using JavacLMD.Utils.Singletons;
using UnityEngine;

namespace  JavacLMD.Utils.Timers
{
    public class TimerManager : ScriptableSingleton<TimerManager>, IGameLogicSystem
    {
        [SerializeField, ReadOnly]
        private List<Timer> _timers = new List<Timer>();
        
        
        public void OnStartup()
        {
            _timers.Clear();
        }

        public void OnUpdate(float deltaTime)
        {
            for (int i = _timers.Count - 1; i >= 0; i--)
            {
                _timers[i].Tick(deltaTime);
            }
        }

        public void OnShutdown()
        {
            Clear();
        }


        public void Register(Timer timer)
        {
            if (_timers.Contains(timer)) return;
            _timers.Add(timer);
        }

        public void Unregister(Timer timer)
        {
            _timers.Remove(timer);
        }
        
        public void Clear()
        {
            foreach (var t in _timers)
                t.Dispose();
            _timers.Clear();
        }
        
        
        
    }
}

