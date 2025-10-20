using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JavacLMD.Utils.Singletons
{

    /// <summary>
    /// Singleton pattern for ScriptableObject assets.
    /// Auto-loads from Resources or creates one in the Editor if missing.
    /// </summary>
    public abstract class ScriptableSingleton<T> : ScriptableObject, ISingleton where T : ScriptableSingleton<T>
    {
        private static T _instance;
        private static readonly object _lock = new object();
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
                        _instance = Resources.Load<T>(typeof(T).Name);

#if UNITY_EDITOR
                        if (_instance == null)
                        {
                            // Create a new asset for editor usage
                            _instance = CreateInstance<T>();
                            string assetPath = $"Assets/Resources/Singletons/{typeof(T).Name}.asset";
                            System.IO.Directory.CreateDirectory("Assets/Resources/Singletons");
                            AssetDatabase.CreateAsset(_instance, assetPath);
                            AssetDatabase.SaveAssets();
                            Debug.Log($"[ScriptableSingleton] Created new asset at {assetPath}");
                        }
#endif
                        _instance.InitSingleton();
                    }
                }

                return _instance;
            }
        }

        public static bool IsInitialized => _isInitialized;

        public void InitSingleton()
        {
            if (_isInitialized) return;
            OnInitializeSingleton();
            _isInitialized = true;
        }

        /// <summary>
        /// Override this to add initialization logic for your asset.
        /// </summary>
        protected virtual void OnInitializeSingleton() { }
    }
}