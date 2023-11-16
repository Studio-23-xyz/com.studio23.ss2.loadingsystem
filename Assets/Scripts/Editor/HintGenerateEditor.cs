using System;
using System.Collections;
using System.Collections.Generic;
using Studio23.SS2.SceneLoadingSystem.Data;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Studio23.SS2.SceneLoadingSystem.Editor
{
    public class HintGenerateEditor : EditorWindow
    {

        private GeneratedHints LoadingHintData;
        private GeneratedHints TemporaryLoadingHintData;

        private string assetName = "HintsData";
        private string folderPath = "Assets/Resources/";
        private Vector2 scrollPosition = Vector2.zero;
        private Dictionary<HintType, List<string>> hintsDictionary = new Dictionary<HintType, List<string>>();
        private Dictionary<HintType, List<string>> temporaryHintsDictionary = new Dictionary<HintType, List<string>>();


        [MenuItem("Studio-23/SceneLoading/Generate Hints Data")]
        public static void ShowWindow()
        {
            GetWindow<HintGenerateEditor>("Create Hints");
        }

        private void OnEnable()
        {
            CreateOrLoadProgressData();
        }


        private void OnGUI()
        {
            EditorGUILayout.LabelField("Generated Hints Editor", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            DisplayHintsEditor();
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Save"))
            {
                SaveHintsData();
            }
        }

       
        private void DisplayHintsEditor()
        {
            foreach (HintType hintType in Enum.GetValues(typeof(HintType)))
            {
                EditorGUILayout.LabelField(hintType.ToString(), EditorStyles.boldLabel);
                List<string> hintList = GetHintList(hintType);
                List<string> temporaryHintList = GetTemporaryHintList(hintType);

                EditorGUI.indentLevel++;

                int listSize = EditorGUILayout.IntField("List Size", hintList.Count);
                while (listSize < temporaryHintList.Count)
                {
                    temporaryHintList.RemoveAt(temporaryHintList.Count - 1);
                }
                while (listSize > temporaryHintList.Count)
                {
                    temporaryHintList.Add("");
                }

                for (int i = 0; i < temporaryHintList.Count; i++)
                {
                    temporaryHintList[i] = EditorGUILayout.TextField("Element " + i, temporaryHintList[i]);
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
        }

        private List<string> GetTemporaryHintList(HintType hintType)
        {
            if (!temporaryHintsDictionary.ContainsKey(hintType))
            {
                temporaryHintsDictionary.Add(hintType, new List<string>());
            }
            return temporaryHintsDictionary[hintType];
        }


        private List<string> GetHintList(HintType hintType)
        {
            if (!hintsDictionary.ContainsKey(hintType))
            {
                hintsDictionary.Add(hintType, new List<string>());
            }
            return hintsDictionary[hintType];
        }

        private void SaveHintsData()
        {

            hintsDictionary.Clear();

            foreach (var pair in temporaryHintsDictionary)
            {
                hintsDictionary.Add(pair.Key, new List<string>(pair.Value));
            }


            GeneratedHints newHints = ScriptableObject.CreateInstance<GeneratedHints>();
            newHints.HintsInfo = new List<HintData>();

            foreach (var hintPair in hintsDictionary)
            {
                HintData hintData = new HintData
                {
                    HintType = hintPair.Key,
                    Hints = hintPair.Value
                };

                newHints.HintsInfo.Add(hintData);
            }

            string fullPath = folderPath + assetName + ".asset";
            AssetDatabase.CreateAsset(newHints, fullPath);
            EditorUtility.SetDirty(newHints);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void CreateOrLoadProgressData()
        {

            LoadingHintData = Resources.Load<GeneratedHints>("HintsData");

            if (LoadingHintData == null)
            {
                string fullPath = folderPath + assetName + ".asset";
                LoadingHintData = CreateInstance<GeneratedHints>();
                AssetDatabase.CreateAsset(LoadingHintData, fullPath);
                EditorUtility.SetDirty(LoadingHintData);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                hintsDictionary.Clear();
                temporaryHintsDictionary.Clear();

                foreach (var hintData in LoadingHintData.HintsInfo)
                {
                    hintsDictionary.Add(hintData.HintType, hintData.Hints);
                    temporaryHintsDictionary.Add(hintData.HintType, new List<string>(hintData.Hints));
                }
            }
        }

        
    }



}