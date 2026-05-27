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
        private Callback<UserStatsStored_t> m_UserStatsStored;
        private Callback<UserAchievementStored_t> m_UserAchievementStored;

        public override void Initialize()
        {
            _statsReceived = Callback<UserStatsReceived_t>.Create(OnStatsReceived);
            m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
            m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);
        }

        public void UnlockAchievement(SteamAchievementDefinition achievement)
        {
            if (achievement == null || achievement.UnlockType != AchievementUnlockType.Manual) return;

            if (SteamUserStats.GetAchievement(achievement.AchievementId, out bool unlocked) && !unlocked)
            {
                SteamUserStats.SetAchievement(achievement.AchievementId);
                SteamUserStats.StoreStats();
            }
        }

        public void AddStat(SteamStatDefinition stat, int amount)
        {
            if (stat == null) return;

            if (!SteamUserStats.GetStat(stat.StatId, out int currentValue))
            {
                Debug.LogWarning($"[SteamStatsService] Stat '{stat.StatId}' not found on Steam Dashboard.");
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
                if (achievement == null || achievement.UnlockType != AchievementUnlockType.ByStat)  continue;
                if (achievement.LinkedStat != stat) continue;
                if (currentValue < achievement.UnlockValue) continue;

                if (SteamUserStats.GetAchievement(achievement.AchievementId, out bool unlocked) && !unlocked)
                {
                    SteamUserStats.SetAchievement(achievement.AchievementId);
                }
            }
        }

        private void OnStatsReceived(UserStatsReceived_t data)
        {
            if (data.m_eResult == EResult.k_EResultOK)
            {
                Debug.Log("[SteamStatsService] Stats successfully received from Steam.");
            }
            else
            {
                Debug.LogWarning($"[SteamStatsService] Failed to receive stats: {data.m_eResult}");
            }
        }

        private void OnAchievementStored(UserAchievementStored_t callback)
        {
            if (0 == callback.m_nMaxProgress)
            {
                Debug.Log("[SteamStatsService] '" + callback.m_rgchAchievementName + "' unlocked!");
            }
            else
            {
                Debug.Log("[SteamStatsService] '" + callback.m_rgchAchievementName + "' progress callback, (" + callback.m_nCurProgress + "," + callback.m_nMaxProgress + ")");
            }
        }

        private void OnUserStatsStored(UserStatsStored_t callback)
        {
            if(EResult.k_EResultOK == callback.m_eResult)
            {
                Debug.Log("[SteamStatsService] StoreStats success.");
                return;
            }

            Debug.LogWarning($"[SteamStatsService] StoreStats failed: {callback.m_eResult}");
        }
    }
}
