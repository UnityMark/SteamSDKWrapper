using UnityEngine;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

namespace Mark.Steamworks
{
    public sealed class SteamBoostrap : MonoBehaviour
    {
        enum SteamService
        {
            Avatar,
            Clan,
            Overlay,
            Presence,
            StatsPlayerOnline,
            Stats
        }

        [SerializeField] private bool _shouldStartAPI;
        [SerializeField] private List<SteamService> _steamService;

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
            foreach (var component in _steamService)
            {
                Type steamComponent = GetServiceType(component);

                if (steamComponent == null) return;

                GameObject gameObject = new GameObject(steamComponent.Name);
                gameObject.transform.SetParent(transform);

                var instance = gameObject.AddComponent(steamComponent) as SteamComponent;

                if (instance != null)
                {
                    instance.Initialize();
                }
                else
                {
                    Destroy(gameObject);
                    return;
                }

                var type = instance.GetType();

                if (_components.ContainsKey(type))
                {
                    Debug.LogWarning($"Компонент типа {type.Name} уже зарегистрирован.");
                    Destroy(gameObject);
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

        private Type GetServiceType(SteamService service) => service switch
        {
            SteamService.Avatar => typeof(SteamAvatarService),
            SteamService.Presence => typeof(SteamPresenceService),
            SteamService.Overlay => typeof(SteamOverlayService),
            SteamService.Clan => typeof(SteamClanService),
            SteamService.StatsPlayerOnline => typeof(SteamStatsPlayerOnlineService),
            SteamService.Stats => typeof(SteamStatsService),
            _ => null,
        };
    }
}


