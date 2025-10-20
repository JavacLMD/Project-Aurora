using System;

namespace JavacLMD.Utils.Singletons
{
    /// <summary>
    /// Thread-safe singleton for non-MonoBehaviour classes that supports initialization.
    /// </summary>
    public abstract class StaticSingleton<T> where T : StaticSingleton<T>, new()
    {
        private static readonly object _lock = new object();
        private static T _instance;
        private static bool _isInitialized;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                        _instance.InternalInitialize();
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Returns true if the singleton has been initialized.
        /// </summary>
        public static bool IsInitialized => _isInitialized;

        /// <summary>
        /// Called automatically once during instance creation.
        /// Override this for initialization logic.
        /// </summary>
        protected virtual void OnInitializeSingleton() { }

        private void InternalInitialize()
        {
            if (_isInitialized) return;
            OnInitialize();
            _isInitialized = true;
        }

        /// <summary>
        /// Optional manual reinitialization or early initialization.
        /// </summary>
        public static void Initialize()
        {
            _ = Instance; // triggers creation and initialization
        }
    }


}
