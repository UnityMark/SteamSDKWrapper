using UnityEngine;

namespace Mark.Steamworks
{
    [CreateAssetMenu(fileName = "SteamStatDefinition", menuName = "Steam/Stat Definition")]
    public class SteamStatDefinition: ScriptableObject
    {
        [SerializeField] private string _statId;
        [SerializeField] private int _defaultValue;

        public string StatId => _statId;
        public int DefaultValue => _defaultValue;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(_statId))
            {
                Debug.LogWarning($"[{nameof(SteamStatDefinition)}] StatId is empty in '{name}'", this);
            }
        }
        #endif
    }
}
