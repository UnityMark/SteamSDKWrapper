using Steamworks;
using UnityEngine;

namespace Mark.Steamworks
{
    public sealed class SteamStatsPlayerOnlineService : SteamComponent
    {
        private int _currentPlayerCount = 0;
        private CallResult<NumberOfCurrentPlayers_t> m_NumberOfCurrentPlayers;

        public int CurrentPlayerCount => _currentPlayerCount;

        public override void Initialize()
        {
            m_NumberOfCurrentPlayers = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumberOfCurrentPlayers);
            UpdatePlayerCount();
        }

        [ContextMenu("Update count players")]
        public void UpdatePlayerCount()
        {
            // Отправляем запрос в Steam
            SteamAPICall_t handle = SteamUserStats.GetNumberOfCurrentPlayers();

            // Привязываем наш CallResult к конкретному вызову
            m_NumberOfCurrentPlayers.Set(handle);
        }

        private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t callback, bool isFailure)
        {
            if (isFailure || callback.m_bSuccess != 1)
            {
                Debug.LogWarning("[SteamStatsPlayerOnlineService] Ошибка при получении количества игроков.");
                return;
            }

            _currentPlayerCount = callback.m_cPlayers;
            Debug.Log($"[SteamStatsPlayerOnlineService] Игроков онлайн: {_currentPlayerCount}");
        }

        private void OnDestroy()
        {
            // Обязательно освобождаем ресурсы, чтобы избежать крэшей
            if (m_NumberOfCurrentPlayers != null)
            {
                m_NumberOfCurrentPlayers.Dispose();
                m_NumberOfCurrentPlayers = null;
            }
        }
    }
}
