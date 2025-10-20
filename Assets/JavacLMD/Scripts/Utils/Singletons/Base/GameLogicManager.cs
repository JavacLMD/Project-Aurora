using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JavacLMD.Utils.Singletons
{
    
    /// <summary>
    /// This singleton handles the execution of scriptable object logic systems that have an update loop
    /// </summary>
    public class GameLogicManager : Singleton<GameLogicManager>
    {
        [Header("Optional: assign logic assets manually")]
        [SerializeField] private List<ScriptableObject> _manualSystemAssets = new();

        [Header("Persistence Options")]
        [SerializeField] private bool _persistAcrossScenes = true;

        protected override bool DefaultPersistence => true;

        private readonly List<IGameLogicSystem> _logicSystems = new();

        protected override void Awake()
        {
            SetPersistence(_persistAcrossScenes);
            base.Awake();

            DiscoverAndRegisterSystems();
            Startup();
        }

        private void DiscoverAndRegisterSystems()
        {
            _logicSystems.Clear();

            // 1. Manual assignment
            foreach (var asset in _manualSystemAssets)
            {
                if (asset is IGameLogicSystem system)
                    _logicSystems.Add(system);
            }

            // 2. Auto-discover ScriptableObject systems in Resources
            var discovered = Resources.LoadAll<ScriptableObject>("")
                .OfType<IGameLogicSystem>()
                .Where(s => !_logicSystems.Contains(s));

            foreach (var system in discovered)
            {
                _logicSystems.Add(system);
                Debug.Log($"[GameLogicManager] Auto-registered: {system.GetType().Name}");
            }

            if (_logicSystems.Count == 0)
                Debug.LogWarning("[GameLogicManager] No logic systems found!");
        }

        private void Startup()
        {
            foreach (var system in _logicSystems)
                system.OnStartup();
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            foreach (var system in _logicSystems)
                system.OnUpdate(dt);
        }

        private void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            Shutdown();
        }

        private void Shutdown()
        {
            foreach (var system in _logicSystems)
                system.OnShutdown();
        }
        
        
        
    }
}