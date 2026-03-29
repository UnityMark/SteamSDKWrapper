using Steamworks;

namespace Mark.Steamworks
{
    public class SteamPlayerData
    {
        public CSteamID Id { get; }
        public string Name { get; }
        public EPersonaState State { get; }

        public SteamPlayerData()
        {
            var id = SteamUser.GetSteamID();
            string name = SteamFriends.GetPersonaName();
            EPersonaState state = SteamFriends.GetPersonaState();

            Id = id;
            Name = name;
            State = state;
        }   
    }
}


