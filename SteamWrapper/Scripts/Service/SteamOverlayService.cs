using Steamworks;
using System;
using UnityEngine;

namespace Mark.Steamworks
{
    public sealed class SteamOverlayService: SteamComponent
    {
        private Callback<GameOverlayActivated_t> _overlayCallback;
        private string _linkGroup = "https://steamcommunity.com/groups/your_group";

        public event Action OnOverlayOpened;
        public event Action OnOverlayClosed;

        public override void Initialize()
        {
            _overlayCallback = Callback<GameOverlayActivated_t>.Create(OnOverlayChanged);
        }

        public void SteamGroup()
        {
            if (string.IsNullOrEmpty(_linkGroup))
            {
                Debug.LogWarning("[SteamOverlayService] —сылка на группу не установлена!");
                return;
            }

            SteamFriends.ActivateGameOverlayToWebPage(_linkGroup);
        }

        private void OnOverlayChanged(GameOverlayActivated_t callback)
        {
            // m_bActive == 1, если оверлей открыт
            if (callback.m_bActive != 0)
            {
                Debug.Log("[SteamOverlayService] Steam Overlay opened.");
                OnOverlayOpened?.Invoke();
            }
            else
            {
                Debug.Log("[SteamOverlayService] Steam Overlay closed.");
                OnOverlayClosed?.Invoke();
            }
        }

        private void OnDestroy()
        {
            if(_overlayCallback != null)
            {
                _overlayCallback.Dispose();
                _overlayCallback = null;
                Debug.Log("[SteamOverlayService] SteamOverlayService: Callback disposed.");
            }
        }
    }
}
