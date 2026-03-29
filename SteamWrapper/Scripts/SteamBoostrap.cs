using UnityEngine;
using System.Collections.Generic;
using System;

namespace Mark.Steamworks
{
    public sealed class SteamBoostrap : MonoBehaviour
    {
        [SerializeField] private bool _shouldStartAPI;
        [SerializeField] private List<SteamComponent> _componentPrefabs;

        private Dictionary<Type, SteamComponent> _components = new Dictionary<Type, SteamComponent>();

        public bool ShouldStartApi => _shouldStartAPI;
        public bool IsInitialized { get; private set; }
        public SteamPlayerData PlayerService { get; private set; }

        public void Initialize()
        {
            if (!ShouldStartApi) return;

            IsInitialized = SteamWrapperManager.Initialized;

            if (!IsInitialized)
            {
                Debug.LogWarning("Steam API is not initialized. Steam features are disabled.");
                return;
            }
            else
            {
                Debug.Log("Steam API initialized.");
            }

            PlayerService = new SteamPlayerData();

            LaunchComponents();
        }

        public void LaunchComponents()
        {
            foreach (var component in _componentPrefabs)
            {
                var instance = Instantiate(component, transform);
                instance.Initialize();

                var type = instance.GetType();

                if (_components.ContainsKey(type))
                {
                    Debug.LogWarning($"Компонент типа {type.Name} уже зарегистрирован.");
                    continue;
                }

                _components.Add(type, instance);
            }
        }

        public T GetSteamComponent<T>() where T : SteamComponent
        {
            if (_components.TryGetValue(typeof(T), out var component))
            {
                return component as T;
            }

            Debug.LogError($"Компонент типа {typeof(T).Name} не найден.");
            return null;
        }
    }
}


