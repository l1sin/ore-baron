using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Yandex : MonoBehaviour
{
    public static Yandex Instance;
    public string Lang;
    public GameController.SaveFile Save;

    [DllImport("__Internal")]
    private static extern string SetLanguage();

    private void Start()
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
        Lang = SetLanguage();
        SceneManager.LoadScene(1);
    }
}
