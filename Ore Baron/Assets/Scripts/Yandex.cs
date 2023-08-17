using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Yandex : MonoBehaviour
{
    public static Yandex Instance;
    public string Lang;
    public GameController.SaveFile Save;

    [DllImport("__Internal")]
    private static extern string GetLanguage();

    [DllImport("__Internal")]
    public static extern void DebugJS(string message);

    [DllImport("__Internal")]
    public static extern void Rate();

    [DllImport("__Internal")]
    public static extern void WatchAdMine();

    [DllImport("__Internal")]
    public static extern void WatchAdClick();

    [DllImport("__Internal")]
    public static extern void BuyMine();

    [DllImport("__Internal")]
    public static extern void BuyClick();

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

    } 
}
