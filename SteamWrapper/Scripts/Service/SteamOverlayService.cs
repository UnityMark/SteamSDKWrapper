using Steamworks;
using System;

namespace Mark.Steamworks
{
    public sealed class SteamOverlayService: SteamComponent
    {
        private Callback<GameOverlayActivated_t> _overlayCallback;
        private string _linkGroup = "";

        public event Action OnOverlayOpened;
        public event Action OnOverlayClosed;

        public override void Initialize()
        {
            _overlayCallback = Callback<GameOverlayActivated_t>.Create(OnOverlayChanged);
        }

        public void SteamGroup()
        {
            SteamFriends.ActivateGameOverlayToWebPage(_linkGroup);
        }

        private void OnOverlayChanged(GameOverlayActivated_t callback)
        {
            if (callback.m_bActive != 0) OnOverlayOpened?.Invoke();
            else OnOverlayClosed?.Invoke();
        }
    }
}
