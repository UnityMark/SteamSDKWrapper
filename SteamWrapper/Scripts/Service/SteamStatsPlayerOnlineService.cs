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
        }

        private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t callback, bool IsFailure)
        {
            if (callback.m_bSuccess != 1 || IsFailure)
            {
                Debug.LogWarning("There was an error retrieving the NumberOfCurrentPlayers.");
            }
            else
            {
                _currentPlayerCount = callback.m_cPlayers;
            }
        }
    }
}
