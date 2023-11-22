using System.Collections;
using System.Collections.Generic;
using System.IO;
using Studio23.SS2.SceneLoadingSystem.Data;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Studio23.SS2.SceneLoadingSystem.Editor
{
    public class TextStyleSettingsEditor : EditorWindow
    {
        private BaseTextStyleData _titleFontSettings;
        private BaseTextStyleData _descriptionFontSettings;
        private readonly string _assetName = "SceneLoadingHintStyle";
        private readonly string _folderPath = "Assets/Resources/";

        [MenuItem("Studio-23/SceneLoading/Generate Hint Style Data")]
        public static void ShowWindow()
        {
            GetWindow<TextStyleSettingsEditor>("Create Hint Style");
        }

        private void OnGUI()
        {
            GUILayout.Label("Create Hint Style Data Creation", EditorStyles.boldLabel);



            GUILayout.Label("Title Font Settings", EditorStyles.boldLabel);
            _titleFontSettings = DrawTextSettingsFields(_titleFontSettings);

            GUILayout.Label("Description Font Settings", EditorStyles.boldLabel);
            _descriptionFontSettings = DrawTextSettingsFields(_descriptionFontSettings);

            if (GUILayout.Button("Create Hint Style Settings")) CreateCreditControllerSettingsDataAsset();
        }

        private BaseTextStyleData DrawTextSettingsFields(BaseTextStyleData baseTextSettings)
        {
            if (baseTextSettings == null)
                baseTextSettings = new BaseTextStyleData();

            baseTextSettings.FontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("Font Asset", baseTextSettings.FontAsset,
                typeof(TMP_FontAsset), false);
            baseTextSettings.FontStyle = (FontStyles)EditorGUILayout.EnumPopup("Font Style", baseTextSettings.FontStyle);


            return baseTextSettings;
        }

        private void CreateCreditControllerSettingsDataAsset()
        {
            // Check if any required fields are null
            if (_titleFontSettings == null || _descriptionFontSettings == null)
            {
                // Show an error message in the editor GUI
                EditorUtility.DisplayDialog("Error", "All fields must be set.", "OK");
                return;
            }

            var hintStyle = CreateInstance<TextStyleSettings>();
            hintStyle.TitleStyle = _titleFontSettings;
            hintStyle.DescriptionStyle = _descriptionFontSettings;


            if (!Directory.Exists(_folderPath)) Directory.CreateDirectory(_folderPath);
            string fullPath = _folderPath + _assetName + ".asset";
            if (File.Exists(fullPath)) File.Delete(fullPath);

            AssetDatabase.CreateAsset(hintStyle, fullPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = hintStyle;
            EditorUtility.SetDirty(hintStyle);
        }
    }

}