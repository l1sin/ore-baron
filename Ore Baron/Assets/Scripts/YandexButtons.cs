using UnityEngine;

public class YandexButtons : MonoBehaviour
{
    public void Rate()
    {
#if UNITY_EDITOR
        Debug.Log("Rate");
#elif UNITY_WEBGL
        Yandex.Rate();
#endif
    }

    public void WatchAdMine()
    {
#if UNITY_EDITOR
        Debug.Log("WatchAdMine");
        GameController.Instance.ToggleMineAdBonus(1);
#elif UNITY_WEBGL
        Yandex.WatchAdMine();
#endif
    }

    public void WatchAdClick()
    {
#if UNITY_EDITOR
        Debug.Log("WatchAdClick");
        GameController.Instance.ToggleClickAdBonus(1);
#elif UNITY_WEBGL
        Yandex.WatchAdClick();
#endif
    }
}
