using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    private static LoadingScreenManager _instance;

    [SerializeField] private LoadingData _loadingData;
    [SerializeField] private GameObject _loaderUI;
    private Slider _progressSlider;

    public static LoadingScreenManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LoadingScreenManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("LoadingScreenManager");
                    _instance = singletonObject.AddComponent<LoadingScreenManager>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Initialize();
    }

    private void Initialize()
    {
        if (_loadingData == null)
        {
            _loadingData = Resources.Load<LoadingData>("LoadingData");
        }

        _progressSlider = _loaderUI.GetComponentInChildren<Slider>();
        _loaderUI.GetComponent<Image>().sprite = _loadingData.LoadingSprite;
    }

    public void EnableLoad()
    {
        LoadScene(1, _loadingData.LoadingTime);
    }

    private async void LoadScene(int index, float loadingTime)
    {
        await LoadSceneAsync(index, loadingTime);
    }

    private async UniTask LoadSceneAsync(int index, float loadingTime)
    {
        _loadingData.LoadingSlider.value = 0;
        _loaderUI.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
        asyncOperation.allowSceneActivation = false;

        float progress = 0;
        float elapsedTime = 0;

        while (!asyncOperation.isDone)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= loadingTime)
            {
                progress = 1;
                asyncOperation.allowSceneActivation = true;
            }
            else
            {
                progress = elapsedTime / loadingTime;
            }

            _progressSlider.value = progress;

            await UniTask.Yield();
        }
    }
}
