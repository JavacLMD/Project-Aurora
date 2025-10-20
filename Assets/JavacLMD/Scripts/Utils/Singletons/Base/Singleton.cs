using UnityEngine;

namespace JavacLMD.Utils.Singletons
{
    /// Generic MonoBehaviour Singleton
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _isQuitting;

        [Header("Singleton Options")]
        [Tooltip("If true, this singleton will persist across scene loads.")]
        [SerializeField] private bool _persistAcrossScenes;

        /// <summary>
        /// Override in derived class to define the default persistence behavior.
        /// Used for runtime-created singletons.
        /// </summary>
        protected virtual bool DefaultPersistence => true;

        public static T Instance
        {
            get
            {
                if (_isQuitting) return null;

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();

                        if (_instance == null)
                        {
                            var go = new GameObject($"{typeof(T).Name} (Singleton)");
                            _instance = go.AddComponent<T>();

                            if (_instance is Singleton<T> singleton)
                                singleton.SetPersistence(singleton.DefaultPersistence);
                        }
                    }

                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;

                // Apply inspector or default persistence
                if (!_persistAcrossScenes && this is Singleton<T> singleton)
                    _persistAcrossScenes = singleton.DefaultPersistence;

                if (_persistAcrossScenes)
                    DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Set persistence dynamically at runtime.
        /// </summary>
        public void SetPersistence(bool persist)
        {
            _persistAcrossScenes = persist;

            if (persist)
                DontDestroyOnLoad(gameObject);
            // Cannot undo DontDestroyOnLoad once applied
        }

        protected virtual void OnApplicationQuit()
        {
            _isQuitting = true;
        }
    }

    
    public interface IGameLogicSystem
    {
        void OnStartup();
        void OnUpdate(float deltaTime);
        void OnShutdown();
    }

    /// ScriptableObject Singleton Base
    public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
    {
        private static T _instance;
        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance != null) return _instance;

                    _instance = Resources.Load<T>($"Singletons/{typeof(T).Name}");

#if UNITY_EDITOR
                    if (_instance == null)
                    {
                        _instance = CreateInstance<T>();
                        string path = $"Assets/Resources/Singletons/{typeof(T).Name}.asset";
                        UnityEditor.AssetDatabase.CreateAsset(_instance, path);
                        UnityEditor.AssetDatabase.SaveAssets();
                        Debug.Log($"[ScriptableSingleton] Created asset: {path}");
                    }
#endif

                    if (_instance is IGameLogicSystem gameLogicSystem)
                        GameLogicManager.Instance.Add(gameLogicSystem);
                    
                    return _instance;
                }
            }
        }
    }
}
