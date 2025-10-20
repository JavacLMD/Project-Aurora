using UnityEditor;
using UnityEngine;

namespace JavacLMD.Utils.Singletons.Editor
{
    [CustomEditor(typeof(GameSystems))]
    public class GameSystemsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // Draws the normal serialized fields (_manualSystems, etc.)

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Runtime Systems", EditorStyles.boldLabel);

            var systems = GameSystems.Instance != null
                ? GameSystems.Instance.GetActiveSystems()
                : null;

            if (systems == null || systems.Count == 0)
            {
                EditorGUILayout.HelpBox("No active systems registered.", MessageType.Info);
                return;
            }

            foreach (var sys in systems)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(sys.GetType().Name, EditorStyles.label);
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Ping", GUILayout.Width(60)))
                    {
                        if (sys is Object unityObj)
                            EditorGUIUtility.PingObject(unityObj);
                    }
                }
            }
        }
    }
}