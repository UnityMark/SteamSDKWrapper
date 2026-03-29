using UnityEngine;

namespace Mark.Steamworks
{
    public enum AchievementUnlockType
    {
        Manual,
        ByStat
    }

    [CreateAssetMenu(fileName = "SteamAchievementDefinition", menuName = "Steam/Achievement Definition")]
    public class SteamAchievementDefinition : ScriptableObject
    {
        [SerializeField] private string _achievementId;
        [SerializeField] private AchievementUnlockType _unlockType;
        [SerializeField] private SteamStatDefinition _linkedStat;
        [SerializeField] private int _unlockValue;

        public string AchievementId => _achievementId;
        public AchievementUnlockType UnlockType => _unlockType;
        public SteamStatDefinition LinkedStat => _linkedStat;
        public int UnlockValue => _unlockValue;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(_achievementId))
            {
                Debug.LogWarning($"[{nameof(SteamAchievementDefinition)}] AchievementId is empty in '{name}'", this);
            }

            if (_unlockType == AchievementUnlockType.ByStat && _linkedStat == null)
            {
                Debug.LogWarning($"[{nameof(SteamAchievementDefinition)}] '{name}' uses ByStat but LinkedStat is null", this);
            }
        }
        #endif
    }
}
