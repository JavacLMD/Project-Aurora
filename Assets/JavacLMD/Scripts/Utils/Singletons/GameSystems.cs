using System.Collections.Generic;
using UnityEngine;

namespace JavacLMD.Utils.Singletons
{

    public interface IGameSystemLogic
    {

        void OnSystemRegister();
        void OnSystemUnregister();
        void OnSystemUpdate(float dt);
    }


    public class GameSystems : MonoSingleton<GameSystems>
    {
        [SerializeField] private List<ScriptableObject> _manualSystems = new();
        private List<IGameSystemLogic> _gameSystems = new();

        public void Register<T>(T gameSystem) where T :IGameSystemLogic
        {
            if (_gameSystems.Contains(gameSystem)) return;
            _gameSystems.Add(gameSystem);
            gameSystem.OnSystemRegister();
        }

        public void Unregister<T>(T gameSystem) where T : IGameSystemLogic
        {
            if (_gameSystems.Contains(gameSystem))
            {
                if (_gameSystems.Remove(gameSystem))
                    gameSystem.OnSystemUnregister();
            }
        }

        protected override void OnInitializeSingleton()
        {

            foreach (var sys in _manualSystems)
            {
                if (sys is IGameSystemLogic gameSystem)
                {
                    Register<IGameSystemLogic>(gameSystem);
                }
            }
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            var temp = _gameSystems.ToArray();
            foreach (var sys in temp)
            {
                Unregister<IGameSystemLogic>(sys);
            }
        }

        public IList<IGameSystemLogic> GetActiveSystems() => _gameSystems;


    }

}