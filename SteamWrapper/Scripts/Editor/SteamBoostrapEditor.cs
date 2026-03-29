#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Mark.Steamworks.Editor
{
    [CustomEditor(typeof(SteamBoostrap))]
    public class SteamBoostrapEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SteamBoostrap bootstrap = (SteamBoostrap)target;

            EditorGUI.BeginDisabledGroup(!Application.isPlaying);

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("State", EditorStyles.boldLabel);
            EditorGUILayout.Toggle("Should Start API", bootstrap.ShouldStartApi);

            if (bootstrap.PlayerService != null)
            {
                EditorGUILayout.LabelField("Player Name", bootstrap.PlayerService.Name);
                EditorGUILayout.LabelField("Player Id", bootstrap.PlayerService.Id.ToString());
                EditorGUILayout.LabelField("Player State", bootstrap.PlayerService.State.ToString());
            }
            else
            {
                EditorGUILayout.HelpBox("PlayerService is null", MessageType.Info);
            }
        }
    }
}
#endif