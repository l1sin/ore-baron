using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameController;

public class Yandex : MonoBehaviour
{
    public static Yandex Instance;
    public string Lang;
    public SaveFile Save;

    [DllImport("__Internal")]
    public static extern string GetLanguage();

    [DllImport("__Internal")]
    public static extern void DebugJS(string message);

    [DllImport("__Internal")]
    public static extern void Rate();

    [DllImport("__Internal")]
    public static extern void WatchAdMine();

    [DllImport("__Internal")]
    public static extern void WatchAdClick();

    [DllImport("__Internal")]
    public static extern void SaveExtern(string data);

    [DllImport("__Internal")]
    public static extern void LoadExtern();

    [DllImport("__Internal")]
    public static extern void FullScreenAd();

    [DllImport("__Internal")]
    public static extern void CallRate();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
        EditorInit();
#endif
    }

    public void StartInit()
    {
        SetLanguage();
        SetSave();
        SceneManager.LoadScene(1);
    }

    public void SetLanguage()
    {
        Lang = GetLanguage();
    }

    public void SetSave()
    {
        LoadExtern();
        if (!Save.Init) CreateSave();
    }

    public void CreateSave()
    {
        string emptySave = Resources.Load<TextAsset>("save").text;
        Save = JsonUtility.FromJson<SaveFile>(emptySave);
    }

    public void ApplySave(string json)
    {
        Save = JsonUtility.FromJson<SaveFile>(json);
    }

    public void EditorInit()
    {
        Lang = "en";
        CreateSave();
        SceneManager.LoadScene(1);
    }
}
