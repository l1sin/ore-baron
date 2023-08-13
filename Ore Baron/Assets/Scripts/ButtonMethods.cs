using UnityEngine;

public class ButtonMethods : MonoBehaviour
{
    public void Toggle(GameObject gameObject)
    {
        if (gameObject.activeInHierarchy) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }
}
