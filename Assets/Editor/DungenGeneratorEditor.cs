#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DungeonGenerator generator = (DungeonGenerator)target;

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Dungeon Controls", EditorStyles.boldLabel);

        if (GUILayout.Button("Spawn Everything"))
        {
            // Clear old dungeon (including NavMesh)
            generator.ClearOldDungeon();

            // Generate dungeon (which also bakes NavMesh, populates props, monsters, and spawns player)
            generator.Generate();

            Debug.Log("Dungeon, Props, NavMesh, Monsters, and Player spawned.");
        }

        if (GUILayout.Button("Clear Everything"))
        {
            // Clear Dungeon
            generator.ClearOldDungeon();

            // Remove Player if exists
            GameObject existingPlayer = GameObject.Find("Player");
            if (existingPlayer != null)
            {
                DestroyImmediate(existingPlayer);
                Debug.Log("Player removed.");
            }
        }
    }
}
#endif
