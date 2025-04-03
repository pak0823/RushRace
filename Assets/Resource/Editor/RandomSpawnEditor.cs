using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RandomSpawn))]
public class RandomSpawnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RandomSpawn spawner = (RandomSpawn)target;
        if (GUILayout.Button("Generate Objects in Scene"))
        {
            spawner.GenerateAll();
        }
    }
}
