using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mark.Steamworks
{
    public sealed class SteamStatsService : SteamComponent
    {
        [SerializeField] private List<SteamStatDefinition> _stats;
        [SerializeField] private List<SteamAchievementDefinition> _achievements;

        private Callback<UserStatsReceived_t> _statsReceived;

        protected Callback<UserStatsStored_t> m_UserStatsStored;
        protected Callback<UserAchievementStored_t> m_UserAchievementStored;

        public override void Initialize()
        {
            _statsReceived = Callback<UserStatsReceived_t>.Create(OnStatsReceived);
            m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
            m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);
        }

        private void OnStatsReceived(UserStatsReceived_t data)
        {
            if (data.m_eResult != EResult.k_EResultOK) return;
        }

        public void UnlockAchievement(SteamAchievementDefinition achievement)
        {
            if (achievement == null) return;
            if (achievement.UnlockType != AchievementUnlockType.Manual) return;

            SteamUserStats.GetAchievement(achievement.AchievementId, out bool unlocked);

            if (unlocked) return;

            SteamUserStats.SetAchievement(achievement.AchievementId);
            SteamUserStats.StoreStats();
        }

        public void AddStat(SteamStatDefinition stat, int amount)
        {
            if (stat == null) return;

            if (!SteamUserStats.GetStat(stat.StatId, out int currentValue))
            {
                Debug.LogWarning($"Steam stat '{stat.StatId}' was not found.");
                return;
            }

            currentValue += amount;

            SteamUserStats.SetStat(stat.StatId, currentValue);
            CheckAchievements(stat, currentValue);
            SteamUserStats.StoreStats();
        }

        private void CheckAchievements(SteamStatDefinition stat, int currentValue)
        {
            foreach (var achievement in _achievements)
            {
                if (achievement == null)  continue;

                if (achievement.UnlockType != AchievementUnlockType.ByStat) continue;

                if (achievement.LinkedStat == null)
                {
                    Debug.LogWarning($"Achievement '{achievement.AchievementId}' has ByStat type, but LinkedStat is null.");
                    continue;
                }

                if (achievement.LinkedStat != stat) continue;

                if (currentValue < achievement.UnlockValue) continue;

                SteamUserStats.GetAchievement(achievement.AchievementId, out bool unlocked);

                if (unlocked) continue;

                SteamUserStats.SetAchievement(achievement.AchievementId);
            }
        }

        private void OnAchievementStored(UserAchievementStored_t callback)
        {
            if (0 == callback.m_nMaxProgress)
            {
                Debug.Log("Achievement '" + callback.m_rgchAchievementName + "' unlocked!");
            }
            else
            {
                Debug.Log("Achievement '" + callback.m_rgchAchievementName + "' progress callback, (" + callback.m_nCurProgress + "," + callback.m_nMaxProgress + ")");
            }
        }

        private void OnUserStatsStored(UserStatsStored_t callback)
        {
            if(EResult.k_EResultOK == callback.m_eResult)
            {
                Debug.Log("StoreStats - success");
                return;
            }

            Debug.LogWarning("StoreStats - failed");
        }
    }
}
