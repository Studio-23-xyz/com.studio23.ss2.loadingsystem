using System.Collections;
using System.Collections.Generic;
using System.IO;
using Studio23.SS2.SceneLoadingSystem.Data;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Studio23.SS2.SceneLoadingSystem.Editor
{
    public class ProgressBarEditor : EditorWindow
    {

        private LoadingScreenData LoadingScreenData;
        private LoadingScreenData TemporaryLoadingScreenData;
        private SerializedObject serializedObject;
        private SerializedProperty progressBarProperty;

        private string assetName = "ProgressBarData";
        private string folderPath = "Assets/Resources/";


        [MenuItem("Studio-23/SceneLoading/Generate Progress Bar Data")]
        public static void ShowWindow()
        {
            GetWindow<ProgressBarEditor>("ProgressBar Editor");
        }

        private void OnEnable()
        {
            CreateOrLoadProgressData();
            CloneData();
            serializedObject = new SerializedObject(TemporaryLoadingScreenData);
            progressBarProperty = serializedObject.FindProperty("ProgressBarData");
        }

        private void OnGUI()
        {
            GUILayout.Label("ProgressBar Editor", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            //serializedObject.Update();

            EditorGUILayout.PropertyField(progressBarProperty, true);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save"))
            {
                SaveProgressData();
            }
        }

        private void CreateOrLoadProgressData()
        {

            LoadingScreenData = Resources.Load<LoadingScreenData>("ProgressBarData");

            if (LoadingScreenData == null)
            {
                string fullPath = folderPath + assetName + ".asset";
                LoadingScreenData = CreateInstance<LoadingScreenData>();
                AssetDatabase.CreateAsset(LoadingScreenData, fullPath);
                EditorUtility.SetDirty(LoadingScreenData);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void SaveProgressData()
        {

            EditorUtility.CopySerialized(TemporaryLoadingScreenData, LoadingScreenData);
            EditorUtility.SetDirty(LoadingScreenData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        private void CloneData()
        {
            TemporaryLoadingScreenData = Instantiate(LoadingScreenData);
            TemporaryLoadingScreenData.name = LoadingScreenData.name;
        }
    }
}