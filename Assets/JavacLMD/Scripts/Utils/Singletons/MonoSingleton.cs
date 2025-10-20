using UnityEngine;

namespace JavacLMD.Utils.Singletons
{

    public interface ISingleton
    {
        void InitSingleton();
    }

    /// <summary>
    /// Generic MonoBehaviour Singleton with optional persistence and initialization.
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _isInitialized;
        private static bool _isQuitting;

        [Header("MonoSingleton Settings")]
        [Tooltip("If true, this singleton persists across scene loads.")]
        [SerializeField] private bool _dontDestroyOnLoad = true;

        public static T Instance
        {
            get
            {
                if (_isQuitting) return null;

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindFirstObjectByType<T>();

                        if (_instance == null)
                        {
                            var obj = new GameObject(typeof(T).Name);
                            _instance = obj.AddComponent<T>();
                        }

                        if (_instance._dontDestroyOnLoad)
                            DontDestroyOnLoad(_instance.gameObject);
                    }

                    return _instance;
                }
            }
        }

        public static bool IsInitialized => _isInitialized;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = (T)this;
                if (_dontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
                InitSingleton();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnApplicationQuit() => _isQuitting = true;

        /// <summary>
        /// Called automatically once per instance, or can be manually called for deferred init.
        /// </summary>
        public void InitSingleton()
        {
            if (_isInitialized) return;
            OnInitializeSingleton();
            _isInitialized = true;
        }

        /// <summary>
        /// Override this to add custom startup logic.
        /// </summary>
        protected virtual void OnInitializeSingleton() { }
    }
}