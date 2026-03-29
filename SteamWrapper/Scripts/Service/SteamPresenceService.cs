using Steamworks;
using UnityEngine;

namespace Mark.Steamworks
{
    public sealed class SteamPresenceService : SteamComponent
    {
        public override void Initialize()
        {
            SetMenuStatus();
        }

        public void SetMenuStatus()
        {
            SteamFriends.SetRichPresence("status", "In menu");
            SteamFriends.SetRichPresence("steam_display", "#Status");
        }

        public void SetInGameStatus(string levelName = null)
        {
            SteamFriends.SetRichPresence("status", levelName == null ? "Playing" : $"Playing {levelName}");
            SteamFriends.SetRichPresence("steam_display", "#Status");
        }

        public void ClearStatus()
        {
            SteamFriends.ClearRichPresence();
        }
    }
}
