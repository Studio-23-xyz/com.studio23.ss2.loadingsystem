using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.IO;

namespace Studio23.SS2.SceneLoadingSystem.Editor
{
    public class SceneListEditor : EditorWindow
    {
        static List<string> sceneNames = new List<string>();
        private static string _className = "SceneTable";
        private static string _nameSpace = "Studio23.SS2.SceneLoadingSystem.Data";


        [MenuItem("Studio-23/SceneLoading/Generate Scene Data")]
        public static void GetAllScenesInBuild()
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

            foreach (var scene in scenes)
            {
                string sceneName = Path.GetFileNameWithoutExtension(scene.path);
                sceneNames.Add(sceneName);
            }

            GenerateStringProperties();
        }

        private static void GenerateStringProperties()
        {


            string scriptContent = $"namespace {_nameSpace}\n{{\n";

            scriptContent += $"\tpublic static class {_className}\n\t{{\n";

            foreach (var scene in sceneNames)
            {
                var sceneName = scene.Replace(" ", "");
                scriptContent += $"\t\tpublic static readonly string {sceneName} = \"{scene}\";\n";
            }

            scriptContent += "\t}\n";
            scriptContent += "}";

            string scriptPath = Path.Combine("Assets", $"{_className}.cs");
            if (File.Exists(scriptPath))
            {
                File.Delete(scriptPath);
            }

            File.WriteAllText(scriptPath, scriptContent);
            AssetDatabase.Refresh();
        }
    }
}