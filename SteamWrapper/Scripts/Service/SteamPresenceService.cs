using Steamworks;

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
            SteamFriends.SetRichPresence("steam_display", "#Status_MainMenu");
        }

        public void SetInGameStatus(int level)
        {
            SteamFriends.SetRichPresence("level", $"{level}");
            SteamFriends.SetRichPresence("steam_display", "#Status_Level");
        }

        public void ClearStatus()
        {
            SteamFriends.ClearRichPresence();
        }

        private void OnApplicationQuit()
        {
            ClearStatus();
        }
    }
}

/* Example Rich File

"lang"
{
	"Language"	"english"
	"Tokens"
	{
		"#Status_MainMenu"	"In the main menu"
		"#Status_Level"		"Playing level %level%"
	}
}

*/