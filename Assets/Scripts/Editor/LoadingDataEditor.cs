using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(LoadingData))]
public class LoadingDataEditor : EditorWindow
{
    private Sprite _loadingSprite;
    private Slider _loadingSlider;
    private float _loadingTime;

    [MenuItem("Studio-23/Loading Data", false, 2)]
    public static void ShowWindow()
    {
        GetWindow<LoadingDataEditor>("Loading Data");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Loading Data", EditorStyles.boldLabel);

        _loadingSprite = (Sprite)EditorGUILayout.ObjectField("Loading Sprite", _loadingSprite, typeof(Sprite), true);
        _loadingSlider = (Slider)EditorGUILayout.ObjectField("Loading Slider", _loadingSlider, typeof(Slider), true);
        _loadingTime = EditorGUILayout.FloatField("Loading Time", _loadingTime);

        if (GUILayout.Button("Create Loading Data"))
        {
            CreateLoadingData();
        }
    }

    private void CreateLoadingData()
    {
        LoadingData loadingData = ScriptableObject.CreateInstance<LoadingData>();

        loadingData.LoadingSprite = _loadingSprite;
        loadingData.LoadingSlider = _loadingSlider;
        loadingData.LoadingTime = _loadingTime;

        AssetDatabase.CreateAsset(loadingData, "Assets/LoadingData.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Loading Data created at Assets/LoadingData.asset");
    }
}
