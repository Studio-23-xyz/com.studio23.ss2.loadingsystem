using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Studio23.SS2.SceneLoadingSystem.Data;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Studio23.SS2.SceneLoadingSystem.Editor
{
    public class HintGenerateEditor : EditorWindow
    {

        private HintScriptableObject LoadingHintData;

        private string assetName = "HintsData";
        private string folderPath = "Assets/Resources/";
        private Vector2 scrollPosition = Vector2.zero;
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
                List<string> temporaryHintList = GetTemporaryHintList(hintType);

                EditorGUI.indentLevel++;

                int listSize = EditorGUILayout.IntField("List Size", temporaryHintList.Count);
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




        private void SaveHintsData()
        {

            HintScriptableObject newHints = ScriptableObject.CreateInstance<HintScriptableObject>();
            newHints.HintsInfo = new List<HintData>();

            foreach (var hintPair in temporaryHintsDictionary)
            {
                HintData hintData = new HintData
                {
                    HintType = hintPair.Key,
                    Hints = hintPair.Value
                };

                newHints.HintsInfo.Add(hintData);
            }

            string fullPath = folderPath + assetName + ".asset";
            if(File.Exists(fullPath)) File.Delete(fullPath);
                
            AssetDatabase.CreateAsset(newHints, fullPath);
            EditorUtility.SetDirty(newHints);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void CreateOrLoadProgressData()
        {

            LoadingHintData = Resources.Load<HintScriptableObject>("HintsData");

            if (LoadingHintData != null)
            {
                temporaryHintsDictionary.Clear();
                foreach (var hintData in LoadingHintData.HintsInfo)
                {
                    temporaryHintsDictionary.Add(hintData.HintType, new List<string>(hintData.Hints));
                }
            }
        }

        
    }



}