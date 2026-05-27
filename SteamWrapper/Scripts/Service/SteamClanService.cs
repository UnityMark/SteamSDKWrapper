using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mark.Steamworks
{
    public sealed class SteamClanService : SteamComponent
    {
        [Serializable]
        public struct ClanData
        {
            public CSteamID Id;
            public string Name;
            public string Tag; 
        }

        private List<ClanData> _cachedClans = new List<ClanData>();

        public IReadOnlyList<ClanData> CachedClans => _cachedClans;

        public override void Initialize()
        {
            RefreshClanCache();
        }

        /// <summary>
        /// Собирает данные о группах один раз и сохраняет в листе.
        /// </summary>
        public void RefreshClanCache()
        {
            _cachedClans.Clear();

            int clanCount = SteamFriends.GetClanCount();
            Debug.Log($"[SteamClanService] Найдено групп: {clanCount}");

            for (int i = 0; i < clanCount; i++)
            {
                CSteamID clanId = SteamFriends.GetClanByIndex(i);

                _cachedClans.Add(new ClanData
                {
                    Id = clanId,
                    Name = SteamFriends.GetClanName(clanId),
                    Tag = SteamFriends.GetClanTag(clanId)
                });
            }
        }

        /// <summary>
        /// Поиск группы по имени в кэше.
        /// </summary>
        public ClanData? GetClanByName(string name)
        {
            return _cachedClans.Find(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}