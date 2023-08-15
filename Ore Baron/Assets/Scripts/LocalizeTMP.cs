using TMPro;
using UnityEngine;

public class LocalizeTMP : MonoBehaviour
{
    public TextMeshProUGUI TMP;
    public string Text;
    private void Start()
    {
        string text = (string)GameController.Instance.Localization.GetType().GetField(Text).GetValue(GameController.Instance.Localization);
        TMP.text = text;
    }
}
