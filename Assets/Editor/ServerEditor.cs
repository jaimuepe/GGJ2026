#nullable enable

using Masks;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Server))]
    public class ServerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Server script = (Server)target;

            if (GUILayout.Button("Retrieve"))
            {
                script.RetrieveOtherPlayers(playersData =>
                    {
                        foreach (var playerData in playersData)
                        {
                            Debug.LogError(playerData);
                        }
                    },
                    (error) =>
                    {
                        Debug.LogError($"Error: {error}");
                    });
            }
        }
    }
}