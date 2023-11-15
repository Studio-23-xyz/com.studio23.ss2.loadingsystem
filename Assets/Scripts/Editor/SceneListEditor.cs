using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class SceneListEditor : EditorWindow
{
    [MenuItem("Studio-23/Get Scenes in Build")]
    public static void ShowWindow()
    {
        GetWindow<SceneListEditor>("Scene List");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Get Scenes"))
        {
            GetAllScenesInBuild();
        }
    }

    private void GetAllScenesInBuild()
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

        List<string> sceneNames = new List<string>();

        foreach (var scene in scenes)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);
            sceneNames.Add(sceneName);
        }

        // Log scene names to console or do something else with the list
        foreach (var name in sceneNames)
        {
            Debug.Log(name);
        }
    }
}